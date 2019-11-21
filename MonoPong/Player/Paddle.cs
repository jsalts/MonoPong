using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPong.Player
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Paddle
    {
        private readonly List<Keys> _upKeys = new List<Keys>();
        private readonly List<Keys> _downKeys = new List<Keys>();
        private Vector2 _paddlePosition;
        private float _gameSpeed = 1f;

        private Texture2D _paddleTexture;
        private SpriteBatch _paddleSprite;

        public Paddle(int x, int y)
        {
            _paddlePosition = new Vector2(x, y);
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

        public float GetX()
        {
            return _paddlePosition.X;
        }

        public float GetY()
        {
            return _paddlePosition.Y;
        }

        private Vector2 GetPosition()
        {
            return _paddlePosition;
        }

        private void MovePaddle(float speed)
        {
            _paddlePosition.Y += speed;
            if (_paddlePosition.Y < 0)
                _paddlePosition.Y = 0;
            if (_paddlePosition.Y + 100 > 480)
                _paddlePosition.Y = 380;
        }

        public void HandleKeystrokes()
        {
            var upPressed = _upKeys.Any(x => Keyboard.GetState().IsKeyDown(x));
            var downPressed = _downKeys.Any(x => Keyboard.GetState().IsKeyDown(x));

            if (upPressed)
            {
                MovePaddle(-1 * GameSpeed);
            }

            if (downPressed)
            {
                MovePaddle(1 * GameSpeed);
            }
        }

        public void Draw()
        {
            _paddleSprite.Begin();
            _paddleSprite.Draw(_paddleTexture, GetPosition(), Color.White);
            _paddleSprite.End();
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            _paddleSprite = new SpriteBatch(graphicsDevice);
            _paddleTexture = new Texture2D(graphicsDevice, 20, 100);
            Color[] paddleColorData = new Color[20 * 100];
            for (int i = 0; i < 2000; i++)
            {
                paddleColorData[i] = Color.White;
            }
            _paddleTexture.SetData(paddleColorData);
        }

        public void Initialize()
        {

        }
    }
}