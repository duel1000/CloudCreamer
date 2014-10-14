using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    public class Tile
    {
        protected Texture2D texture;

        public Rectangle Rectangle { get; protected set; }
        public Rectangle BoundingBox { get; protected set; }

        public string Status = "Alive";

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, Color.White);
        }
    }

    class EarthTile : Tile
    {
        public EarthTile(Rectangle newRectangle)
        {
            texture = Content_Manager.GetInstance().Textures["singleEarthBlock"];
            this.Rectangle = newRectangle;
            BoundingBox = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height - 10);
        }
    }

    public class BrickTile : Tile
    {
        public BrickTile(Rectangle newRectangle)
        {
            texture = Content_Manager.GetInstance().Textures["brick"];
            this.Rectangle = newRectangle;
            BoundingBox = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height - 10);
        }
    }
}
