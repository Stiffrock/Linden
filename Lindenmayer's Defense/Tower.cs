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
    class Tower : GameObject
    {              
        public Tower(Vector2 pos) :base(pos)
        {
            this.pos = pos;
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
