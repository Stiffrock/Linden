using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lindenmayers_Defense
{
  class World
  {
    public ParticleManager ParticleManager { get; private set; }
    public Base baseTower;
    Tower testTower;
    List<GameObject> gameObjects;
    List<Projectile> projectiles;
    GUI.UserInterface UI;

    public World()
    {
      ParticleManager = new ParticleManager();
      gameObjects = new List<GameObject>();
      projectiles = new List<Projectile>();
      UI = new GUI.UserInterface();
      baseTower = new Base(this, AssetManager.GetTexture("dot"), new Vector2(800, 300));
      AddGameObject(baseTower);
      SpawnTestEnemies(5);
      testTower = new Tower(this, AssetManager.GetTexture("dot"), new Vector2(600, 500));
      AddGameObject(testTower);
    }

    private void SpawnTestEnemies(int nrToSpawn)
    {
      for (int i = 0; i < nrToSpawn; i++)
      {
        Vector2 pos = new Vector2(100, 200 + i * 100);
        Enemy spawn = new Enemy(this, AssetManager.GetTexture("pixel"), pos, 10, 50, 20);
        spawn.Scale = 100;
        AddGameObject(spawn);
      }
    }

    public void Update(GameTime gt)
    {
      UI.Update(gt);
      for (int i = 0; i < gameObjects.Count; i++)
      {
        GameObject go = gameObjects[i];
        go.Update(gt);
        foreach (GameObject go2 in gameObjects)
        {
          if (go != go2 && go.CollidesWith(go2))
            go.DoCollision(go2);
        }
      }
      for(int i = 0; i < projectiles.Count; i++)
      {
        Projectile p = projectiles[i];
        p.Update(gt);
        foreach (GameObject go in gameObjects)
        {
          if(p.CollidesWith(go))
            p.DoCollision(go);
        }
      }
      gameObjects.RemoveAll(go => go.Disposed);
      projectiles.RemoveAll(p => p.Disposed);

      ParticleManager.Update(gt);
    }
    public void Draw(SpriteBatch sb)
    {
      foreach (GameObject go in gameObjects)
      {
        go.Draw(sb);
      }
      foreach(Projectile p in projectiles)
      {
        p.Draw(sb);
      }
      ParticleManager.Draw(sb);
      UI.Draw(sb);

    }
    public void AddGameObject(GameObject go)
    {
      gameObjects.Add(go);
    }
    public List<GameObject> GetGameObjects()
    {
      return gameObjects;
    }
    public void AddProjectile(Projectile p)
    {
      projectiles.Add(p);
    }
  }
}
