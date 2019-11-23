using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPong.Components
{
    public class NinjaStar
    {
        private readonly List<Paddle> _paddles;

        public NinjaStar(Vector2 position)
        {
            _paddles = new List<Paddle>
            {
                new Paddle(position.X, position.Y, 0),
                new Paddle(position.X, position.Y, 90),
                new Paddle(position.X, position.Y, 180),
                new Paddle(position.X, position.Y, 270)
            };
        }

        public float GameSpeed { get; set; }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            _paddles.ForEach(p => p.LoadContent(graphicsDevice));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _paddles.ForEach(p =>
            {
                p.RotationDegrees += GameSpeed;
                p.Draw(spriteBatch);
            });
        }
    }
}