using Microsoft.Xna.Framework;
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

    void CollectProjectileData(bool saveToFile)
    {
      int width = 128;
      int height = 128;
      int ceiling = 25;
      int resolution = 8;
      float[,] frequencyMap = new float[width, height];
      float highest = 0;

      List<Projectile> projectiles = world.GetProjectiles();
      List<GameObject> towers = world.GetGameObjects();
      towers.RemoveAll(x => !(x is Tower));

      foreach (Tower t in towers)
      {
        foreach (Projectile p in projectiles)
        {
          if (p.Tower != t)
            continue;

          Vector2 posOffset = (p.pos - t.pos) / resolution;
          int X = (int)(posOffset.X) + width / 2;
          int Y = (int)(posOffset.Y) + height / 2;
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

      for (int i = 0; i < frequencyMap.GetLength(0); i++)
      {
        for (int j = 0; j < frequencyMap.GetLength(1); j++)
        {
          frequencyMap[i, j] /= ceiling;
        }
      }

      heatMap = HeatMap.Generate(GraphicsDevice, frequencyMap);
      if (saveToFile)
      {
        Stream stream = File.Create("heatMap.png");
        heatMap.SaveAsPng(stream, heatMap.Width, heatMap.Height);
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
      if (Input.KeyPressed(Keys.F2))
      {
        showHeatMap = !showHeatMap;
      }
      if (Input.KeyPressed(Keys.F3))
      {
        updateHeatMap = !updateHeatMap;
      }
      if (Input.KeyPressed(Keys.F1))
      {
        ui.RandomizeComponents();
        world.TowerManager.CreateTower(Vector2.Zero, ui.GenerateGrammar(), ui.GetTextures());
        world.TowerManager.PlaceTower(new Vector2(Utility.RandomInt(0, ScreenWidth), Utility.RandomInt(0, ScreenHeight)));
      }
      world.Update(gameTime);
      ui.Update(gameTime);

      if (ui.GetTowerCreator().ClickedOn())
      {
        world.TowerManager.CreateTower(Input.GetMousePos(), ui.GenerateGrammar(), ui.GetTextures());
      }
      if (updateHeatMap)
        CollectProjectileData(false);
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
      string mouseXY = Input.GetMousePos().ToString();
      spriteBatch.DrawString(AssetManager.GetFont("font1"), mouseXY, Vector2.Zero, Color.White);
      //spriteBatch.Draw(dot, Input.GetMousePos(), null, Color.Purple, 0.0f, new Vector2((float)dot.Width / 2, (float)dot.Height / 2), 0.025f, SpriteEffects.None, 0.0f);
      //spriteBatch.Draw(dot, Input.GetMousePos(), null, test, 0.0f, new Vector2((float)dot.Width / 2, (float)dot.Height / 2), 0.25f, SpriteEffects.None, 0.0f);
      spriteBatch.End();

      spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
      if (showHeatMap && heatMap != null)
        spriteBatch.Draw(heatMap, Input.GetMousePos(), null, Color.White, 0.0f, new Vector2((float)heatMap.Width / 2, (float)heatMap.Height / 2), 8.0f, SpriteEffects.None, 0.0f);
      spriteBatch.End();
      base.Draw(gameTime);
    }
  }
}
