using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lindenmayers_Defense
{
  /// <summary>
  /// Manages the placing and upgrading of towers
  /// </summary>
  class TowerManager
  {
    protected World world;
    private Tower t;
    public bool hasTower;
    public TowerManager(World world)
    {
      this.world = world;
      t = null;
    }
    public void SetTowerType(Tower t)
    {
    }
    public void CreateTower(Vector2 pos)
    {
      if (!hasTower)
      {
        string g = world.GetGrammar();
        t = new Tower(world, AssetManager.GetTexture("tower01"), pos);
        t.towerGrammar = g;
        hasTower = true;
        world.AddGameObject(t);

      }

    }
    public void Update(GameTime gt)
    {
   
      if (t != null && hasTower)
      {
        t.pos = Input.GetMousePos();
        
      }

      if (hasTower && Input.RightMouseButtonClicked())
      {
        hasTower = false;

      }
      //if (Input.LeftMouseButtonClicked())
      //{
      //  CreateTower(Input.GetMousePos());
      //}
    }
    public void Draw(SpriteBatch sb)
    {

    }
  }
}
