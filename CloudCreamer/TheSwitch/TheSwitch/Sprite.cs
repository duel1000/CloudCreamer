using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheSwitch
{
    public class Sprite
    {
        private Texture2D texture;
        private readonly Vector2 position;
        private readonly int rows;
        private readonly int columns;
        private readonly int framesPerSecond;

        public Sprite(Texture2D texture, Vector2 position) : this(texture,position,1,1,1)
        {
            this.texture = texture;
            this.position = position;
        }

        public Sprite(Texture2D texture, Vector2 position, int rows, int columns, int framesPerSecond)
        {
            this.texture = texture;
            this.position = position;
            this.rows = rows;
            this.columns = columns;
            this.framesPerSecond = framesPerSecond;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}