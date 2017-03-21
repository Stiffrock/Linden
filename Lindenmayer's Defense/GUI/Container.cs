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
  /// <summary>
  /// Clickable container that should hold a component.
  /// </summary>
  class Container
  {
    private Texture2D tex;
    public Vector2 pos;
    public Rectangle rec;
    private Color color, defaultColor, highlightColor;
    public LComponent component;
    public string name;
    public Tower tower;
    public List<int> statList;
    private bool showComponentInfo;
    public bool ComponentArray, mouseOverEffect;
    

    public Container(Texture2D tex, Vector2 pos)
    {
      this.tex = tex;
      this.pos = pos;
      this.rec = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);
      component = new LComponent(null, null);
      statList = new List<int>();
      name = null;
      mouseOverEffect = true;
      color = Color.White;
      defaultColor = Color.Blue;
      highlightColor = Color.White;
    }

    public void SetComponent(LComponent component)
    {
      this.component = component;
    }

    public void Update(GameTime gt)
    {
      if (mouseOverEffect)
      {
        if (rec.Contains(Input.GetMousePoint()))
        {
          color = highlightColor;
          showComponentInfo = true;
        }
        else
        {
          showComponentInfo = false;
          color = defaultColor;
        }
      }
     
    }

    public  void Draw(SpriteBatch sb)
    {
      sb.Draw(tex, pos, rec, color);
      if (showComponentInfo && name != null)
      {     
        sb.DrawString(AssetManager.GetFont("font1"), name, new Vector2(100, 580), Color.Black, 0.0f , Vector2.Zero, 1.0f, SpriteEffects.None, 0);
      }
    }
  }
}
