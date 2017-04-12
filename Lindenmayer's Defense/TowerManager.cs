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
    private int cost;

    public TowerManager(World world)
    {
      this.world = world;
      t = null;
      cost = 0;
      grammarComponents = new Dictionary<string, LComponent>();
      InitGrammarComponents();
    }

    public Dictionary<string, LComponent> GetGrammarComponents()
    {
      return grammarComponents;
    }
    private void InitGrammarComponents()
    {
      grammarComponents.Add("spinner left", new LComponent(AssetManager.GetTexture("rune1"), Vector2.Zero, "LL"));
      grammarComponents.Add("wave", new LComponent(AssetManager.GetTexture("rune2"), Vector2.Zero, "W"));
      //grammarComponents.Add("wave", new LComponent(AssetManager.GetTexture("rune2"), Vector2.Zero, "[WY][VY]"));
      grammarComponents.Add("arrow", new LComponent(AssetManager.GetTexture("rune3"), Vector2.Zero, "sSFF"));
      grammarComponents.Add("fork", new LComponent(AssetManager.GetTexture("rune4"), Vector2.Zero,"fK0FF"));
      grammarComponents.Add("spinner right", new LComponent(AssetManager.GetTexture("rune5"), Vector2.Zero, "RR"));
      grammarComponents.Add("explosive", new LComponent(AssetManager.GetTexture("rune6"), Vector2.Zero, "eEF"));
      grammarComponents.Add("homing", new LComponent(AssetManager.GetTexture("rune7"), Vector2.Zero,"hHF"));
      grammarComponents.Add("slow", new LComponent(AssetManager.GetTexture("rune8"), Vector2.Zero, "zZF"));
      grammarComponents.Add("volley", new LComponent(AssetManager.GetTexture("rune9"), Vector2.Zero, "fQ0F"));
    }
    public void SetTowerType(Tower t)
    {
    }
    public bool CreateTower(Vector2 pos, string grammar, Texture2D[] textures, int cost)
    {
      if (!towerOnMouse && world.ResourceManager.CanAfford(cost))
      {
        //string g = world.GetGrammar();
        t = new Tower(world, AssetManager.GetTexture("tower01"), pos, grammar, 3);
        t.componentTextures = textures;
        towerOnMouse = true;
        return true;
      }
      return false;
    }
    public void PlaceTower(Vector2 pos)
    {
      if (!towerOnMouse)
        return;
      t.pos = pos;
      world.AddGameObject(t);
      towerOnMouse = false;
    }
    public void Update(GameTime gt)
    { 
      if (t != null && towerOnMouse)
        t.pos = Input.GetMousePos();

      if (towerOnMouse && Input.RightMouseButtonClicked())
      {
        PlaceTower(t.pos);
      }
    }
    public void Draw(SpriteBatch sb)
    {
      if (t != null && towerOnMouse)
        t.Draw(sb);
    }
  }
}
