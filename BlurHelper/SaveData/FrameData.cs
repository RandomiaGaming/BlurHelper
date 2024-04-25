namespace BlurHelper
{
    public sealed class FrameData
    {
        #region Public Variables
        public ushort BlurPositionX;
        public ushort BlurPositionY;
        public ushort BlurSize;
        #endregion
        #region Public Constructors
        public FrameData(ushort blurPositionX, ushort blurPositionY, ushort blurSize)
        {
            BlurPositionX = blurPositionX;
            BlurPositionY = blurPositionY;
            BlurSize = blurSize;
        }
        #endregion
        #region Public Overrides
        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != typeof(FrameData))
            {
                return false;
            }
            FrameData a = (FrameData)obj;
            return BlurPositionX == a.BlurPositionX && BlurPositionY == a.BlurPositionY && BlurSize == a.BlurSize;
        }
        public override string ToString()
        {
            return $"BlurHelper.FrameData({BlurPositionX}, {BlurPositionY}, {BlurSize})";
        }
        #endregion
        #region Public Opperators
        public static bool operator ==(FrameData a, FrameData b)
        {
            return a.BlurPositionX == b.BlurPositionX && a.BlurPositionY == b.BlurPositionY && a.BlurSize == b.BlurSize;
        }
        public static bool operator !=(FrameData a, FrameData b)
        {
            return a.BlurPositionX != b.BlurPositionX || a.BlurPositionY != b.BlurPositionY || a.BlurSize != b.BlurSize;
        }
        #endregion
        #region Public Static Methods
        public static FrameData Deserialize(string serializedString)
        {
            if (serializedString is null)
            {
                throw new System.Exception("serializedData cannot be null.");
            }
            else if (serializedString is "")
            {
                throw new System.Exception("serializedData cannot be empty.");
            }
            string[] valueStrings = serializedString.Split(':');
            if (valueStrings.Length is 3)
            {
                try
                {
                    return new FrameData(ushort.Parse(valueStrings[0]), ushort.Parse(valueStrings[1]), ushort.Parse(valueStrings[2]));
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
        public static string Serialize(FrameData frameData)
        {
            return $"{frameData.BlurPositionX}:{frameData.BlurPositionY}:{frameData.BlurSize}";
        }
        #endregion
    }
}