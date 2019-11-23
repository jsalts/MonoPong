using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoPong.Effects;

namespace MonoPong.Components
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Paddle
    {
        public float RotationDegrees
        {
            get { return _rotationDegrees; }
            set
            {
                if (_boost1 != null)
                {
                    _boost1.RotationDegrees = value;
                    _boost2.RotationDegrees = value;
                }

                _rotationDegrees = value;
            }
        }

        private int _paddleWidth = 21;
        private int _paddleHeight = 101;
        private readonly List<Keys> _upKeys = new List<Keys>();
        private readonly List<Keys> _downKeys = new List<Keys>();
        private readonly List<Keys> _boostKeys = new List<Keys>();
        private readonly List<Keys> _rotateKeys = new List<Keys>();
        private Vector2 _paddlePosition;
        private float _gameSpeed = 1f;

        public BoundingBox BoundingBox
        {
            get
            {
                var left = _paddlePosition.X - _paddleWidth / 2f;
                var top = _paddlePosition.Y - _paddleHeight / 2f;
                var topLeft = new Vector3(left, top, 0);
                var right = _paddlePosition.X + _paddleWidth / 2f;
                var bottom = _paddlePosition.Y + _paddleHeight / 2f;
                var bottomRight = new Vector3(right, bottom, 0);
                return new BoundingBox(topLeft, bottomRight);
            }
        }

        private Texture2D PaddleTexture { get; set; }
        private readonly Boost _boost1;
        private readonly Boost _boost2;
        private int _boost1Width = 12;
        private int _boost1Height = 50;
        private int _boost2Width = 10;
        private int _boost2Height = 26;
        private int _boostState;
        private float _rotationDegrees;

        public Paddle(float x, float y, int rotationDegrees)
        {
            _paddlePosition = new Vector2(x, y);
            
            RotationDegrees = rotationDegrees;
            _boost1 = new Boost(
                this,
                _paddleWidth + 4,
                (_paddleHeight - _boost1Height) / 2f,
                _boost1Width,
                _boost1Height,
                rotationDegrees);

            _boost2 = new Boost(
                this,
                _paddleWidth + 8 + _boost1Width,
                (_paddleHeight - _boost2Height) / 2f,
                _boost2Width,
                _boost2Height,
                rotationDegrees);
        }

        public float GameSpeed
        {
            get { return _gameSpeed; }
            set { _gameSpeed = value; }
        }

        public void AddUpKeys(Keys key)
        {
            _upKeys.Add(key);
        }

        public void AddDownKeys(Keys key)
        {
            _downKeys.Add(key);
        }

        public void AddBoostKeys(Keys key)
        {
            _boostKeys.Add(key);
        }

        public void AddRotateKeys(Keys key)
        {
            _rotateKeys.Add(key);
        }

        public float GetX()
        {
            return _paddlePosition.X;
        }

        public float GetY()
        {
            return _paddlePosition.Y;
        }

        public Vector2 GetPosition()
        {
            return _paddlePosition;
        }

        private void MovePaddle(float ySpeed)
        {
            if (_paddlePosition.Y - ySpeed <= 0 + (_paddleHeight / 2) && ySpeed < 0)
                ySpeed = 0 + (_paddleHeight / 2) - _paddlePosition.Y;
            if (_paddlePosition.Y + ySpeed >= 380 + (_paddleHeight / 2) && ySpeed > 0)
                ySpeed = 380 + (_paddleHeight / 2) - _paddlePosition.Y;

            _paddlePosition.Y += ySpeed;
        }

        public void HandleKeystrokes()
        {
            var upPressed = _upKeys.Any(x => Keyboard.GetState().IsKeyDown(x));
            var downPressed = _downKeys.Any(x => Keyboard.GetState().IsKeyDown(x));
            var boostPressed = _boostKeys.Any(x => Keyboard.GetState().IsKeyDown(x));
            var rotatePressed = _rotateKeys.Any(x => Keyboard.GetState().IsKeyDown(x));

            if (upPressed)
            {
                MovePaddle(-1.5f * GameSpeed);
            }

            if (downPressed)
            {
                MovePaddle(1.5f * GameSpeed);
            }

            if (boostPressed && _boostState == 0)
            {
                _boostState = 1;
            }

            if (rotatePressed)
            {
                RotationDegrees += 1 % 360;
            }
        }

        private void SetColor(Color color)
        {
            Color[] paddleColorData = new Color[_paddleWidth * _paddleHeight];
            for (int i = 0; i < _paddleWidth * _paddleHeight; i++)
            {
                paddleColorData[i] = color;
            }

            PaddleTexture.SetData(paddleColorData);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        
            spriteBatch.Draw(
                PaddleTexture,
                GetPosition(),
                null,
                Color.White,
                RotationDegrees * (float) Math.PI / 180,
                new Vector2(_paddleWidth / 2f, _paddleHeight / 2f),
                1,
                SpriteEffects.None,
                1);

            if (_boost1.Status())
            {
                _boost1.Draw(spriteBatch);
            }

            if (_boost2.Status())
            {
                _boost2.Draw(spriteBatch);
            }
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            //Set up Paddle
            PaddleTexture = new Texture2D(graphicsDevice, _paddleWidth, _paddleHeight);
            SetColor(Color.White);

            //Set up Boost 1
            _boost1.BoostTexture = new Texture2D(graphicsDevice, _boost1Width, _boost1Height);
            _boost1.SetColor(Color.Blue);

            //Set up Boost 2
            _boost2.BoostTexture = new Texture2D(graphicsDevice, _boost2Width, _boost2Height);
            _boost2.SetColor(Color.CornflowerBlue);
        }

        public void Initialize()
        {
        }

        public void ManageBoost()
        {
            //Increment Cooldown State
            if (_boostState < 0)
                _boostState++;
            //Boost1 Active
            else if (_boostState == 1)
            {
                _boost1.On();
                _boostState++;
            }
            else if (_boostState > 1 && _boostState < 10)
                _boostState++;
            //Boost2 Active
            else if (_boostState == 10)
            {
                _boost1.Off();
                _boost2.On();
                _boostState++;
            }
            else if (_boostState > 10 && _boostState < 20)
                _boostState++;
            //Boost on Cooldown
            else if (_boostState >= 20)
            {
                _boost2.Off();
                _boostState = -100;
            }
        }

        public void CheckCollision(Ball ball)
        {
            bool isLeftPaddle = _rotationDegrees == 0;
            bool isRightPaddle = _rotationDegrees == 180;

            if (ball.BallBox.Intersects(_boost2.BoundingBox) && (ball.BallSpeed.X < 0 && isLeftPaddle || ball.BallSpeed.X > 0 && isRightPaddle))
            {
                ball.BallSpeed.X = ball.BallSpeed.X * -3;
            }
            else if (ball.BallBox.Intersects(_boost1.BoundingBox) && (ball.BallSpeed.X < 0 && isLeftPaddle || ball.BallSpeed.X > 0 && isRightPaddle))
            {
                ball.BallSpeed.X = ball.BallSpeed.X * -2;
            }
            else if (ball.BallBox.Intersects(BoundingBox) && (ball.BallSpeed.X < 0 && isLeftPaddle || ball.BallSpeed.X > 0 && isRightPaddle))
            {
                ball.BallSpeed.X = ball.BallSpeed.X * -1;
            }
        }
    }
}