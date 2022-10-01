namespace BlurHelper
{
    public sealed class FrameData
    {
        #region Public Variables
        public double BlurPositionX;
        public double BlurPositionY;
        public double BlurSize;
        #endregion
        #region Public Constructors
        public FrameData(double blurPositionX, double blurPositionY, double blurSize)
        {
            if(blurPositionX is double.NaN)
            {
                throw new System.Exception("blurPositionX cannot be NaN.");
            }
            if(blurPositionX < 0 || blurPositionX > 1)
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
            else if (serializedString.Contains(";"))
            {
                throw new System.Exception("serializedData was invalid.");
            }
            if (serializedString[serializedString.Length - 1] is '\n')
            {
                serializedString = serializedString.Substring(0, serializedString.Length - 1);
            }
            string[] valueStrings = serializedString.Split(':');
            if (valueStrings.Length is 3)
            {
                try
                {
                    return new FrameData(double.Parse(valueStrings[0]), double.Parse(valueStrings[1]), double.Parse(valueStrings[2]));
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
            return $"{frameData.BlurPositionX}:{frameData.BlurPositionY}:{frameData.BlurSize};";
        }
        #endregion
    }
}