namespace BlurHelper
{
    public class BlurHelperGame : Microsoft.Xna.Framework.Game
    {
        private Microsoft.Xna.Framework.GraphicsDeviceManager _xnaGraphicsDeviceManager = null;
        private Microsoft.Xna.Framework.Graphics.GraphicsDevice _xnaGraphicsDevice = null;
        private Microsoft.Xna.Framework.GameWindow _xnaGameWindow = null;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _xnaSpriteBatch = null;
        private int _viewportWidth = 1920;
        private int _viewportHeight = 1080;

        private Microsoft.Xna.Framework.Rectangle _xnaViewportRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 1920, 1080);

        private Microsoft.Xna.Framework.Graphics.Texture2D _selectionTexture = null;
        private Microsoft.Xna.Framework.Graphics.Texture2D _keyFrameTexture = null;
        private Microsoft.Xna.Framework.Graphics.Texture2D _exitMarkerKeyFrameTexture = null;

        private Microsoft.Xna.Framework.Graphics.Texture2D _currentFrame = null;
        public BlurHelperGame()
        {
            InactiveSleepTime = new System.TimeSpan(0);
            TargetElapsedTime = new System.TimeSpan(10000000 / 60);
            MaxElapsedTime = new System.TimeSpan(long.MaxValue);
            IsFixedTimeStep = false;
            IsMouseVisible = true;

            _xnaGraphicsDeviceManager = new Microsoft.Xna.Framework.GraphicsDeviceManager(this);
            _xnaGraphicsDeviceManager.GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile.Reach;
            _xnaGraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            _xnaGraphicsDeviceManager.HardwareModeSwitch = false;
            _xnaGraphicsDeviceManager.IsFullScreen = false;
            _xnaGraphicsDeviceManager.PreferHalfPixelOffset = false;
            _xnaGraphicsDeviceManager.PreferredBackBufferFormat = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color;
            _xnaGraphicsDeviceManager.SupportedOrientations = Microsoft.Xna.Framework.DisplayOrientation.LandscapeLeft | Microsoft.Xna.Framework.DisplayOrientation.LandscapeRight | Microsoft.Xna.Framework.DisplayOrientation.Portrait | Microsoft.Xna.Framework.DisplayOrientation.PortraitDown | Microsoft.Xna.Framework.DisplayOrientation.Unknown | Microsoft.Xna.Framework.DisplayOrientation.Default;
            _xnaGraphicsDeviceManager.ApplyChanges();

            _xnaGraphicsDevice = GraphicsDevice;

            _xnaGraphicsDevice.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend;
            _xnaGraphicsDevice.DepthStencilState = Microsoft.Xna.Framework.Graphics.DepthStencilState.None;
            _xnaGraphicsDevice.RasterizerState = Microsoft.Xna.Framework.Graphics.RasterizerState.CullNone;

            _xnaGameWindow = Window;

            _xnaGameWindow.AllowAltF4 = true;
            _xnaGameWindow.AllowUserResizing = true;
            _xnaGameWindow.IsBorderless = false;
            _xnaGameWindow.Position = new Microsoft.Xna.Framework.Point(_xnaGraphicsDevice.Adapter.CurrentDisplayMode.Width / 4, _xnaGraphicsDevice.Adapter.CurrentDisplayMode.Height / 4);
            _xnaGameWindow.Title = "Blur Helper";

            _xnaGameWindow.ClientSizeChanged += ResizeCallback;

            _viewportWidth = _xnaGraphicsDevice.Viewport.Width;
            _viewportHeight = _xnaGraphicsDevice.Viewport.Height;
            _xnaViewportRect = new Microsoft.Xna.Framework.Rectangle(0, 0, _viewportWidth, _viewportHeight);

            _xnaGraphicsDeviceManager.PreferredBackBufferWidth = _viewportWidth;
            _xnaGraphicsDeviceManager.PreferredBackBufferHeight = _viewportHeight;
            _xnaGraphicsDeviceManager.ApplyChanges();

            _xnaSpriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(_xnaGraphicsDevice);
            _xnaSpriteBatch.Name = "Main Sprite Batch";
            _xnaSpriteBatch.Tag = null;

            Program.Load();

            System.Reflection.Assembly assembly = typeof(Program).Assembly;

            _exitMarkerKeyFrameTexture = Microsoft.Xna.Framework.Graphics.Texture2D.FromStream(_xnaGraphicsDevice, assembly.GetManifestResourceStream("BlurHelper.Icons.ExitMarkerKeyFrame.png"));
            _keyFrameTexture = Microsoft.Xna.Framework.Graphics.Texture2D.FromStream(_xnaGraphicsDevice, assembly.GetManifestResourceStream("BlurHelper.Icons.KeyFrame.png"));
            _selectionTexture = Microsoft.Xna.Framework.Graphics.Texture2D.FromStream(_xnaGraphicsDevice, assembly.GetManifestResourceStream("BlurHelper.Icons.Selection.png"));

            ReloadCurrentFrame();
        }
        private void ResizeCallback(object sender, System.EventArgs e)
        {
            if (_xnaGraphicsDeviceManager.IsFullScreen)
            {
                _viewportWidth = _xnaGraphicsDevice.Adapter.CurrentDisplayMode.Width;
                _viewportHeight = _xnaGraphicsDevice.Adapter.CurrentDisplayMode.Height;
            }
            else
            {
                _viewportWidth = _xnaGraphicsDevice.Viewport.Width;
                _viewportHeight = _xnaGraphicsDevice.Viewport.Height;
            }

            _xnaViewportRect.Width = _viewportWidth;
            _xnaViewportRect.Height = _viewportHeight;

            if (_xnaGraphicsDeviceManager.PreferredBackBufferWidth != _viewportWidth || _xnaGraphicsDeviceManager.PreferredBackBufferHeight != _viewportHeight)
            {
                _xnaGraphicsDeviceManager.PreferredBackBufferWidth = _viewportWidth;
                _xnaGraphicsDeviceManager.PreferredBackBufferHeight = _viewportHeight;
                _xnaGraphicsDeviceManager.ApplyChanges();
            }
        }
        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Program.CheckSave();

