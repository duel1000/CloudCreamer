using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3.Managers
{
    public class BackgroundManager
    {
        private List<BackgroundElement> backgroundElements = new List<BackgroundElement>(); 

        public BackgroundManager()
        {
            
        }

        public void AddBackgroundElement(BackgroundElement backgroundElement)
        {
            this.backgroundElements.Add(backgroundElement);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var backgroundElement in backgroundElements)
            {
                backgroundElement.Draw(spriteBatch);
            }
        }
    }

    public class BackgroundElement
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle destinationRectangle;

        public BackgroundElement(Vector2 position, String filename)
        {
            this.position = position;
            this.texture = Content_Manager.GetInstance().Textures[filename];
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,destinationRectangle, Color.White);
        }
    }
}
