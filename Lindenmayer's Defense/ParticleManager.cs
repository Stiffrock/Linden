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

    public void GenerateParticle(Texture2D tex, Vector2 pos, float lifeTime, float speed, float scale = 1.0f, Color? color = null)
    {
      float lt = lifeTime * (float)(1 + Game1.rnd.NextDouble() * 0.4f - 0.2f);
      if(color == null)
        color = new Color(Game1.rnd.Next(0, 255), Game1.rnd.Next(0, 255), Game1.rnd.Next(0, 255));
      Vector2 dir = new Vector2((float)Game1.rnd.NextDouble() * 2 - 1, (float)Game1.rnd.NextDouble() * 2 - 1);
      float sp = speed * (float)(1 + Game1.rnd.NextDouble() * 0.4f - 0.2f);
      float sc = scale * (float)(1 + Game1.rnd.NextDouble() * 0.4f - 0.2f);

      Particle p = new Particle(tex, pos, lt, (Color)color, sc, dir*sp, 0.0f, 1.0f, true);
      particles.Add(p);
    }
    public void CreateParticle(Texture2D tex, Vector2 pos, float lifeTime, Color color, float scale, Vector2 velocity, float rotationalSpeed, float alpha, bool fades)
    {
      Particle p = new Particle(tex, pos, lifeTime, color, scale, velocity, rotationalSpeed, alpha, fades);
      particles.Add(p);
    }
    public void CreateExplosion(Vector2 pos, float radius, Color color, float duration)
    {
      Particle exp = new Particle(AssetManager.GetTexture("particle03"), pos, duration, color, radius/64.0f, Vector2.Zero, 0.0f, 1.0f, true);
      //Particle exp = new Particle(AssetManager.GetTexture("dot"), pos, duration, color, radius / 512.0f, Vector2.Zero, 0.0f, 0.8f, true);
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
