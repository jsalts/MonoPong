using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPong.Components
{
    public class Ball
    {
        public readonly int _ballSize = 25;
        public Texture2D BallTexture { get; set; }
        public float GameSpeed { get; set; }
        public bool OOB { get; set; }
        public Vector2 BallPosition;       
        public Vector2 BallSpeed;
        public BoundingBox BallBox
        {
            get
            {
                return new BoundingBox(
                    new Vector3(BallPosition, 0),
                    new Vector3(BallPosition.X + _ballSize, BallPosition.Y + _ballSize, 0
                ));
            }
        }

        public Ball(Vector2 ballPosition)
        {
            OOB = false;
            GameSpeed = 1f;
            BallPosition = ballPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BallTexture, BallPosition, Color.White);
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

        public void DetermineBallPosition(GraphicsDevice graphicsDevice)
        {
            //Move ball
            BallPosition.X += BallSpeed.X * GameSpeed;
            BallPosition.Y += BallSpeed.Y * GameSpeed;

            //Check for right player paddle collision
            var leftX = BallPosition.X;
            var right = BallPosition.X + _ballSize;
           
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