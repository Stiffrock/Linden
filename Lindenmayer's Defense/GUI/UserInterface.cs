using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lindenmayers_Defense.GUI
{
  #region Class definition
  class UserInterface
  {
    private TowerContainer towerBox;
    private ComponentContainer[,] inventoryArray;
    private ComponentContainer[] selectionArray;  
    private List<ComponentContainer> componentContainerList;
    private List<StatContainer> statContainerList;
    private List<LComponent> componentList;
    private Point TowerBoxRecSize;
    //public List<LComponent> result;
    private Vector2 compOffset;
    private TowerManager tm;
    private StatContainer displayedStatContainer;
    private Dictionary<string, LComponent> grammarComponents;
    private World world;
    private InventoryManager inventoryManager;
    private int CurrentTowerCost;

    /*    
    componentContainerList keeps track of all the containers, if a container from inventory is clicked, a copy of its component
    is created but set with the selection arrays position, the recieveing container from selection array gets its name.       
    */

    public UserInterface(TowerManager tm, World world)
    {
      componentList = new List<LComponent>();  
      componentContainerList = new List<ComponentContainer>();
      statContainerList = new List<StatContainer>(); 

      inventoryArray = new ComponentContainer[3, 6];
      selectionArray = new ComponentContainer[5];   
      compOffset = new Vector2(12, 8);
      displayedStatContainer = null;
      TowerBoxRecSize = new Point(100, 100);
      this.tm = tm;
      this.world = world;
      grammarComponents = tm.GetGrammarComponents();
      inventoryManager = new InventoryManager();
      inventoryArray = inventoryManager.CreateInventory(3, 6, ref componentContainerList);
      selectionArray = inventoryManager.CreateSelectionArray(5, ref componentContainerList);
      InitTowerBox();
      InitComponents();
      CurrentTowerCost = 0;
    
    }
    #endregion

    #region Properties
    public Container GetTowerCreator()
    {
      return towerBox;
    }

    public int GetCost() { return CurrentTowerCost; }
    public bool MouseIntersect(Rectangle uiObject)
    {
      return uiObject.Contains(Input.GetMousePoint());
    }

    public string GenerateGrammar()
    {
      string result = "";
      for (int i = 0; i < componentList.Count; i++)
      {
        if (componentList[i].chosen)
        {
          result += componentList[i].grammar;
        }
      }
      if (!result.Contains('Y'))
        result.Insert(0, "X");
      return result;
    }

    public Texture2D[] GetTextures()
    {
      Texture2D[] temp = new Texture2D[5];
      int count = 0;
      for (int i = 0; i < componentList.Count; i++)
      {
        if (componentList[i].chosen)
        {
          temp[count] = componentList[i].tex;
          ++count;
        }
      }
      return temp;
    }
    #endregion

    #region Initialize UI
    private void InitComponents()
    {
      int i = 0, j = 0;
      foreach (KeyValuePair<string, LComponent> entry in grammarComponents)
      {
        inventoryArray[j, i].component = entry.Value;
        inventoryArray[j, i].name = entry.Key;
        inventoryArray[j, i].component.pos = inventoryArray[j, i].pos + compOffset;
        componentList.Add(inventoryArray[j, i].component);

        ++i;
        if (i >= inventoryArray.GetLength(1))
        {
          i = 0;
          ++j;
        }
      }
    }

    private void InitTowerBox()
    {
      towerBox = new TowerContainer(AssetManager.GetTexture("container2"), new Vector2((Game1.ScreenWidth / 2 + Game1.ScreenWidth / 4) - TowerBoxRecSize.X / 2, (Game1.ScreenHeight - Game1.ScreenHeight / 6) - 10));
      towerBox.rec.Size = TowerBoxRecSize;
    }

    private void BuildStatWindow(GameObject t)
    {
      Tower temp = (Tower)t;
      StatContainer towerStatContainer = new StatContainer(AssetManager.GetTexture("panel"), t.pos, temp, world);
      towerStatContainer.componentTextures = temp.componentTextures;
      displayedStatContainer = towerStatContainer;
    }
    #endregion

    #region HandleEvents
    private void HandleStatDisplay()
    {
      List<GameObject> golist = world.GetGameObjects();
      foreach (GameObject t in golist)
      {
        if (t is Tower)
        {
          Tower temp = (Tower)t;
          if (MouseIntersect(temp.hitbox))
          {
            BuildStatWindow(t);
            temp.displayStats = true;
            break;
          }
          if (displayedStatContainer != null && !MouseIntersect(displayedStatContainer.rec))
          {
            for (int i = 0; i < displayedStatContainer.statButtonList.Count; i++)
            {
              if (!MouseIntersect(displayedStatContainer.statButtonList[i].hitbox))
              {
                temp.displayStats = false;
                displayedStatContainer = null;
                break;
              }
            }
          }
        }
      }
    }

    private void StatButtonClick()
    {
      if (displayedStatContainer != null)
        displayedStatContainer.StatButtonClick();
    }

    private void HandleAddComponent(int i)
    {
      if (componentContainerList[i].name == null) 
        return;

      if (MouseIntersect(componentContainerList[i].rec))
      {
        for (int j = 0; j < selectionArray.GetLength(0); j++)
        {
          if (selectionArray[j].name == null && !componentContainerList[i].bInSelectionArray)           
          {
            inventoryManager.AddToSelectionArray(ref selectionArray, ref componentList, componentContainerList, compOffset, i, j);
            break;
          }
        }
      }
    }

    private void HandleRemoveComponent()
    {
      for (int i = 0; i < selectionArray.GetLength(0); i++)
      {
        if (selectionArray[i].name != null && MouseIntersect(selectionArray[i].rec))
        {
          for (int j = 0; j < componentList.Count; j++)
          {
            if (componentList[j].pos == selectionArray[i].pos + compOffset)
            {
              inventoryManager.RemoveFromSelectionArray(ref selectionArray,ref componentList, i, j);
              break;
            }
          }
        }
      }
    }

    public void SelectRandomComponents()
    {
      for (int i = 0; i < selectionArray.Length; i++)
      {
        for (int j = 0; j < componentList.Count; j++)
        {
          if (componentList[j].pos == selectionArray[i].pos + compOffset)
          {
            inventoryManager.RemoveFromSelectionArray(ref selectionArray, ref componentList, i, j);
            break;
          }
        }
        int index = Utility.RandomInt(0, componentList.Count() - 1);
        inventoryManager.AddToSelectionArray(ref selectionArray, ref componentList, componentContainerList, compOffset, index, i);
      }
    }

    private void UpdateTowerCost()
    {
      int count = 0;
      for (int i = 0; i < selectionArray.GetLength(0); i++)
      {
        if (selectionArray[i].name != null)
          ++count;
      }
      CurrentTowerCost = count * 10;   
    }

    #endregion

    #region Update/Draw
    public virtual void Update(GameTime gt)
    {
      towerBox.Update(gt);
      UpdateTowerCost();
      if (Input.KeyPressed(Keys.R))
        SelectRandomComponents();

      for (int i = 0; i < componentContainerList.Count; i++) 
      {
        componentContainerList[i].Update(gt);
        if (Input.LeftMouseButtonClicked())
          HandleAddComponent(i);
      }
      if (Input.LeftMouseButtonClicked())
      {
        HandleRemoveComponent();
        HandleStatDisplay();
        StatButtonClick();
      }
    }

    public virtual void Draw(SpriteBatch sb)
    {
      towerBox.Draw(sb);
      sb.DrawString(AssetManager.GetFont("font1"), "Cost of tower: " + CurrentTowerCost.ToString(), new Vector2(765, 850), Color.GreenYellow);

      for (int i = 0; i < componentContainerList.Count; i++)
        componentContainerList[i].Draw(sb);

      for (int i = 0; i < componentList.Count; i++)
        componentList[i].Draw(sb);

      if (displayedStatContainer != null)
        displayedStatContainer.Draw(sb);
    }
    #endregion
  }
}
