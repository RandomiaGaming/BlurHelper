namespace BlurHelper
{
    public sealed class KeyFrame
    {
        #region Public Variables
        public readonly uint FrameIndex;
        public readonly double BlurPositionX;
        public readonly double BlurPositionY;
        public readonly double BlurSize;
        public readonly bool ExitMarker;
        #endregion
        #region Public Constructors
        public KeyFrame(uint frameIndex, double blurPositionX, double blurPositionY, double blurSize, bool exitMarker)
        {
            FrameIndex = frameIndex;
            if (blurPositionX is double.NaN)
            {
                throw new System.Exception("blurPositionX cannot be NaN.");
            }
            if (blurPositionX < 0 || blurPositionX > 1)
            {
                throw new System.Exception("blurPositionX must be between 0 and 1.");
            }
            BlurPositionX = blurPositionX;
            if (blurPositionY is double.NaN)
            {
                throw new System.Exception("blurPositionY cannot be NaN.");
            }
            if (blurPositionY < 0 || blurPositionY > 1)
            {
                throw new System.Exception("blurPositionY must be between 0 and 1.");
            }
            BlurPositionY = blurPositionY;
            if (blurSize is double.NaN)
            {
                throw new System.Exception("blurSize cannot be NaN.");
            }
            if (blurPositionX < 0 || blurPositionX > 1)
            {
                throw new System.Exception("blurSize must be between 0 and 1.");
            }
            BlurSize = blurSize;
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
            else if (serializedString.Contains(";"))
            {
                throw new System.Exception("serializedString was invalid.");
            }
            string[] valueStrings = serializedString.Split(':');
            if (valueStrings.Length is 4)
            {
                try
                {
                    return new KeyFrame(uint.Parse(valueStrings[0]), double.Parse(valueStrings[1]), double.Parse(valueStrings[2]), double.Parse(valueStrings[3]), false);
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
                    return new KeyFrame(uint.Parse(valueStrings[0]), double.Parse(valueStrings[1]), double.Parse(valueStrings[2]), double.Parse(valueStrings[3]), true);
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
                return $"{keyFrameData.FrameIndex}:{keyFrameData.BlurPositionX}:{keyFrameData.BlurPositionY}:{keyFrameData.BlurSize}:ExitMarker;";
            }
            else
            {
                return $"{keyFrameData.FrameIndex}:{keyFrameData.BlurPositionX}:{keyFrameData.BlurPositionY}:{keyFrameData.BlurSize};";
            }
        }
        #endregion
    }
}