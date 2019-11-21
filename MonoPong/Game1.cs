using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPong
{
    public class Game1 : Game
    {
        private float _gameSpeed = 10f;

        private int _ballSize = 25;
        private int _boost1Width = 12;
        private int _boost2Width = 10;
        private int _boost1Height = 50;
        private int _boost2Height = 20;
        private int _paddleWidth = 20;
        private int _paddleHeight = 100;

        GraphicsDeviceManager _graphics;
        SpriteBatch _ballSprite;
        SpriteBatch _playerPaddleSprite;
        SpriteBatch _aiPaddleSprite;
        SpriteBatch _playerBoost1Sprite;
        SpriteBatch _playerBoost2Sprite;
        Texture2D _ballTexture;
        Texture2D _paddleTexture;
        Texture2D _boost1Texture;
        Texture2D _boost2Texture;
        Vector2 _ballPosition;
        Vector2 _ballSpeed;
        Paddle _playerPaddle;
        Paddle _aiPaddle;
        int _playerBoostState;
        private int _boost1Offset;
        private int _boost2Offset;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _ballPosition = new Vector2(50, 100);
            _playerPaddle = new Paddle(30,20);
            _aiPaddle = new Paddle(750, 70);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _ballTexture = new Texture2D(this.GraphicsDevice, _ballSize, _ballSize);
            ColorBall(Color.White);

            _paddleTexture = new Texture2D(this.GraphicsDevice, 20, 100);
            Color[] paddleColorData = new Color[20 * 100];
            for (int i = 0; i < 2000; i++)
            {
                paddleColorData[i] = Color.White;
            }
            _paddleTexture.SetData(paddleColorData);

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
            _boost1Offset = (_paddleHeight - _boost1Height) / 2;
            _boost2Offset = (_paddleHeight - _boost2Height) / 2;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _ballSprite = new SpriteBatch(GraphicsDevice);
            _playerPaddleSprite = new SpriteBatch(GraphicsDevice);
            _aiPaddleSprite = new SpriteBatch(GraphicsDevice);
            _playerBoost1Sprite = new SpriteBatch(GraphicsDevice);
            _playerBoost2Sprite = new SpriteBatch(GraphicsDevice);

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
            HandleKeystrokes();

            DetermineBallPosition();
            PlayerBoost();
            
            base.Update(gameTime);
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
            if (_playerBoostState < 0)
                _playerBoostState++;
            //Boost 1 Interaction
            if (_playerBoostState > 0)
            {
                if ( _ballSpeed.X < 0 ) { 
                    if (_ballPosition.X <= _playerPaddle.GetX() + _paddleWidth + _boost1Width + 5 && _ballPosition.X > _playerPaddle.GetX() + _paddleWidth + 5 - (_gameSpeed * _ballSpeed.X))
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
            if (_playerBoostState > 5)
            {
                if (_ballSpeed.X < 0)
                {
                    if (_ballPosition.X <= _playerPaddle.GetX() + _paddleWidth +_boost1Width + _boost2Width + 10 && _ballPosition.X > _playerPaddle.GetX() + _paddleWidth + _boost1Width + 10 - (_gameSpeed * _ballSpeed.X))
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
            if (_playerBoostState > 10)
                _playerBoostState = -100;
        }

        private void HandleKeystrokes()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) 
                Exit();
            //Control AI paddle
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                _aiPaddle.MovePaddle(1 * _gameSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                _aiPaddle.MovePaddle(-1 * _gameSpeed);
            //Control Player Paddle
            if (Keyboard.GetState().IsKeyDown(Keys.O) || Keyboard.GetState().IsKeyDown(Keys.S))
                _playerPaddle.MovePaddle(1 * _gameSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.OemComma) || Keyboard.GetState().IsKeyDown(Keys.W))
                _playerPaddle.MovePaddle(-1 * _gameSpeed);
            if (_playerBoostState == 0 && (Keyboard.GetState().IsKeyDown(Keys.E) || Keyboard.GetState().IsKeyDown(Keys.D)))
                _playerBoostState = 1;
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

            // TODO: Add your drawing code here
            _ballSprite.Begin();
            _ballSprite.Draw(_ballTexture, _ballPosition, Color.White);
            _ballSprite.End();
            _playerPaddleSprite.Begin();
            _playerPaddleSprite.Draw(_paddleTexture, _playerPaddle.GetPosition(), Color.White);
            _playerPaddleSprite.End();
            _aiPaddleSprite.Begin();
            _aiPaddleSprite.Draw(_paddleTexture, _aiPaddle.GetPosition(), Color.White);
            _aiPaddleSprite.End();
            if (_playerBoostState > 4)
            {
                Vector2 boostPosition = new Vector2(_playerPaddle.GetX() + _paddleWidth + _boost1Width + 10, _playerPaddle.GetY() + _boost2Offset);
                _playerBoost1Sprite.Begin();
                _playerBoost1Sprite.Draw(_boost2Texture, boostPosition, Color.White);
                _playerBoost1Sprite.End();
            }
            else if (_playerBoostState > 0)
            {
                Vector2 boostPosition = new Vector2(_playerPaddle.GetX() + _paddleWidth + 5, _playerPaddle.GetY() + _boost1Offset);
                _playerBoost2Sprite.Begin();
                _playerBoost2Sprite.Draw(_boost1Texture, boostPosition, Color.White);
                _playerBoost2Sprite.End();
            }
            base.Draw(gameTime);
        }
    }
}
