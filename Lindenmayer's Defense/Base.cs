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
    float health;
    public Base(World world, Texture2D tex, Vector2 pos):base(world, tex,pos)
    {
      shootCooldown = 100000.0f;
      Scale = 0.5f;
      health = 100;
      color = Color.Black;
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
  }
}
