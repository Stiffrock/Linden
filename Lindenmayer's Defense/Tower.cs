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
    static int TowerIDCounter = 0;

    public int TowerID { get; private set; }
    public LSystem L { get; protected set; }

    public Enemy target;
    protected Vector2 muzzlePos;
    protected float aggroRadius;
    protected float shootCooldown;
    protected double shootTimer;
    protected World world;
    public Texture2D[] componentTextures;
    public float damage, fireRate, turnSpeed, speed, size, health;
    public int damageLvl, fireRateLvl, turnSpeedLvl, speedLvl, sizeLvl, healthLvl, generationLvl;
    private int damageCost, firerateCost, turnspeedCost, speedCost, sizeCost, healthCost, generationCost;
    private float damageFactor, firerateFactor, turnspeedFactor, speedFactor, sizeFactor, healthFactor;
    private int generations, costFactor;
    public bool displayStats;

    public Tower(World world, Texture2D tex, Vector2 pos, string grammar = "f[+++LR[X]][---RL[X]]", int generations = 3) : base(tex, pos)
    {
      TowerID = TowerIDCounter++;
      this.pos = pos;
      this.world = world;
      L = new LSystem("X", grammar);
      L.Evolve(generations);
      muzzlePos = new Vector2(0, -35);
      costFactor = 10;
      origin += new Vector2(0, 15);
      aggroRadius = 5000.0f;
      shootCooldown = 100.0f;
      shootTimer = shootCooldown;
      componentTextures = new Texture2D[4];
      target = null;
      Layer = CollisionLayer.TOWER;
      LayerMask = CollisionLayer.NONE;
      color = new Color(Game1.rnd.Next(255), Game1.rnd.Next(255), Game1.rnd.Next(255));
      damageLvl = fireRateLvl = turnSpeedLvl = speedLvl = sizeLvl = healthLvl = generationLvl = 1;
      generationLvl = generations = L.Generations;
      InitStats();
    }
    //private float damage, firerate, turnspeed, speed, size, health, generations;

    public int GetDamageCost() { return damageLvl * costFactor; }
    public int GetFireRateCost() { return fireRateLvl * costFactor; }
    public int GetTurnSpeedCost() { return turnSpeedLvl * costFactor; }
    public int GetSpeedCost() { return speedLvl * costFactor; }
    public int GetSizeCost() { return sizeLvl * costFactor; }
    public int GetHealthCost() { return healthLvl * costFactor; }
    public int GetGenerationCost() { return generationLvl * (costFactor * 2); }

    private void InitStats()
    {
      damage = 10.0f;
      damageFactor = 1.2f;

      fireRate = 5.0f;
      firerateFactor = 1.2f;
      shootCooldown = 1000 / fireRate;

      turnSpeed = (float)Math.PI;
      turnspeedFactor = 1.2f;

      speed = 150.0f;
      speedFactor = 1.2f;

      size = 1.0f;
      sizeFactor = 1.1f;

      health = 100.0f;
      healthFactor = 1.2f;
    }


    public void IncreaseLevel_Damage(int x)
    {
      damageLvl += x;
      damage = damage * damageFactor;
    }

    public void IncreaseLevel_Firerate(int x)
    {
      fireRateLvl += x;
      fireRate = fireRate * firerateFactor;
      shootCooldown = 1000 / fireRate;
    }
    public void IncreaseLevel_TurnSpeed(int x)
    {
      turnSpeedLvl += x;
      turnSpeed = turnSpeed * turnspeedFactor;
    }
    public void IncreaseLevel_Speed(int x)
    {
      speedLvl += x;
      speed = speed * speedFactor;

    }
    public void IncreaseLevel_Size(int x)
    {
      sizeLvl += x;
      size = (float)Math.Pow(sizeFactor, sizeLvl);
    }
    public void IncreaseLevel_Health(int x)
    {
      healthLvl += x;
      health = health * healthFactor;
    }

    public void IncreaseLevel_Generations(int x)
    {
      L.Evolve(x);
      generationLvl = generations = L.Generations;
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
        if (go is Enemy && !go.Disposed)
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
      Vector2 spawnPos = pos + Vector2.Transform(muzzlePos, Matrix.CreateRotationZ(rotation));
      LProjectile p = new LProjectile(world, this, AssetManager.GetTexture("bullet01"), spawnPos, L.Str, this.Forward(), speed, damage);
      p.Scale = size*0.7f;
      p.color = color;
      world.AddProjectile(p);
      for (int i = 0; i < 3; i++)
        world.ParticleManager.GenerateParticle(AssetManager.GetTexture("particle01"), spawnPos + Forward() * 15, 0.3f, 120, 0.5f, Color.White);
    }

    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      if (hitbox.Contains(Input.GetMousePoint()) && Input.KeyPressed(Keys.Delete))
        this.Die();

      AcquireTarget();
      if (target != null)
      {
        float targetAngle = Utility.Vector2ToAngle(target.pos - pos);
        rotation += Utility.TurnAngle(rotation, targetAngle, turnSpeed, gt);
      }
      else
      {
        rotation += Utility.TurnAngle(rotation, 0, (float)Math.PI, gt);
      }
      shootTimer += gt.ElapsedGameTime.Milliseconds;
      if (shootTimer >= shootCooldown)
      {
        ShootProjectile();
        shootTimer = 0;
      }
    }
  }
}
