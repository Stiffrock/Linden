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
  /// Clickable container that should hold a component.
  /// </summary>
  class Container
  {
    private Texture2D tex;
    private Vector2 pos;
    public Rectangle rec;
    private Color color;

    public Container(Texture2D tex, Vector2 pos)
    {
      this.tex = tex;
      this.pos = pos;
      this.rec = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);
      color = Color.Blue;
    }

    public void Update(GameTime gt)
    {
      if (rec.Contains(Input.GetMousePoint()))
      {
        color = Color.White;
      }
      else
      {
        color = Color.Blue;
      }
    }

    public  void Draw(SpriteBatch sb)
    {
      sb.Draw(tex, pos, rec, color);
    }
  }
}
