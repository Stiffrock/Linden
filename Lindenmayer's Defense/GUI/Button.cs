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
  class Button : GameObject
  {
    //public Vector2 pos;
    public string statID;
    public Button(Texture2D tex, Vector2 pos, string statID) : base(tex, pos)
    {
      this.statID = statID;
      Scale = 0.05f;
      color = Color.Red;

    }
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
    }
  }
}
