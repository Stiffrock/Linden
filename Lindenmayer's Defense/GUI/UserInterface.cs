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
    private Container towerBox;
    private List<Container> containerList;


    public UserInterface()
    {
      containerList = new List<Container>();    
      inventoryArray = new Container[3, 6];
      componentArray = new Container[5];  
      InitInventoryArray();
      InitComponentArray();
      InitTowerBox();
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
        containerList.Add(componentArray[i]);

      }
    }
    
    public virtual void Update(GameTime gt)
    {
      for (int i = 0; i < containerList.Count; i++)
      {
        containerList[i].Update(gt);
      }
    }

    public virtual void Draw(SpriteBatch sb)
    {
      for (int i = 0; i < containerList.Count; i++)
      {
        containerList[i].Draw(sb);
      }
    }

  }
}
