using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;


namespace Lindenmayers_Defense
{
  class Projectile : GameObject
  {
    Vector2 targetPos;
    public Projectile(Texture2D tex, Vector2 pos, Vector2 targetPos) : base(tex, pos)
    {
      this.pos = pos;
      this.targetPos = targetPos;
      drawHitbox = false;
      Scale = 0.05f;
    }

    private void MoveTo(Vector2 target, GameTime gt)
    {
      Vector2 termPos = new Vector2(target.X - tex.Width, target.Y - tex.Height);
      if (pos != termPos)
      {
        Vector2 direction = target - this.pos;
        direction.Normalize();
        this.pos += direction * 1000 * (float)gt.ElapsedGameTime.TotalSeconds;
      }
      if (GetDistanceToTarget(target) < 20)
      {
        Die();
        Disposed = true;
      }
    }
    protected double GetDistanceToTarget(Vector2 target)
    {
      float x = pos.X - target.X;
      float y = pos.Y - target.Y;
      return Math.Sqrt(x * x + y * y);
    }
    public override void Update(GameTime gt)
    {
      MoveTo(targetPos, gt);

    }
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
    }
  }
}
