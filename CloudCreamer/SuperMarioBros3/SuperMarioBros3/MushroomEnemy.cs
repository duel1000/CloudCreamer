using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    public class MushroomEnemy : SpriteAnimation
    {
        private bool walkingLeft = false;
        public bool IsDead { get; set; }
        public bool Spawned { get; set; }
        public bool DeathAnimationPlayed { get; set; }
        public bool SquishSoundPlayed { get; set; }
        public bool IsShot { get; set; }

        public bool IsSquished = false;
        private float squishAnimationTime;

        public MushroomEnemy(Vector2 position) : base("evilmushroom", position, 1,1,1)
        {
            IsDead = false;
            velocity.X = 2f;
            BoundingBox = new Rectangle(DestinationRectangle.X + 5, DestinationRectangle.Y,
                DestinationRectangle.Width - 10, DestinationRectangle.Height + 5);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Spawned) return;

            if (IsDead && !DeathAnimationPlayed && IsSquished)
            {
                RunDeathBySquishAnimation(gameTime);
                return;
            }

            position += velocity;

            if (velocity.Y < 10)
                velocity.Y += 0.45f;

            BoundingBox = new Rectangle(DestinationRectangle.X + 5, DestinationRectangle.Y,
                DestinationRectangle.Width - 10, DestinationRectangle.Height + 5);

            base.Update(gameTime);
        }

        private void RunDeathBySquishAnimation(GameTime gameTime)
        {
            squishAnimationTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            texture = Content_Manager.GetInstance().Textures["squishedMushroom"];
            rows = 1;
            columns = 1;
            framesPerSecond = 0;

            if (squishAnimationTime > 1200)
                DeathAnimationPlayed = true;
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

        public void KillEnemy()
        {
            IsDead = true;
        }

        public void SquishEnemy()
        {
            IsDead = true;
            IsSquished = true;
        }

        public void HitByShot()
        {
            IsShot = true;
            KillEnemy();
        }
    }

    public class Turtle : SpriteAnimation
    {
        private bool walkingLeft = false;
        public bool IsDead { get; set; }
        public bool Spawned { get; set; }

        public Turtle(Vector2 position) : base("turtlewalk", position, 1,2,10)
        {
            IsDead = false;
            velocity.X = 1.8f;
            BoundingBox = new Rectangle(DestinationRectangle.X + 5, DestinationRectangle.Y,
                DestinationRectangle.Width - 10, DestinationRectangle.Height + 5);
            flipSprite = true;
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
                    velocity.X = -1.8f;
                    walkingLeft = true;
                    flipSprite = !flipSprite;
                }
            }
            else if (BoundingBox.TouchRightOf(tile.BoundingBox))
            {
                if (walkingLeft)
                {
                    velocity.X = 1.8f;
                    walkingLeft = false;
                    flipSprite = !flipSprite;
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
                    velocity.X = -1.8f;
                    walkingLeft = true;
                    flipSprite = !flipSprite;
                }
            }
            else if (BoundingBox.TouchRightOf(objectBoundingBox))
            {
                if (walkingLeft)
                {
                    velocity.X = 1.8f;
                    walkingLeft = false;
                    flipSprite = !flipSprite;
                }
            }
        }
    }
}
