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
    public TowerManager TowerManager { get; private set; }
    EnemyManager enemyManager;
    List<GameObject> gameObjects;
    List<Projectile> projectiles;

    Dictionary<string, LComponent> grammarComponents;
    public Base baseTower;
    Tower testTower;

    public World()
    {
      ParticleManager = new ParticleManager();
      enemyManager = new EnemyManager(this);
      TowerManager = new TowerManager(this);
      gameObjects = new List<GameObject>();
      projectiles = new List<Projectile>();
      grammarComponents = new Dictionary<string, LComponent>();
      baseTower = new Base(this, AssetManager.GetTexture("tower02"), new Vector2(800, 300));
      AddGameObject(baseTower);
      testTower = new Tower(this, AssetManager.GetTexture("tower01"), new Vector2(600, 500));
      AddGameObject(testTower);
    }

    //public string GetGrammar()
    //{
    //  return UI.GetResult();
    //}
    //public Texture2D[] GetComponentTextures()
    //{
    //  return UI.GetTextures();
    //}
    //public void Update(GameTime gt)
    //{
    //  UI.Update(gt);
    //  if (UI.GetTowerCreator().rec.Contains(Input.GetMousePoint()) && Input.LeftMouseButtonClicked())
    //  {
    //    TowerManager.CreateTower(Input.GetMousePos());
    //  }

    public void Update(GameTime gt)
    {
      if (Input.KeyPressed(Keys.Space))
        enemyManager.IsActive = !enemyManager.IsActive;
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
          {
            p.DoCollision(go);
          }
        }
      }
      gameObjects.RemoveAll(go => go.Disposed);
      projectiles.RemoveAll(p => p.Disposed);

      TowerManager.Update(gt);
      ParticleManager.Update(gt);
      enemyManager.Update(gt);
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
      TowerManager.Draw(sb);
      ParticleManager.Draw(sb);
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
    public List<Projectile> GetProjectiles()
    {
      return projectiles;
    }
  }
}
