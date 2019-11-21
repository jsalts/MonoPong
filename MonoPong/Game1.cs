using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoPong.Player;

namespace MonoPong
{
    public class Game1 : Game
    {
        private float _gameSpeed = 5f;

        private int _ballSize = 25;
        private int _boost1Width = 12;
        private int _boost2Width = 10;
        private int _boost1Height = 50;
        private int _boost2Height = 26;
        private int _paddleWidth = 20;
        private int _paddleHeight = 100;

        SpriteBatch _ballSprite;
        SpriteBatch _playerBoost1Sprite;
        SpriteBatch _playerBoost2Sprite;
        SpriteBatch _aiBoost1Sprite;
        SpriteBatch _aiBoost2Sprite;
        Texture2D _ballTexture;
        Texture2D _boost1Texture;
        Texture2D _boost2Texture;
        Vector2 _ballPosition;
        Vector2 _ballSpeed;
        readonly Paddle _playerPaddle;
        readonly Paddle _aiPaddle;
        int _playerBoostState;
        int _aiBoostState;
        private int _boost1Offset;
        private int _boost2Offset;

        public Game1()
        { 
            var unused = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _ballPosition = new Vector2(50, 100);
            _playerPaddle = new Paddle(30,70);
            _aiPaddle = new Paddle(750, 70);

            _playerPaddle.AddUpKeys(Keys.W);
            _playerPaddle.AddUpKeys(Keys.OemComma);           
            _playerPaddle.AddDownKeys(Keys.O);
            _playerPaddle.AddDownKeys(Keys.S);
            
            _aiPaddle.AddUpKeys(Keys.Up);
            _aiPaddle.AddDownKeys(Keys.Down);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _aiPaddle.Initialize();
            _playerPaddle.Initialize();

            _ballTexture = new Texture2D(this.GraphicsDevice, _ballSize, _ballSize);
            ColorBall(Color.White);

            _boost1Texture = new Texture2D(this.GraphicsDevice, _boost1Width, _boost1Height);
            Color[] boost1ColorData = new Color[_boost1Width * _boost1Height];
            for (int i = 0; i < _boost1Width*_boost1Height; i++)
            {
                boost1ColorData[i] = Color.Blue;
            }
            _boost1Texture.SetData(boost1ColorData);

            _boost2Texture = new Texture2D(this.GraphicsDevice, _boost2Width, _boost2Height);
            Color[] boost2ColorData = new Color[_boost2Width * _boost2Height];
            for (int i = 0; i < _boost2Width * _boost2Height; i++)
            {
                boost2ColorData[i] = Color.CornflowerBlue;
            }
            _boost2Texture.SetData(boost2ColorData);

            base.Initialize();

            _ballSpeed = new Vector2(1, 1);
            _playerBoostState = 0;
            _aiBoostState = 0;
            _boost1Offset = (_paddleHeight - _boost1Height) / 2;
            _boost2Offset = (_paddleHeight - _boost2Height) / 2;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _playerPaddle.LoadContent(GraphicsDevice);
            _aiPaddle.LoadContent(GraphicsDevice);
            
            // Create a new SpriteBatch, which can be used to draw textures.
            _ballSprite = new SpriteBatch(GraphicsDevice);
            _playerBoost1Sprite = new SpriteBatch(GraphicsDevice);
            _playerBoost2Sprite = new SpriteBatch(GraphicsDevice);
            _aiBoost1Sprite = new SpriteBatch(GraphicsDevice);
            _aiBoost2Sprite = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            UpdateGameSpeed();
            HandleKeystrokes();

            DetermineBallPosition();
            PlayerBoost();
            AiBoost();
            
            base.Update(gameTime);
        }

        private void UpdateGameSpeed()
        {
            _aiPaddle.GameSpeed = _gameSpeed;
            _playerPaddle.GameSpeed = _gameSpeed;
        }

