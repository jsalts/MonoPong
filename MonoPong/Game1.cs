using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoPong.Player;

namespace MonoPong
{
    public class Game1 : Game
    {
        private float _gameSpeed = 5f;

        private int _boost1Width = 12;
        private int _boost2Width = 10;
        private int _boost1Height = 50;
        private int _boost2Height = 26;
        private int _paddleWidth = 20;
        private int _paddleHeight = 100;

        private string _gameState = "game";

        SpriteBatch _spriteBatch;
        SpriteFont _pauseText;
        Texture2D _boost1Texture;
        Texture2D _boost2Texture;
        readonly Paddle _playerPaddle;
        readonly Paddle _aiPaddle;
        int _playerBoostState;
        int _aiBoostState;
        private int _boost1Offset;
        private int _boost2Offset;
        private readonly Ball _ball;
        KeyboardState lastKeyboardState;


        public Game1()
        {
            var unused = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _ball = new Ball(new Vector2(50, 100));
            _playerPaddle = new Paddle(30, 70);
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

            _ball.BallTexture = new Texture2D(GraphicsDevice, _ball._ballSize, _ball._ballSize);
            _ball.ColorBall(Color.White);

            _boost1Texture = new Texture2D(GraphicsDevice, _boost1Width, _boost1Height);
            Color[] boost1ColorData = new Color[_boost1Width * _boost1Height];
            for (int i = 0; i < _boost1Width * _boost1Height; i++)
            {
                boost1ColorData[i] = Color.Blue;
            }

            _boost1Texture.SetData(boost1ColorData);

            _boost2Texture = new Texture2D(GraphicsDevice, _boost2Width, _boost2Height);
            Color[] boost2ColorData = new Color[_boost2Width * _boost2Height];
            for (int i = 0; i < _boost2Width * _boost2Height; i++)
            {
                boost2ColorData[i] = Color.CornflowerBlue;
            }

            _boost2Texture.SetData(boost2ColorData);

            base.Initialize();

            _ball.BallSpeed = new Vector2(1, 1);
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
            _ball.BallSprite = new SpriteBatch(GraphicsDevice);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pauseText = Content.Load<SpriteFont>("PauseText");

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

            switch (_gameState)
            {
                case "game":
                    _ball.DetermineBallPosition(_aiPaddle, _playerPaddle, _paddleHeight, _paddleWidth);
                    PlayerBoost();
                    AiBoost();
                    break;
                case "pause":
                    break;
            }

            PlayerBoost();
            AiBoost();

            base.Update(gameTime);
        }

        private void UpdateGameSpeed()
        {
            _aiPaddle.GameSpeed = _gameSpeed;
            _playerPaddle.GameSpeed = _gameSpeed;
            _ball.GameSpeed = _gameSpeed;
        }

