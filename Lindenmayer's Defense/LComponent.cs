using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Lindenmayers_Defense
{
  class LComponent
  {
    public string grammar;
    public string info;
    public Texture2D tex;
    public Vector2 pos;
    public Rectangle rec;
    public bool chosen;

    public LComponent(Texture2D tex, string grammar)
    {
      this.tex = tex;
      this.grammar = grammar;
    }

    public void Draw(SpriteBatch sb)
    {
      sb.Draw(tex, pos, rec, Color.Red);
    }

    public void Update(GameTime gt)
    {
    }
  }
}
