using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lindenmayers_Defense
{
  class Sprite
  {
    public Texture2D tex;
    public Vector2 pos;
    public float rotation = 0.0f;
    public float scale = 1.0f;
    public Color color = Color.White;
    public float alpha = 1.0f;
    public Vector2 origin;
    public Rectangle? spriteRec = null;
    public float layerDepth = 0.0f;

    public Sprite(Texture2D tex, Vector2 pos)
    {
      this.tex = tex;
      this.pos = pos;
      origin = new Vector2(tex.Width / 2.0f, tex.Height / 2.0f);
    }
    public Sprite(Sprite other)
    {
      tex = other.tex;
      pos = other.pos;
      rotation = other.rotation;
      scale = other.scale;
      color = other.color;
      alpha = other.alpha;
      origin = other.origin;
      spriteRec = other.spriteRec;
      layerDepth = other.layerDepth;
    }
    public virtual void Draw(SpriteBatch sb)
    {
      sb.Draw(tex, pos, spriteRec, color * alpha, rotation, origin, scale, SpriteEffects.None, layerDepth);
    }
  }
}
