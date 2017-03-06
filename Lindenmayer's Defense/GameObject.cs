﻿using System;
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
    public enum CollisionLayer
    {
      NONE = 0,
      TOWER = 1,
      ENEMY = 2,
      PROJECTILE = 4
    }

    public Texture2D tex;
    public Vector2 origin;
    public Rectangle? spriteRec = null;
    public Color color = Color.White;
    public float alpha = 1f;
    public float layerDepth = 0f;

    public Vector2 pos;
    public float rotation = 0.0f;
    public Rectangle hitbox;

    private float scale = 1.0f;
    public float Scale { get { return scale; } set { scale = value; UpdateHitbox(); } }

    public CollisionLayer layer { get; protected set; }
    public CollisionLayer layerMask;

    public bool Disposed { get; protected set; }

    public bool drawHitbox = false;
    public GameObject(Texture2D tex, Vector2 pos)
    {
      this.tex = tex;
      this.pos = pos;
      origin = new Vector2(tex.Width / 2.0f, tex.Height / 2.0f);
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
      if ((layerMask & other.layer) != CollisionLayer.NONE && hitbox.Intersects(other.hitbox))
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
    public virtual void Draw(SpriteBatch sb)
    {
      sb.Draw(tex, pos, spriteRec, color * alpha, rotation, origin, Scale, SpriteEffects.None, layerDepth);
      if (drawHitbox)
        sb.Draw(AssetManager.GetTexture("pixel"), hitbox, Color.Blue * 0.5f);
    }
  }
}
