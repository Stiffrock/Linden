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
    protected float ExplosionRadius { get; set; }
    public LExpProjectile(World world, Tower owner, Texture2D tex, Vector2 pos, string axiom, string xRule, int generations, Vector2 direction, float speed, float damage, float explosionRadius, float accuracy = 100, bool targetSeeking = false)
      : base(world, owner, tex, pos, axiom, xRule, generations, direction, speed, damage, accuracy, targetSeeking)
    {
      this.ExplosionRadius = explosionRadius;
    }
    public override void DoCollision(GameObject other)
    {
      base.DoCollision(other);
    }
    public override void Die()
    {
      ExplosionRadius *= Scale;
      for (int i = 0; i < 3; i++)
      {
        world.ParticleManager.GenerateParticle(AssetManager.GetTexture("particle01"), pos, 0.5f, 100.0f, 0.5f*Scale, color);
      }
      world.ParticleManager.CreateExplosion(pos, ExplosionRadius, color, 0.25f);
      List<GameObject> gos = world.GetGameObjects();
      foreach (var go in gos)
      {
        if(go is Enemy && Vector2.DistanceSquared(pos, go.pos) <= ExplosionRadius * ExplosionRadius)
        {
          ((Enemy)go).TakeDamage(damage / 2);
        }
      }
      base.Die();
    }
  }
}
