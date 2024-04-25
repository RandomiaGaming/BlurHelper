namespace BlurHelper
{
    public class BlurHelperGame : Microsoft.Xna.Framework.Game
    {
        #region Private Constants
        private const string exitMarkerKeyFrameResourceName = "BlurHelper.BlurHelper.KeyframeEditor.Icons.ExitMarkerKeyFrame.png";
        private const string keyFrameResourceName = "BlurHelper.BlurHelper.KeyframeEditor.Icons.KeyFrame.png";
        private const string selectionResourceName = "BlurHelper.BlurHelper.KeyframeEditor.Icons.Selection.png";
        private const long autoSaveIntervalTicks = 100000000;
        #endregion
        #region Private Variables
        private string _videoFrameDirectoryPath = null;
        private string _saveDataPath = null;
        private SaveData _currentSaveData = null;
        private long _lastAutoSaveTimeTicks = 0;
        private Microsoft.Xna.Framework.Graphics.Texture2D _exitMarkerKeyFrameTexture = null;
        private Microsoft.Xna.Framework.Graphics.Texture2D _keyFrameTexture = null;
        private Microsoft.Xna.Framework.Graphics.Texture2D _selectionTexture = null;
        private Microsoft.Xna.Framework.Graphics.Texture2D _currentFrameTexture = null;
        private Microsoft.Xna.Framework.GraphicsDeviceManager _graphicsDeviceManager = null;
        private Microsoft.Xna.Framework.Graphics.GraphicsDevice _graphicsDevice = null;
        private Microsoft.Xna.Framework.GameWindow _gameWindow = null;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch = null;
        private Microsoft.Xna.Framework.Rectangle _viewportRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 1920, 1080);
        private uint _totalFrameCount = 0;
        #endregion
        public BlurHelperGame(string videoFrameDirectoryPath, string saveDataPath)
        {
            if (videoFrameDirectoryPath is null)
            {
                throw new System.Exception("videoFrameDirectoryPath cannot be null.");
            }
            else if (videoFrameDirectoryPath is "")
            {
                throw new System.Exception("videoFrameDirectoryPath cannot be empty.");
            }
            else if (!System.IO.Directory.Exists(videoFrameDirectoryPath))
            {
                throw new System.Exception("Directory at videoFrameDirectoryPath does not exist.");
            }
            _videoFrameDirectoryPath = videoFrameDirectoryPath;
            if (saveDataPath is null)
            {
                throw new System.Exception("saveDataPath cannot be null.");
            }
            else if (saveDataPath is "")
            {
                throw new System.Exception("saveDataPath cannot be empty.");
            }
            else if (!System.IO.File.Exists(saveDataPath))
            {
                throw new System.Exception("File at saveDataPath does not exist.");
            }
            _saveDataPath = saveDataPath;

            _totalFrameCount = (uint)System.IO.Directory.GetFiles(_videoFrameDirectoryPath).LongLength;

            _graphicsDeviceManager = new Microsoft.Xna.Framework.GraphicsDeviceManager(this);
            _graphicsDeviceManager.GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile.Reach;
            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            _graphicsDeviceManager.HardwareModeSwitch = false;
            _graphicsDeviceManager.IsFullScreen = false;
            _graphicsDeviceManager.PreferHalfPixelOffset = false;
            _graphicsDeviceManager.PreferredBackBufferFormat = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color;
            _graphicsDeviceManager.SupportedOrientations = Microsoft.Xna.Framework.DisplayOrientation.LandscapeLeft | Microsoft.Xna.Framework.DisplayOrientation.LandscapeRight | Microsoft.Xna.Framework.DisplayOrientation.Portrait | Microsoft.Xna.Framework.DisplayOrientation.PortraitDown | Microsoft.Xna.Framework.DisplayOrientation.Unknown | Microsoft.Xna.Framework.DisplayOrientation.Default;
            _graphicsDeviceManager.ApplyChanges();

            _graphicsDevice = GraphicsDevice;

            _graphicsDevice.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend;
            _graphicsDevice.DepthStencilState = Microsoft.Xna.Framework.Graphics.DepthStencilState.None;
            _graphicsDevice.RasterizerState = Microsoft.Xna.Framework.Graphics.RasterizerState.CullNone;

            Load();

            System.Reflection.Assembly assembly = typeof(Program).Assembly;

            _exitMarkerKeyFrameTexture = Microsoft.Xna.Framework.Graphics.Texture2D.FromStream(_graphicsDevice, assembly.GetManifestResourceStream(exitMarkerKeyFrameResourceName));
            _keyFrameTexture = Microsoft.Xna.Framework.Graphics.Texture2D.FromStream(_graphicsDevice, assembly.GetManifestResourceStream(keyFrameResourceName));
            _selectionTexture = Microsoft.Xna.Framework.Graphics.Texture2D.FromStream(_graphicsDevice, assembly.GetManifestResourceStream(selectionResourceName));

            _gameWindow = Window;

            _gameWindow.AllowAltF4 = true;
            _gameWindow.AllowUserResizing = true;
            _gameWindow.IsBorderless = false;
            _gameWindow.Position = new Microsoft.Xna.Framework.Point(_graphicsDevice.Adapter.CurrentDisplayMode.Width / 4, _graphicsDevice.Adapter.CurrentDisplayMode.Height / 4);
            _gameWindow.Title = "Blur Helper";

            _gameWindow.ClientSizeChanged += ResizeCallback;

            _viewportRect = new Microsoft.Xna.Framework.Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);

            _graphicsDeviceManager.PreferredBackBufferWidth = _viewportRect.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = _viewportRect.Height;
            _graphicsDeviceManager.ApplyChanges();

            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(_graphicsDevice);
            _spriteBatch.Name = "Main Sprite Batch";
            _spriteBatch.Tag = null;

            InactiveSleepTime = new System.TimeSpan(0);
            TargetElapsedTime = new System.TimeSpan(10000000 / 60);
            MaxElapsedTime = new System.TimeSpan(long.MaxValue);
            IsFixedTimeStep = false;
            IsMouseVisible = true;
        }
        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            InputHelper.Tick();

            if (InputHelper.SKey.Down && InputHelper.ControlKey)
            {
                Save();
            }
            else if (InputHelper.LKey.Down && InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                Load();
            }
            else if (InputHelper.DKey && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                IncrementFrameIndex(1);
            }
            else if (InputHelper.AKey && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                IncrementFrameIndex(-1);
            }
            else if (InputHelper.DKey && !InputHelper.ControlKey && !InputHelper.SpaceKey && InputHelper.ShiftKey)
            {
                IncrementFrameIndex(10);
            }
            else if (InputHelper.AKey && !InputHelper.ControlKey && !InputHelper.SpaceKey && InputHelper.ShiftKey)
            {
                IncrementFrameIndex(-10);
            }
            else if (InputHelper.DKey && !InputHelper.ControlKey && InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                IncrementFrameIndex(100);
            }
            else if (InputHelper.AKey && !InputHelper.ControlKey && InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                IncrementFrameIndex(-100);
            }
            else if (InputHelper.DKey.Down && InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                IncrementFrameIndex(1);
            }
            else if (InputHelper.AKey.Down && InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                IncrementFrameIndex(-1);
            }
            else if (InputHelper.RightArrowKey.Down && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                KeyFrame nextKeyFrame = _currentSaveData.GetNextKeyFrame(_currentSaveData.FrameIndex, false);
                if (!(nextKeyFrame is null))
                {
                    SetFrameIndex(nextKeyFrame.FrameIndex);
                }
            }
            else if (InputHelper.LeftArrowKey.Down && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                KeyFrame previousKeyFrame = _currentSaveData.GetPreviousKeyFrame(_currentSaveData.FrameIndex, false);
                if (!(previousKeyFrame is null))
                {
                    SetFrameIndex(previousKeyFrame.FrameIndex);
                }
            }
            else if (InputHelper.RightArrowKey.Down && InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                KeyFrame bestMatch = null;
                foreach (KeyFrame keyFrame in _currentSaveData.KeyFrames)
                {
                    if (keyFrame.FrameIndex > _currentSaveData.FrameIndex && keyFrame.ExitMarker && (bestMatch is null || keyFrame.FrameIndex < bestMatch.FrameIndex))
                    {
                        bestMatch = keyFrame;
                    }
                }
                if (!(bestMatch is null))
                {
                    SetFrameIndex(bestMatch.FrameIndex);
                }
            }
            else if (InputHelper.DeleteKey.Down && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                _currentSaveData.DeleteKeyFrame(_currentSaveData.FrameIndex);
            }
            else if (InputHelper.Side1Click.Down && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                KeyFrame previousKeyFrame = _currentSaveData.GetPreviousKeyFrame(_currentSaveData.FrameIndex, true);
                if (!(previousKeyFrame is null))
                {
                    _currentSaveData.ModifyOrAddKeyFrame(new KeyFrame(_currentSaveData.FrameIndex, previousKeyFrame.BlurPositionX, previousKeyFrame.BlurPositionY, previousKeyFrame.BlurSize, false));
                }
            }
            else if (InputHelper.Side0Click.Down && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                KeyFrame previousKeyFrame = _currentSaveData.GetPreviousKeyFrame(_currentSaveData.FrameIndex, true);
                if (!(previousKeyFrame is null))
                {
                    _currentSaveData.ModifyOrAddKeyFrame(new KeyFrame(_currentSaveData.FrameIndex, previousKeyFrame.BlurPositionX, previousKeyFrame.BlurPositionY, previousKeyFrame.BlurSize, true));
                }
            }
            else if (InputHelper.MiddleClick.Down && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                KeyFrame nextKeyFrame = _currentSaveData.GetNextKeyFrame(_currentSaveData.FrameIndex, true);
                if (!(nextKeyFrame is null))
                {
                    _currentSaveData.ModifyOrAddKeyFrame(new KeyFrame(_currentSaveData.FrameIndex, nextKeyFrame.BlurPositionX, nextKeyFrame.BlurPositionY, nextKeyFrame.BlurSize, false));
                }
            }
            else if (InputHelper.MiddleClick.Down && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                KeyFrame nextKeyFrame = _currentSaveData.GetNextKeyFrame(_currentSaveData.FrameIndex, true);
                if (!(nextKeyFrame is null))
                {
                    _currentSaveData.ModifyOrAddKeyFrame(new KeyFrame(_currentSaveData.FrameIndex, nextKeyFrame.BlurPositionX, nextKeyFrame.BlurPositionY, nextKeyFrame.BlurSize, false));
                }
            }
            else if (InputHelper.LeftClick && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                FrameData frameData = _currentSaveData.GetFrameData(_currentSaveData.FrameIndex);
                if (frameData is null)
                {
                    frameData = new FrameData((ushort)(_currentFrameTexture.Width / 2), (ushort)(_currentFrameTexture.Height / 2), 100);
                }
                int mouseXScaled = (InputHelper.MouseXAxis.Value * _currentFrameTexture.Width) / _viewportRect.Width;
                int mouseYScaled = (InputHelper.MouseYAxis.Value * _currentFrameTexture.Height) / _viewportRect.Height;
                if (mouseXScaled < 0)
                {
                    mouseXScaled = 0;
                }
                if (mouseXScaled >= _currentFrameTexture.Width)
                {
                    mouseXScaled = _currentFrameTexture.Width - 1;
                }
                if (mouseYScaled < 0)
                {
                    mouseYScaled = 0;
                }
                if (mouseYScaled >= _currentFrameTexture.Height)
                {
                    mouseYScaled = _currentFrameTexture.Height - 1;
                }
                _currentSaveData.ModifyOrAddKeyFrame(new KeyFrame(_currentSaveData.FrameIndex, (ushort)mouseXScaled, (ushort)mouseYScaled, frameData.BlurSize, false));
            }
            else if (InputHelper.RightClick && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                FrameData frameData = _currentSaveData.GetFrameData(_currentSaveData.FrameIndex);
                if (frameData is null)
                {
                    frameData = new FrameData((ushort)(_currentFrameTexture.Width / 2), (ushort)(_currentFrameTexture.Height / 2), 100);
                }
                int mouseXScaled = (InputHelper.MouseXAxis.Value * _currentFrameTexture.Width) / _viewportRect.Width;
                int mouseYScaled = (InputHelper.MouseYAxis.Value * _currentFrameTexture.Height) / _viewportRect.Height;
                if (mouseXScaled < 0)
                {
                    mouseXScaled = 0;
                }
                if (mouseXScaled >= _currentFrameTexture.Width)
                {
                    mouseXScaled = _currentFrameTexture.Width - 1;
                }
                if (mouseYScaled < 0)
                {
                    mouseYScaled = 0;
                }
                if (mouseYScaled >= _currentFrameTexture.Height)
                {
                    mouseYScaled = _currentFrameTexture.Height - 1;
                }
                _currentSaveData.ModifyOrAddKeyFrame(new KeyFrame(_currentSaveData.FrameIndex, (ushort)mouseXScaled, (ushort)mouseYScaled, frameData.BlurSize, true));
            }
            else if (InputHelper.ScrollWheelAxis && !InputHelper.ControlKey && !InputHelper.SpaceKey && !InputHelper.ShiftKey)
            {
                KeyFrame keyFrame = _currentSaveData.GetKeyFrame(_currentSaveData.FrameIndex);
                FrameData frameData = _currentSaveData.GetFrameData(_currentSaveData.FrameIndex);
                if (frameData is null)
                {
                    frameData = new FrameData((ushort)(_currentFrameTexture.Width / 2), (ushort)(_currentFrameTexture.Height / 2), 100);
                }
                frameData.BlurSize = (ushort)(frameData.BlurSize + InputHelper.ScrollWheelAxis.Delta);
                if (keyFrame is null || !keyFrame.ExitMarker)
                {
                    _currentSaveData.ModifyOrAddKeyFrame(new KeyFrame(_currentSaveData.FrameIndex, frameData.BlurPositionX, frameData.BlurPositionY, frameData.BlurSize, false));
                }
                else
                {
                    _currentSaveData.ModifyOrAddKeyFrame(new KeyFrame(_currentSaveData.FrameIndex, frameData.BlurPositionX, frameData.BlurPositionY, frameData.BlurSize, true));
                }
            }


                /* if (mouseXScaled >= 0 && mouseYScaled >= 0 && mouseXScaled < 1920 && mouseYScaled < 1080)
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
                 }*/



                _graphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

                _spriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred, Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied, Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp, null, null, null, null);

                _spriteBatch.Draw(_currentFrameTexture, _viewportRect, Microsoft.Xna.Framework.Color.White);

                FrameData currentFrameData = _currentSaveData.GetFrameData(_currentSaveData.FrameIndex);

                if (!(currentFrameData is null))
                {
                    int width = (currentFrameData.BlurSize * _viewportRect.Width) / _currentFrameTexture.Width;
                    int height = (currentFrameData.BlurSize * _viewportRect.Height) / _currentFrameTexture.Height;
                    int x = (currentFrameData.BlurPositionX * _viewportRect.Width) / _currentFrameTexture.Width;
                    int y = (currentFrameData.BlurPositionY * _viewportRect.Height) / _currentFrameTexture.Height;

                    _spriteBatch.Draw(_selectionTexture, new Microsoft.Xna.Framework.Rectangle(x - (width / 2), y - (height / 2), width, height), Microsoft.Xna.Framework.Color.White);
                }

                KeyFrame currentKeyFrame = _currentSaveData.GetKeyFrame(_currentSaveData.FrameIndex);

                if (!(currentKeyFrame is null))
                {
                    if (currentKeyFrame.ExitMarker)
                    {
                        _spriteBatch.Draw(_exitMarkerKeyFrameTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, _viewportRect.Width / 20, _viewportRect.Width / 20), Microsoft.Xna.Framework.Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(_keyFrameTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, _viewportRect.Width / 20, _viewportRect.Width / 20), Microsoft.Xna.Framework.Color.White);
                    }
                }

                _spriteBatch.End();

                if (System.DateTime.Now.Ticks - _lastAutoSaveTimeTicks > autoSaveIntervalTicks)
                {
                    Save();
                }
            }



            private void IncrementFrameIndex(int incrementAmount)
            {
                if (incrementAmount < 0)
                {
                    if ((uint)(-incrementAmount) > _currentSaveData.FrameIndex)
                    {
                        SetFrameIndex(0);
                    }
                    else
                    {
                        SetFrameIndex(_currentSaveData.FrameIndex - (uint)(-incrementAmount));
                    }
                }
                else
                {
                    if (uint.MaxValue - ((uint)incrementAmount) < _currentSaveData.FrameIndex)
                    {
                        SetFrameIndex(_totalFrameCount - 1);
                    }
                    else
                    {
                        uint newFrameIndex = _currentSaveData.FrameIndex + (uint)incrementAmount;
                        if (newFrameIndex > _totalFrameCount - 1)
                        {
                            SetFrameIndex(_totalFrameCount - 1);
                        }
                        else
                        {
                            SetFrameIndex(newFrameIndex);
                        }
                    }
                }
            }
            private void SetFrameIndex(uint frameIndex)
            {
                if (frameIndex > _totalFrameCount - 1)
                {
                    frameIndex = _totalFrameCount - 1;
                }
                _currentSaveData.FrameIndex = frameIndex;
                if (!(_currentFrameTexture is null))
                {
                    Microsoft.Xna.Framework.Graphics.Texture2D _oldFrame = _currentFrameTexture;
                    System.Threading.ThreadPool.QueueUserWorkItem((object state) =>
                    {
                        _oldFrame.Dispose();
                    });
                }
                _currentFrameTexture = Microsoft.Xna.Framework.Graphics.Texture2D.FromFile(_graphicsDevice, $"{_videoFrameDirectoryPath}\\{_currentSaveData.FrameIndex}.png");
                System.Console.ForegroundColor = System.ConsoleColor.White;
                System.Console.WriteLine($"Switched to frame {_currentSaveData.FrameIndex}.");
            }



            private void ResizeCallback(object sender, System.EventArgs e)
            {
                if (_graphicsDeviceManager.IsFullScreen)
                {
                    _viewportRect.Width = _graphicsDevice.Adapter.CurrentDisplayMode.Width;
                    _viewportRect.Height = _graphicsDevice.Adapter.CurrentDisplayMode.Height;
                }
                else
                {
                    _viewportRect.Width = _graphicsDevice.Viewport.Width;
                    _viewportRect.Height = _graphicsDevice.Viewport.Height;
                }
                if (_graphicsDeviceManager.PreferredBackBufferWidth != _viewportRect.Width || _graphicsDeviceManager.PreferredBackBufferHeight != _viewportRect.Height)
                {
                    _graphicsDeviceManager.PreferredBackBufferWidth = _viewportRect.Width;
                    _graphicsDeviceManager.PreferredBackBufferHeight = _viewportRect.Height;
                    _graphicsDeviceManager.ApplyChanges();
                }
            }
            public void Load()
            {
                if (!System.IO.File.Exists(_saveDataPath))
                {
                    _currentSaveData = new SaveData(0, new System.Collections.Generic.List<KeyFrame>());
                }
                else
                {
                    string serializedString = System.IO.File.ReadAllText(_saveDataPath);
                    _currentSaveData = SaveData.Deserialize(serializedString);
                }
                _lastAutoSaveTimeTicks = System.DateTime.Now.Ticks;
                SetFrameIndex(_currentSaveData.FrameIndex);
                System.Console.ForegroundColor = System.ConsoleColor.DarkYellow;
                System.Console.WriteLine("Successfully Loaded Save Data!");
            }
            public void Save()
            {
                string serializedString = SaveData.Serialize(_currentSaveData);
                System.IO.File.WriteAllText(_saveDataPath, serializedString);
                _lastAutoSaveTimeTicks = System.DateTime.Now.Ticks;
                System.Console.ForegroundColor = System.ConsoleColor.Cyan;
                System.Console.WriteLine("Successfully Saved Data!");
            }
        }
    }