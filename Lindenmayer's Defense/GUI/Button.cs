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
  class Button
  {
    private Rectangle hitBox;
    public Texture2D tex;
    public Vector2 pos;
    public string statID;
    public Button(Texture2D tex, Vector2 pos, string statID)
    {
      this.statID = statID;
      this.tex = tex;
      this.pos = new Vector2(pos.X, pos.Y);
      hitBox = new Rectangle((int)pos.X, (int)pos.Y, 20, 20);

    }
    public Rectangle GetHitbox()
    {
      return hitBox;
    }
  }
}
