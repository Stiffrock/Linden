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

    public TowerManager(World world)
    {
      this.world = world;
    }
    public void SetTowerType(Tower t)
    {
    }
    void CreateTower(Vector2 pos)
    {
      Tower t = new Tower(AssetManager.GetTexture("dot"), pos);
      t.Scale = 0.1f;
      world.AddGameObject(t);
    }
    public void Update(GameTime gt)
    {
      if(Input.LeftMouseButtonClicked())
      {
        CreateTower(Input.GetMousePos());
      }
    }
    public void Draw(SpriteBatch sb)
    {

    }
  }
}
