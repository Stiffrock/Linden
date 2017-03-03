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
  class Tower : GameObject
  {
    protected BoundingSphere aggroRange;
    protected float Cooldown;
    protected float Radius;
    private Enemy target;
    private double timer;
    public List<GameObject> projectileList;
    protected World world;

    public Tower(World world, Texture2D tex, Vector2 pos) : base(tex, pos)
    {
      this.pos = pos;
      this.world = world;
      layer = CollisionLayer.TOWER;
      Radius = tex.Width * 2;
      Cooldown = 200.0f;
      timer = Cooldown;
      Scale = 0.1f;
      aggroRange = new BoundingSphere(new Vector3(origin.X, origin.Y, 0f), Radius);
      projectileList = new List<GameObject>();
      target = null;    

    }

    public void AggroCollision(GameObject other)
    {
      BoundingSphere temp = new BoundingSphere(new Vector3(origin.X, origin.Y, 0f), other.tex.Width);
      if (aggroRange.Intersects(temp) && other is Enemy)
      {
        if (target != null)
        {
          if (GetDistanceToTarget(target.pos) > GetDistanceToTarget(other.pos))
          {
            double x = GetDistanceToTarget(target.pos);
            target = (Enemy)other;
          }
        }
        else
        {
          target = (Enemy)other;
        }
      }
    }

    protected double GetDistanceToTarget(Vector2 target) // Borde göras static för den används i projectile atm
    {
      float x = pos.X - target.X;
      float y = pos.Y - target.Y;
      return Math.Sqrt(x * x + y * y);
    }

    private void ClearTarget()
    {
      if (target != null)
      {
        if (GetDistanceToTarget(target.pos) > Radius || target.Disposed)
        {
          target = null;
        }
      }
    }

    protected void ShootProjectile()
    {
      Projectile p = new Projectile(world, AssetManager.GetTexture("dot"), pos, new Vector2(1,1), 50.0f, 1000.0f, 10.0f);
      //LProjectile p = new LProjectile(world, AssetManager.GetTexture("dot"), pos, "X", 5);
      projectileList.Add(p);
    }

    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
      foreach (var item in projectileList)
      {
        item.Draw(sb);
      }
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      foreach (var item in projectileList)
      {
        item.Update(gt);
      }
      projectileList.RemoveAll(go => go.Disposed);

      timer += gt.ElapsedGameTime.Milliseconds;
      if (target != null && timer >= Cooldown)
      {
        ShootProjectile();
        timer = 0;
      }
      ClearTarget();
    }
  }
}
