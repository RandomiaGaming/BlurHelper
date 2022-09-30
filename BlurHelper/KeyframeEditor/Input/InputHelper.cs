namespace BlurHelper
{
    public static class InputHelper
    {
        #region Buttons
        public static VirtualButton D = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D); });
        public static VirtualButton A = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A); });
        public static VirtualButton S = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S); });
        public static VirtualButton L = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L); });
        public static VirtualButton Right = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right); });
        public static VirtualButton Left = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left); });
        public static VirtualButton LeftClick = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed; });
        public static VirtualButton RightClick = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed; });
        public static VirtualButton Side0 = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.XButton1 == Microsoft.Xna.Framework.Input.ButtonState.Pressed; });
        public static VirtualButton Side1 = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.XButton2 == Microsoft.Xna.Framework.Input.ButtonState.Pressed; });
        public static VirtualButton R = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed; });
        public static VirtualButton Delete = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Delete); });
        public static VirtualButton Control = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) || keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl); });
        public static VirtualButton Shift = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) || keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl); });
        public static VirtualButton Space = new VirtualButton((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) || keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl); });
        #endregion
        #region Axises
        public static VirtualAxis Scroll = new VirtualAxis((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.ScrollWheelValue; });
        public static VirtualAxis X = new VirtualAxis((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.X; });
        public static VirtualAxis Y = new VirtualAxis((Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState) => { return mouseState.Y; });
        #endregion
        public static void Tick()
        {
            Microsoft.Xna.Framework.Input.KeyboardState keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            D.Tick(keyboardState, mouseState);
            A.Tick(keyboardState, mouseState);
            S.Tick(keyboardState, mouseState);
            L.Tick(keyboardState, mouseState);
            Right.Tick(keyboardState, mouseState);
            Left.Tick(keyboardState, mouseState);
            LeftClick.Tick(keyboardState, mouseState);
            RightClick.Tick(keyboardState, mouseState);
            Side0.Tick(keyboardState, mouseState);
            Side1.Tick(keyboardState, mouseState);
            R.Tick(keyboardState, mouseState);
            Delete.Tick(keyboardState, mouseState);
            Control.Tick(keyboardState, mouseState);
            Shift.Tick(keyboardState, mouseState);
            Space.Tick(keyboardState, mouseState);

            Scroll.Tick(keyboardState, mouseState);
            X.Tick(keyboardState, mouseState);
            Y.Tick(keyboardState, mouseState);
        }
    }
}
