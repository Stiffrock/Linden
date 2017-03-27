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
  class Container : GameObject
  {
    public Rectangle rec;
    protected bool showComponentInfo, effect, clickEffect, mouseOverEffect;
    protected Color defaultColor, effectColor, currentColor;
    //protected Texture2D tex;
    //public Vector2 pos;
    //public Rectangle rec;
    //protected Color color;


    public Container(Texture2D tex, Vector2 pos) : base(tex,pos)
    {
      this.tex = tex;
      this.pos = pos;
      this.rec = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
      origin = Vector2.Zero;
    }
    public bool ClickedOn()
    {
      return Input.LeftMouseButtonClicked() && rec.Contains(Input.GetMousePoint());
    }

    public override void Update(GameTime gt)
    {
      if (mouseOverEffect)
      {
        if (rec.Contains(Input.GetMousePoint()))
        {
          showComponentInfo = true;
          effect = true;
        }
        else
        {
          showComponentInfo = false;
          effect = false;
          color = defaultColor;
        }
      }
    }

    public override void Draw(SpriteBatch sb)
    {
      sb.Draw(tex, rec, color);
    }
  }
}
