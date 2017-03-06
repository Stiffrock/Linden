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
    float initialAlpha;
    double lifeTime;
    double elapsedTime;
    bool fades;
    public Particle(Texture2D tex, Vector2 pos, float lifeTime, Color color, float alpha, bool fades) : base(tex, pos)
    {
      this.color = color;
      this.alpha = alpha;
      initialAlpha = alpha;
      this.lifeTime = lifeTime;
      this.fades = fades;
      elapsedTime = 0;
    }
    public override void Update(GameTime gt)
    {
      elapsedTime += gt.ElapsedGameTime.Milliseconds;
      if (fades)
        alpha = (float)((1 - elapsedTime / lifeTime) * initialAlpha);
      if (elapsedTime >= lifeTime)
        Die();
    }
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
    }
  }
}
