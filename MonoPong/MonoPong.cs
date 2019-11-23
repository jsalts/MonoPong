using System;
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
        private double _score;
        private double _highScore;
        private int _maxBalls;

        private int _boost1Width = 12;
        private int _boost2Width = 10;
        private int _boost1Height = 50;
        private int _boost2Height = 26;

        private enum gameStates { menu, game, pause, lose };
        private gameStates _gameState;

        SpriteBatch _spriteBatch;
        SpriteFont _pauseText;
        SpriteFont _scoreText;
        Texture2D _boost1Texture;
        Texture2D _boost2Texture;
        Paddle _paddleOne;
        Paddle _paddleTwo;
        private readonly List<Ball> _balls = new List<Ball>();
        KeyboardState _lastKeyboardState;
        //private NinjaStar _ninjaStar;

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
            _gameState = gameStates.game;
            var viewPortHeight = GraphicsDevice.Viewport.Height;
            var viewPortWidth = GraphicsDevice.Viewport.Width;
            SpawnBall();

            //_ninjaStar = new NinjaStar(new Vector2(viewPortWidth / 2f, viewPortHeight / 2f));
            
            _paddleOne = new Paddle(20, viewPortHeight / 2f, 0);
            _paddleOne.AddUpKeys(Keys.W);
            _paddleOne.AddUpKeys(Keys.OemComma);
            _paddleOne.AddDownKeys(Keys.O);
            _paddleOne.AddDownKeys(Keys.S);
            _paddleOne.AddBoostKeys(Keys.D);
            _paddleOne.AddBoostKeys(Keys.E);
            //_paddleOne.AddRotateKeys(Keys.Right);
            _paddleOne.Initialize();

            _paddleTwo = new Paddle(viewPortWidth - 20, viewPortHeight / 2f, 180);
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
            //_ninjaStar.LoadContent(GraphicsDevice);

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pauseText = Content.Load<SpriteFont>("PauseText");
            _scoreText = Content.Load<SpriteFont>("ScoreText");

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
            UpdateGameSpeed(_gameSpeed);
            HandleKeystrokes();

            switch (_gameState)
            {
                case gameStates.game:
                    _balls.ForEach(b => b.DetermineBallPosition(GraphicsDevice));

                    if (CheckCollision())
                    {
                        SpawnBall();
                    }
                    _paddleOne.ManageBoost();
                    _paddleTwo.ManageBoost();
                    ClearOOB();
                    UpdateScore();
                    if (_balls.Count == 0)
                        _gameState = gameStates.lose;
                    break;
                case gameStates.pause:
                    break;
                case gameStates.lose:
                    if (_score > _highScore)
                    {
                        _highScore = _score;
                    }
                    if (_balls.Count > 0)
                    {
                        _gameState = gameStates.game;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateGameSpeed(float tempSpeed)
        {
            //_paddleTwo.GameSpeed = tempSpeed;
            //_paddleOne.GameSpeed = tempSpeed;
            //_ninjaStar.GameSpeed = tempSpeed;
            _balls.ForEach(b => b.GameSpeed = tempSpeed);
        }

        private bool CheckCollision()
        {
            bool spawn = false;

            _balls.ForEach(b =>
            {
                //Check paddle 1
                int result;
                result = _paddleOne.CheckCollision(b);
                if (result >= 2)
                {
                    spawn = true;
                }
                if (result >= 1)
                {
                    b.BallSpeed.X = b.BallSpeed.X * -1 * result;
                }
                result = _paddleTwo.CheckCollision(b);
                //check paddle 2
                if (result >= 2)
                {
                    spawn = true;
                }
                if (result >= 1)
                {
                    b.BallSpeed.X = b.BallSpeed.X * -1 * result;
                }
            });
            return spawn;
        }

        private void ClearOOB()
        {
            _balls.ForEach(b =>
            {
                if (b.BallPosition.X < 0 || b.BallPosition.X > GraphicsDevice.Viewport.Width)
                {
                    b.OOB = true;
                }
            });
            _balls.RemoveAll(ball => ball.OOB == true);
        }

        private void Restart()
        {
            _score = 0;
            SpawnBall();
            _gameState = gameStates.game;
        }
        private void UpdateScore()
        {
            _score += Math.Pow(2, _balls.Count);
            if (_balls.Count > _maxBalls )
            {
                _maxBalls = _balls.Count;
            }
        }
        private void SpawnBall()
        {
            var newBall = new Ball(new Vector2(50, 100));
            newBall.BallSpeed = new Vector2(1, 1);
            newBall.BallTexture = new Texture2D(GraphicsDevice, newBall._ballSize, newBall._ballSize);
            newBall.ColorBall(Color.White);
            _balls.Add(newBall);
        }

        private void HandleKeystrokes()
        {
            //Quit the game anytime escape is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            switch (_gameState)
            {
                case gameStates.game:
                    _paddleOne.HandleKeystrokes();
                    _paddleTwo.HandleKeystrokes();

                    if (!Keyboard.GetState().IsKeyDown(Keys.Space) & _lastKeyboardState.IsKeyDown(Keys.Space))
                    {
                        _gameState = gameStates.pause;
                        UpdateGameSpeed(0);
                    }
                    break;
                case gameStates.pause:
                    if (!Keyboard.GetState().IsKeyDown(Keys.Space) & _lastKeyboardState.IsKeyDown(Keys.Space))
                    {
                        _gameState = gameStates.game;
                        UpdateGameSpeed(_gameSpeed);
                    }
                    break;
                case gameStates.lose:
                    if (Keyboard.GetState().IsKeyDown(Keys.Z) || Keyboard.GetState().IsKeyDown(Keys.OemSemicolon))
                    {
                        Restart();
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
            if (_gameState == gameStates.game || _gameState == gameStates.pause || _gameState == gameStates.lose)
            {
                if (_gameState == gameStates.pause)
                {
                    _spriteBatch.DrawString(_pauseText, "PAUSED", new Vector2(200, 170), Color.White);
                }
                if (_gameState == gameStates.lose)
                {
                    _spriteBatch.DrawString(_pauseText, "GAME OVER", new Vector2(120, 100), Color.White);
                    _spriteBatch.DrawString(_pauseText, "Z to Retry", new Vector2(200, 210), Color.White);
                    _spriteBatch.DrawString(_scoreText, "High Score: " + _highScore, new Vector2(250, 340), Color.White);
                    _spriteBatch.DrawString(_scoreText, "Max Balls: " + _maxBalls, new Vector2(300, 400), Color.White);

                }
                //Draw Top Status
                _spriteBatch.DrawString(_scoreText, "Score: " + _score, new Vector2(10, 0), Color.White);
                _spriteBatch.DrawString(_scoreText, "Balls: " + _balls.Count.ToString(), new Vector2(620, 0), Color.White);
                _paddleOne.Draw(_spriteBatch);
                _paddleTwo.Draw(_spriteBatch);
                
                _balls.ForEach(b => b.Draw(_spriteBatch));

                //_ninjaStar.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}