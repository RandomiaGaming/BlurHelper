namespace BlurHelper
{
    public sealed class KeyFrame
    {
        #region Public Variables
        public uint FrameIndex;
        public ushort BlurPositionX;
        public ushort BlurPositionY;
        public ushort BlurSize;
        public bool ExitMarker;
        #endregion
        #region Public Constructors
        public KeyFrame(uint frameIndex, ushort blurPositionX, ushort blurPositionY, ushort blurSize, bool exitMarker)
        {
            FrameIndex = frameIndex;
            BlurPositionX = blurPositionX;
            BlurPositionY = blurPositionY;
            BlurSize = blurSize;
            ExitMarker = exitMarker;
        }
        #endregion
        #region Public Overrides
        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != typeof(KeyFrame))
            {
                return false;
            }
            KeyFrame a = (KeyFrame)obj;
            return FrameIndex == a.FrameIndex && BlurPositionX == a.BlurPositionX && BlurPositionY == a.BlurPositionY && BlurSize == a.BlurSize && ExitMarker == a.ExitMarker;
        }
        public override string ToString()
        {
            return $"BlurHelper.KeyFrameData({BlurPositionX}, {BlurPositionY}, {BlurSize}, {ExitMarker})";
        }
        #endregion
        #region Public Opperators
        public static bool operator ==(KeyFrame a, KeyFrame b)
        {
            return a.FrameIndex == b.FrameIndex && a.BlurPositionX == b.BlurPositionX && a.BlurPositionY == b.BlurPositionY && a.BlurSize == b.BlurSize && a.ExitMarker == b.ExitMarker;
        }
        public static bool operator !=(KeyFrame a, KeyFrame b)
        {
            return a.FrameIndex != b.FrameIndex || a.BlurPositionX != b.BlurPositionX || a.BlurPositionY != b.BlurPositionY || a.BlurSize != b.BlurSize || a.ExitMarker != b.ExitMarker;
        }
        #endregion
        #region Public Static Methods
        public static KeyFrame Deserialize(string serializedString)
        {
            if (serializedString is null)
            {
                throw new System.Exception("serializedString cannot be null.");
            }
            else if (serializedString is "")
            {
                throw new System.Exception("serializedString cannot be empty.");
            }
            string[] valueStrings = serializedString.Split(':');
            if (valueStrings.Length is 4)
            {
                try
                {
                    return new KeyFrame(uint.Parse(valueStrings[0]), ushort.Parse(valueStrings[1]), ushort.Parse(valueStrings[2]), ushort.Parse(valueStrings[3]), false);
                }
                catch
                {
                    throw new System.Exception("serializedData was invalid.");
                }
            }
            else if (valueStrings.Length is 5)
            {
                if (!(valueStrings[4].ToLower() is "exitmarker"))
                {
                    throw new System.Exception("serializedData was invalid.");
                }
                try
                {
                    return new KeyFrame(uint.Parse(valueStrings[0]), ushort.Parse(valueStrings[1]), ushort.Parse(valueStrings[2]), ushort.Parse(valueStrings[3]), true);
                }
                catch
                {
                    throw new System.Exception("serializedData was invalid.");
                }
            }
            else
            {
                throw new System.Exception("serializedData was invalid.");
            }
        }
        public static string Serialize(KeyFrame keyFrameData)
        {
            if (keyFrameData.ExitMarker)
            {
                return $"{keyFrameData.FrameIndex}:{keyFrameData.BlurPositionX}:{keyFrameData.BlurPositionY}:{keyFrameData.BlurSize}:ExitMarker";
            }
            else
            {
                return $"{keyFrameData.FrameIndex}:{keyFrameData.BlurPositionX}:{keyFrameData.BlurPositionY}:{keyFrameData.BlurSize}";
            }
        }
        #endregion
    }
}