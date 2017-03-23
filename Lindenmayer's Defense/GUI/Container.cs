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
    protected Texture2D tex;
    public Vector2 pos;
    public Rectangle rec;

    protected bool showComponentInfo;
    protected Color color, defaultColor, highlightColor;
    public bool mouseOverEffect;
    

    public Container(Texture2D tex, Vector2 pos)
    {
      this.tex = tex;
      this.pos = pos;
      this.rec = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);

    
      mouseOverEffect = true;
      color = Color.White;
      defaultColor = Color.Blue;
      highlightColor = Color.White;
    }

    public virtual void Update(GameTime gt)
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

    public virtual void Draw(SpriteBatch sb)
    {
      sb.Draw(tex, pos, rec, color);
    
    }
  }
}
