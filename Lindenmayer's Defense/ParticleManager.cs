using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lindenmayers_Defense
{
  class ParticleManager
  {
    List<Particle> particles;

    public ParticleManager()
    {
      particles = new List<Particle>();
    }

    public void CreateExplosion(Vector2 pos, float radius, Color color, float duration)
    {
      Particle exp = new Particle(AssetManager.GetTexture("dot"), pos, duration, color, 0.8f, true, Vector2.Zero);
      exp.Scale = radius / 512.0f;
      particles.Add(exp);
    }

    public void Update(GameTime gt)
    {
      for (int i = 0; i < particles.Count; i++)
      {
        particles[i].Update(gt);
      }
      particles.RemoveAll(p => p.Disposed);
    }

    public void Draw(SpriteBatch sb)
    {
      for (int i = 0; i < particles.Count; i++)
      {
        particles[i].Draw(sb);
      }
    }
  }
}
