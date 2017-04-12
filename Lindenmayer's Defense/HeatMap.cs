using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lindenmayers_Defense
{
  class HeatMap
  {
    static Color[] heatMapColors = new Color[7]
    {
      new Color(0, 0, 0),
      new Color(0, 0, 255),
      new Color(0, 255, 255),
      new Color(0, 255, 0),
      new Color(255, 255, 0),
      new Color(255, 0, 0),
      new Color(255, 255, 255)
    };

    public static Texture2D Generate(GraphicsDevice graphics, float[,] values)
    {
      int width = values.GetLength(0);
      int height = values.GetLength(1);
      Color[] colorMap = new Color[width * height];
      for (int i = 0; i < width; i++)
      {
        for (int j = 0; j < height; j++)
        {
          colorMap[i + j * width] = GetHeatMapColor(values[i, j]);
        }
      }
      Texture2D heatMap = new Texture2D(graphics, width, height);
      heatMap.SetData<Color>(colorMap);
      return heatMap;
    }

    protected static Color GetHeatMapColor(float value)
    {
      int index1, index2;
      float fractBetween;
      if (value <= 0)
        return heatMapColors[0];
      else if (value >= 1)
        return heatMapColors[heatMapColors.Length - 1];
      else
      {
        value = value * heatMapColors.Length - 1;
        index1 = (int)value;
        index2 = index1 + 1;
        fractBetween = value - index1;
      }
      return Utility.LerpColor(heatMapColors[index1], heatMapColors[index2], fractBetween);
    }
  }
}
