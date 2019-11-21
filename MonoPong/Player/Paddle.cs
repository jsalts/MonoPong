using Microsoft.Xna.Framework;

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
}