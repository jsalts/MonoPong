using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPong.Effects
{
    public class Boost
    {

        public Vector2 _boostPosition;
        public int _boostSizeX;
        public int _boostSizeY;
        public float _boostMultiplier;
        public Texture2D BoostTexture { get; set; }
        public bool _boostActive;


        public Boost(int xPos, int yPos, int xSize, int ySize, float multi)
        {
            _boostPosition = new Vector2(xPos, yPos);
            _boostSizeX = xSize;
            _boostSizeY = ySize;
            _boostMultiplier = multi;
            _boostActive = false;
        }

        public Vector2 GetPosition()
        {
            return _boostPosition;
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

        public void Move(float xSpeed, float ySpeed)
        {
            _boostPosition.X += xSpeed;
            _boostPosition.Y += ySpeed;
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
            spriteBatch.Draw(BoostTexture, GetPosition(), Color.White);
        }
    }
}