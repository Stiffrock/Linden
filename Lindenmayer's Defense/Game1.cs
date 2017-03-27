﻿using Microsoft.Xna.Framework;
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
    public static readonly int ScreenWidth = 1920;
    public static readonly int ScreenHeight = 1080;

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    TowerManager towerManager;
    World world;
    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      graphics.IsFullScreen = false;
      graphics.PreferredBackBufferWidth = 1920;
      graphics.PreferredBackBufferHeight = 1080;
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
      AssetManager.AddTexture("bullet01", Content.Load<Texture2D>("bullet01"));
      AssetManager.AddTexture("tower01", Content.Load<Texture2D>("tower01"));
      AssetManager.AddTexture("tower02", Content.Load<Texture2D>("tower02"));
      AssetManager.AddTexture("towerplatform", Content.Load<Texture2D>("towerplatform"));
      AssetManager.AddTexture("enemy01", Content.Load<Texture2D>("enemy01"));
      AssetManager.AddTexture("particle01", Content.Load<Texture2D>("particle01"));
      AssetManager.AddTexture("particle02", Content.Load<Texture2D>("particle02"));
      AssetManager.AddTexture("particle03", Content.Load<Texture2D>("particle03"));
      AssetManager.AddTexture("particle04", Content.Load<Texture2D>("particle04"));
      AssetManager.AddTexture("button", Content.Load<Texture2D>("arrow"));
      AssetManager.AddTexture("container1", Content.Load<Texture2D>("container1"));
      AssetManager.AddTexture("container2", Content.Load<Texture2D>("container2"));
      AssetManager.AddTexture("panel", Content.Load<Texture2D>("panel"));
      AssetManager.AddTexture("pixel", Content.Load<Texture2D>("pixel"));
      AssetManager.AddTexture("rune1", Content.Load<Texture2D>("rune1"));
      AssetManager.AddTexture("rune2", Content.Load<Texture2D>("rune2"));
      AssetManager.AddTexture("rune3", Content.Load<Texture2D>("rune3"));
      AssetManager.AddTexture("rune4", Content.Load<Texture2D>("rune4"));
      AssetManager.AddTexture("rune5", Content.Load<Texture2D>("rune5"));
      AssetManager.AddTexture("rune6", Content.Load<Texture2D>("rune6"));
      AssetManager.AddTexture("rune7", Content.Load<Texture2D>("rune7"));
      AssetManager.AddTexture("rune8", Content.Load<Texture2D>("rune8"));
      AssetManager.AddTexture("rune9", Content.Load<Texture2D>("rune9"));
      AssetManager.AddTexture("rune10", Content.Load<Texture2D>("rune10"));
      AssetManager.AddFont("font1", Content.Load<SpriteFont>("Font"));

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
