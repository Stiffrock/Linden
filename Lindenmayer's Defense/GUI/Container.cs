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
    //protected Texture2D tex;
    //public Vector2 pos;
    public Rectangle rec;

    protected bool showComponentInfo;
    //protected Color color;
    protected Color defaultColor, highlightColor;
    public bool mouseOverEffect;


    public Container(Texture2D tex, Vector2 pos) : base(tex,pos)
    {
      this.tex = tex;
      this.pos = pos;
      this.rec = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);
      origin = Vector2.Zero;
      mouseOverEffect = true;
      color = Color.White;
      defaultColor = Color.Blue;
      highlightColor = Color.White;
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
          color = highlightColor;
          showComponentInfo = true;
        }
        else
        {
          showComponentInfo = false;
          color = defaultColor;
        }
      }
    }

    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
      //sb.Draw(tex, rec, Color.Blue * 0.5f);
      //sb.Draw(tex, rec, color);
    }
  }
}
