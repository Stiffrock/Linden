using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lindenmayers_Defense
{
  class ValueBar
  {
    public float Value { get { return value; } set { this.value = value > maxValue ? maxValue : value; } }
    public float MaxValue { get { return maxValue; } protected set { value = maxValue; } }

    public Rectangle rec;
    Texture2D pixel;
    float value, maxValue;
    Color foreColor, backColor;

    public ValueBar(Rectangle rec, float maxValue, float value, Color foreColor, Color backColor)
    {
      pixel = AssetManager.GetTexture("pixel");
      this.rec = rec;
      this.maxValue = maxValue;
      this.value = value;
      this.foreColor = foreColor;
      this.backColor = backColor;
    }
    public void SetPos(Vector2 position)
    {
      rec.X = (int)position.X - (int)(rec.Width / 2.0f);
      rec.Y = (int)position.Y - (int)(rec.Height / 2.0f);
    }
    private Rectangle CalculateFrontRectangle()
    {
      int X = rec.X;
      int Y = rec.Y;
      int width = (int)(rec.Width * (value / maxValue));
      int height = rec.Height;
      Rectangle front = new Rectangle(X, Y, width, height);
      return front;
    }
    public void Draw(SpriteBatch sb)
    {
      sb.Draw(pixel, rec, null, backColor);
      sb.Draw(pixel, CalculateFrontRectangle(), null, foreColor);
    }
  }
}
