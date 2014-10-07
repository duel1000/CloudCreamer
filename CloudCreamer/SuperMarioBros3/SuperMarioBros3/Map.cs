using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3
{
    class Map
    { 
        private List<EarthTile> earthTiles = new List<EarthTile>();

        public List<EarthTile> EarthTiles
        {
            get { return earthTiles; }
        }

        private int width, height;

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public Map()
        {
            
        }

        public void GenerateMap(int size)
        {
            for (int x = 0; x < 500; x++)
            {
                earthTiles.Add(new EarthTile(new Rectangle(x * size, 440, size, size)));

                width = (x + 1)*size;
                height = 600;
            }

            //TEST
            earthTiles.RemoveAt(7);
            earthTiles.RemoveAt(7);
            earthTiles.RemoveAt(7);
            earthTiles.RemoveAt(7);
            earthTiles.RemoveAt(7);
            earthTiles.RemoveAt(7);
            earthTiles.RemoveAt(7);
            earthTiles.RemoveAt(7);
            earthTiles.RemoveAt(15);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (EarthTile tile in earthTiles)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
