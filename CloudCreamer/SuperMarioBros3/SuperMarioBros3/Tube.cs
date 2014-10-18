using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    public class Tube
    {
        protected Texture2D texture;

        public Rectangle Rectangle { get; protected set; }
        public Rectangle BoundingBox { get; protected set; }

        public Tube(Vector2 position)
        {
            texture = Content_Manager.GetInstance().Textures["tube"];
            this.Rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            BoundingBox = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, new Rectangle(0,0,texture.Width,texture.Height), Color.White, 0f, new Vector2(0,0), SpriteEffects.None, 1f);
        }
    }
}
