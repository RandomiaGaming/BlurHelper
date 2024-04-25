namespace BlurHelper
{
    public sealed class VirtualAxis
    {
        private int _history = 0;
        private AxisQueryEvent _axisQueryEvent = null;
        public int Value { get; private set; } = 0;
        public int Delta { get; private set; } = 0;
        public VirtualAxis(AxisQueryEvent axisQueryEvent)
        {
            if (axisQueryEvent is null)
            {
                throw new System.Exception("axisQueryEvent cannot be null.");
            }
            _axisQueryEvent = axisQueryEvent;
        }
        public void Tick(Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState)
        {
            Value = _axisQueryEvent.Invoke(keyboardState, mouseState);
            Delta = Value - _history;
            _history = Value;
        }
        public static implicit operator bool(VirtualAxis virtualAxis)
        {
            return !(virtualAxis.Delta is 0);
        }
        public static implicit operator int(VirtualAxis virtualAxis)
        {
            return virtualAxis.Value;
        }
    }
}