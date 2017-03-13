﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lindenmayers_Defense
{
  class EnemyManager
  {
    World world;
    double spawnElapsed;
    double spawnNext;
    double spawnMin;
    double spawnMax;

    public EnemyManager(World world)
    {
      this.world = world;
      spawnMin = 0.5f;
      spawnMax = 2.0f;
    }
    public void Update(GameTime gt)
    {
      spawnElapsed += gt.ElapsedGameTime.TotalSeconds;
      if (spawnElapsed >= spawnNext)
      {
        SpawnEnemy();
        spawnElapsed = 0;
        spawnNext = Game1.rnd.NextDouble() * (spawnMax - spawnMin) + spawnMin;
      }
    }
    private void SpawnEnemy()
    {
      float X, Y;
      if(Game1.rnd.NextDouble() < Game1.ScreenWidth / Game1.ScreenHeight)
      {
        X = Game1.rnd.Next(-150, 150);
        if (X >= -50)
          X += Game1.ScreenWidth + 50;
        Y = Game1.rnd.Next(-150, Game1.ScreenWidth+150);
      }
      else
      {
        Y = Game1.rnd.Next(-150, 150);
        if (Y >= -50)
          Y += Game1.ScreenHeight + 50;
        X = Game1.rnd.Next(-150, Game1.ScreenWidth + 150);
      }
      Vector2 spawnPos = new Vector2(X, Y);

      int stats = 350;
      int speed = Game1.rnd.Next(10, 100);
      stats -= speed * 2;
      int damage = Game1.rnd.Next(10, (stats - 20) / 3);
      stats -= damage * 3;
      int health = stats;

      Enemy e = new Enemy(world, AssetManager.GetTexture("enemy01"), spawnPos, damage, health, speed);
      world.AddGameObject(e);
    }
  }
}
