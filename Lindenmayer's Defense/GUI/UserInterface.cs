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
    private ComponentContainer[,] inventoryArray;
    private ComponentContainer[] componentArray;
    private TowerContainer towerBox;
    private List<ComponentContainer> compContainerList;
    private List<StatContainer> statContainerList;
    private List<LComponent> componentList;
    private Point TowerBoxRecSize;
    public List<LComponent> result;
    private Vector2 compOffset;
    private TowerManager tm;
    private StatContainer displayedStatContainer;
    private Dictionary<string, LComponent> grammarComponents;
    private World world;

    public UserInterface(TowerManager tm, World world)
    {
      compContainerList = new List<ComponentContainer>();
      statContainerList = new List<StatContainer>();
      inventoryArray = new ComponentContainer[3, 6];
      componentArray = new ComponentContainer[5];
      compOffset = new Vector2(12, 8);
      displayedStatContainer = null;
      TowerBoxRecSize = new Point(100, 100);
      this.tm = tm;
      grammarComponents = tm.GetGrammarComponents();
      componentList = new List<LComponent>();
      result = new List<LComponent>();
      this.world = world;
      InitInventoryArray();
      InitComponentArray();
      InitTowerBox();
      InitComponent();
    }
    #endregion

    #region Properties
    public Container GetTowerCreator()
    {
      return towerBox;
    }
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
    private void InitComponent()
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

    private void InitInventoryArray()
    {
      for (int i = 0; i < inventoryArray.GetLength(0); i++)
      {
        for (int j = 0; j < inventoryArray.GetLength(1); j++)
        {
          inventoryArray[i, j] = new ComponentContainer(AssetManager.GetTexture("container2"), new Vector2((Game1.ScreenWidth / 2 - (Game1.ScreenWidth / 4) - 225/*offset*/) + 55 * j, (Game1.ScreenHeight - Game1.ScreenHeight / 8) - 55 * i));
          compContainerList.Add(inventoryArray[i, j]);
        }
      }
    }

    private void InitComponentArray()
    {
      for (int i = 0; i < componentArray.GetLength(0); i++)
      {
        componentArray[i] = new ComponentContainer(AssetManager.GetTexture("container2"), new Vector2(Game1.ScreenWidth / 2 - 137.5f /*offset*/ + 55 * i, (Game1.ScreenHeight - Game1.ScreenHeight / 8)));
        componentArray[i].ComponentArray = true;
        compContainerList.Add(componentArray[i]);
      }
    }
    private void BuildStatWindow(GameObject t)
    {
      Tower temp = (Tower)t;
      StatContainer towerStatContainer = new StatContainer(AssetManager.GetTexture("panel"), t.pos, temp);
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
      if (compContainerList[i].name == null)
        return;

      if (MouseIntersect(compContainerList[i].rec))
      {
        for (int j = 0; j < componentArray.GetLength(0); j++)
        {
          if (componentArray[j].name == null && !compContainerList[i].ComponentArray)
          {
            componentArray[j].component = componentList[i];
            componentArray[j].name = compContainerList[i].name;

            LComponent temp = new LComponent(componentList[i].tex, new Vector2(componentArray[j].pos.X + compOffset.X, componentArray[j].pos.Y + compOffset.Y), componentList[i].grammar);
            temp.rec = new Rectangle((int)temp.pos.X, (int)temp.pos.Y, 40, 40);
            temp.chosen = true;
            result.Add(temp);
            componentList.Add(temp);
            break;
          }
        }
      }
    }

    private void HandleRemoveComponent()
    {
      for (int i = 0; i < componentArray.GetLength(0); i++)
      {
        if (componentArray[i].name != null && MouseIntersect(componentArray[i].rec))
        {
          for (int j = 0; j < componentList.Count; j++)
          {
            if (componentList[j].pos == componentArray[i].pos + compOffset)
            {
              componentArray[i].component = null;
              componentArray[i].name = null;
              componentList.Remove(componentList[j]);
              break;
            }
          }
        }
      }
    }
    #endregion

    #region Update/Draw
    public virtual void Update(GameTime gt)
    {
      towerBox.Update(gt);
      if (Input.KeyPressed(Keys.R))
      {
        for (int i = 0; i < componentArray.Length; i++)
        {
          for (int j = 0; j < componentList.Count; j++)
          {
            if (componentList[j].pos == componentArray[i].pos + compOffset)
            {
              componentArray[i].component = null;
              componentArray[i].name = null;
              componentList.Remove(componentList[j]);
              break;
            }
          }
          int index = Utility.RandomInt(0, componentList.Count() - 1);
          componentArray[i].component = componentList[index];
          componentArray[i].name = compContainerList[index].name;

          LComponent temp = new LComponent(componentList[index].tex, new Vector2(componentArray[i].pos.X + compOffset.X, componentArray[i].pos.Y + compOffset.Y), componentList[index].grammar);
          temp.rec = new Rectangle((int)temp.pos.X, (int)temp.pos.Y, 40, 40);
          temp.chosen = true;
          result.Add(temp);
          componentList.Add(temp);
        }
      }
      for (int i = 0; i < compContainerList.Count; i++)
      {
        compContainerList[i].Update(gt);
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

      for (int i = 0; i < compContainerList.Count; i++)
        compContainerList[i].Draw(sb);

      for (int i = 0; i < componentList.Count; i++)
        componentList[i].Draw(sb);

      if (displayedStatContainer != null)
        displayedStatContainer.Draw(sb);
    }
    #endregion
  }
}
