using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Lindenmayers_Defense
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    public static Random rnd = new Random();
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    TowerManager towerManager;
    World world;
    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      graphics.IsFullScreen = false;
      graphics.PreferredBackBufferWidth = 1200;
      graphics.PreferredBackBufferHeight = 800;
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
      // TODO: Add your initialization logic here
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
      AssetManager.AddTexture("dot", Content.Load<Texture2D>("dot"));
      AssetManager.AddTexture("pixel", Content.Load<Texture2D>("pixel"));

      //world måste vara sist om den ska anväda inladdade texturer
      world = new World();
      towerManager = new TowerManager(world);
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      if(this.IsActive)
        Input.Update();
      if (Input.KeyDown(Keys.Escape))
        Exit();
      world.Update(gameTime);
      towerManager.Update(gameTime);
      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      // TODO: Add your drawing code here
      spriteBatch.Begin();
      world.Draw(spriteBatch);
      spriteBatch.End();
      base.Draw(gameTime);
    }
  }
}
