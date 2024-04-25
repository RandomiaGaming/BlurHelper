namespace BlurHelper
{
    public static class InputHelper
    {
        #region Buttons
        public static VirtualButton DKey = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D); });
        public static VirtualButton AKey = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A); });
        public static VirtualButton SKey = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S); });
        public static VirtualButton LKey = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L); });
        public static VirtualButton RightArrowKey = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right); });
        public static VirtualButton LeftArrowKey = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left); });
        public static VirtualButton LeftClick = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed; });
        public static VirtualButton RightClick = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed; });
        public static VirtualButton MiddleClick = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed; });
        public static VirtualButton Side0Click = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.XButton1 == Microsoft.Xna.Framework.Input.ButtonState.Pressed; });
        public static VirtualButton Side1Click = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.XButton2 == Microsoft.Xna.Framework.Input.ButtonState.Pressed; });
        public static VirtualButton DeleteKey = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Delete); });
        public static VirtualButton ControlKey = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) || keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl); });
        public static VirtualButton ShiftKey = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) || keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift); });
        public static VirtualButton SpaceKey = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space); });
        #endregion
        #region Axises
        public static VirtualAxis ScrollWheelAxis = new VirtualAxis((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.ScrollWheelValue / 120; });
        public static VirtualAxis MouseXAxis = new VirtualAxis((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.X; });
        public static VirtualAxis MouseYAxis = new VirtualAxis((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.Y; });
        #endregion
        public static void Tick()
        {
            Microsoft.Xna.Framework.Input.KeyboardState keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            DKey.Tick(keyboardState, mouseState);
            AKey.Tick(keyboardState, mouseState);
            SKey.Tick(keyboardState, mouseState);
            LKey.Tick(keyboardState, mouseState);
            RightArrowKey.Tick(keyboardState, mouseState);
            LeftArrowKey.Tick(keyboardState, mouseState);
            LeftClick.Tick(keyboardState, mouseState);
            RightClick.Tick(keyboardState, mouseState);
            MiddleClick.Tick(keyboardState, mouseState);
            Side0Click.Tick(keyboardState, mouseState);
            Side1Click.Tick(keyboardState, mouseState);
            DeleteKey.Tick(keyboardState, mouseState);
            ControlKey.Tick(keyboardState, mouseState);
            ShiftKey.Tick(keyboardState, mouseState);
            SpaceKey.Tick(keyboardState, mouseState);

            ScrollWheelAxis.Tick(keyboardState, mouseState);
            MouseXAxis.Tick(keyboardState, mouseState);
            MouseYAxis.Tick(keyboardState, mouseState);
        }
    }
}