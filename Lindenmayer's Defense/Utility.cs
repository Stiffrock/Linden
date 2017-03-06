using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lindenmayers_Defense
{
  public class Utility
  {
    /// <summary>
    /// Converts a rotation into a direction. 0 rotation means straight up.
    /// </summary>
    /// <param name="radians">Rotation to convert, in radians.</param>
    /// <returns>Returns a normalized vector.</returns>
    public static Vector2 AngleToVector2(float radians)
    {
      Vector2 direction = new Vector2((float)Math.Sin(radians), (float)-Math.Cos(radians));
      return direction;
    }
    /// <summary>
    /// Converts a Vector2 into a rotation. A direction of (0, -1) is 0 rotation.
    /// </summary>
    /// <param name="v">Vector2 to convert.</param>
    /// <returns>Rotation in radians.</returns>
    public static float Vector2ToAngle(Vector2 v)
    {
      return (float)Math.Atan2(v.Y, v.X) + (float)Math.PI / 2f; ;
    }
  }
}