            InputHelper.Tick();

            if (InputHelper.S.Down && InputHelper.Control && !InputHelper.Space && !InputHelper.Shift)
            {
                Program.Save();
            }
            else if (InputHelper.L.Down && InputHelper.Control && !InputHelper.Space && !InputHelper.Shift)
            {
                Program.Load();
            }
            else if (InputHelper.S.Down && InputHelper.Control && !InputHelper.Space && !InputHelper.Shift)
            {
                Program.CurrentSaveData.CurrentFrameID += 10;

                if (Program.CurrentSaveData.CurrentFrameID > 252432)
                {
                    Program.CurrentSaveData.CurrentFrameID = 252432;
                }

                ReloadCurrentFrame();

                System.Console.WriteLine($"Changed to frame {Program.CurrentSaveData.CurrentFrameID} of {Program.SourceVideoFrameCount} which is %{Program.CurrentSaveData.CurrentFrameID * 100.0 / Program.SourceVideoFrameCount} complete. Only {Program.SourceVideoFrameCount - Program.CurrentSaveData.CurrentFrameID - 4646} frames left!");
            }
            else if (ADown && (!ADownLastFrame || !controlDown))
            {
                Program.CurrentSaveData.CurrentFrameID -= 10;

                if (Program.CurrentSaveData.CurrentFrameID > 2147483647)
                {
                    Program.CurrentSaveData.CurrentFrameID = 0;
                }

                ReloadCurrentFrame();

                System.Console.WriteLine($"Changed to frame {Program.CurrentSaveData.CurrentFrameID} of {Program.SourceVideoFrameCount} which is %{Program.CurrentSaveData.CurrentFrameID * 100.0 / Program.SourceVideoFrameCount} complete. Only {Program.SourceVideoFrameCount - Program.CurrentSaveData.CurrentFrameID - 4646} frames left!");
            }
            else if (spaceDown)
            {
                if (DDown && (!DDownLastFrame || !controlDown))
                {
                    Program.CurrentSaveData.CurrentFrameID += 100;

                    if (Program.CurrentSaveData.CurrentFrameID > 252432)
                    {
                        Program.CurrentSaveData.CurrentFrameID = 252432;
                    }

                    ReloadCurrentFrame();

                    System.Console.WriteLine($"Changed to frame {Program.CurrentSaveData.CurrentFrameID} of {Program.SourceVideoFrameCount} which is %{Program.CurrentSaveData.CurrentFrameID * 100.0 / Program.SourceVideoFrameCount} complete. Only {Program.SourceVideoFrameCount - Program.CurrentSaveData.CurrentFrameID - 4646} frames left!");
                }
                else if (ADown && (!ADownLastFrame || !controlDown))
                {
                    Program.CurrentSaveData.CurrentFrameID -= 100;

                    if (Program.CurrentSaveData.CurrentFrameID > 2147483647)
                    {
                        Program.CurrentSaveData.CurrentFrameID = 0;
                    }

                    ReloadCurrentFrame();

                    System.Console.WriteLine($"Changed to frame {Program.CurrentSaveData.CurrentFrameID} of {Program.SourceVideoFrameCount} which is %{Program.CurrentSaveData.CurrentFrameID * 100.0 / Program.SourceVideoFrameCount} complete. Only {Program.SourceVideoFrameCount - Program.CurrentSaveData.CurrentFrameID - 4646} frames left!");
                }
            }
            else
            {
                if (DDown && (!DDownLastFrame || !controlDown))
                {
                    Program.CurrentSaveData.CurrentFrameID += 1;

                    if (Program.CurrentSaveData.CurrentFrameID > 252432)
                    {
                        Program.CurrentSaveData.CurrentFrameID = 252432;
                    }

                    ReloadCurrentFrame();

                    System.Console.WriteLine($"Changed to frame {Program.CurrentSaveData.CurrentFrameID} of {Program.SourceVideoFrameCount} which is %{Program.CurrentSaveData.CurrentFrameID * 100.0 / Program.SourceVideoFrameCount} complete. Only {Program.SourceVideoFrameCount - Program.CurrentSaveData.CurrentFrameID - 4646} frames left!");
                }
                else if (ADown && (!ADownLastFrame || !controlDown))
                {
                    Program.CurrentSaveData.CurrentFrameID -= 1;

                    if (Program.CurrentSaveData.CurrentFrameID > 2147483647)
                    {
                        Program.CurrentSaveData.CurrentFrameID = 0;
                    }

                    ReloadCurrentFrame();

                    System.Console.WriteLine($"Changed to frame {Program.CurrentSaveData.CurrentFrameID} of {Program.SourceVideoFrameCount} which is %{Program.CurrentSaveData.CurrentFrameID * 100.0 / Program.SourceVideoFrameCount} complete. Only {Program.SourceVideoFrameCount - Program.CurrentSaveData.CurrentFrameID - 4646} frames left!");
                }
            }

