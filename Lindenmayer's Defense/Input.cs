using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Lindenmayers_Defense
{
  public static class Input
  {
    static MouseState mouseState;
    static MouseState oldMouseState;
    static KeyboardState kbState;
    static KeyboardState oldKbState;
    static int oldMouseWheelValue;
    static int mouseWheelValue;
    public static void Update()
    {
      oldMouseWheelValue = mouseWheelValue;
      oldMouseState = mouseState;
      oldKbState = kbState;
      mouseState = Mouse.GetState();
      kbState = Keyboard.GetState();
      mouseWheelValue = mouseState.ScrollWheelValue;
    }
    public static Vector2 GetMousePos() { return new Vector2(mouseState.X, mouseState.Y); }
    public static bool LeftMouseButtonDown() { return mouseState.LeftButton == ButtonState.Pressed; }
    public static bool RightMouseButtonDown() { return mouseState.RightButton == ButtonState.Pressed; }
    public static bool MiddleMouseButtonDown() { return mouseState.MiddleButton == ButtonState.Pressed; }
    public static bool LeftMouseButtonClicked() { return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released; }
    public static bool RightMouseButtonClicked() { return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released; }
    public static bool MiddleMouseButtonClicked() { return mouseState.MiddleButton == ButtonState.Pressed && oldMouseState.MiddleButton == ButtonState.Released; }
    public static bool KeyDown(Keys key) { return kbState.IsKeyDown(key); }
    public static bool KeyPressed(Keys key) { return kbState.IsKeyDown(key) && oldKbState.IsKeyUp(key); }
    public static MouseState GetMouseState() { return mouseState; }
    public static KeyboardState GetKeyboardState() { return kbState; }
    public static int GetDeltaMouseWheel() { return mouseWheelValue - oldMouseWheelValue; }
  }
}
