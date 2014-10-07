using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3
{
    class Tile
    {
        protected Texture2D texture;

        public Rectangle Rectangle { get; protected set; }

        public static ContentManager Content { protected get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, Color.White);
        }
    }

    class EarthTile : Tile
    {
        public EarthTile(Rectangle newRectangle)
        {
            texture = Content.Load<Texture2D>("singleEarthBlock");
            this.Rectangle = newRectangle;
        }
    }
}