            if (RightDown && !RightDownLastFrame)
            {
                KeyFrame next = Program.CurrentSaveData.GetNextKeyFrame(Program.CurrentSaveData.CurrentFrameID, false);

                if (!(next is null))
                {
                    Program.CurrentSaveData.CurrentFrameID = next.FrameID;

                    ReloadCurrentFrame();

                    System.Console.WriteLine($"Changed to frame {Program.CurrentSaveData.CurrentFrameID} of {Program.SourceVideoFrameCount} which is %{Program.CurrentSaveData.CurrentFrameID * 100.0 / Program.SourceVideoFrameCount} complete. Only {Program.SourceVideoFrameCount - Program.CurrentSaveData.CurrentFrameID - 4646} frames left!");
                }
            }
            else if (LeftDown && !LeftDownLastFrame)
            {
                KeyFrame previous = Program.CurrentSaveData.GetPreviousKeyFrame(Program.CurrentSaveData.CurrentFrameID, false);

                if (!(previous is null))
                {
                    Program.CurrentSaveData.CurrentFrameID = previous.FrameID;

                    ReloadCurrentFrame();

                    System.Console.WriteLine($"Changed to frame {Program.CurrentSaveData.CurrentFrameID} of {Program.SourceVideoFrameCount} which is %{Program.CurrentSaveData.CurrentFrameID * 100.0 / Program.SourceVideoFrameCount} complete. Only {Program.SourceVideoFrameCount - Program.CurrentSaveData.CurrentFrameID - 4646} frames left!");
                }
            }

            if (mouseXScaled >= 0 && mouseYScaled >= 0 && mouseXScaled < 1920 && mouseYScaled < 1080)
            {
                if (mouseLeftDown)
                {
                    FrameData frameData = Program.CurrentSaveData.GetFrameData(Program.CurrentSaveData.CurrentFrameID);

                    if (frameData is null)
                    {
                        frameData = new FrameData();
                    }

                    Program.CurrentSaveData.ModifyOrAddKeyFrame(new KeyFrame(Program.CurrentSaveData.CurrentFrameID, (ushort)mouseXScaled, (ushort)mouseYScaled, frameData.BlurWidth, false));
                }
                else if (mouseRightDown)
                {
                    FrameData frameData = Program.CurrentSaveData.GetFrameData(Program.CurrentSaveData.CurrentFrameID);

                    if (frameData is null)
                    {
                        frameData = new FrameData();
                    }

                    Program.CurrentSaveData.ModifyOrAddKeyFrame(new KeyFrame(Program.CurrentSaveData.CurrentFrameID, (ushort)mouseXScaled, (ushort)mouseYScaled, frameData.BlurWidth, true));
                }
            }

            if (deleteDown)
            {
                Program.CurrentSaveData.DeleteKeyFrame(Program.CurrentSaveData.CurrentFrameID);
            }
            else if (mouseSideDown)
            {
                KeyFrame previous = Program.CurrentSaveData.GetPreviousKeyFrame(Program.CurrentSaveData.CurrentFrameID, true);

                if (!(previous is null))
                {
                    Program.CurrentSaveData.ModifyOrAddKeyFrame(new KeyFrame(Program.CurrentSaveData.CurrentFrameID, previous.PositionX, previous.PositionY, previous.BlurWidth, false));
                }
            }

