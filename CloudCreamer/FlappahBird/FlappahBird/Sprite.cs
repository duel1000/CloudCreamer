using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappahBird
{
    public class Sprite
    {
        private readonly Texture2D texture;
        public float Height 
        {
            get
            {
                return texture.Width/columns;
            }
        }

        public float Width
        {
            get
            {
                return texture.Height / rows;
            }
        }

        public Vector2 position;
        private readonly int rows;
        private readonly int columns;
        private readonly double framesPerSecond;
        private int totalFrames;
        private double timeSinceLastFrame;
        private int currentFrame;
        protected Vector2 Velocity { get; set; }
        protected float RotationAngle { get; set; }
        protected Vector2 Origin { get; set; }

        public Sprite(Texture2D texture, Vector2 position)
            : this(texture, position, 1, 1, 1)
        {
           
        }

        public Sprite(Texture2D texture, Vector2 position, int rows, int columns, double framesPerSecond)
        {
            this.texture = texture;
            this.position = position;
            this.rows = rows;
            this.columns = columns;
            this.framesPerSecond = framesPerSecond;
            totalFrames = rows*columns;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var imageWidth = texture.Width/columns;
            var imageHeight = texture.Height/rows;

            var currentRow = currentFrame/columns;
            var currentColumn = currentFrame%columns;

            var sourceRectangle = new Rectangle(imageWidth*currentColumn, imageHeight*currentRow, imageWidth,
                                                imageHeight);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, imageWidth, imageHeight);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, RotationAngle, Origin, SpriteEffects.None, 1);
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrame > SecondsBetweenFrames())
            {
                currentFrame++;
                timeSinceLastFrame = 0;
            }

            if (currentFrame == totalFrames)
                currentFrame = 0;
        }

        private double SecondsBetweenFrames()
        {
            return 1/framesPerSecond;
        }
    }
}