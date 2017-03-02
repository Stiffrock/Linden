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
    protected Vector2 Velocity { get; set; }
    protected float Accuracy { get; set; }
    protected Vector2 TurnRate { get; set; }
    protected float Speed { get; set; }

    protected float Damage { get; set; }

    protected bool Seeking { get; set; }
    protected double Lifetime { get; set; }


    protected Vector2 targetPos;
    protected BoundingSphere aggroRange;
    protected float Radius;
    protected Random rand;
    protected World world;

    public Projectile(World world, Texture2D tex, Vector2 pos, Vector2 velocity, float accuracy, float speed, float damage) : base(tex, pos)

    {
      this.pos = pos;
      this.world = world;
      this.Accuracy = accuracy;
      this.Velocity = velocity;
      this.Speed = speed;
      this.Damage = damage;
      Lifetime = 3.0f;
      Radius = tex.Width * 2;
      hitbox.Width -= tex.Width / 2;
      hitbox.Height -= tex.Height / 2;
      aggroRange = new BoundingSphere(new Vector3(origin.X, origin.Y, 0f), Radius);
      drawHitbox = false;
      Seeking = false;
      Scale = 0.05f;
      targetPos = SetTargetPos();
    }

    private Vector2 SetTargetPos()
    {
      Vector2 newTarget = Vector2.Zero;
      bool first = true;

      foreach (GameObject e in world.GetGameObjects())
      {
        if (e is Enemy)
        {
          BoundingSphere temp = new BoundingSphere(new Vector3(origin.X, origin.Y, 0f), e.tex.Width);

          if (temp.Intersects(aggroRange))
          {
            if (first)
            {
              newTarget = e.pos;
            }
            else
            {
              if (GetDistanceToTarget(e.pos) < GetDistanceToTarget(newTarget))
              {
                newTarget = e.pos;
              }
            }
          }
        }
      }

      Vector2 Offset = CalculateAccuracy(newTarget);
      return Offset;
    }

    /// <summary>
    /// Based on max accuracy being 100% meaning correct projectile target. Using projectiles accuracy as base for how close to the target it will land.
    /// example: 64% meaning get its target position with a 34% offset
    /// </summary>
    private Vector2 CalculateAccuracy(Vector2 target)
    {
      rand = new Random();
      int Negative = rand.Next(0, 2);

      float acc = rand.Next((int)Accuracy, 100);
      float accPercent = (acc / 100);
      double magnitude = GetDistanceToTarget(target);
      double offMagnitude = magnitude * (1 - accPercent);

      float x = 0, y = 0;
      if (Negative == 0)
      {
        x = (float)(target.X + Math.Cos(90.0) * offMagnitude);
        y = (float)(target.Y + Math.Sin(90.0) * offMagnitude);
      }
      if (Negative == 1)
      {
        x = (float)(target.X + Math.Cos(-90.0) * offMagnitude);
        y = (float)(target.Y + Math.Sin(-90.0) * offMagnitude);
      }


      return new Vector2(x, y);
    }


    private void CheckForCollision()
    {
      foreach (GameObject e in world.GetGameObjects())
      {
        if (e is Enemy)
        {
          if (hitbox.Intersects(e.hitbox))
            if (CollidesWith(e))
            {
              Enemy enemy = (Enemy)e;
              enemy.TakeDamage(Damage);
              Die();
              DoCollision(e);
            }
        }

      }
    }

    public override bool CollidesWith(GameObject other)
    {
      if (other.hitbox.Intersects(hitbox))
      {
        return true;
      }
      return false;
    }

    public override void DoCollision(GameObject other)
    {
      Enemy enemy = (Enemy)other;
      enemy.TakeDamage(Damage);
      Die();
    }

    private void MoveTo(GameTime gt)
    {
      if (pos != targetPos)
      {
        Vector2 dir = targetPos - this.pos;
        dir.Normalize();
        this.pos += dir * Speed * (float)gt.ElapsedGameTime.TotalSeconds;
      }

      if (GetDistanceToTarget(targetPos) <= 20)
      {
        Die();
      }
    }
    protected double GetDistanceToTarget(Vector2 target)
    {
      float x = pos.X - target.X;
      float y = pos.Y - target.Y;
      return Math.Sqrt(x * x + y * y);
    }

    public override void Update(GameTime gt)
    {
      base.Update(gt);
      Lifetime -= gt.ElapsedGameTime.Seconds;
      if (Lifetime <= 0)
      {
        Die();
      }
      MoveTo(gt);
      CheckForCollision();

    }
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
    }
  }
}
