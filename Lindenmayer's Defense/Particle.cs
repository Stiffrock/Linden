using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lindenmayers_Defense
{
  class Particle : GameObject
  {
    Vector2 velocity;
    float initialAlpha;
    double lifeTime;
    double elapsedTime;
    bool fades;
    public Particle(Texture2D tex, Vector2 pos, float lifeTime, Color color, float alpha, bool fades, Vector2 velocity) : base(tex, pos)
    {
      this.color = color;
      this.alpha = alpha;
      initialAlpha = alpha;
      this.lifeTime = lifeTime;
      this.fades = fades;
      elapsedTime = 0;
      this.velocity = velocity;
    }
    public override void Update(GameTime gt)
    {
      elapsedTime += gt.ElapsedGameTime.TotalSeconds;
      if (fades)
        alpha = (float)((1 - elapsedTime / lifeTime) * initialAlpha);
      if (elapsedTime >= lifeTime)
        Die();

      pos += velocity * (float)gt.ElapsedGameTime.TotalSeconds;
    }
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
    }
  }
}
