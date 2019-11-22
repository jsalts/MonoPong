using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPong.Player
{
    public class Ball
    {
        public readonly int _ballSize = 25;
        public SpriteBatch BallSprite { get; set; }
        public Texture2D BallTexture { get; set; }
        public float GameSpeed { get; set; }

        public Vector2 BallPosition;
        
        public Vector2 BallSpeed;

        public Ball(Vector2 ballPosition)
        {
            BallPosition = ballPosition;
        }

        public void Draw()
        {
            BallSprite.Begin();
            BallSprite.Draw(BallTexture, BallPosition, Color.White);
            BallSprite.End();
        }
        
        public void ColorBall(Color color)
        {
            Color[] ballColorData = new Color[_ballSize * _ballSize];
            for (int i = 0; i < _ballSize * _ballSize; i++)
            {
                ballColorData[i] = color;
            }

            BallTexture.SetData(ballColorData);
        }

        public void DetermineBallPosition(Paddle aiPaddle, Paddle playerPaddle, int paddleHeight, int paddleWidth, GraphicsDevice graphicsDevice)
        {
            //Move ball
            BallPosition.X += BallSpeed.X * GameSpeed;
            BallPosition.Y += BallSpeed.Y * GameSpeed;

            //Check for right player paddle collision
            if (BallPosition.X + _ballSize >= aiPaddle.GetX() && BallPosition.X + _ballSize < aiPaddle.GetX() + (GameSpeed * BallSpeed.X))
            {
                if (BallPosition.Y > aiPaddle.GetY() && BallPosition.Y < aiPaddle.GetY() + paddleHeight) BallSpeed.X = -1;
                if (BallPosition.Y + _ballSize > aiPaddle.GetY() && BallPosition.Y + _ballSize < aiPaddle.GetY() + paddleHeight) BallSpeed.X = -1;
            }
            //Check for left player paddle collision
            if (BallPosition.X <= playerPaddle.GetX() + paddleWidth && BallPosition.X > playerPaddle.GetX() + paddleWidth + (GameSpeed * BallSpeed.X))
            {
                if (BallPosition.Y > playerPaddle.GetY() && BallPosition.Y < playerPaddle.GetY() + paddleHeight) BallSpeed.X = 1;
                if (BallPosition.Y + _ballSize > playerPaddle.GetY() && BallPosition.Y + _ballSize < playerPaddle.GetY() + paddleHeight) BallSpeed.X = 1;
            }
            //Check for bottom collision
            if (BallPosition.Y + _ballSize > 480)
            {
                BallSpeed.Y = -1;
            }
            //Check for top collision
            if (BallPosition.Y < 0)
            {
                BallSpeed.Y = 1;
            }
            
            if (BallPosition.X < 0)
            {
                BallSpeed.X *= -1;
            }
            if (BallPosition.X + _ballSize > graphicsDevice.Viewport.Width)
            {
                BallSpeed.X *= -1;
            }


            //Update Ball Color
            if (BallSpeed.X > 2 || BallSpeed.X < -2)
            {
                ColorBall(Color.Red);
            }
            else if (BallSpeed.X > 1 || BallSpeed.X < -1)
            {
                ColorBall(Color.Yellow);
            }
            else
            {
                ColorBall(Color.White);
            }
        }
        
    }
}