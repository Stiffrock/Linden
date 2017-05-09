using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;


namespace Lindenmayers_Defense
{

  class Projectile : GameObject
  {
    public float TurnRate { get; set; }
    public float Speed { get; set; }
    public double Lifetime { get; set; }
    public float ExplosionRadius { get; set; }
    public Vector2 Velocity { get; set; }
    public float Damage { get; set; }
    public GameObject Target { get; set; }
    public Tower Tower { get; protected set; }
    protected bool debug;

    protected World world;

    public Projectile(World world, Tower owner, Texture2D tex, Vector2 pos, Vector2 direction, float speed, float damage,
      float explosionRadius = 0.0f, float turnRate = 0.2f, GameObject target = null)
      : base(tex, pos)
    {
      this.world = world;
      this.Tower = owner;
      rotation = Utility.Vector2ToAngle(direction);
      this.Velocity = direction * speed;
      this.Speed = speed;
      this.Damage = damage;
      this.TurnRate = turnRate;
      this.Target = target != null ? target : owner.target;
      this.ExplosionRadius = explosionRadius;
      color = Color.Purple;
      Lifetime = 15.0f;
      Layer = CollisionLayer.PROJECTILE;
      LayerMask = CollisionLayer.ENEMY;
    }
    public Projectile(Projectile other) : base(other)
    {
      TurnRate = other.TurnRate;
      Speed = other.Speed;
      Lifetime = other.Lifetime;
      ExplosionRadius = other.ExplosionRadius;
      Velocity = other.Velocity;
      Damage = other.Damage;
      Target = other.Target;
      Tower = other.Tower;
      world = other.world;
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      if (Target == null || Target.Disposed)
        AcquireTarget();
      Velocity = Forward() * Speed;

      world.ParticleManager.GenerateParticle(tex, pos, 0.2f, 0.0f, 0.5f * Scale, color * 0.5f);
      Lifetime -= (float)gt.ElapsedGameTime.TotalSeconds;
      if (Lifetime < 0.5f)
      {
        alpha = (float)(Lifetime / 0.5f);
      }
      if (Lifetime <= 0)
      {
        Die();
      }
    }
    public virtual void DebugUpdate(GameTime gt)
    {
      Velocity = Forward() * Speed;
      Lifetime -= (float)gt.ElapsedGameTime.TotalSeconds;
      if (Lifetime <= 0)
        Die();
    }
    public override void DoCollision(GameObject other)
    {
      if (other is Enemy)
      {
        Enemy enemy = (Enemy)other;
        enemy.TakeDamage(Damage);
        Die();
      }
    }
    protected void AcquireTarget()
    {
      List<GameObject> gameObjects = world.GetGameObjects();
      float closest = 99999;
      foreach (GameObject go in gameObjects)
      {
        if (go is Enemy && !go.Disposed)
        {
          float distance = Vector2.Distance(go.pos, pos);
          if (distance <= closest)
          {
            Target = (Enemy)go;
            closest = distance;
          }
        }
      }
    }
    protected void Explode()
    {
      ExplosionRadius *= Scale;
      for (int i = 0; i < 3; i++)
      {
        world.ParticleManager.GenerateParticle(AssetManager.GetTexture("particle01"), pos, 0.5f, 100.0f, 0.5f * Scale, color);
      }
      world.ParticleManager.CreateExplosion(pos, ExplosionRadius, color, 0.25f);
      List<GameObject> gos = world.GetGameObjects();
      foreach (var go in gos)
      {
        if (go is Enemy && Vector2.DistanceSquared(pos, go.pos) <= ExplosionRadius * ExplosionRadius)
        {
          ((Enemy)go).TakeDamage(Damage / 4);
        }
      }
    }
    public override void Die()
    {
      if (ExplosionRadius > 0.1f)
        Explode();
      base.Die();
    }
  }
}
