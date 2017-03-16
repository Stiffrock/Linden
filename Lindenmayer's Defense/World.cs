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
    TowerManager towerManager;
    EnemyManager enemyManager;
    List<GameObject> gameObjects;
    List<Projectile> projectiles;

    GUI.UserInterface UI;
    Dictionary<string, LComponent> grammarComponents;
    public Base baseTower;
    Tower testTower;

    public World()
    {
      ParticleManager = new ParticleManager();
      enemyManager = new EnemyManager(this);
      towerManager = new TowerManager(this);
      gameObjects = new List<GameObject>();
      projectiles = new List<Projectile>();

      grammarComponents = new Dictionary<string, LComponent>();
      InitGrammarComponents();
      UI = new GUI.UserInterface(grammarComponents, this);

      baseTower = new Base(this, AssetManager.GetTexture("tower02"), new Vector2(800, 300));
      AddGameObject(baseTower);
      SpawnTestEnemies(5);
      testTower = new Tower(this, AssetManager.GetTexture("tower01"), new Vector2(600, 500));
      AddGameObject(testTower);
    }

    private void InitGrammarComponents()
    {
      grammarComponents.Add("spinner left", new LComponent(AssetManager.GetTexture("dot"), "LL"));
      grammarComponents.Add("wave", new LComponent(AssetManager.GetTexture("dot"), "W"));
      grammarComponents.Add("arrow", new LComponent(AssetManager.GetTexture("dot"), "P"));
      grammarComponents.Add("fork", new LComponent(AssetManager.GetTexture("dot"), "[+FY]-F"));
      grammarComponents.Add("spinner right", new LComponent(AssetManager.GetTexture("dot"), "RR"));
    }

    private void SpawnTestEnemies(int nrToSpawn)
    {
      for (int i = 0; i < nrToSpawn; i++)
      {
        Vector2 pos = new Vector2(100, 200 + i * 100);
        Enemy spawn = new Enemy(this, AssetManager.GetTexture("enemy01"), pos, 10, 50, 20);
        //spawn.Scale = 100;
        AddGameObject(spawn);
      }
    }

    public string GetGrammar()
    {
      return UI.GetResult();
    }
    public void Update(GameTime gt)
    {
      UI.Update(gt);
      if (UI.towerBox.rec.Contains(Input.GetMousePoint()) && Input.LeftMouseButtonClicked())
      {
        towerManager.CreateTower(Input.GetMousePos());
      }
      if (Input.RightMouseButtonClicked())
      {
        string res = UI.GetResult();
      }

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

      towerManager.Update(gt);
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
