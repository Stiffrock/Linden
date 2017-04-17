﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Lindenmayers_Defense.GUI;
using System.Collections.Generic;
using System.IO;

namespace Lindenmayers_Defense
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    public static Random rnd = new Random();
    public static readonly int ScreenWidth = 1800;
    public static readonly int ScreenHeight = 1000;

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    UserInterface ui;
    World world;

    bool showHeatMap, updateHeatMap;
    Texture2D heatMap;
    int nrOfTowers;
    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      graphics.IsFullScreen = false;
      graphics.PreferredBackBufferWidth = ScreenWidth;
      graphics.PreferredBackBufferHeight = ScreenHeight;
      graphics.ApplyChanges();
      Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      this.IsMouseVisible = true;
      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);
      AssetManager.LoadContent(Content);


      //world måste vara sist om den ska anväda inladdade texturer
      world = new World();
      ui = new UserInterface(world.TowerManager, world);
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    float[,] frequencyMap = new float[256, 256];
    void CollectProjectileData(bool saveToFile)
    {
      int width = 256;
      int height = 256;
      int ceiling = 50;
      int resolution = 8;
      float highest = 0;

      List<Projectile> projectiles = world.GetProjectiles();
      List<GameObject> towers = new List<GameObject>(world.GetGameObjects());
      towers.RemoveAll(x => !(x is Tower));
      foreach (Tower t in towers)
      {
        foreach (Projectile p in projectiles)
        {
          if (p.Tower != t)
            continue;

          Vector2 posOffset = (p.pos - t.pos) / resolution;
          int X = (int)(posOffset.X + width / 2.0f);
          int Y = (int)(posOffset.Y + height / 1.5f);
          X = MathHelper.Clamp(X, 0, width - 1);
          Y = MathHelper.Clamp(Y, 0, height - 1);
          highest = Math.Max(++frequencyMap[X, Y], highest);
        }
      }

      if (saveToFile)
      {
        StreamWriter sw = new StreamWriter("projectile_data.txt", false);
        foreach (Tower t in towers)
        {
          string data = "Tower" + t.TowerID.ToString() + '\n';
          foreach (Projectile p in projectiles)
          {
            if (p.Tower != t)
              continue;
            Vector2 posOffset = p.pos - t.pos;
            data += posOffset.X.ToString() + '\t' + posOffset.Y.ToString() + '\n';
          }
          data += '\n';
          sw.Write(data);
        }
        sw.Close();
      }

      float[,] normalizedValues = new float[width, height];
      for (int i = 0; i < frequencyMap.GetLength(0); i++)
      {
        for (int j = 0; j < frequencyMap.GetLength(1); j++)
        {
          normalizedValues[i, j] = frequencyMap[i, j] / ceiling;
        }
      }

      heatMap = HeatMap.Generate(GraphicsDevice, normalizedValues);
      if (saveToFile)
      {
        Stream stream = File.Create("heatMap.png");
        heatMap.SaveAsPng(stream, heatMap.Width, heatMap.Height);
        stream.Close();
      }
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      if (this.IsActive)
        Input.Update();
      if (Input.KeyDown(Keys.Escape))
        Exit();
      if (Input.KeyPressed(Keys.F5))
      {

        CollectProjectileData(true);
      }
      if (Input.KeyPressed(Keys.F6))
      {
        for (int i = 0; i < 1000; i++)
        {
          ui.SelectRandomComponents();
          world.TowerManager.CreateTower(Vector2.Zero, ui.GenerateGrammar(), ui.GetTextures(), 0);
          world.TowerManager.PlaceTower(Vector2.Zero);
        }
        for (int i = 0; i < 600; i++)
        {
          world.DebugUpdate(gameTime);
        }
        CollectProjectileData(true);
        world.GetGameObjects().Clear();
        world.GetProjectiles().Clear();
      }
      if (Input.KeyPressed(Keys.F2))
      {
        showHeatMap = !showHeatMap;
      }
      if (Input.KeyPressed(Keys.F3))
      {
        updateHeatMap = !updateHeatMap;
      }
      if (Input.KeyPressed(Keys.F4))
      {
        foreach (var item in world.GetGameObjects())
        {
          item.Die();
        }
      }
      if (Input.KeyPressed(Keys.F1))
      {
        ui.SelectRandomComponents();
        world.TowerManager.CreateTower(Vector2.Zero, ui.GenerateGrammar(), ui.GetTextures(), 0);
        world.TowerManager.PlaceTower(new Vector2(Utility.RandomInt(0, ScreenWidth), Utility.RandomInt(0, ScreenHeight)));
      }
      world.Update(gameTime);
      ui.Update(gameTime);

      if (ui.GetTowerCreator().ClickedOn())
      {
        bool success = false;
        success = world.TowerManager.CreateTower(Input.GetMousePos(), ui.GenerateGrammar(), ui.GetTextures(), ui.GetCost());
        if (success)
          world.ResourceManager.RemoveGold(ui.GetCost());
      }
      if (updateHeatMap)
        CollectProjectileData(false);
      List<GameObject> towers = new List<GameObject>(world.GetGameObjects());
      towers.RemoveAll(x => !(x is Tower));
      nrOfTowers = towers.Count;
      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      spriteBatch.Begin();
      world.Draw(spriteBatch);
      ui.Draw(spriteBatch);
      //show mouse position
      Texture2D dot = AssetManager.GetTexture("dot");
      spriteBatch.Draw(dot, Input.GetMousePos(), null, Color.Purple, 0.0f, new Vector2((float)dot.Width / 2, (float)dot.Height / 2), 0.025f, SpriteEffects.None, 0.0f);
      spriteBatch.End();

      spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
      spriteBatch.DrawString(AssetManager.GetFont("font1"), Input.GetMousePos().ToString(), Vector2.Zero, Color.White);
      spriteBatch.DrawString(AssetManager.GetFont("font1"), "Towers: " + nrOfTowers.ToString(), new Vector2(0, 50), Color.White);
      if (showHeatMap && heatMap != null)
        spriteBatch.Draw(heatMap, Input.GetMousePos(), null, Color.White, 0.0f, new Vector2((float)heatMap.Width / 2, (float)heatMap.Height / 2), 4.0f, SpriteEffects.None, 0.0f);
      spriteBatch.End();
      base.Draw(gameTime);
    }
  }
}
