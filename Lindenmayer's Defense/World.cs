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
    }
    public void Update(GameTime gt)
    {

    }
    public void Draw(SpriteBatch sb)
    {

    }
    public void PostGameObject(GameObject go)
    {
      gameObjects.Add(go);
    }
    public List<GameObject> GetGameObjects()
    {
      return gameObjects;
    }
  }
}
