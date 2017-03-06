using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lindenmayers_Defense
{
  class LExpProjectile : LProjectile
  {
    public LExpProjectile(World world, Tower owner, Texture2D tex, Vector2 pos, string axiom, int generations, Vector2 direction, float speed, float damage, float accuracy = 100, bool targetSeeking = false)
      : base(world, owner, tex, pos, axiom, generations, direction, speed, damage, accuracy, targetSeeking)
    {
    }
    public override void DoCollision(GameObject other)
    {
      base.DoCollision(other);
      world.ParticleManager.CreateExplosion(pos, 50, color, 250.0f);
    }
    public override void Die()
    {
      base.Die();
    }
  }
}
