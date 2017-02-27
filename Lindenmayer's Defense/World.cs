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
    List<GameObject> gameObjects;
    public World()
    {
      gameObjects = new List<GameObject>();
      SpawnTestEnemies(10);
    }


    private void SpawnTestEnemies(int nrToSpawn)
    {
      for (int i = 0; i < nrToSpawn; i++)
      {
        Vector2 pos = new Vector2(100 + i * 10, 500);
        Enemy spawn = new Enemy(AssetManager.GetTexture("dot"), pos);
        AddGameObject(spawn);
      }
    }
    
    public void Update(GameTime gt)
    {
      foreach(GameObject go in gameObjects)
      {
        go.Update(gt);
      }
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
