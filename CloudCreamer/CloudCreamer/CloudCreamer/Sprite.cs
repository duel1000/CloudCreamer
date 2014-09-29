using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CloudCreamer
{
    public class Sprite
    {
        private readonly Texture2D texture;
        private Vector2 position;
        private readonly Rectangle movementBounds;
        

        protected float Speed { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        protected Vector2 Velocity { get; set; }

        public Sprite(Texture2D texture, Vector2 position, Rectangle movementBounds)
        {
            this.texture = texture;
            this.position = position;
            this.movementBounds = movementBounds;
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public virtual void Update(KeyboardState keyboardState, GameTime gameTime)
        {
            var newPosition = position + (Velocity* (float) gameTime.ElapsedGameTime.TotalSeconds);

            if (Blocked(newPosition))
                return;

            position = newPosition;
        }

        private bool Blocked(Vector2 newPosition)
        {
            var boundingBox = CreateBoundingBoxFromPosition(newPosition);

            return !movementBounds.Contains(boundingBox);
        }

        private Rectangle CreateBoundingBoxFromPosition(Vector2 position)
        {
            return new Rectangle((int) position.X, (int) position.Y, (int) Width, (int) Height);
        }
    }
}