        private void PlayerBoost()
        {
            if (_playerBoostState < 0)
                _playerBoostState++;
            //Boost 1 Interaction
            else if (_playerBoostState > 0 && _playerBoostState <= 10)
            {
                if (_ball.BallSpeed.X < 0)
                {
                    if (_ball.BallPosition.X <= _playerPaddle.GetX() + _paddleWidth + _boost1Width + 6 &&
                        _ball.BallPosition.X >
                        _playerPaddle.GetX() + _paddleWidth + 4 - (_gameSpeed * _ball.BallSpeed.X))
                    {
                        if (_ball.BallPosition.Y > _playerPaddle.GetY() + _boost1Offset && _ball.BallPosition.Y <
                            _playerPaddle.GetY() + _boost1Offset + _boost1Height)
                            _ball.BallSpeed.X = _ball.BallSpeed.X * -2f;
                        else if (_ball.BallPosition.Y + _ball._ballSize > _playerPaddle.GetY() + _boost1Offset &&
                                 _ball.BallPosition.Y + _ball._ballSize <
                                 _playerPaddle.GetY() + _boost1Offset + _boost1Height)
                            _ball.BallSpeed.X = _ball.BallSpeed.X * -2f;
                    }
                }

                _playerBoostState++;
            }
            //Boost 2 Interaction
            else if (_playerBoostState > 10 && _playerBoostState <= 20)
            {
                if (_ball.BallSpeed.X < 0)
                {
                    if (_ball.BallPosition.X <=
                        _playerPaddle.GetX() + _paddleWidth + _boost1Width + _boost2Width + 10 && _ball.BallPosition.X >
                        _playerPaddle.GetX() + _paddleWidth + _boost1Width + 6 - (_gameSpeed * _ball.BallSpeed.X))
                    {
                        if (_ball.BallPosition.Y > _playerPaddle.GetY() + _boost2Offset && _ball.BallPosition.Y <
                            _playerPaddle.GetY() + _boost2Offset + _boost2Height)
                            _ball.BallSpeed.X = _ball.BallSpeed.X * -3f;
                        else if (_ball.BallPosition.Y + _ball._ballSize > _playerPaddle.GetY() + _boost2Offset &&
                                 _ball.BallPosition.Y + _ball._ballSize <
                                 _playerPaddle.GetY() + _boost2Offset + _boost2Height)
                            _ball.BallSpeed.X = _ball.BallSpeed.X * -3f;
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
            if (_aiBoostState < 0)
                _aiBoostState++;
            //Boost 1 Interaction
            else if (_aiBoostState > 0 && _aiBoostState <= 10)
            {
                if (_ball.BallSpeed.X > 0)
                {
                    if (_ball.BallPosition.X + _ball._ballSize >= _aiPaddle.GetX() - _boost1Width - 4 &&
                        _ball.BallPosition.X + _ball._ballSize <
                        _aiPaddle.GetX() - 4 + (_gameSpeed * _ball.BallSpeed.X))
                    {
                        if (_ball.BallPosition.Y > _aiPaddle.GetY() + _boost1Offset &&
                            _ball.BallPosition.Y < _aiPaddle.GetY() + _boost1Offset + _boost1Height)
                            _ball.BallSpeed.X = _ball.BallSpeed.X * -2f;
                        else if (_ball.BallPosition.Y + _ball._ballSize > _aiPaddle.GetY() + _boost1Offset &&
                                 _ball.BallPosition.Y + _ball._ballSize <
                                 _aiPaddle.GetY() + _boost1Offset + _boost1Height)
                            _ball.BallSpeed.X = _ball.BallSpeed.X * -2f;
                    }
                }

                _aiBoostState++;
            }
            //Boost 2 Interaction
            else if (_aiBoostState > 10 && _aiBoostState <= 20)
            {
                if (_ball.BallSpeed.X > 0)
                {
                    if (_ball.BallPosition.X + _ball._ballSize >= _aiPaddle.GetX() - _boost1Width - _boost2Width - 10 &&
                        _ball.BallPosition.X + _ball._ballSize <
                        _aiPaddle.GetX() - _boost1Width - 6 + (_gameSpeed * _ball.BallSpeed.X))
                    {
                        if (_ball.BallPosition.Y > _aiPaddle.GetY() + _boost2Offset &&
                            _ball.BallPosition.Y < _aiPaddle.GetY() + _boost2Offset + _boost2Height)
                            _ball.BallSpeed.X = _ball.BallSpeed.X * -3f;
                        else if (_ball.BallPosition.Y + _ball._ballSize > _aiPaddle.GetY() + _boost2Offset &&
                                 _ball.BallPosition.Y + _ball._ballSize <
                                 _aiPaddle.GetY() + _boost2Offset + _boost2Height)
                            _ball.BallSpeed.X = _ball.BallSpeed.X * -3f;
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
            //Quit the game anytime escape is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (_gameState)
            {
                case "game":
                    _playerPaddle.HandleKeystrokes();
                    _aiPaddle.HandleKeystrokes();

                    if (_playerBoostState == 0 &&
                        (Keyboard.GetState().IsKeyDown(Keys.E) || Keyboard.GetState().IsKeyDown(Keys.D)))
                        _playerBoostState = 1;
                    if (_aiBoostState == 0 && (Keyboard.GetState().IsKeyDown(Keys.Left)))
                        _aiBoostState = 1;
                    if (!Keyboard.GetState().IsKeyDown(Keys.Space) & lastKeyboardState.IsKeyDown(Keys.Space))
                        _gameState = "pause";
                    break;
                case "pause":
                    if (!Keyboard.GetState().IsKeyDown(Keys.Space) & lastKeyboardState.IsKeyDown(Keys.Space))
                        _gameState = "game";
                    break;
            }

            lastKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            if (_gameState == "game" || _gameState == "pause")
            {
                if (_gameState == "pause")
                {
                    _spriteBatch.DrawString(_pauseText, "PAUSED", new Vector2(200, 150), Color.White);
                }

                _playerPaddle.Draw();
                _aiPaddle.Draw();
                _ball.Draw();

                //Boost 2 display
                if (_playerBoostState > 10)
                {
                    Vector2 boostPosition = new Vector2(_playerPaddle.GetX() + _paddleWidth + _boost1Width + 8,
                        _playerPaddle.GetY() + _boost2Offset);
                    _spriteBatch.Draw(_boost2Texture, boostPosition, Color.White);
                }
                //Player Boost 1 display
                else if (_playerBoostState > 0)
                {
                    Vector2 boostPosition = new Vector2(_playerPaddle.GetX() + _paddleWidth + 4,
                        _playerPaddle.GetY() + _boost1Offset);
                    _spriteBatch.Draw(_boost1Texture, boostPosition, Color.White);
                }

                //AI Boost 2 display
                if (_aiBoostState > 10)
                {
                    Vector2 boostPosition = new Vector2(_aiPaddle.GetX() - _boost1Width - _boost2Width - 8,
                        _aiPaddle.GetY() + _boost2Offset);
                    _spriteBatch.Draw(_boost2Texture, boostPosition, Color.White);
                }
                //AI Boost 1 display
                else if (_aiBoostState > 0)
                {
                    Vector2 boostPosition = new Vector2(_aiPaddle.GetX() - _boost1Width - 4,
                        _aiPaddle.GetY() + _boost1Offset);
                    _spriteBatch.Draw(_boost1Texture, boostPosition, Color.White);
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}