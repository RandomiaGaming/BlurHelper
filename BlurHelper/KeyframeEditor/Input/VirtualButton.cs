namespace BlurHelper
{
    public sealed class VirtualButton
    {
        private bool _history = false;
        private ButtonQueryEvent _buttonQueryEvent = null;
        public bool Down { get; private set; } = false;
        public bool Pressed { get; private set; } = false;
        public bool Up { get; private set; } = false;
        public VirtualButton(ButtonQueryEvent buttonQueryEvent)
        {
            if(buttonQueryEvent is null)
            {
                throw new System.Exception("buttonQueryEvent cannot be null.");
            }
            _buttonQueryEvent = buttonQueryEvent;
        }
        public void Tick(Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState)
        {
            Pressed = _buttonQueryEvent.Invoke(keyboardState, mouseState);
            if(_history && !Pressed)
            {
                Up = true;
            }
            else
            {
                Up = false;
            }
            if(!_history && Pressed)
            {
                Down = true;
            }
            else
            {
                Down = false;
            }
            _history = Pressed;
        }
        public static implicit operator bool(VirtualButton virtualButton)
        {
            return virtualButton.Pressed;
        }
    }
}