using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    public class SpriteAnimation
    {
        protected Texture2D texture;
        protected int rows;
        protected int columns;
        protected int currentFrame;
        protected int startFrame;
        protected int endFrame;
        protected int imageWidth;
        protected int imageHeight;
        protected double framesPerSecond;
        protected double timeSinceLastFrame;
        protected float rotationAngle;
        protected Vector2 origin;
        protected bool animationPlayedOnce = false;
        protected bool flipSprite;
        
        public Vector2 position;
        public Rectangle BoundingBox;
        protected Rectangle DestinationRectangle;
        protected Rectangle SourceRectangle;
        protected Vector2 velocity;

        public SpriteAnimation()
        {
            this.imageWidth = texture.Width / columns;
            this.imageHeight = texture.Height / rows;
            endFrame = rows * columns;
            startFrame = 0;
        }

        public SpriteAnimation(String fileName, Vector2 position, int rows, int columns, double framesPerSecond)
        {
            texture = Content_Manager.GetInstance().Textures[fileName];
            this.position = position;
            this.rows = rows;
            this.columns = columns;
            this.framesPerSecond = framesPerSecond;
            this.imageWidth = texture.Width / columns;
            this.imageHeight = texture.Height / rows;
            endFrame = rows * columns;
            startFrame = 0;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var currentRow = currentFrame / columns;
            var currentColumn = currentFrame % columns;

            imageWidth = texture.Width / columns;
            imageHeight = texture.Height / rows;

            SourceRectangle = new Rectangle(imageWidth * currentColumn, imageHeight * currentRow, imageWidth,
                                                imageHeight);

            DestinationRectangle = new Rectangle((int)position.X, (int)position.Y, imageWidth, imageHeight);

            var spriteDirection = flipSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(texture, 
                             DestinationRectangle, 
                             SourceRectangle, 
                             Color.White, 
                             rotationAngle, 
                             origin, 
                             spriteDirection, 
                             1);
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

            if (currentFrame == endFrame)
            {
                currentFrame = startFrame;
                animationPlayedOnce = true;
            }
                
        }

        private double SecondsBetweenFrames()
        {
            return 1 / framesPerSecond;
        }

    }
}