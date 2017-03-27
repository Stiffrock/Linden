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
  class ComponentContainer : Container
  {
    public string name;
    public LComponent component;
    public bool ComponentArray;

    public ComponentContainer(Texture2D tex, Vector2 pos) : base(tex, pos)
    {
      name = null;
    }
    public void SetComponent(LComponent component)
    {
      this.component = component;
    }

    public void ComponentClick()
    {

    }

    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
      //sb.Draw(tex, pos, rec, color);
      if (showComponentInfo && name != null)
      {
        sb.DrawString(AssetManager.GetFont("font1"), name, new Vector2(100, 580), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
      }
    }
  }
}
