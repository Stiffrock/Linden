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
      mouseOverEffect = true;
      effectColor = Color.Blue;
    }
    public void SetComponent(LComponent component)
    {
      this.component = component;
    }

    public void ComponentClick(SpriteBatch sb)
    {
      if (clickEffect)
        sb.Draw(AssetManager.GetTexture("container1"), rec, Color.White);
      else
        sb.Draw(AssetManager.GetTexture("container2"), rec, Color.White);
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
    }
    public override void Draw(SpriteBatch sb)
    {
      if (effect)
        sb.Draw(AssetManager.GetTexture("container1"), rec, Color.LightBlue);
      else
        sb.Draw(AssetManager.GetTexture("container2"), rec, Color.LightBlue*0.5f);

      if (showComponentInfo && name != null && !ComponentArray)
        sb.DrawString(AssetManager.GetFont("font1"), name, new Vector2(pos.X, (Game1.ScreenHeight - Game1.ScreenHeight / 4)), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

      if (showComponentInfo && name != null && ComponentArray)
        sb.DrawString(AssetManager.GetFont("font1"), name, new Vector2(pos.X, (Game1.ScreenHeight - Game1.ScreenHeight / 7- 7)), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
    }
  }
}
