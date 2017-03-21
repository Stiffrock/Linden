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
    private Dictionary<string, LComponent> grammarComponents;    
    private bool towerOnMouse;

    public TowerManager(World world)
    {
      this.world = world;
      t = null;
      grammarComponents = new Dictionary<string, LComponent>();
      InitGrammarComponents();
    }

    public Dictionary<string, LComponent> GetGrammarComponents()
    {
      return grammarComponents;
    }
    private void InitGrammarComponents()
    {
      grammarComponents.Add("spinner left", new LComponent(AssetManager.GetTexture("dot"), "LL"));
      grammarComponents.Add("wave", new LComponent(AssetManager.GetTexture("dot"), "W"));
      grammarComponents.Add("arrow", new LComponent(AssetManager.GetTexture("dot"), "SF"));
      grammarComponents.Add("fork", new LComponent(AssetManager.GetTexture("dot"), "[+fY][-fY]F"));
      //grammarComponents.Add("fork", new LComponent(AssetManager.GetTexture("dot"), "[+FY][-FY]"));
      grammarComponents.Add("spinner right", new LComponent(AssetManager.GetTexture("dot"), "RR"));
      grammarComponents.Add("explosive", new LComponent(AssetManager.GetTexture("dot"), "EF"));
      grammarComponents.Add("homing", new LComponent(AssetManager.GetTexture("dot"), "HF"));
      grammarComponents.Add("slow", new LComponent(AssetManager.GetTexture("dot"), "ZF"));
    }
    public void SetTowerType(Tower t)
    {
    }
    public void CreateTower(Vector2 pos)
    {
      if (!towerOnMouse)
      {
        string g = world.GetGrammar();
        t = new Tower(world, AssetManager.GetTexture("tower01"), pos, g, 5);
        towerOnMouse = true;
        world.AddGameObject(t);
      }
    }
    public void Update(GameTime gt)
    { 
      if (t != null && towerOnMouse)
        t.pos = Input.GetMousePos();        

      if (towerOnMouse && Input.RightMouseButtonClicked())
        towerOnMouse = false;
    }
    public void Draw(SpriteBatch sb)
    {
    }
  }
}
