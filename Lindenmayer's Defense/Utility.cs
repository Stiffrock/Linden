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
    static Random rnd = new Random();
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
    /// <summary>
    /// Returns a delta rotation up to a specified amount.
    /// </summary>
    /// <param name="rotation">Starting rotation</param>
    /// <param name="targetAngle">Target rotation</param>
    /// <param name="amount">Maximum amount to rotate by</param>
    /// <returns>The amount actually rotated and direction of the rotation</returns>
    public static float TurnAngle(float rotation, float targetAngle, float amount, GameTime gt = null)
    {
      if (gt != null)
        amount *= (float)gt.ElapsedGameTime.TotalSeconds;
      rotation = MathHelper.WrapAngle(rotation);
      targetAngle = MathHelper.WrapAngle(targetAngle);
      float angleDiff = MathHelper.WrapAngle(targetAngle - rotation);
      float turnAmount;
      if (Math.Abs(angleDiff) < amount)
        turnAmount = angleDiff;
      else
        turnAmount = amount * (angleDiff > 0 ? 1 : -1);
      return turnAmount;
    }
    public static Color LerpColor(Color a, Color b, float amount)
    {
      a.R = (byte)MathHelper.Lerp((float)a.R, (float)b.R, amount);
      a.G = (byte)MathHelper.Lerp((float)a.G, (float)b.G, amount);
      a.B = (byte)MathHelper.Lerp((float)a.B, (float)b.B, amount);
      a.A = (byte)MathHelper.Lerp((float)a.A, (float)b.A, amount);
      return a;
    }
    public static int RandomInt(int min, int max)
    {
      return rnd.Next(min, max);
    }
  }
}
