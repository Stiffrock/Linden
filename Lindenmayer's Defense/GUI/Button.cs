using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lindenmayers_Defense.GUI
{
  /// <summary>
  /// Clickable button, triggers an action
  /// </summary>
  class Button : Sprite
  {
    //public Vector2 pos;
    public Rectangle hitbox;
    public string statID;
    public Button(Texture2D tex, Vector2 pos, string statID) : base(tex, pos)
    {
      this.statID = statID;
      scale = 0.5f;
      color = Color.Red;
      hitbox.Width = (int)(tex.Width * scale);
      hitbox.Height = (int)(tex.Height * scale);
      hitbox.X = (int)(pos.X - (0.5f * hitbox.Width));
      hitbox.Y = (int)(pos.Y - (0.5f * hitbox.Height));
    }
    public override void Draw(SpriteBatch sb)
    {
      sb.Draw(AssetManager.GetTexture("pixel"), hitbox, Color.Blue * 0.5f);
      base.Draw(sb);
    }
  }
}
