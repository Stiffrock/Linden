using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lindenmayers_Defense.GUI
{
  class TowerContainer : Container
  {
    public TowerContainer(Texture2D tex, Vector2 pos) : base(tex, pos)
    {
      mouseOverEffect = true;
      this.rec = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);

    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
    }
    public override void Draw(SpriteBatch sb)
    {
      if (effect)
        sb.Draw(AssetManager.GetTexture("container1"), rec, Color.LightBlue * 0.8f);
      else
        sb.Draw(AssetManager.GetTexture("container2"), rec, Color.LightBlue * 0.5f);

    }
  }
}
