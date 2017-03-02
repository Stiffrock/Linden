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
    public Base baseTower;
    Tower testTower;
    List<GameObject> gameObjects;
    //List<GameObject> projectileList;

    public World()
    {
      gameObjects = new List<GameObject>();
      baseTower = new Base(this,AssetManager.GetTexture("dot"), new Vector2(800, 300));
      AddGameObject(baseTower);
      SpawnTestEnemies(5);
      testTower = new Tower(this,AssetManager.GetTexture("dot"), new Vector2(600, 500));
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
      foreach (GameObject go in gameObjects)
      {
        go.Update(gt);
        foreach (GameObject go2 in gameObjects)
        {
          if (go != go2 && go.CollidesWith(go2))
            go.DoCollision(go2);

          if (go is Tower && go != go2 && !(go is Base))
          {
            Tower t = (Tower)go;
            t.AggroCollision(go2);
          }
        }
      }

      gameObjects.RemoveAll(go => go.Disposed);
    }
    public void Draw(SpriteBatch sb)
    {
      foreach (GameObject go in gameObjects)
      {
        go.Draw(sb);
      }
    }
    public void AddGameObject(GameObject go)
    {
      gameObjects.Add(go);
    }
    public List<GameObject> GetGameObjects()
    {
      return gameObjects;
    }
  }
}
