using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Paddle
    {
        private Vector2 paddlePosition;

        public Paddle(int x, int y)
        {
            this.paddlePosition = new Vector2(x, y);
        }
        public float GetX()
        {
            return paddlePosition.X;
        }
        public float GetY()
        {
            return paddlePosition.Y;
        }
        public Vector2 GetPosition()
        {
            return paddlePosition;
        }
        public void MovePaddle(float speed)
        {
            paddlePosition.Y += speed;
            if (paddlePosition.Y < 0)
                paddlePosition.Y = 0;
            if (paddlePosition.Y + 100 > 480)
                paddlePosition.Y = 380;
        }
        public Vector2 PaddlePosition
        {
            get { return paddlePosition; }
            set { PaddlePosition = value;  }
        }
    }

    public class Game1 : Game
    {
        private float _gameSpeed = 3f;
        GraphicsDeviceManager _graphics;
        SpriteBatch _ballSprite;
        SpriteBatch _playerPaddleSprite;
        SpriteBatch _aiPaddleSprite;
        Texture2D _ballTexture;
        Texture2D _paddleTexture;
        Vector2 _ballPosition;
        Vector2 _ballSpeed;
        Paddle _playerPaddle;
        Paddle _aiPaddle;

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
            _ballTexture = new Texture2D(this.GraphicsDevice, 100, 100);
            Color[] ballColorData = new Color[100 * 100];
            for (int i = 0; i < 10000; i++)
            {
                ballColorData[i] = Color.White;
            }   
            _ballTexture.SetData(ballColorData);

            _paddleTexture = new Texture2D(this.GraphicsDevice, 20, 100);
            Color[] paddleColorData = new Color[20 * 100];
            for (int i = 0; i < 2000; i++)
            {
                paddleColorData[i] = Color.White;
            }
            _paddleTexture.SetData(paddleColorData);

            base.Initialize();

            _ballSpeed = new Vector2(1, 0);
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
            
            base.Update(gameTime);
        }

        private void DetermineBallPosition()
        {
            //Move ball
            _ballPosition.X += _ballSpeed.X * _gameSpeed;

            //Check for right player paddle collision
            if (_ballPosition.X+100 == _aiPaddle.GetX())
            {
                if (_ballPosition.Y > _aiPaddle.GetY() && _ballPosition.Y < _aiPaddle.GetY() + 100)
                    _ballSpeed.X = -1;
                if (_ballPosition.Y + 100 > _aiPaddle.GetY() && _ballPosition.Y + 100 < _aiPaddle.GetY() + 100)
                    _ballSpeed.X = -1;
            }
            //Check for left player paddle collision
            if (_ballPosition.X == _playerPaddle.GetX() + 20)
            {
                if (_ballPosition.Y > _playerPaddle.GetY() && _ballPosition.Y < _playerPaddle.GetY() + 100)
                    _ballSpeed.X = 1;
                if (_ballPosition.Y + 100 > _playerPaddle.GetY() && _ballPosition.Y + 100 < _playerPaddle.GetY() + 100)
                    _ballSpeed.X = 1;
            }

            //Check for game over
            if (_ballPosition.X < 0)
                Exit();
            if (_ballPosition.X+100 > this.GraphicsDevice.Viewport.Width)
                Exit();
        }

        private void HandleKeystrokes()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) 
                Exit();
            
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                //_aiPaddlePosition.Y += 1 * _gameSpeed;
                _aiPaddle.MovePaddle(1 * _gameSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                //_aiPaddlePosition.Y -= 1 * _gameSpeed;
                _aiPaddle.MovePaddle(-1 * _gameSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.O) || Keyboard.GetState().IsKeyDown(Keys.S))
                _playerPaddle.MovePaddle(1 * _gameSpeed);
            if (Keyboard.GetState().IsKeyDown(Keys.OemComma) || Keyboard.GetState().IsKeyDown(Keys.W))
                _playerPaddle.MovePaddle(-1 * _gameSpeed);
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
            base.Draw(gameTime);
        }
    }
}
