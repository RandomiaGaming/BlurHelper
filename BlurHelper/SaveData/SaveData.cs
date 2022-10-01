namespace BlurHelper
{
    public sealed class SaveData
    {
        #region Public Variables
        public readonly uint FrameIndex = 0;
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
                    if (KeyFrames[i] != b.KeyFrames[i])
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
            if(a.FrameIndex != b.FrameIndex)
            {
                return false;
            }
            if (a.KeyFrames is null != b.KeyFrames is null)
            {
                return false;
            }
            if(!(a.KeyFrames is null))
            {
                if(a.KeyFrames.Count != b.KeyFrames.Count)
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
            string[] valueStrings = serializedString.Split(';');

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

        public override string ToString()
        {
            string output = $"{FrameIndex};";
            for (int i = 0; i < KeyFrames.Count; i++)
            {
                output += $"\n{KeyFrames[i]}";
            }
            return output;
        }
        public FrameData GetFrameData(uint targetFrameID)
        {
            KeyFrame next = GetNextKeyFrame(targetFrameID, true);
            KeyFrame previous = GetPreviousKeyFrame(targetFrameID, true);
            if (next is null && previous is null)
            {
                return null;
            }
            if (next is null)
            {
                return new FrameData(previous.BlurPositionX, previous.BlurPositionY, previous.BlurSize);
            }
            else if (previous is null || previous.ExitMarker)
            {
                return null;
            }
            else if (next.FrameIndex == targetFrameID)
            {
                return new FrameData(next.BlurPositionX, next.BlurPositionY, next.BlurSize);
            }
            else
            {
                double outputBlurPositionX = MathHelper.Scale(targetFrameID, previous.FrameIndex, next.FrameIndex, previous.BlurPositionX, next.BlurPositionX);
                double outputBlurPositionY = MathHelper.Scale(targetFrameID, previous.FrameIndex, next.FrameIndex, previous.BlurPositionY, next.BlurPositionY);
                double outputBlurSize = MathHelper.Scale(targetFrameID, previous.FrameIndex, next.FrameIndex, previous.BlurSize, next.BlurSize);
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
            foreach (KeyFrame keyFrame in KeyFrames)
            {
                if (keyFrame.FrameIndex == keyFrame.FrameIndex)
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
            foreach (KeyFrame existingKeyFrame in KeyFrames)
            {
                if (keyFrame.FrameIndex == keyFrame.FrameIndex)
                {
                    existingKeyFrame.BlurPositionX = keyFrame.BlurPositionX;
                    existingKeyFrame.BlurPositionY = keyFrame.BlurPositionY;
                    existingKeyFrame.BlurSize = keyFrame.BlurSize;
                    existingKeyFrame.ExitMarker = keyFrame.ExitMarker;
                    return true;
                }
            }
            KeyFrames.Add(keyFrame);
            return false;
        }
    }
}