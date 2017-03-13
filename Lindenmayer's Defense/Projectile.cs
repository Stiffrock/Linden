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
    public Vector2 Velocity { get; set; }
    /// <summary>
    /// 0 = 45 degree spread in either direction, 100 = 0 degree spread
    /// </summary>
    protected float Accuracy { get; set; }
    protected Vector2 TurnRate { get; set; }
    protected float Speed { get; set; }
    protected float Damage { get; set; }
    protected bool bIsTargetSeeking { get; set; }
    protected double Lifetime { get; set; }
    protected float aggroRadius;
    protected World world;
    public Tower owner;
    public float damage;


    public Projectile(World world, Tower owner, Texture2D tex, Vector2 pos, Vector2 direction, float speed, float damage, float accuracy = 100, bool targetSeeking = false) : base(tex, pos)
    {
      this.pos = pos;
      this.world = world;
      this.Accuracy = accuracy;
      this.Velocity = direction * speed;
      rotation = Utility.Vector2ToAngle(direction);
      this.Speed = speed;
      this.Damage = damage;
      this.bIsTargetSeeking = targetSeeking;
      this.owner = owner;
      damage = owner.towerDamage;
      color = Color.Purple;
      Lifetime = 15.0f;
      aggroRadius = tex.Width * 2;
      //Scale = 0.04f;
      Accuracy = 100;
      CalculateAccuracy();
      Layer = CollisionLayer.PROJECTILE;
      LayerMask = CollisionLayer.ENEMY;
    }

    /// <summary>
    /// </summary>
    protected void CalculateAccuracy()
    {
      if (Accuracy >= 100)
        return;
      float degrees = 45 - (Accuracy / 100 * 45);
      float spread = (float)(Game1.rnd.NextDouble() * degrees * 2) - degrees;
      //int Negative = rand.Next(0, 2);
      //float acc = rand.Next(85, 100) - 100;

      //if (Negative == 1)
      //  acc *= -1.0f;
      rotation = spread * (float)(Math.PI / 180);
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

    protected void SkewVelocity(Vector2 vel, GameTime gt)
    {
      Velocity += vel * (float)gt.ElapsedGameTime.TotalSeconds;
    }

    protected void SkewDirection(float angle, GameTime gt)
    {
      rotation += angle * (float)(Math.PI / 180) * (float)gt.ElapsedGameTime.TotalSeconds;
    }

    public override void Update(GameTime gt)
    {
      base.Update(gt);
      Velocity = Forward() * Speed;
      //SkewVelocity(new Vector2(0.0f, 0.0f), gt);
      //SkewDirection(-45.0f, gt);

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
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
    }
  }
}
