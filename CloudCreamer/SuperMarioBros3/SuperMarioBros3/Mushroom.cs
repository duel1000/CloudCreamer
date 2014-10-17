using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarioBros3
{
    public class Mushroom : SpriteAnimation
    {
        private bool walkingLeft = false;
        public bool IsDead { get; set; }

        public Mushroom(Vector2 position) : base("evilmushroom", position, 1,1,1)
        {
            IsDead = false;
            velocity.X = 2f;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;

            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            BoundingBox = new Rectangle(DestinationRectangle.X + 5, DestinationRectangle.Y, DestinationRectangle.Width - 10, DestinationRectangle.Height);

            base.Update(gameTime);
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
    }
}
