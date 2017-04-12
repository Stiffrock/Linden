using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lindenmayers_Defense
{
  class ResourceManager
  {
    private int gold;
    public int Gold { get { return gold; }  private set { gold = value; } }

    public ResourceManager(int startingGold = 0)
    {
      this.gold = startingGold;
    }
    public void AddGold(int amount)
    {
      Gold += amount;
    }
    public void RemoveGold(int amount)
    {
      Gold -= amount;
    }

    public bool CanAfford(int amount)
    {
      return Gold >= amount;
    }

    public void Draw(SpriteBatch sb)
    {
      sb.DrawString(AssetManager.GetFont("font1"), "Current gold: " + Gold.ToString(), new Vector2(765, 830), Color.GreenYellow);
    }

  }
}






