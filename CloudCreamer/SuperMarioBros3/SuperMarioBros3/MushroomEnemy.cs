using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarioBros3
{
    public class MushroomEnemy : SpriteAnimation
    {
        private bool walkingLeft = false;
        public bool IsDead { get; set; }
        public bool Spawned { get; set; }

        public MushroomEnemy(Vector2 position) : base("evilmushroom", position, 1,1,1)
        {
            IsDead = false;
            velocity.X = 2f;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Spawned) return;
            
            position += velocity;

            if (velocity.Y < 10)
                velocity.Y += 0.45f;

            BoundingBox = new Rectangle(DestinationRectangle.X + 5, DestinationRectangle.Y,
                DestinationRectangle.Width - 10, DestinationRectangle.Height + 5);

            base.Update(gameTime);
        }

        public void CheckIfSpawnPointReached(float x)
        {
            if (this.position.X - 1000 < x)
                Spawned = true;
        }

        public void TileCollision(Tile tile)
        {
                if (BoundingBox.TouchTopOf(tile.BoundingBox))
                {
                    position.Y = tile.BoundingBox.Y - DestinationRectangle.Height;
                    velocity.Y = 0f;
                }
                else if (BoundingBox.TouchLeftOf(tile.BoundingBox))
                {
                    if (!walkingLeft)
                    {
                        velocity.X -= 4;
                        walkingLeft = true;
                        flipSprite = true;
                    }
                }
                else if (BoundingBox.TouchRightOf(tile.BoundingBox))
                {
                    if (walkingLeft)
                    {
                        velocity.X += 4;
                        walkingLeft = false;
                        flipSprite = false;
                    }
                }
        }

        public void SimpleCollision(Rectangle objectBoundingBox)
        {
            if (BoundingBox.TouchTopOf(objectBoundingBox))
            {
                position.Y = objectBoundingBox.Y - DestinationRectangle.Height;
                velocity.Y = 0f;
            }
            else if (BoundingBox.TouchLeftOf(objectBoundingBox))
            {
                if (!walkingLeft)
                {
                    velocity.X -= 4;
                    walkingLeft = true;
                    flipSprite = true;
                }
            }
            else if (BoundingBox.TouchRightOf(objectBoundingBox))
            {
                if (walkingLeft)
                {
                    velocity.X += 4;
                    walkingLeft = false;
                    flipSprite = false;
                }
            }
        }
    }
}
