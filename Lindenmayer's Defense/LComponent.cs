using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Lindenmayers_Defense
{
  class LComponent : GameObject
  {
    public string grammar;
    public Rectangle rec;
    public bool chosen;

    public LComponent(Texture2D tex, Vector2 pos, string grammar) : base(tex, pos)
    {
      this.tex = tex;
      this.grammar = grammar;
      origin = Vector2.Zero;
      rec = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
      //spriteRec = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
      scale = 0.5f;

    }
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
    }
  }
}
