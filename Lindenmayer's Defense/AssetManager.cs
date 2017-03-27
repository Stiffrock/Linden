using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace Lindenmayers_Defense
{
  public static class AssetManager
  {
    public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
    public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();

    public static void LoadContent(ContentManager Content)
    {
      AddTexture("dot", Content.Load<Texture2D>("dot"));
      AddTexture("bullet01", Content.Load<Texture2D>("bullet01"));
      AddTexture("tower01", Content.Load<Texture2D>("tower01"));
      AddTexture("tower02", Content.Load<Texture2D>("tower02"));
      AddTexture("towerplatform", Content.Load<Texture2D>("towerplatform"));
      AddTexture("enemy01", Content.Load<Texture2D>("enemy01"));
      AddTexture("particle01", Content.Load<Texture2D>("particle01"));
      AddTexture("particle02", Content.Load<Texture2D>("particle02"));
      AddTexture("particle03", Content.Load<Texture2D>("particle03"));
      AddTexture("particle04", Content.Load<Texture2D>("particle04"));
      AddTexture("pixel", Content.Load<Texture2D>("pixel"));
      AddTexture("button", Content.Load<Texture2D>("button"));
      AddTexture("container1", Content.Load<Texture2D>("container1"));
      AddTexture("container2", Content.Load<Texture2D>("container2"));
      AddTexture("panel", Content.Load<Texture2D>("panel"));

      AddFont("font1", Content.Load<SpriteFont>("Font"));
    }

    public static void AddTexture(string name, Texture2D tex)
    {
      Textures.Add(name, tex);
    }

    public static Texture2D GetTexture(string name)
    {
      return Textures[name];
    }
    public static void AddFont(string name, SpriteFont font)
    {
      Fonts.Add(name, font);
    }

    public static SpriteFont GetFont(string name)
    {
      return Fonts[name];
    }
  }
}
