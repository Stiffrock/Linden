﻿using System;
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
      grammarComponents.Add("spinner left", new LComponent(AssetManager.GetTexture("rune1"), Vector2.Zero, "LL"));
      grammarComponents.Add("wave", new LComponent(AssetManager.GetTexture("rune2"), Vector2.Zero, "F[WY][VY]"));
      grammarComponents.Add("arrow", new LComponent(AssetManager.GetTexture("rune3"), Vector2.Zero, "sSF"));
      grammarComponents.Add("fork", new LComponent(AssetManager.GetTexture("rune4"), Vector2.Zero,"f[+FY][-FY]"));
      //grammarComponents.Add("fork", new LComponent(AssetManager.GetTexture("dot"), "[+FY][-FY]"));
      grammarComponents.Add("spinner right", new LComponent(AssetManager.GetTexture("rune5"), Vector2.Zero, "RR"));
      grammarComponents.Add("explosive", new LComponent(AssetManager.GetTexture("rune6"), Vector2.Zero, "eEF"));
      grammarComponents.Add("homing", new LComponent(AssetManager.GetTexture("rune7"), Vector2.Zero,"hHF"));
      grammarComponents.Add("slow", new LComponent(AssetManager.GetTexture("rune8"), Vector2.Zero, "zZF"));
    }
    public void SetTowerType(Tower t)
    {
    }
    public void CreateTower(Vector2 pos, string grammar, Texture2D[] textures)
    {
      if (!towerOnMouse)
      {
        //string g = world.GetGrammar();
        t = new Tower(world, AssetManager.GetTexture("tower01"), pos, grammar, 3);
        t.componentTextures = textures;
        //t.componentTextures = world.GetComponentTextures();
        towerOnMouse = true;
      }
    }
    public void Update(GameTime gt)
    { 
      if (t != null && towerOnMouse)
        t.pos = Input.GetMousePos();

      if (towerOnMouse && Input.RightMouseButtonClicked())
      {
        world.AddGameObject(t);
        towerOnMouse = false;
      }
    }
    public void Draw(SpriteBatch sb)
    {
      if (t != null && towerOnMouse)
        t.Draw(sb);
    }
  }
}
