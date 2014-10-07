using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros2
{
    public class Sprite
    {
        protected Texture2D texture;

        private int imageWidth { get; set; }
        private int imageHeight { get; set; }
        public Vector2 position;
        private int rows;
        private int columns;
        protected double framesPerSecond;
        protected int totalFrames;
        private double timeSinceLastFrame;
        protected int currentFrame;
        protected Vector2 velocity;
        protected float RotationAngle { get; set; }
        protected Vector2 Origin { get; set; }
        public Rectangle BoundingBox { get; set; }
        public float Speed { get; set; }
        public bool AnimationOn = true;
        protected bool MovingLeft = false;
        public bool inAir = false;

        public Sprite(Texture2D texture, Vector2 position)
            : this(texture, position, 1, 1, 1)
        {
           
        }

        public Sprite(Texture2D texture, Vector2 position, int rows, int columns, double framesPerSecond)
        {
            velocity = new Vector2(0,0);
            this.texture = texture;
            this.position = position;
            this.rows = rows;
            this.columns = columns;
            this.framesPerSecond = framesPerSecond;
            this.imageWidth = texture.Width / columns;
            this.imageHeight = texture.Height / rows;
            totalFrames = rows*columns;
        }

        public float VelocityY
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var currentRow = currentFrame / columns;
            var currentColumn = currentFrame % columns;

            var sourceRectangle = new Rectangle(imageWidth * currentColumn, imageHeight * currentRow, imageWidth,
                                                imageHeight);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, imageWidth, imageHeight);

            var spriteDirection = MovingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, RotationAngle, Origin, spriteDirection, 1);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (AnimationOn)
            {
                UpdateAnimation(gameTime);
            }
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