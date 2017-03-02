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
    protected Vector2 Accuracy { get; set; }
    protected Vector2 TurnRate { get; set; }
    protected float Speed { get; set; }
    protected float Damage { get; set; }
    protected bool Seeking{ get; set; }
    protected double Lifetime{ get; set; }

    protected Vector2 targetPos;
    protected BoundingSphere aggroRange;
    protected float Radius;
    protected Random rand;
    protected World world;



    public Projectile(World world, Texture2D tex, Vector2 pos, Vector2 velocity, Vector2 accuracy, float speed, float damage) : base(tex, pos)
    {
      this.pos = pos;
      this.world = world;
      this.Accuracy = accuracy;
      this.Velocity = velocity;
      this.Speed = speed;
      this.Damage = damage;
      Radius = tex.Width * 2;
      hitbox.Width -= tex.Width / 2;
      hitbox.Height -= tex.Height / 2;
      aggroRange = new BoundingSphere(new Vector3(origin.X, origin.Y, 0f), Radius);
      drawHitbox = false;
      Seeking = false;   
      Scale = 0.1f;
      targetPos = SetTargetPos();
    }     

    private Vector2 SetTargetPos()
    {
      Vector2 newTarget = Vector2.Zero;
      bool first = true;

      //100 max
      rand = new Random();
      Vector2 Acc = new Vector2(rand.Next((int)Accuracy.X, 100), rand.Next((int)Accuracy.Y, 100));
      Acc.Normalize();

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
      Vector2 newTargetPos = new Vector2(newTarget.X * Acc.X, newTarget.Y * Acc.Y);
      return newTargetPos;
    }

    private void DoCollision()
    {
      foreach (GameObject e in world.GetGameObjects())
      {
        if (e is Enemy)
        {
          if (hitbox.Intersects(e.hitbox))
          {
            Enemy enemy = (Enemy)e;
            enemy.TakeDamage(Damage);
            Die();
          }
        }
   
      }
    }

    private void MoveTo(GameTime gt)
    {
     
    //  Vector2 newTargetPos;
      //if (!Seeking)
      //{
      //  newTargetPos = new Vector2(targetPos.X + Accuracy.X, targetPos.Y + Accuracy.Y);
      //}
      //else
      //{
      //  newTargetPos = new Vector2(targetPos.X, targetPos.Y);
      //}
      if (pos != targetPos)
      {
        Vector2 dir = targetPos - this.pos;
        dir.Normalize();
        this.pos += dir * Speed * (float)gt.ElapsedGameTime.TotalSeconds;
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

      MoveTo(gt);
      DoCollision();    
      }
      public override void Draw(SpriteBatch sb)
      {
          base.Draw(sb);
      }
}


//  class Projectile : GameObject
//  {
//    Vector2 targetPos;
//    public Projectile(Texture2D tex, Vector2 pos, Vector2 targetPos) : base(tex, pos)
//    {
//      this.pos = pos;
//      this.targetPos = targetPos;
//      drawHitbox = false;
//      Scale = 0.05f;
//    }

//    private void MoveTo(Vector2 target, GameTime gt)
//    {
//      Vector2 termPos = new Vector2(target.X - tex.Width, target.Y - tex.Height);
//      if (pos != termPos)
//      {
//        Vector2 direction = target - this.pos;
//        direction.Normalize();
//        this.pos += direction * 1000 * (float)gt.ElapsedGameTime.TotalSeconds;
//      }
//      if (GetDistanceToTarget(target) < 20)
//      {
//        Die();
//        Disposed = true;
//      }
//    }
//    protected double GetDistanceToTarget(Vector2 target)
//    {
//      float x = pos.X - target.X;
//      float y = pos.Y - target.Y;
//      return Math.Sqrt(x * x + y * y);
//    }
//    public override void Update(GameTime gt)
//    {
//      MoveTo(targetPos, gt);

//    }
//    public override void Draw(SpriteBatch sb)
//    {
//      base.Draw(sb);
//    }
//  }
//>>>>>>> a66eff422afd3d57ce2d885e02275c470251dd30
}
