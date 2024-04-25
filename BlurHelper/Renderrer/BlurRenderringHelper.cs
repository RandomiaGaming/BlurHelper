namespace BlurHelper
{
    public static class BlurRenderringHelper
    {
        public static void Render(string inputFrameDirectoryPath, string outputFrameDirectoryPath, uint frameCount, System.Drawing.Size lowresSize, bool deleteSource, string saveDataPath)
        {
            System.Console.WriteLine("Beginning opperation...");

            CPUHelper.SetAffinityHighest();

            CPUHelper.SetPriorityHighest();

            SaveData saveData = SaveData.Deserialize(System.IO.File.ReadAllText(saveDataPath));

            RenderFrameInputPacket[] parameters = new RenderFrameInputPacket[frameCount];

            for (uint i = 0; i < frameCount; i++)
            {
                parameters[i].sourceFramePath = $"{inputFrameDirectoryPath}\\{i + 1}.png";
                parameters[i].destinationFramePath = $"{outputFrameDirectoryPath}\\{i + 1}.png";
                parameters[i].frameData = saveData.GetFrameData(i);
                parameters[i].lowresSize = lowresSize;
                parameters[i].deleteSource = deleteSource;
            }

            System.Console.WriteLine("Beginning render...");

            ThreadingHelper.RunParamLambdas<RenderFrameInputPacket>((RenderFrameInputPacket renderFrameInputPacket) =>
            {
                RenderFrame(renderFrameInputPacket.sourceFramePath, renderFrameInputPacket.destinationFramePath, renderFrameInputPacket.frameData, renderFrameInputPacket.lowresSize, renderFrameInputPacket.deleteSource);
            }, parameters, (RenderFrameInputPacket parameter, int completedParameterCount, int totalParameterCount) =>
            {
                System.Console.ForegroundColor = System.ConsoleColor.White;
                System.Console.WriteLine($"Renderred frame \"{parameter.sourceFramePath}\". \"{completedParameterCount}\" of \"{totalParameterCount}\" complete.");
            }, (RenderFrameInputPacket parameter, System.Exception exception, int completedParameterCount, int totalParameterCount) =>
            {
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.WriteLine($"Failed to render frame \"{parameter.sourceFramePath}\" due to exception \"{exception.Message}\". \"{completedParameterCount}\" of \"{totalParameterCount}\" complete.");
                System.IO.File.WriteAllText(parameter.destinationFramePath + ".txt", $"Failed to render frame \"{parameter.sourceFramePath}\" due to exception \"{exception.Message}\". \"{completedParameterCount}\" of \"{totalParameterCount}\" complete.");
            }, 1, 100);
        }
        private struct RenderFrameInputPacket
        {
            public string sourceFramePath;
            public string destinationFramePath;
            public FrameData frameData;
            public System.Drawing.Size lowresSize;
            public bool deleteSource;
        }
        public static void RenderFrame(string sourceFramePath, string destinationFramePath, FrameData frameData, System.Drawing.Size lowresSize, bool deleteSource)
        {
            if (frameData is null)
            {
                if (deleteSource)
                {
                    System.IO.File.Move(sourceFramePath, destinationFramePath);
                }
                else
                {
                    System.IO.File.Copy(sourceFramePath, destinationFramePath);
                }
                return;
            }

            System.Drawing.Bitmap frame = new System.Drawing.Bitmap(sourceFramePath);

            System.Drawing.Bitmap lowres = new System.Drawing.Bitmap(lowresSize.Width, lowresSize.Height);

            System.Drawing.Graphics lowresGraphics = System.Drawing.Graphics.FromImage(lowres);
            lowresGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            lowresGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            lowresGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            lowresGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            lowresGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            lowresGraphics.DrawImage(frame, new System.Drawing.Rectangle(0, 0, lowres.Width, lowres.Height), new System.Drawing.Rectangle(0, 0, frame.Width, frame.Height), System.Drawing.GraphicsUnit.Pixel);

            lowresGraphics.Dispose();

            System.Drawing.Bitmap pixelated = new System.Drawing.Bitmap(frame.Width, frame.Height);

            System.Drawing.Graphics pixelatedGraphics = System.Drawing.Graphics.FromImage(pixelated);

            pixelatedGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            pixelatedGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            pixelatedGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            pixelatedGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            pixelatedGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            pixelatedGraphics.DrawImage(lowres, new System.Drawing.Rectangle(0, 0, pixelated.Width, pixelated.Height), new System.Drawing.Rectangle(0, 0, lowres.Width, lowres.Height), System.Drawing.GraphicsUnit.Pixel);

            pixelatedGraphics.Dispose();

            lowres.Dispose();

            System.Drawing.Graphics frameGraphics = System.Drawing.Graphics.FromImage(frame);
            frameGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            frameGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            frameGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            frameGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            frameGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            int halfBlurWidth = frameData.BlurSize / 2;

            System.Drawing.Rectangle blurRect = new System.Drawing.Rectangle(frameData.BlurPositionX - halfBlurWidth, frameData.BlurPositionY - halfBlurWidth, frameData.BlurSize, frameData.BlurSize);

            frameGraphics.DrawImage(pixelated, blurRect, blurRect, System.Drawing.GraphicsUnit.Pixel);

            frameGraphics.Dispose();

            pixelated.Dispose();

            frame.Save(destinationFramePath, System.Drawing.Imaging.ImageFormat.Png);

            frame.Dispose();

            if (deleteSource)
            {
                System.IO.File.Delete(sourceFramePath);
            }
        }
    }
}