﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lindenmayers_Defense
{
  class Base : Tower, IDamageable
  {
    Texture2D platform;
    float health;
    public Base(World world, Texture2D tex, Vector2 pos) : base(world, tex, pos)
    {
      origin = new Vector2(tex.Width / 2, tex.Height / 2);
      platform = AssetManager.GetTexture("towerplatform");
      shootCooldown = 200.0f;
      L = new LSystem("(--fff++ssFhFhFhFhFhFhFhFhFhF)(++fff--ssFhFhFhFhFhFhFhFhFhF)", "", 0.0f);
      L.Evolve(5);
      Scale = 2.0f;
      health = 1000;
      color = Color.White;
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
    }
    public void TakeDamage(float damage)
    {
      health -= damage;
      if (health <= 0)
        Die();
    }
    public override void Draw(SpriteBatch sb)
    {
      sb.Draw(platform, pos, spriteRec, color * alpha, 0.0f, origin, Scale, SpriteEffects.None, layerDepth);
      base.Draw(sb);
    }
  }
}
