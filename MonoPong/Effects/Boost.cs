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

        private Texture2D _boostTexture;


        public Boost(int xPos, int yPos, int xSize, int ySize)
        {
            _boostPosition = new Vector2(xPos, yPos);
            _boostSizeX = xSize;
            _boostSizeY = ySize;
        }
    }
}