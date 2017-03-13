using System;
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
  class GameObject : Sprite
  {
    public enum CollisionLayer
    {
      NONE = 0,
      TOWER = 1,
      ENEMY = 2,
      PROJECTILE = 4
    }

    protected new float scale = 1.0f;
    public float Scale { get { return scale; } set { scale = value; UpdateHitbox(); } }
    public CollisionLayer Layer { get; protected set; }
    public CollisionLayer LayerMask { get; set; }
    public bool Disposed { get; protected set; }

    public Rectangle hitbox;
    public bool drawHitbox = false;
    public GameObject(Texture2D tex, Vector2 pos) : base(tex, pos)
    {
      hitbox = new Rectangle((int)(pos.X - origin.X), (int)(pos.Y - origin.Y), tex.Width, tex.Height);
      UpdateHitbox();
    }
    protected void UpdateHitbox()
    {
      hitbox.Width = (int)(tex.Width * Scale);
      hitbox.Height = (int)(tex.Height * Scale);
      hitbox.X = (int)(pos.X - (0.5f * hitbox.Width));
      hitbox.Y = (int)(pos.Y - (0.5f * hitbox.Height));
    }
    public Vector2 Forward()
    {
      Vector2 direction = new Vector2((float)Math.Sin(rotation), (float)-Math.Cos(rotation));
      return direction;
    }
    public virtual void Die()
    {
      Disposed = true;
    }
    public bool CollidesWith(GameObject other)
    {
      if ((LayerMask & other.Layer) != CollisionLayer.NONE && hitbox.Intersects(other.hitbox))
        return true;
      return false;
    }
    public virtual void DoCollision(GameObject other)
    {

    }
    public virtual void Update(GameTime gt)
    {
      UpdateHitbox();
    }
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
      if (drawHitbox)
        sb.Draw(AssetManager.GetTexture("pixel"), hitbox, Color.Blue * 0.5f);
    }
  }
}
