using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lindenmayers_Defense
{
  class EnemyManager
  {
    public bool IsActive { get; set; }
    World world;
    double spawnElapsed;
    double spawnNext;
    double spawnMin;
    double spawnMax;
    double baseSpawnRate;
    double difficulty;

    public EnemyManager(World world)
    {
      this.world = world;
      difficulty = 1.0;
      baseSpawnRate = 5.0f;
      spawnMin = 3.0;
      spawnMax = 5.0;
    }
    public void Update(GameTime gt)
    {
      if (!IsActive)
        return;
      spawnMax = baseSpawnRate * (1.0 / difficulty);
      spawnMin = spawnMax / 2.0;
      difficulty += gt.ElapsedGameTime.TotalSeconds / 100;
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
      if (Game1.rnd.NextDouble() < (double)Game1.ScreenHeight / (double)Game1.ScreenWidth)
      {
        X = Game1.rnd.Next(-200, 100);
        if (X >= -50)
          X += Game1.ScreenWidth + 50;
        Y = Game1.rnd.Next(-150, Game1.ScreenWidth + 150);
      }
      else
      {
        Y = Game1.rnd.Next(-200, 100);
        if (Y >= -50)
          Y += Game1.ScreenHeight + 50;
        X = Game1.rnd.Next(-150, Game1.ScreenWidth + 150);
      }
      Vector2 spawnPos = new Vector2(X, Y);

      int stats = (int)(500 * difficulty);//350;
      int speed = Game1.rnd.Next(30, 100);
      stats -= speed * 2;
      int damage = Game1.rnd.Next(10, (stats - 20) / 4);
      stats -= damage * 4;
      int health = stats;

      Enemy e = new Enemy(world, AssetManager.GetTexture("enemy01"), spawnPos, damage, health, speed);
      world.AddGameObject(e);
    }
  }
}
