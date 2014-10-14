using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    class Player : SpriteAnimation
    {
        private bool hasJumped = false;
        private bool walking = false;
        private bool jumpKeyReleased;

        public Player() : base("mario", new Vector2(64,384),1,9,1)
        {

        }

        public void Update(GameTime gameTime)
        {
            position += velocity;

            Input(gameTime);

            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            DestinationRectangle = new Rectangle((int)position.X, (int)position.Y, imageWidth, imageHeight);
            BoundingBox = DestinationRectangle;

            base.Update(gameTime);
        }

        private void Input(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
                flipSprite = false;
                WalkAnimation();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X = -(float) gameTime.ElapsedGameTime.TotalMilliseconds/3;
                flipSprite = true;
                WalkAnimation();
            }

            else
            {
                StopWalkAnimation();
                velocity.X = 0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && hasJumped == false && jumpKeyReleased)
            {
                position.Y -= 5f;
                velocity.Y = -9f;
                hasJumped = true;
                jumpKeyReleased = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
                jumpKeyReleased = true;
        }

        private void StopWalkAnimation()
        {
            walking = false;
            currentFrame = 1;
            framesPerSecond = 0;
            endFrame = 1;
        }

        private void WalkAnimation()
        {
            if (walking == false)
            {
                currentFrame = 1;
                rows = 1;
                columns = 9;
                framesPerSecond = 10;
                endFrame = 9;
                walking = true;
            }
        }

        public void Collision(Tile tile, int xOffset, int yOffset)
        {
            if (BoundingBox.TouchTopOf(tile.BoundingBox))
            {
                DestinationRectangle.Y = tile.BoundingBox.Y - DestinationRectangle.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }

            if (BoundingBox.TouchLeftOf(tile.BoundingBox))
            {
                position.X = tile.BoundingBox.X - DestinationRectangle.Width - 2;
            }
            if (BoundingBox.TouchRightOf(tile.BoundingBox))
            {
                position.X = tile.BoundingBox.X + tile.BoundingBox.Width + 2;
            }
            if (BoundingBox.TouchBottomOf(tile.BoundingBox))
            {
                if(velocity.Y < 0)
                    velocity.Y = 1f;
                var type = tile.GetType();
                if (type.Name == "BrickTile")
                {
                    tile.Status = "Destroy";
                }
            }

            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - DestinationRectangle.Width) position.X = xOffset - DestinationRectangle.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > yOffset - DestinationRectangle.Height) position.Y = yOffset - DestinationRectangle.Height;
        }
        
    }
}
