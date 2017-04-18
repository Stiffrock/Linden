using System;
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
    ValueBar healthBar;
    public Base(World world, Texture2D tex, Vector2 pos) : base(world, tex, pos)
    {
      origin = new Vector2(tex.Width / 2, tex.Height / 2);
      platform = AssetManager.GetTexture("towerplatform");
      shootCooldown = 1600.0f;
      L = new LSystem("(--fff++ssssssssFFFFFFFFFF)(++fff--ssssssssFFFFFFFFFF)", "", 0.0f);
      L.Evolve(5);
      size = 2.0f;
      Scale = 2.0f;
      fireRate = 0.625f;
      damage = 20;
      health = 1500;
      healthBar = new ValueBar(new Rectangle(0, 0, 150, 10), health, health, Color.Green*0.7f, Color.Red * 0.5f);
      healthBar.SetPos(pos+new Vector2(0,70));
      color = Color.White;
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
    }
    public void TakeDamage(float damage)
    {
      health -= damage;
      healthBar.Value = health;
      if (health <= 0)
        Die();
    }
    public override void Draw(SpriteBatch sb)
    {
      sb.Draw(platform, pos, spriteRec, color * alpha, 0.0f, origin, Scale, SpriteEffects.None, layerDepth);
      base.Draw(sb);
      healthBar.Draw(sb);
    }
  }
}
