﻿using System.Collections.Generic;
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
