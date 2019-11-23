using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoPong.Components;

namespace MonoPong.Effects
{
    public class Boost
    {
        public float RotationDegrees { get; set; }

        private readonly Vector2 _boostPosition;
        private readonly int _boostSizeX;
        private readonly int _boostSizeY;
        public Texture2D BoostTexture { get; set; }
        private bool _boostActive;

        public Boost(float xPos, float yPos, int xSize, int ySize, int rotationDegrees)
        {
            RotationDegrees = rotationDegrees;
            _boostPosition = new Vector2(xPos, yPos);
            _boostSizeX = xSize;
            _boostSizeY = ySize;
            _boostActive = false;
        }

        public void SetColor(Color color)
        {
            Color[] colorData = new Color[_boostSizeX * _boostSizeY];
            for (int i = 0; i < _boostSizeX * _boostSizeY; i++)
            {
                colorData[i] = color;
            }

            BoostTexture.SetData(colorData);
        }
        
        public bool Status()
        {
            return _boostActive;
        }
        public void On()
        {
            _boostActive = true;
        }

        public void Off()
        {
            _boostActive = false;
        }

        public void Draw(SpriteBatch spriteBatch, Paddle paddle)
        {
            spriteBatch.Draw(
                BoostTexture, 
                paddle.GetPosition(), 
                null, 
                Color.White, 
                RotationDegrees * (float)Math.PI / 180,
                -_boostPosition, 
                1, 
                SpriteEffects.None, 
                1);
        }
    }
}