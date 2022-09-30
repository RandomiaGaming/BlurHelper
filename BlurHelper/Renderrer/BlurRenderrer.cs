namespace BlurHelper
{
    public static class BlurRenderrer
    {
        public static void Render(string videoFrameDirectoryPath, BlurPathData blurPathData)
        {
            //Get frame data
            FrameData frameData = Program.CurrentSaveData.GetFrameData(targetFrameID);
            //Return original if no blur position set
            if (frameData is null)
            {
                System.IO.File.Copy($"E:\\Play\\Frames\\{targetFrameID + 1}.png", $"E:\\Play\\Renders\\{targetFrameID + 1}.png");
                return;
            }
            //Load new frame
            System.Drawing.Bitmap canvas = new System.Drawing.Bitmap($"E:\\Play\\Frames\\{targetFrameID + 1}.png");
            //Create new canvas
            System.Drawing.Graphics canvasGraphics = System.Drawing.Graphics.FromImage(canvas);
            //Apply canvas settings
            canvasGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            canvasGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            canvasGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            canvasGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            canvasGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            //Update minimap
            minimapGraphics.DrawImage(canvas, miniRect, fullRect, System.Drawing.GraphicsUnit.Pixel);
            //Update blurmap
            blurmapGraphics.DrawImage(minimap, fullRect, miniRect, System.Drawing.GraphicsUnit.Pixel);
            //Calculate halfBlurWidth
            int halfBlurWidth = frameData.Size / 2;
            //Calculate blurRect
            System.Drawing.Rectangle blurRect = new System.Drawing.Rectangle(frameData.PositionX - halfBlurWidth, frameData.PositionY - halfBlurWidth, frameData.Size, frameData.Size);
            //Blitz blurmap to canvas
            canvasGraphics.DrawImage(blurmap, blurRect, blurRect, System.Drawing.GraphicsUnit.Pixel);
            //Save canvas
            canvas.Save($"E:\\Play\\Renders\\{targetFrameID + 1}.png", System.Drawing.Imaging.ImageFormat.Png);
            //Dispose old canvas
            canvasGraphics.Dispose();
            //Dispose old frame
            canvas.Dispose();
        }
    }
}