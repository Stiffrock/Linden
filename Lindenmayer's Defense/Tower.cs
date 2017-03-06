﻿using System.Collections.Generic;
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
    protected float aggroRadius;
    protected float shootCooldown;
    private double shootTimer;
    private Enemy target;
    protected World world;

    public Tower(World world, Texture2D tex, Vector2 pos) : base(tex, pos)
    {
      this.pos = pos;
      this.world = world;
      aggroRadius = tex.Width * 2;   // send as parameter instead?
      shootCooldown = 200.0f;
      shootTimer = shootCooldown;
      Scale = 0.1f;
      target = null;
      layer = CollisionLayer.TOWER;
    }

    /// <summary>
    /// Sets the target. Target is null if no valid targets can be found.
    /// </summary>
    protected virtual void AcquireTarget()
    {
      List<GameObject> gameObjects = world.GetGameObjects();
      target = null;
      float closest = aggroRadius;
      foreach (GameObject go in gameObjects)
      {
        if(go is Enemy && !go.Disposed)
        {
          float distance = Vector2.Distance(go.pos, pos);
          if (distance <= closest)
          {
            target = (Enemy)go;
            closest = distance;
          }
        }
      }
    }

    protected void ShootProjectile()
    {
     // Projectile p = new Projectile(world, AssetManager.GetTexture("dot"), pos, new Vector2(1,1), 50.0f, 1000.0f, 10.0f);
      LProjectile p = new LProjectile(world, AssetManager.GetTexture("dot"), pos, "X", 5);
      world.AddProjectile(p);
    }

    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);

      AcquireTarget();

      shootTimer += gt.ElapsedGameTime.Milliseconds;
      if (target != null && shootTimer >= shootCooldown)
      {
        ShootProjectile();
        shootTimer = 0;
      }
    }
  }
}
