namespace BlurHelper
{
    public class BlurRenderrerGame : Microsoft.Xna.Framework.Game
    {
        private Microsoft.Xna.Framework.GraphicsDeviceManager _XNAGraphicsDeviceManager = null;
        private Microsoft.Xna.Framework.Graphics.GraphicsDevice _XNAGraphicsDevice = null;
        private Microsoft.Xna.Framework.GameWindow _XNAGameWindow = null;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _XNASpriteBatch = null;
        private Microsoft.Xna.Framework.Rectangle _XNAViewportRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 1920, 1080);

        private Microsoft.Xna.Framework.Graphics.Texture2D _CurrentFrame = null;

        private Microsoft.Xna.Framework.Graphics.RenderTarget2D _Minimap = null;
        private Microsoft.Xna.Framework.Graphics.RenderTarget2D _Blurmap = null;
        private Microsoft.Xna.Framework.Graphics.RenderTarget2D _OutputCanvas = null;

        private Microsoft.Xna.Framework.Graphics.Texture2D _SelectionTexture = null;

        private Microsoft.Xna.Framework.Rectangle _FullRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 1920, 1080);
        private Microsoft.Xna.Framework.Rectangle _MiniRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 192, 108);

        public int _viewportWidth = 1920;
        public int _viewportHeight = 1080;

        private uint currentFrameID = 122168;
        public BlurRenderrerGame()
        {
            InactiveSleepTime = new System.TimeSpan(0);
            TargetElapsedTime = new System.TimeSpan(10000000 / 60);
            MaxElapsedTime = new System.TimeSpan(long.MaxValue);
            IsFixedTimeStep = false;
            IsMouseVisible = true;

            _XNAGraphicsDeviceManager = new Microsoft.Xna.Framework.GraphicsDeviceManager(this);
            _XNAGraphicsDeviceManager.GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile.Reach;
            _XNAGraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            _XNAGraphicsDeviceManager.HardwareModeSwitch = false;
            _XNAGraphicsDeviceManager.IsFullScreen = false;
            _XNAGraphicsDeviceManager.PreferHalfPixelOffset = false;
            _XNAGraphicsDeviceManager.PreferredBackBufferFormat = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color;
            _XNAGraphicsDeviceManager.SupportedOrientations = Microsoft.Xna.Framework.DisplayOrientation.LandscapeLeft | Microsoft.Xna.Framework.DisplayOrientation.LandscapeRight | Microsoft.Xna.Framework.DisplayOrientation.Portrait | Microsoft.Xna.Framework.DisplayOrientation.PortraitDown | Microsoft.Xna.Framework.DisplayOrientation.Unknown | Microsoft.Xna.Framework.DisplayOrientation.Default;
            _XNAGraphicsDeviceManager.ApplyChanges();

            _XNAGraphicsDevice = GraphicsDevice;

            _XNAGraphicsDevice.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend;
            _XNAGraphicsDevice.DepthStencilState = Microsoft.Xna.Framework.Graphics.DepthStencilState.None;
            _XNAGraphicsDevice.RasterizerState = Microsoft.Xna.Framework.Graphics.RasterizerState.CullNone;

            _XNAGameWindow = Window;

            _XNAGameWindow.AllowAltF4 = true;
            _XNAGameWindow.AllowUserResizing = true;
            _XNAGameWindow.IsBorderless = false;
            _XNAGameWindow.Position = new Microsoft.Xna.Framework.Point(_XNAGraphicsDevice.Adapter.CurrentDisplayMode.Width / 4, _XNAGraphicsDevice.Adapter.CurrentDisplayMode.Height / 4);
            _XNAGameWindow.Title = "Blur Renderrer";

            _XNAGameWindow.ClientSizeChanged += ResizeCallback;

            _viewportWidth = _XNAGraphicsDevice.Viewport.Width;
            _viewportHeight = _XNAGraphicsDevice.Viewport.Height;
            _XNAViewportRect = new Microsoft.Xna.Framework.Rectangle(0, 0, _viewportWidth, _viewportHeight);

            _XNAGraphicsDeviceManager.PreferredBackBufferWidth = _viewportWidth;
            _XNAGraphicsDeviceManager.PreferredBackBufferHeight = _viewportHeight;
            _XNAGraphicsDeviceManager.ApplyChanges();

            _XNASpriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(_XNAGraphicsDevice);
            _XNASpriteBatch.Name = "Main Sprite Batch";
            _XNASpriteBatch.Tag = null;

            _CurrentFrame = new Microsoft.Xna.Framework.Graphics.Texture2D(_XNAGraphicsDevice, 1920, 1080);

            _Minimap = new Microsoft.Xna.Framework.Graphics.RenderTarget2D(_XNAGraphicsDevice, 192, 108);
            _Blurmap = new Microsoft.Xna.Framework.Graphics.RenderTarget2D(_XNAGraphicsDevice, 1920, 1080);
            _OutputCanvas = new Microsoft.Xna.Framework.Graphics.RenderTarget2D(_XNAGraphicsDevice, 1920, 1080);

            _SelectionTexture = Microsoft.Xna.Framework.Graphics.Texture2D.FromFile(_XNAGraphicsDevice, $"E:\\Play\\Icons\\Selection.png");

            Program.Load();
        }
        private void ResizeCallback(object sender, System.EventArgs e)
        {
            if (_XNAGraphicsDeviceManager.IsFullScreen)
            {
                _viewportWidth = _XNAGraphicsDevice.Adapter.CurrentDisplayMode.Width;
                _viewportHeight = _XNAGraphicsDevice.Adapter.CurrentDisplayMode.Height;
            }
            else
            {
                _viewportWidth = _XNAGraphicsDevice.Viewport.Width;
                _viewportHeight = _XNAGraphicsDevice.Viewport.Height;
            }

            _XNAViewportRect.Width = _viewportWidth;
            _XNAViewportRect.Height = _viewportHeight;

            if (_XNAGraphicsDeviceManager.PreferredBackBufferWidth != _viewportWidth || _XNAGraphicsDeviceManager.PreferredBackBufferHeight != _viewportHeight)
            {
                _XNAGraphicsDeviceManager.PreferredBackBufferWidth = _viewportWidth;
                _XNAGraphicsDeviceManager.PreferredBackBufferHeight = _viewportHeight;
                _XNAGraphicsDeviceManager.ApplyChanges();
            }
        }
        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _CurrentFrame = Microsoft.Xna.Framework.Graphics.Texture2D.FromFile(_XNAGraphicsDevice, $"E:\\Play\\Frames\\{currentFrameID + 1}.png");

            FrameData currentFrameData = Program.CurrentSaveData.GetFrameData(currentFrameID);

            _XNAGraphicsDevice.SetRenderTarget(_Minimap);

            _XNASpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied, Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp, null, null, null, null);

            _XNASpriteBatch.Draw(_CurrentFrame, _MiniRect, _FullRect, Microsoft.Xna.Framework.Color.White);

            _XNASpriteBatch.End();

            _XNAGraphicsDevice.SetRenderTarget(_Blurmap);

            _XNASpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied, Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp, null, null, null, null);

            _XNASpriteBatch.Draw(_Minimap, _FullRect, _MiniRect, Microsoft.Xna.Framework.Color.White);

            _XNASpriteBatch.End();

            _XNAGraphicsDevice.SetRenderTarget(_OutputCanvas);

            _XNASpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied, Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp, null, null, null, null);

            _XNASpriteBatch.Draw(_CurrentFrame, _FullRect, _FullRect, Microsoft.Xna.Framework.Color.White);

            int halfBlurWidth = currentFrameData.BlurWidth / 2;

            Microsoft.Xna.Framework.Rectangle BlurRegion = new Microsoft.Xna.Framework.Rectangle(currentFrameData.PositionX - halfBlurWidth, currentFrameData.PositionY - halfBlurWidth, currentFrameData.BlurWidth, currentFrameData.BlurWidth);

            _XNASpriteBatch.Draw(_Blurmap, BlurRegion, BlurRegion, Microsoft.Xna.Framework.Color.White);

            _XNASpriteBatch.End();

            Save(_OutputCanvas, $"E:\\Play\\Renders\\{currentFrameID + 1}.png");

            _XNAGraphicsDevice.SetRenderTarget(null);

            _XNASpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied, Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp, null, null, null, null);

            _XNASpriteBatch.Draw(_OutputCanvas, _XNAViewportRect, _FullRect, Microsoft.Xna.Framework.Color.White);


            if (!(currentFrameData is null))
            {
                int width = (currentFrameData.BlurWidth * _viewportWidth) / 1920;
                int height = (currentFrameData.BlurWidth * _viewportHeight) / 1080;
                int x = (currentFrameData.PositionX * _viewportWidth) / 1920;
                int y = (currentFrameData.PositionY * _viewportHeight) / 1080;

                _XNASpriteBatch.Draw(_SelectionTexture, new Microsoft.Xna.Framework.Rectangle(x - (width / 2), y - (height / 2), width, height), Microsoft.Xna.Framework.Color.White);
            }

            _XNASpriteBatch.End();

            _CurrentFrame.Dispose();

            currentFrameID++;

            System.Console.WriteLine(gameTime.ElapsedGameTime.Ticks / 1000);
        }
        public static void Save(Microsoft.Xna.Framework.Graphics.Texture2D texture2D, string path)
        {
            System.IO.FileStream fileStream = System.IO.File.Open(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);

            texture2D.SaveAsPng(fileStream, texture2D.Width, texture2D.Height);

            fileStream.Close();

            fileStream.Dispose();
        }
    }
}