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
  class UserInterface
  {
    private Container[,] inventoryArray;
    private Container[] componentArray;
    public Container towerBox;
    private List<Container> containerList;
    private List<LComponent> componentList;
    public List<LComponent> result;
    private Dictionary<string, LComponent> grammarComponenets;


    public UserInterface(Dictionary<string, LComponent> grammarComponenets)
    {
      containerList = new List<Container>();    
      inventoryArray = new Container[3, 6];
      componentArray = new Container[5];
      this.grammarComponenets = grammarComponenets;
      componentList = new List<LComponent>();
      result = new List<LComponent>();
      InitInventoryArray();
      InitComponentArray();
      InitTowerBox();
      InitComponent();
    }

    private void InitComponent()
    {
      int i = 0, j = 0;
      foreach (KeyValuePair<string, LComponent> entry in grammarComponenets)
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
      towerBox = new Container(AssetManager.GetTexture("pixel"), new Vector2(950, 655));
      towerBox.rec.Size = new Point(100, 100);
      containerList.Add(towerBox);
    }

    private void InitInventoryArray()
    {
      for (int i = 0; i < inventoryArray.GetLength(0); i++)
      {
        for (int j = 0; j < inventoryArray.GetLength(1); j++)
        {
          inventoryArray[i, j] = new Container(AssetManager.GetTexture("pixel"), new Vector2(100 + 55 * j, 600 + 55 * i));
          containerList.Add(inventoryArray[i, j]);
        }
      }
    }

    private void InitComponentArray()
    {
      for (int i = 0; i < componentArray.GetLength(0); i++)
      {
        componentArray[i] = new Container(AssetManager.GetTexture("pixel"), new Vector2(550 + 55 * i, 705));
        componentArray[i].ComponentArray = true;
        containerList.Add(componentArray[i]);
      }
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
      if (containerList[i].name != null)
      {
        if (MouseIntersect(containerList[i].rec))
        {
          for (int j = 0; j < componentArray.GetLength(0); j++)
          {
            if (componentArray[j].name == null && !containerList[i].ComponentArray)
            {
              componentArray[j].component = componentList[i];
              componentArray[j].name = containerList[i].name;

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
    }

    private void RemoveComponent()
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
      for (int i = 0; i < containerList.Count; i++)
      {
        containerList[i].Update(gt);
        if (Input.LeftMouseButtonClicked())
          HandleComponent(i);
      }
      if (Input.LeftMouseButtonClicked())
        RemoveComponent();


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
      result += "X";
      return result;
    }

    public virtual void Draw(SpriteBatch sb)
    {
      for (int i = 0; i < containerList.Count; i++)
      {
        containerList[i].Draw(sb);
      }
      for (int i = 0; i < componentList.Count; i++)
      {
        componentList[i].Draw(sb);
      }
    }

  }
}