        private void DetermineBallPosition()
        {
            //Move ball
            _ballPosition.X += _ballSpeed.X * _gameSpeed;
            _ballPosition.Y += _ballSpeed.Y * _gameSpeed;

            //Check for right player paddle collision
            if (_ballPosition.X + _ballSize >= _aiPaddle.GetX() && _ballPosition.X + _ballSize < _aiPaddle.GetX() + (_gameSpeed * _ballSpeed.X))
            {
                if (_ballPosition.Y > _aiPaddle.GetY() && _ballPosition.Y < _aiPaddle.GetY() + _paddleHeight)
                    _ballSpeed.X = -1;
                if (_ballPosition.Y + _ballSize > _aiPaddle.GetY() && _ballPosition.Y + _ballSize < _aiPaddle.GetY() + _paddleHeight)
                    _ballSpeed.X = -1;
            }
            //Check for left player paddle collision
            if (_ballPosition.X <= _playerPaddle.GetX() + _paddleWidth && _ballPosition.X > _playerPaddle.GetX() + _paddleWidth + (_gameSpeed * _ballSpeed.X))
            {
                if (_ballPosition.Y > _playerPaddle.GetY() && _ballPosition.Y < _playerPaddle.GetY() + _paddleHeight)
                    _ballSpeed.X = 1;
                if (_ballPosition.Y + _ballSize > _playerPaddle.GetY() && _ballPosition.Y + _ballSize < _playerPaddle.GetY() + _paddleHeight)
                    _ballSpeed.X = 1;
            }
            //Check for bottom collision
            if (_ballPosition.Y + _ballSize > 480)
                _ballSpeed.Y = -1;
            //Check for top collision
            if (_ballPosition.Y < 0)
                _ballSpeed.Y = 1;
            //Check for game over
            if (_ballPosition.X < 0)
                Exit();
            if (_ballPosition.X + _ballSize > this.GraphicsDevice.Viewport.Width)
                //Exit();
                _ballSpeed.X = _ballSpeed.X * -1;

            //Update Ball Color
            if (_ballSpeed.X > 2 || _ballSpeed.X < -2)
                ColorBall(Color.Red);
            else if (_ballSpeed.X > 1 || _ballSpeed.X < -1)
                ColorBall(Color.Yellow);
            else
                ColorBall(Color.White);
        }

        private void PlayerBoost()
        {
            if (_playerBoostState == 0)
                return;
            //Increment Boost Cooldown Timer
            else if (_playerBoostState < 0)
                _playerBoostState++;
            //Boost 1 Interaction
            else if (_playerBoostState > 0 && _playerBoostState <= 10)
            {
                if ( _ballSpeed.X < 0 ) { 
                    if (_ballPosition.X <= _playerPaddle.GetX() + _paddleWidth + _boost1Width + 6 && _ballPosition.X > _playerPaddle.GetX() + _paddleWidth + 4 - (_gameSpeed * _ballSpeed.X))
                    {
                        if (_ballPosition.Y > _playerPaddle.GetY() + _boost1Offset && _ballPosition.Y < _playerPaddle.GetY() + _boost1Offset + _boost1Height)
                            _ballSpeed.X = _ballSpeed.X * -2f;
                        else if (_ballPosition.Y + _ballSize > _playerPaddle.GetY() + _boost1Offset && _ballPosition.Y + _ballSize < _playerPaddle.GetY() + _boost1Offset + _boost1Height)
                            _ballSpeed.X = _ballSpeed.X * -2f;
                    }
                }
                _playerBoostState++;
            }
            //Boost 2 Interaction
            else if (_playerBoostState > 10 && _playerBoostState <= 20)
            {
                if (_ballSpeed.X < 0)
                {
                    if (_ballPosition.X <= _playerPaddle.GetX() + _paddleWidth +_boost1Width + _boost2Width + 10 && _ballPosition.X > _playerPaddle.GetX() + _paddleWidth + _boost1Width + 6 - (_gameSpeed * _ballSpeed.X))
                    {
                        if (_ballPosition.Y > _playerPaddle.GetY() + _boost2Offset && _ballPosition.Y < _playerPaddle.GetY() + _boost2Offset + _boost2Height)
                            _ballSpeed.X = _ballSpeed.X * -3f;
                        else if (_ballPosition.Y + _ballSize > _playerPaddle.GetY() + _boost2Offset && _ballPosition.Y + _ballSize < _playerPaddle.GetY() + _boost2Offset + _boost2Height)
                            _ballSpeed.X = _ballSpeed.X * -3f;
                    }
                }
                _playerBoostState++;
            }
            //Activate Boost Cooldown
            else if (_playerBoostState > 20)
                _playerBoostState = -100;
        }

