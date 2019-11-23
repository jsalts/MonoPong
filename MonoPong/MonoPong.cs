using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoPong.Components;

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
        Paddle _paddleOne;
        Paddle _paddleTwo;
        private int _boost1Offset;
        private int _boost2Offset;
        private List<Ball> _balls = new List<Ball>();
        private int _ballCounter = 0;
        private Ball _ball;
        KeyboardState _lastKeyboardState;
        private NinjaStar _ninjaStar;

        public Game1()
        {
            var unused = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            var viewPortHeight = GraphicsDevice.Viewport.Height;
            var viewPortWidth = GraphicsDevice.Viewport.Width;
            SpawnBall();

            _ninjaStar = new NinjaStar(new Vector2(viewPortWidth / 2f, viewPortHeight / 2f));
            
            _paddleOne = new Paddle(20, viewPortHeight/2, 0);
            _paddleOne.AddUpKeys(Keys.W);
            _paddleOne.AddUpKeys(Keys.OemComma);
            _paddleOne.AddDownKeys(Keys.O);
            _paddleOne.AddDownKeys(Keys.S);
            _paddleOne.AddBoostKeys(Keys.D);
            _paddleOne.AddBoostKeys(Keys.E);
            //_paddleOne.AddRotateKeys(Keys.Right);
            _paddleOne.Initialize();

            _paddleTwo = new Paddle(viewPortWidth - 20, viewPortHeight / 2, 180);
            _paddleTwo.AddUpKeys(Keys.Up);
            _paddleTwo.AddDownKeys(Keys.Down);
            _paddleTwo.AddBoostKeys(Keys.Left);
            //_paddleTwo.AddRotateKeys(Keys.Right);
            _paddleTwo.Initialize();

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

            _boost1Offset = (_paddleHeight - _boost1Height) / 2;
            _boost2Offset = (_paddleHeight - _boost2Height) / 2;
            
            UpdateGameSpeed(_gameSpeed);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _paddleOne.LoadContent(GraphicsDevice);
            _paddleTwo.LoadContent(GraphicsDevice);
            _ninjaStar.LoadContent(GraphicsDevice);

            // Create a new SpriteBatch, which can be used to draw textures.
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
            HandleKeystrokes();

            switch (_gameState)
            {
                case "game":
                    foreach (var ball in _balls)
                    {
                        ball.DetermineBallPosition(_paddleTwo, _paddleOne, _paddleHeight, _paddleWidth, GraphicsDevice);
                    }
                    CheckCollision();
                    _paddleOne.ManageBoost();
                    _paddleTwo.ManageBoost();
                    break;
                case "pause":
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateGameSpeed(float tempSpeed)
        {
            _paddleTwo.GameSpeed = tempSpeed;
            _paddleOne.GameSpeed = tempSpeed;
            foreach (var ball in _balls)
                ball.GameSpeed = tempSpeed;
            _ninjaStar.GameSpeed = tempSpeed;
        }

        private void CheckCollision()
        {
            foreach (var ball in _balls)
                _paddleOne.CheckCollision(ball);
            foreach (var ball in _balls)
                _paddleTwo.CheckCollision(ball);
        }

        private void SpawnBall()
        {
            int i = _balls.Count;
            _balls.Add(new Ball(new Vector2(50, 100)));
            _balls[i].BallSpeed = new Vector2(1, 1);
            _balls[i].BallTexture = new Texture2D(GraphicsDevice, _balls[i]._ballSize, _balls[i]._ballSize);
            _balls[i].ColorBall(Color.White);

        }

        private void HandleKeystrokes()
        {
            //Quit the game anytime escape is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Z) || Keyboard.GetState().IsKeyDown(Keys.OemSemicolon))
                SpawnBall();

            switch (_gameState)
            {
                case "game":
                    _paddleOne.HandleKeystrokes();
                    _paddleTwo.HandleKeystrokes();

                    if (!Keyboard.GetState().IsKeyDown(Keys.Space) & _lastKeyboardState.IsKeyDown(Keys.Space))
                    {
                        _gameState = "pause";
                        UpdateGameSpeed(0);
                    }
                    break;
                case "pause":
                    if (!Keyboard.GetState().IsKeyDown(Keys.Space) & _lastKeyboardState.IsKeyDown(Keys.Space))
                    {
                        _gameState = "game";
                        UpdateGameSpeed(_gameSpeed);
                    }
                    break;
            }

            _lastKeyboardState = Keyboard.GetState();
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
                //_spriteBatch.DrawString(_pauseText, _ball.GetAngle().ToString(), new Vector2(0, 0), Color.White);
                _paddleOne.Draw(_spriteBatch);
                _paddleTwo.Draw(_spriteBatch);
                foreach (var ball in _balls)
                    ball.Draw(_spriteBatch);
                _ninjaStar.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}