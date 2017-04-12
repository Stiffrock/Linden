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
  class InventoryManager
  {

    public InventoryManager()
    {
    }

    public ComponentContainer[,] CreateInventory(int x, int y, ref List<ComponentContainer> componentContainerList)
    {
      ComponentContainer[,] inventoryArray = new ComponentContainer[x, y];
      for (int i = 0; i < inventoryArray.GetLength(0); i++)
      {
        for (int j = 0; j < inventoryArray.GetLength(1); j++)
        {
          inventoryArray[i, j] = new ComponentContainer(AssetManager.GetTexture("container2"), new Vector2((Game1.ScreenWidth / 2 - (Game1.ScreenWidth / 4) - 225/*offset*/) + 55 * j, (Game1.ScreenHeight - Game1.ScreenHeight / 8) - 55 * i));
          componentContainerList.Add(inventoryArray[i, j]);
        }
      }
      return inventoryArray;
    }

    public ComponentContainer[] CreateSelectionArray(int x, ref List<ComponentContainer> componentContainerList)
    {
      ComponentContainer[] componentArray = new ComponentContainer[x];

      for (int i = 0; i < componentArray.GetLength(0); i++)
      {
        componentArray[i] = new ComponentContainer(AssetManager.GetTexture("container2"), new Vector2(Game1.ScreenWidth / 2 - 137.5f /*offset*/ + 55 * i, (Game1.ScreenHeight - Game1.ScreenHeight / 8)));
        componentArray[i].bInSelectionArray = true;
        componentContainerList.Add(componentArray[i]);
      }
      return componentArray;
    }


    public void RemoveFromSelectionArray(ref ComponentContainer[] componentContainer, ref List<LComponent> componentList, int i, int j)
    {
      componentContainer[i].component = null;
      componentContainer[i].name = null;
      componentList.Remove(componentList[j]);
    }

    public void AddToSelectionArray(ref ComponentContainer[] selectionArray, ref List<LComponent> componentList, List<ComponentContainer> componentContainerList, Vector2 compOffset, int i, int j)
    {
      selectionArray[j].component = componentList[i];
      selectionArray[j].name = componentContainerList[i].name;

      LComponent temp = new LComponent(componentList[i].tex, new Vector2(selectionArray[j].pos.X + compOffset.X, selectionArray[j].pos.Y + compOffset.Y), componentList[i].grammar);
      temp.rec = new Rectangle((int)temp.pos.X, (int)temp.pos.Y, 40, 40);
      temp.chosen = true;
      componentList.Add(temp);
    }
  }

}
