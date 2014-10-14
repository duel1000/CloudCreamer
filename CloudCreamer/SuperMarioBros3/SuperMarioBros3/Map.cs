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
        private List<BrickTile> brickTiles = new List<BrickTile>();
        private ExplosionManager explosionManager = new ExplosionManager();
        private SoundManager soundManager;

        public List<EarthTile> EarthTiles
        {
            get { return earthTiles; }
        }
        public List<BrickTile> BrickTiles
        {
            get { return brickTiles; }
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

        public Map(ExplosionManager explosionManager, SoundManager soundManager)
        {
            this.explosionManager = explosionManager;
            this.soundManager = soundManager;
        }

        public void GenerateMap(int size)
        {
            for (int x = 0; x < 500; x++)
            {
                earthTiles.Add(new EarthTile(new Rectangle(x * size, 440, size, size)));

                width = (x + 1)*size;
                height = 480; //Hardcoded
            }

            brickTiles.Add(new BrickTile(new Rectangle(250, 300, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(300, 300, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(350, 300, 50, 50)));

            brickTiles.Add(new BrickTile(new Rectangle(500, 350, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(550, 350, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(600, 300, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(750, 300, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(900, 250, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(950, 200, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(1000, 150, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(1150, 100, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(1500, 350, 50, 50)));
            brickTiles.Add(new BrickTile(new Rectangle(2000, 350, 50, 50)));

            //TEST
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(15);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < brickTiles.Count(); i++)
            {
                if (brickTiles[i].Status == "Destroy")
                {
                    explosionManager.CreateTileExplosion(brickTiles[i]);
                    soundManager.brickExplosionEffect();
                    brickTiles.Remove(brickTiles[i]);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (EarthTile tile in earthTiles)
            {
                tile.Draw(spriteBatch);
            }
            foreach (BrickTile brick in brickTiles)
            {
                brick.Draw(spriteBatch);
            }
        }
    }
}
