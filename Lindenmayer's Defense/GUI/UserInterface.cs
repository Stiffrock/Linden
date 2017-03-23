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
  /// TO DO
  /// Switch to arrays instead of lists
  /// 
  class UserInterface
  {
    private ComponentContainer[,] inventoryArray;
    private ComponentContainer[] componentArray;
    private ComponentContainer towerBox;
    private List<ComponentContainer> compContainerList;
    private List<StatContainer> statContainerList;
    private List<TowerContainer> towerContainerList;
    private List<LComponent> componentList;
    public List<LComponent> result;
    private TowerManager tm;
    private StatContainer displayedStatContainer;
    private Dictionary<string, LComponent> grammarComponents;
    private World world;

    public UserInterface(TowerManager tm, World world)
    {
      compContainerList = new List<ComponentContainer>();
      statContainerList = new List<StatContainer>();
      towerContainerList = new List<TowerContainer>();
      inventoryArray = new ComponentContainer[3, 6];
      componentArray = new ComponentContainer[5];
      displayedStatContainer = null;
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


    public Container GetTowerCreator()
    {
      return towerBox;
    }

    private void InitComponent()
    {
      int i = 0, j = 0;
      foreach (KeyValuePair<string, LComponent> entry in grammarComponents)
      {
        inventoryArray[j, i].component = entry.Value;
        inventoryArray[j, i].name = entry.Key;
        inventoryArray[j, i].component.pos = inventoryArray[j, i].pos;
        inventoryArray[j, i].component.rec = new Rectangle(100, 100, 20, 20);//inventoryArray[j, i].rec;
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
      towerBox = new ComponentContainer(AssetManager.GetTexture("pixel"), new Vector2(950, 655));
      towerBox.rec.Size = new Point(100, 100);
      compContainerList.Add(towerBox);
    }

    private void InitInventoryArray()
    {
      for (int i = 0; i < inventoryArray.GetLength(0); i++)
      {
        for (int j = 0; j < inventoryArray.GetLength(1); j++)
        {
          inventoryArray[i, j] = new ComponentContainer(AssetManager.GetTexture("pixel"), new Vector2(100 + 55 * j, 600 + 55 * i));
          compContainerList.Add(inventoryArray[i, j]);
        }
      }
    }

    private void InitComponentArray()
    {
      for (int i = 0; i < componentArray.GetLength(0); i++)
      {
        componentArray[i] = new ComponentContainer(AssetManager.GetTexture("pixel"), new Vector2(550 + 55 * i, 705));
        componentArray[i].ComponentArray = true;
        compContainerList.Add(componentArray[i]);
      }
    }

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
            RemoveStatWindow(t);
            temp.displayStats = false;
          }
        }    
      }          
    }

    private void StatButtonClick()
    {
      if (Input.LeftMouseButtonClicked())
      {
        if (displayedStatContainer != null)
          displayedStatContainer.StatButtonClick();     
      }
    }

    private void RemoveStatWindow(GameObject t)
    {
      Tower temp = (Tower)t;
      displayedStatContainer = null;      
    }

    private void BuildStatWindow(GameObject t)
    {
      Tower temp = (Tower)t;    
      StatContainer towerStatContainer = new StatContainer(AssetManager.GetTexture("pixel"), t.pos, temp);  
      displayedStatContainer = towerStatContainer;
    }

    public bool MouseIntersect(Rectangle uiObject)
    {
      if (uiObject.Contains(Input.GetMousePoint()))
      {
        return true;
      }
      return false;
    }

    private void HandleComponent(int i)
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

            LComponent temp = new LComponent(componentList[i].tex, componentList[i].grammar);
            temp.pos = componentArray[j].pos;
            temp.rec = new Rectangle(100, 100, 20, 20);
            temp.chosen = true;
            result.Add(temp);
            componentList.Add(temp);
            break;
          }
        }
      }
    }

    private void CheckforRemoveComponent()
    {
      for (int i = 0; i < componentArray.GetLength(0); i++)
      {
        if (componentArray[i].name != null && MouseIntersect(componentArray[i].rec))
        {
          for (int j = 0; j < componentList.Count; j++)
          {
            if (componentList[j].pos == componentArray[i].pos)
            {
              componentArray[i].component = null;
              componentArray[i].name = null;
              componentList.Remove(componentList[j]);
            }
          }
        }
      }
    }

    public virtual void Update(GameTime gt)
    {
      for (int i = 0; i < compContainerList.Count; i++)
      {
        compContainerList[i].Update(gt);
        if (Input.LeftMouseButtonClicked())
          HandleComponent(i);
      }
      if (Input.LeftMouseButtonClicked())
      {
        CheckforRemoveComponent();
        HandleStatDisplay();
        StatButtonClick();
      }
    }

    public string GetResult()
    {
      string result = "";
      for (int i = 0; i < componentList.Count; i++)
      {
        if (componentList[i].chosen)
        {
          result += componentList[i].grammar;
        }
      }
      if(!result.Contains('Y'))
        result += "X";
      return result;
    }

    public virtual void Draw(SpriteBatch sb)
    {
      if (displayedStatContainer!= null)      
        displayedStatContainer.Draw(sb);   

      for (int i = 0; i < compContainerList.Count; i++)
        compContainerList[i].Draw(sb);
  
      for (int i = 0; i < componentList.Count; i++)
        componentList[i].Draw(sb);

      for (int i = 0; i < towerContainerList.Count; i++)
        towerContainerList[i].Draw(sb);
    }

  }
}
