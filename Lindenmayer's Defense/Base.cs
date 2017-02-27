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
    public Base(Texture2D tex, Vector2 pos):base(tex,pos)
    {
      health = 100;
    }
    public void TakeDamage(float damage)
    {
      health -= damage;
      if (health <= 0)
        Die();
    }
  }
}
