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
        public BoundingBox _boundingBox;

        public Boost(float xPos, float yPos, int width, int height, int rotationDegrees)
        {
            RotationDegrees = rotationDegrees;
            _boostPosition = new Vector2(xPos, yPos);
            _boostSizeX = width;
            _boostSizeY = height;
            _boostActive = false;
        }
        public BoundingBox BoundingBox
        {
            get
            {
                Vector3 topLeft = new Vector3(0,0,0);
                Vector3 botRight = new Vector3(0,0,0);
                if (RotationDegrees == 0)
                {
                    topLeft = new Vector3(_boostPosition, 0);
                    botRight = new Vector3(_boostPosition.X + _boostSizeX, _boostPosition.Y + _boostSizeY, 0);
                }
                else if (RotationDegrees == 180)
                {
                    var rotatedX = (_boostPosition.X * Math.Cos(RotationDegrees) - (_boostPosition.Y * Math.Sin(RotationDegrees)));
                    topLeft = new Vector3((float)rotatedX - _boostSizeX, _boostPosition.Y, 0);
                    botRight = new Vector3((float)rotatedX,_boostPosition.Y + _boostSizeY, 0);
                }
                return new BoundingBox(topLeft, botRight);
            }
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
                new Vector2(-_boostPosition.X, BoostTexture.Height / 2f), 
                1, 
                SpriteEffects.None, 
                1);
        }
    }
}