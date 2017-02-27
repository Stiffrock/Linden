using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lindenmayers_Defense
{
  class GameObject
  {
    public Texture2D tex;
    public Vector2 origin;
    public Rectangle? spriteRec = null;
    public Color color = Color.White;
    public float alpha = 1f;
    public float layerDepth = 0f;

    public Vector2 pos;
    public float rotation = 0.0f;
    public float scale = 1.0f;
    public Rectangle hitbox;

    public bool drawHitbox = true;
    public GameObject(Texture2D tex, Vector2 pos)
    {
      this.tex = tex;
      this.pos = pos;
      origin = new Vector2(tex.Width / 2.0f, tex.Height / 2.0f);
      hitbox = new Rectangle((int)(pos.X - origin.X), (int)(pos.Y - origin.Y), tex.Width, tex.Height);
    }

    public virtual void Update(GameTime gt)
    {

    }
    public virtual void Draw(SpriteBatch sb)
    {
      sb.Draw(tex, pos, spriteRec, color * alpha, rotation, origin, scale, SpriteEffects.None, layerDepth);
      if (drawHitbox)
        sb.Draw(AssetManager.GetTexture("pixel"), hitbox, Color.Blue * 0.5f);
    }
  }
}
