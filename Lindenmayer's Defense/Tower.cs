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
    private Enemy target;
    protected Vector2 muzzlePos;
    protected float aggroRadius;
    protected float shootCooldown;
    private double shootTimer;
    protected World world;
    public string towerGrammar;

    public Tower(World world, Texture2D tex, Vector2 pos) : base(tex, pos)
    {
      this.pos = pos;
      this.world = world;
      muzzlePos = new Vector2(0, -35);
      origin += new Vector2(0, 15);
      aggroRadius = tex.Width * 2;   // send as parameter instead?
      shootCooldown = 200.0f;
      shootTimer = shootCooldown;
      //Scale = 0.1f;
      target = null;
      towerGrammar = "f[+++LR[X]][---RL[X]]";
      layer = CollisionLayer.TOWER;
      layerMask = CollisionLayer.NONE;
      color = Color.BlueViolet;
      color = new Color(Game1.rnd.Next(255), Game1.rnd.Next(255), Game1.rnd.Next(255));
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
      rotation += 0.5f;
      Vector2 spawnPos = pos + Vector2.Transform(muzzlePos, Matrix.CreateRotationZ(rotation));
      LProjectile p = new LProjectile(world, this, AssetManager.GetTexture("bullet01"), spawnPos, "X", towerGrammar, 5, this.Forward(), 150.0f, 10.0f);
      p.color = color;
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
      if (/*target != null &&*/ shootTimer >= shootCooldown)
      {
        ShootProjectile();
        shootTimer = 0;
      }
    }
  }
}
