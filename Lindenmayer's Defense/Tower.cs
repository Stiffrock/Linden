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
    public Enemy target;
    protected Vector2 muzzlePos;
    protected float aggroRadius;
    protected float shootCooldown;
    protected double shootTimer;
    protected World world;
    public string towerGrammar;
    public float towerDamage;

    public Tower(World world, Texture2D tex, Vector2 pos) : base(tex, pos)
    {
      this.pos = pos;
      this.world = world;
      muzzlePos = new Vector2(0, -35);
      origin += new Vector2(0, 15);
      //aggroRadius = tex.Width * 2;   // send as parameter instead?
      aggroRadius = 5000.0f;
      shootCooldown = 1000.0f;
      shootTimer = shootCooldown;
      //Scale = 0.1f;
      target = null;
      towerGrammar = "f[+++LR[X]][---RL[X]]";
      Layer = CollisionLayer.TOWER;
      LayerMask = CollisionLayer.NONE;
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

    protected virtual void ShootProjectile()
    {
      if (target == null)
        return;
      rotation = Utility.Vector2ToAngle(target.pos - pos);
      Vector2 spawnPos = pos + Vector2.Transform(muzzlePos, Matrix.CreateRotationZ(rotation));
      LProjectile p = new LProjectile(world, this, AssetManager.GetTexture("bullet01"), spawnPos, "(X)", towerGrammar, 5, this.Forward(), 150.0f, 10);
      p.color = color;
      world.AddProjectile(p);
      for (int i = 0; i < 3; i++)
        world.ParticleManager.GenerateParticle(AssetManager.GetTexture("particle01"), spawnPos+Forward()*15, 0.3f, 120, 0.5f, Color.White);
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