            if (scrollWheelDeltaScaled != 0)
            {
                KeyFrame keyFrame = Program.CurrentSaveData.GetKeyFrame(Program.CurrentSaveData.CurrentFrameID);

                bool marksEnd = false;

                if (!(keyFrame is null) && keyFrame.IsExit)
                {
                    marksEnd = keyFrame.IsExit;
                }

                FrameData frameData = Program.CurrentSaveData.GetFrameData(Program.CurrentSaveData.CurrentFrameID);

                if (frameData is null)
                {
                    frameData = new FrameData();
                }

                int newBlurWidth = frameData.BlurWidth + scrollWheelDeltaScaled;

                if (newBlurWidth < 40)
                {
                    newBlurWidth = 40;
                }
                else if (newBlurWidth > 400)
                {
                    newBlurWidth = 400;
                }

                Program.CurrentSaveData.ModifyOrAddKeyFrame(new KeyFrame(Program.CurrentSaveData.CurrentFrameID, frameData.PositionX, frameData.PositionY, (ushort)newBlurWidth, marksEnd));
            }
            else if (RDown)
            {
                KeyFrame keyFrame = Program.CurrentSaveData.GetKeyFrame(Program.CurrentSaveData.CurrentFrameID);

                bool marksEnd = false;

                if (!(keyFrame is null) && keyFrame.IsExit)
                {
                    marksEnd = keyFrame.IsExit;
                }

                FrameData frameData = Program.CurrentSaveData.GetFrameData(Program.CurrentSaveData.CurrentFrameID);

                if (frameData is null)
                {
                    frameData = new FrameData();
                }

                Program.CurrentSaveData.ModifyOrAddKeyFrame(new KeyFrame(Program.CurrentSaveData.CurrentFrameID, frameData.PositionX, frameData.PositionY, 64, marksEnd));
            }



            _xnaGraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            _xnaSpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied, Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp, null, null, null, null);

            _xnaSpriteBatch.Draw(_currentFrame, _xnaViewportRect, Microsoft.Xna.Framework.Color.White);

            FrameData currentFrameData = Program.CurrentSaveData.GetFrameData(Program.CurrentSaveData.CurrentFrameID);

            if (!(currentFrameData is null))
            {
                int width = (currentFrameData.BlurWidth * _viewportWidth) / 1920;
                int height = (currentFrameData.BlurWidth * _viewportHeight) / 1080;
                int x = (currentFrameData.PositionX * _viewportWidth) / 1920;
                int y = (currentFrameData.PositionY * _viewportHeight) / 1080;

                _xnaSpriteBatch.Draw(_selectionTexture, new Microsoft.Xna.Framework.Rectangle(x - (width / 2), y - (height / 2), width, height), Microsoft.Xna.Framework.Color.White);
            }

            KeyFrame currentKeyFrame = Program.CurrentSaveData.GetKeyFrame(Program.CurrentSaveData.CurrentFrameID);

            if (!(currentKeyFrame is null))
            {
                if (currentKeyFrame.IsExit)
                {
                    _xnaSpriteBatch.Draw(_exitMarkerKeyFrameTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, _viewportWidth / 20, _viewportWidth / 20), Microsoft.Xna.Framework.Color.White);
                }
                else
                {
                    _xnaSpriteBatch.Draw(_keyFrameTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, _viewportWidth / 20, _viewportWidth / 20), Microsoft.Xna.Framework.Color.White);
                }
            }

            _xnaSpriteBatch.End();

            DDownLastFrame = DDown;
            ADownLastFrame = ADown;
            SDownLastFrame = SDown;
            LDownLastFrame = LDown;
            RightDownLastFrame = RightDown;
            LeftDownLastFrame = LeftDown;
            scrollWheelValueLastFrame = scrollWheelValue;
        }
        private int _currentFrameID = 0;
        public void SwitchFrame(int newFrameID)
        {
            if (!(_currentFrame is null))
            {
                System.Threading.ThreadPool.QueueUserWorkItem((object state) =>
                {
                    Microsoft.Xna.Framework.Graphics.Texture2D _oldFrame = (Microsoft.Xna.Framework.Graphics.Texture2D)state;
                    _oldFrame.Dispose();
                }, _currentFrame);
            }
            _currentFrameID = newFrameID;
            _currentFrame = Microsoft.Xna.Framework.Graphics.Texture2D.FromFile(_xnaGraphicsDevice, $"E:\\Play\\Frames\\{_currentFrameID}.png");
        }
    }
}