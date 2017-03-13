using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lindenmayers_Defense
{
  class Particle : Sprite
  {
    public bool Disposed { get; protected set; }

    Vector2 velocity;
    float rotationalSpeed;
    float lifeTime;
    float elapsedTime;
    bool fades;
    float initialAlpha;

    public Particle(Texture2D tex, Vector2 pos, float lifeTime, Color color, float scale, Vector2 velocity, float rotationalSpeed, float alpha, bool fades) : base(tex, pos)
    {
      this.color = color;
      this.alpha = alpha;
      this.velocity = velocity;
      this.rotationalSpeed = rotationalSpeed;
      this.lifeTime = lifeTime;
      this.scale = scale;
      elapsedTime = 0;
      this.fades = fades;
      initialAlpha = alpha;
    }
    public void Update(GameTime gt)
    {
      elapsedTime += (float)gt.ElapsedGameTime.TotalSeconds;
      if (fades)
        alpha = (float)((1 - elapsedTime / lifeTime) * initialAlpha);
      if (elapsedTime >= lifeTime)
        Disposed = true;
      pos += velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      rotation += rotationalSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
    }
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
    }
  }
}
