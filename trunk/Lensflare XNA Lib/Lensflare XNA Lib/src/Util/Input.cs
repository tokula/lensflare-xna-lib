using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Util {
    public static class Input {
        private static KeyboardState previousKeyboardState;
        private static MouseState previousMouseState;

        public enum MouseButton {
            LeftButton, RightButton, MiddleButton, XButton1, XButton2
        }

        public static void Update() {
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
        }

        public static bool KeyboardPressed(Keys key) {
            return Keyboard.GetState().IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }

        public static bool KeyboardPressing(Keys key) {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public static bool MousePressed(MouseButton mouseButton) {
            switch (mouseButton) {
                case MouseButton.LeftButton:
                return Mouse.GetState().LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
                case MouseButton.RightButton:
                return Mouse.GetState().RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
                case MouseButton.MiddleButton:
                return Mouse.GetState().MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released;
                case MouseButton.XButton1:
                return Mouse.GetState().XButton1 == ButtonState.Pressed && previousMouseState.XButton1 == ButtonState.Released;
                case MouseButton.XButton2:
                return Mouse.GetState().XButton2 == ButtonState.Pressed && previousMouseState.XButton2 == ButtonState.Released;
            }
            return false;
        }

        public static bool MousePressing(MouseButton mouseButton) {
            switch (mouseButton) {
                case MouseButton.LeftButton:
                return Mouse.GetState().LeftButton == ButtonState.Pressed;
                case MouseButton.RightButton:
                return Mouse.GetState().RightButton == ButtonState.Pressed;
                case MouseButton.MiddleButton:
                return Mouse.GetState().MiddleButton == ButtonState.Pressed;
                case MouseButton.XButton1:
                return Mouse.GetState().XButton1 == ButtonState.Pressed;
                case MouseButton.XButton2:
                return Mouse.GetState().XButton2 == ButtonState.Pressed;
            }
            return false;
        }

        public static Vector2 MousePosition {
            get {
                MouseState mouseState = Mouse.GetState();
                return new Vector2(mouseState.X, mouseState.Y);
            }
        }

        public static int MouseDeltaX {
            get {
                return Mouse.GetState().X - previousMouseState.X;
            }
        }

        public static int MouseDeltaY {
            get {
                return Mouse.GetState().Y - previousMouseState.Y;
            }
        }
    }
}
