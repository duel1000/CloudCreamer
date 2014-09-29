using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappahBird
{
    public class Sprite
    {
        private readonly Texture2D texture;
        
        protected Vector2 position;

        protected Vector2 Velocity { get; set; }

        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}