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
  class Enemy : GameObject
  {

    public Enemy(Texture2D tex, Vector2 pos) : base(tex, pos)
    {
    this.pos = pos;
    scale = 0.05f;
    color = Color.Red;
    drawHitbox = false;   
    }

    public void MoveTo(Vector2 target)
    {     
      if (pos != target)
      {
        Vector2 dir = new Vector2(target.X - pos.X, target.Y - pos.Y);
        dir.Normalize();
        pos += dir;
      }
    }

    public override void Draw(SpriteBatch sb)
    {
        base.Draw(sb);
    }

    public override void Update(GameTime gt)
    {
     MoveTo(new Vector2(500,300));
        base.Update(gt);
    }
  }
}
