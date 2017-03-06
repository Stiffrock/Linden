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
    protected int MaxAccuracyOffset;

    public Projectile(World world, Texture2D tex, Vector2 pos, Vector2 velocity, float accuracy, float speed, float damage) : base(tex, pos)
    {
      this.pos = pos;
      this.world = world;
      this.Accuracy = accuracy;
      this.Velocity = velocity;
      this.Speed = speed;
      this.Damage = damage;
      MaxAccuracyOffset = 200;
      Lifetime = 3.0f;
      Radius = tex.Width * 2;
      hitbox.Width -= tex.Width / 2;
      hitbox.Height -= tex.Height / 2;
      aggroRange = new BoundingSphere(new Vector3(origin.X, origin.Y, 0f), Radius);
      drawHitbox = false;
      Seeking = false;
      Scale = 0.05f;
      CalculateAccuracy();
    }

    /// <summary>
    /// Based on max accuracy being 100% meaning correct projectile target. Using projectiles accuracy as base for how close to the target it will land.
    /// example: 64% meaning get its target position with a 34% offset
    /// </summary>
    protected void CalculateAccuracy()
    {
      rand = new Random();
      int Negative = rand.Next(0, 2);
      float acc = rand.Next((int)75, 100) - 100;
      
      if (Negative == 1)
        acc *= -1.0f;

      //target = Vector2.Transform(target, Matrix.CreateRotationZ(acc * (float)(Math.PI / 180)));
      rotation = acc * (float)(Math.PI / 180);
    }

    private void CheckForCollision()
    {
      foreach (GameObject e in world.GetGameObjects())
      {
        if (e is Enemy)
          if (hitbox.Intersects(e.hitbox))
            if (CollidesWith(e))
            {
              Enemy enemy = (Enemy)e;
              enemy.TakeDamage(Damage);
              Die();
              DoCollision(enemy);
            }
      }
    }

    public override bool CollidesWith(GameObject other)
    {
      if (other.hitbox.Intersects(hitbox))
        return true;
      return false;
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

    private void MoveTo(GameTime gt)
    {
      if (pos != targetPos)
      {
        Vector2 dir = targetPos - this.pos;
        dir.Normalize();
        this.pos += dir * Speed * (float)gt.ElapsedGameTime.TotalSeconds;
      }

      if (GetDistanceToTarget(targetPos) <= 20)
        Die();
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
