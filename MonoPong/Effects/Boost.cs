using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoPong.Components;

namespace MonoPong.Effects
{
    public class Boost
    {
        public float RotationDegrees { get; set; }

        private readonly Vector2 _relativePosition;
        private readonly Paddle _paddle;
        private readonly int _boostSizeX;
        private readonly int _boostSizeY;
        public Texture2D BoostTexture { get; set; }
        private bool _boostActive;
        public BoundingBox _boundingBox;


        public Boost(Paddle paddle, float xPos, float yPos, int width, int height, int rotationDegrees)
        {
            RotationDegrees = rotationDegrees;
            _relativePosition = new Vector2(xPos, paddle.GetPosition().Y);
            _paddle = paddle;
            _boostSizeX = width;
            _boostSizeY = height;
            _boostActive = false;
        }

        private Vector2 BoostPosition
        {
            get { return _paddle.GetPosition() + _relativePosition; }
        }

        public BoundingBox BoundingBox
        {
            get
            {
                Vector3 topLeft = new Vector3(0,0,0);
                Vector3 botRight = new Vector3(0,0,0);
                if (RotationDegrees == 0)
                {
                    var left = BoostPosition.X - _boostSizeX / 2f;
                    var top = BoostPosition.Y - _boostSizeY / 2f;
                    var right = BoostPosition.X + _boostSizeX / 2f;
                    var bottom = BoostPosition.Y + _boostSizeY / 2f;
                    
                    topLeft = new Vector3(left, top, 0);
                    botRight = new Vector3(right, bottom, 0);
                }
                else if (RotationDegrees == 180)
                {
//                    var rotatedX = (_relativePosition.X * Math.Cos(RotationDegrees) - (_relativePosition.Y * Math.Sin(RotationDegrees)));
//                    topLeft = new Vector3((float)rotatedX - _boostSizeX, _relativePosition.Y, 0);
//                    botRight = new Vector3((float)rotatedX,_relativePosition.Y + _boostSizeY, 0);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                BoostTexture, 
                _paddle.GetPosition(), 
                null, 
                Color.White, 
                RotationDegrees * (float)Math.PI / 180,
                new Vector2(-_relativePosition.X, BoostTexture.Height / 2f), 
                1, 
                SpriteEffects.None, 
                1);
        }
    }
}