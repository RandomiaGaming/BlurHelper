namespace BlurHelper
{
    public sealed class SaveData
    {
        public uint FrameIndex = 0;
        public System.Collections.Generic.List<KeyFrame> KeyFrames = new System.Collections.Generic.List<KeyFrame>();
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
        public BlurPath(string serializedData)
        {
            if (serializedData is null)
            {
                throw new System.Exception("serializedData cannot be null.");
            }
            else if (serializedData is "")
            {
                throw new System.Exception("serializedData cannot be empty.");
            }
            string[] valueStrings = serializedData.Split(';');
            if (valueStrings.Length is 0)
            {
                throw new System.Exception("serializedData was invalid.");
            }
            try
            {
                FrameIndex = uint.Parse(valueStrings[0]);
                KeyFrames = new System.Collections.Generic.List<KeyFrame>();
                for (int i = 1; i < valueStrings.Length; i++)
                {
                    KeyFrames.Add(new KeyFrame(valueStrings[i]));
                }
            }
            catch
            {
                throw new System.Exception("serializedData was invalid.");
            }
        }
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