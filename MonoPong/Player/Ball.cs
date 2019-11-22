using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPong
{
    public class Ball
    {
        public int _ballSize = 25;
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

//        public void DetermineBallPosition()
//        {
//            //Move ball
//            BallPosition.X += BallSpeed.X * GameSpeed;
//            BallPosition.Y += BallSpeed.Y * GameSpeed;
//
//            //Check for right player paddle collision
//            if (BallPosition.X + _ballSize >= _aiPaddle.GetX() && BallPosition.X + _ballSize < _aiPaddle.GetX() + (GameSpeed * BallSpeed.X))
//            {
//                if (BallPosition.Y > _aiPaddle.GetY() && BallPosition.Y < _aiPaddle.GetY() + _paddleHeight) BallSpeed.X = -1;
//                if (BallPosition.Y + _ballSize > _aiPaddle.GetY() && BallPosition.Y + _ballSize < _aiPaddle.GetY() + _paddleHeight) BallSpeed.X = -1;
//            }
//            //Check for left player paddle collision
//            if (BallPosition.X <= _playerPaddle.GetX() + _paddleWidth && BallPosition.X > _playerPaddle.GetX() + _paddleWidth + (GameSpeed * BallSpeed.X))
//            {
//                if (BallPosition.Y > _playerPaddle.GetY() && BallPosition.Y < _playerPaddle.GetY() + _paddleHeight) BallSpeed.X = 1;
//                if (BallPosition.Y + _ballSize > _playerPaddle.GetY() && BallPosition.Y + _ballSize < _playerPaddle.GetY() + _paddleHeight) BallSpeed.X = 1;
//            }
//            //Check for bottom collision
//            if (BallPosition.Y + _ballSize > 480) BallSpeed.Y = -1;
//            //Check for top collision
//            if (BallPosition.Y < 0) BallSpeed.Y = 1;
//            //Check for game over
//            if (BallPosition.X < 0)
////                Exit();
//            if (BallPosition.X + _ballSize > GraphicsDevice.Viewport.Width)
//                //Exit();
//                BallSpeed.X = BallSpeed.X * -1;
//
//            //Update Ball Color
//            if (BallSpeed.X > 2 || BallSpeed.X < -2)
//                ColorBall(Color.Red);
//            else if (BallSpeed.X > 1 || BallSpeed.X < -1)
//                ColorBall(Color.Yellow);
//            else
//                ColorBall(Color.White);
//        }
    }
}