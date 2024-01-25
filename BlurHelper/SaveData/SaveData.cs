namespace BlurHelper
{
    public sealed class SaveData
    {
        #region Public Variables
        public uint FrameIndex = 0;
        public System.Collections.Generic.List<KeyFrame> KeyFrames = new System.Collections.Generic.List<KeyFrame>();
        #endregion
        #region Public Constructors
        public SaveData(uint frameIndex, System.Collections.Generic.List<KeyFrame> keyFrames)
        {
            FrameIndex = frameIndex;
            if (keyFrames is null)
            {
                throw new System.Exception("keyFrames cannot be null.");
            }
            foreach (KeyFrame keyFrame in keyFrames)
            {
                if (keyFrame is null)
                {
                    throw new System.Exception("keyFrames cannot contain null.");
                }
            }
            KeyFrames = keyFrames;
        }
        #endregion
        #region Public Overrides
        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != typeof(SaveData))
            {
                return false;
            }
            SaveData a = (SaveData)obj;
            if (FrameIndex != a.FrameIndex)
            {
                return false;
            }
            if (KeyFrames is null != a.KeyFrames is null)
            {
                return false;
            }
            if (!(KeyFrames is null))
            {
                if (KeyFrames.Count != a.KeyFrames.Count)
                {
                    return false;
                }
                for (int i = 0; i < KeyFrames.Count; i++)
                {
                    if (KeyFrames[i] != a.KeyFrames[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public override string ToString()
        {
            return $"BlurHelper.SaveData({FrameIndex}, {KeyFrames.Count})";
        }
        #endregion
        #region Public Opperators
        public static bool operator ==(SaveData a, SaveData b)
        {
            if (a.FrameIndex != b.FrameIndex)
            {
                return false;
            }
            if (a.KeyFrames is null != b.KeyFrames is null)
            {
                return false;
            }
            if (!(a.KeyFrames is null))
            {
                if (a.KeyFrames.Count != b.KeyFrames.Count)
                {
                    return false;
                }
                for (int i = 0; i < a.KeyFrames.Count; i++)
                {
                    if (a.KeyFrames[i] != b.KeyFrames[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool operator !=(SaveData a, SaveData b)
        {
            if (a.FrameIndex != b.FrameIndex)
            {
                return true;
            }
            if (a.KeyFrames is null != b.KeyFrames is null)
            {
                return true;
            }
            if (!(a.KeyFrames is null))
            {
                if (a.KeyFrames.Count != b.KeyFrames.Count)
                {
                    return true;
                }
                for (int i = 0; i < a.KeyFrames.Count; i++)
                {
                    if (a.KeyFrames[i] != b.KeyFrames[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
        #region Public Static Methods
        public static SaveData Deserialize(string serializedString)
        {
            if (serializedString is null)
            {
                throw new System.Exception("serializedString cannot be null.");
            }
            else if (serializedString is "")
            {
                throw new System.Exception("serializedString cannot be empty.");
            }
            string[] keyFrameStrings = serializedString.Split(';');
            if (keyFrameStrings.Length is 0 || !(keyFrameStrings[keyFrameStrings.Length - 1] is ""))
            {
                throw new System.Exception("serializedData was invalid.");
            }
            try
            {
                System.Collections.Generic.List<KeyFrame> keyFrames = new System.Collections.Generic.List<KeyFrame>();
                for (int i = 1; i < keyFrameStrings.Length - 1; i++)
                {
                    string keyFrameString = keyFrameStrings[i];
                    while (!(keyFrameString.Length is 0))
                    {
                        char trailingChar = keyFrameString[keyFrameString.Length - 1];
                        if (trailingChar is ' ' || trailingChar is '\n' || trailingChar is '\t' || trailingChar is '\r')
                        {
                            keyFrameString = keyFrameString.Substring(0, keyFrameString.Length - 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (!(keyFrameString.Length is 0))
                    {
                        char leadingChar = keyFrameString[0];
                        if (leadingChar is ' ' || leadingChar is '\n' || leadingChar is '\t' || leadingChar is '\r')
                        {
                            keyFrameString = keyFrameString.Substring(1, keyFrameString.Length - 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (i == 5261)
                    {

                    }
                    keyFrames.Add(KeyFrame.Deserialize(keyFrameString));
                }
                return new SaveData(uint.Parse(keyFrameStrings[0]), keyFrames);
            }
            catch
            {
                throw new System.Exception("serializedData was invalid.");
            }
        }
        public static string Serialize(SaveData saveData)
        {
            string output = $"{saveData.FrameIndex};";
            for (int i = 0; i < saveData.KeyFrames.Count; i++)
            {
                output += $"\n{KeyFrame.Serialize(saveData.KeyFrames[i])};";
            }
            return output;
        }
        #endregion
        #region Public Methods
        public FrameData GetFrameData(uint targetFrameID)
        {
            KeyFrame next = GetNextKeyFrame(targetFrameID, true);
            KeyFrame previous = GetPreviousKeyFrame(targetFrameID, true);
            if (previous is null || (previous.ExitMarker && previous.FrameIndex != targetFrameID))
            {
                return null;
            }
            else if (previous.FrameIndex == targetFrameID || next is null)
            {
                return new FrameData(previous.BlurPositionX, previous.BlurPositionY, previous.BlurSize);
            }
            else
            {
                ushort outputBlurPositionX = (ushort)(MathHelper.Scale(targetFrameID, previous.FrameIndex, next.FrameIndex, previous.BlurPositionX, next.BlurPositionX) + 0.5);
                ushort outputBlurPositionY = (ushort)(MathHelper.Scale(targetFrameID, previous.FrameIndex, next.FrameIndex, previous.BlurPositionY, next.BlurPositionY) + 0.5);
                ushort outputBlurSize = (ushort)(MathHelper.Scale(targetFrameID, previous.FrameIndex, next.FrameIndex, previous.BlurSize, next.BlurSize) + 0.5);
                return new FrameData(outputBlurPositionX, outputBlurPositionY, outputBlurSize);
            }
        }
        public KeyFrame GetNextKeyFrame(uint frameIndex, bool includeOverlaps)
        {
            KeyFrame closestMatch = null;
            foreach (KeyFrame keyFrame in KeyFrames)
            {
                if (keyFrame.FrameIndex == frameIndex && includeOverlaps)
                {
                    return keyFrame;
                }
                else if (keyFrame.FrameIndex > frameIndex && (closestMatch is null || keyFrame.FrameIndex < closestMatch.FrameIndex))
                {
                    closestMatch = keyFrame;
                }
            }
            return closestMatch;
        }
        public KeyFrame GetPreviousKeyFrame(uint frameIndex, bool includeOverlaps)
        {
            KeyFrame closestMatch = null;
            foreach (KeyFrame keyFrame in KeyFrames)
            {
                if (keyFrame.FrameIndex == frameIndex && includeOverlaps)
                {
                    return keyFrame;
                }
                else if (keyFrame.FrameIndex < frameIndex && (closestMatch is null || keyFrame.FrameIndex > closestMatch.FrameIndex))
                {
                    closestMatch = keyFrame;
                }
            }
            return closestMatch;
        }
        public void AddKeyFrame(KeyFrame keyFrame)
        {
            foreach (KeyFrame keyFrameInKeyFrames in KeyFrames)
            {
                if (keyFrame.FrameIndex == keyFrameInKeyFrames.FrameIndex)
                {
                    throw new System.Exception("KeyFrame with given FrameIndex already exists.");
                }
            }
            KeyFrames.Add(keyFrame);
        }
        public bool DeleteKeyFrame(uint frameIndex)
        {
            for (int i = 0; i < KeyFrames.Count; i++)
            {
                if (KeyFrames[i].FrameIndex == frameIndex)
                {
                    KeyFrames.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public KeyFrame GetKeyFrame(uint frameIndex)
        {
            foreach (KeyFrame keyFrame in KeyFrames)
            {
                if (keyFrame.FrameIndex == frameIndex)
                {
                    return keyFrame;
                }
            }
            return null;
        }
        public bool ModifyOrAddKeyFrame(KeyFrame keyFrame)
        {
            foreach (KeyFrame keyFrameInKeyFrames in KeyFrames)
            {
                if (keyFrame.FrameIndex == keyFrameInKeyFrames.FrameIndex)
                {
                    keyFrameInKeyFrames.BlurPositionX = keyFrame.BlurPositionX;
                    keyFrameInKeyFrames.BlurPositionY = keyFrame.BlurPositionY;
                    keyFrameInKeyFrames.BlurSize = keyFrame.BlurSize;
                    keyFrameInKeyFrames.ExitMarker = keyFrame.ExitMarker;
                    return true;
                }
            }
            KeyFrames.Add(keyFrame);
            return false;
        }
        #endregion
    }
}