        private void AiBoost()
        {
            if (_aiBoostState == 0)
                return;
            //Increment Boost Cooldown Timer
            else if (_aiBoostState < 0)
                _aiBoostState++;
            //Boost 1 Interaction
            else if (_aiBoostState > 0 && _aiBoostState <= 10)
            {
                if (_ballSpeed.X > 0)
                {
                    if (_ballPosition.X + _ballSize >= _aiPaddle.GetX() - _boost1Width - 4 && _ballPosition.X + _ballSize < _aiPaddle.GetX() - 4 + (_gameSpeed * _ballSpeed.X))
                    {
                        if (_ballPosition.Y > _aiPaddle.GetY() + _boost1Offset && _ballPosition.Y < _aiPaddle.GetY() + _boost1Offset + _boost1Height)
                            _ballSpeed.X = _ballSpeed.X * -2f;
                        else if (_ballPosition.Y + _ballSize > _aiPaddle.GetY() + _boost1Offset && _ballPosition.Y + _ballSize < _aiPaddle.GetY() + _boost1Offset + _boost1Height)
                            _ballSpeed.X = _ballSpeed.X * -2f;
                    }
                }
                _aiBoostState++;
            }
            //Boost 2 Interaction
            else if (_aiBoostState > 10 && _aiBoostState <= 20)
            {
                if (_ballSpeed.X > 0)
                {
                    if (_ballPosition.X + _ballSize >= _aiPaddle.GetX() - _boost1Width - _boost2Width - 10 && _ballPosition.X + _ballSize < _aiPaddle.GetX() - _boost1Width - 6 + (_gameSpeed * _ballSpeed.X))
                    {
                        if (_ballPosition.Y > _aiPaddle.GetY() + _boost2Offset && _ballPosition.Y < _aiPaddle.GetY() + _boost2Offset + _boost2Height)
                            _ballSpeed.X = _ballSpeed.X * -3f;
                        else if (_ballPosition.Y + _ballSize > _aiPaddle.GetY() + _boost2Offset && _ballPosition.Y + _ballSize < _aiPaddle.GetY() + _boost2Offset + _boost2Height)
                            _ballSpeed.X = _ballSpeed.X * -3f;
                    }
                }
                _aiBoostState++;
            }
            //Activate Boost Cooldown
            else if (_aiBoostState > 20)
                _aiBoostState = -100;
        }

        private void HandleKeystrokes()
        {
            _playerPaddle.HandleKeystrokes();
            _aiPaddle.HandleKeystrokes();
            
            if (_playerBoostState == 0 &&
                (Keyboard.GetState().IsKeyDown(Keys.E) || Keyboard.GetState().IsKeyDown(Keys.D)))
                _playerBoostState = 1;
            if (_aiBoostState == 0 && (Keyboard.GetState().IsKeyDown(Keys.Left)))
                _aiBoostState = 1;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) 
                Exit();
        }

        private void ColorBall(Color color)
        {
            Color[] ballColorData = new Color[_ballSize * _ballSize];
            for (int i = 0; i < _ballSize * _ballSize; i++)
            {
                ballColorData[i] = color;
            }
            _ballTexture.SetData(ballColorData);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _ballSprite.Begin();
            _ballSprite.Draw(_ballTexture, _ballPosition, Color.White);
            _ballSprite.End();

            _playerPaddle.Draw();
            _aiPaddle.Draw();

            //Boost 2 display
            if (_playerBoostState > 10)
            {
                Vector2 boostPosition = new Vector2(_playerPaddle.GetX() + _paddleWidth + _boost1Width + 8, _playerPaddle.GetY() + _boost2Offset);
                _playerBoost1Sprite.Begin();
                _playerBoost1Sprite.Draw(_boost2Texture, boostPosition, Color.White);
                _playerBoost1Sprite.End();
            }
            //Player Boost 1 display
            else if (_playerBoostState > 0)
            {
                Vector2 boostPosition = new Vector2(_playerPaddle.GetX() + _paddleWidth + 4, _playerPaddle.GetY() + _boost1Offset);
                _playerBoost2Sprite.Begin();
                _playerBoost2Sprite.Draw(_boost1Texture, boostPosition, Color.White);
                _playerBoost2Sprite.End();
            }
            //AI Boost 2 display
            if (_aiBoostState > 10)
            {
                Vector2 boostPosition = new Vector2(_aiPaddle.GetX() - _boost1Width - _boost2Width - 8, _aiPaddle.GetY() + _boost2Offset);
                _aiBoost1Sprite.Begin();
                _aiBoost1Sprite.Draw(_boost2Texture, boostPosition, Color.White);
                _aiBoost1Sprite.End();
            }
            //AI Boost 1 display
            else if (_aiBoostState > 0)
            {
                Vector2 boostPosition = new Vector2(_aiPaddle.GetX() - _boost1Width - 4, _aiPaddle.GetY() + _boost1Offset);
                _aiBoost2Sprite.Begin();
                _aiBoost2Sprite.Draw(_boost1Texture, boostPosition, Color.White);
                _aiBoost2Sprite.End();
            }
            base.Draw(gameTime);
        }
    }
}
