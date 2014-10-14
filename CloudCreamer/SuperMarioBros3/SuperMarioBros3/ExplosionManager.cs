using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3
{
    public class ExplosionManager
    {
        private List<BrickExplosion> brickExplosions = new List<BrickExplosion>();
        private bool shiftDirection = false;

        public ExplosionManager()
        {
             
        }

        public void Update(GameTime gameTime)
        {
            foreach (var explosion in brickExplosions)
            {
                if (shiftDirection)
                {
                    explosion.position.X += 0.5f;
                    shiftDirection = !shiftDirection;
                }
                    
                else
                {
                    explosion.position.X -= 0.5f;
                    shiftDirection = !shiftDirection;
                }

                explosion.Update(gameTime);
                if (explosion.IsDone())
                    brickExplosions.Remove(explosion);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var explosion in brickExplosions)
            {
                explosion.Draw(spriteBatch);
            }
        }

        public void CreateTileExplosion(BrickTile brickTile)
        {
            var topLeft = new Vector2(brickTile.Rectangle.X + 15, brickTile.Rectangle.Y);
            var topRight = new Vector2(brickTile.Rectangle.X - 15 + brickTile.Rectangle.Width, brickTile.Rectangle.Y);
            var bottomLeft = new Vector2(brickTile.Rectangle.X + 15, brickTile.Rectangle.Y + brickTile.Rectangle.Height - 20);
            var bottomRight = new Vector2(brickTile.Rectangle.X - 15 + brickTile.Rectangle.Width, brickTile.Rectangle.Y + brickTile.Rectangle.Height - 20);
            brickExplosions.Add(new BrickExplosion(topLeft));
            brickExplosions.Add(new BrickExplosion(topRight));
            brickExplosions.Add(new BrickExplosion(bottomLeft));
            brickExplosions.Add(new BrickExplosion(bottomRight));
        }
    }

    public class BrickExplosion : SpriteAnimation
    {
        private Vector2 velocity;

        public bool IsDone()
        {
            return animationPlayedOnce;
        }

        public BrickExplosion(Vector2 position) : base("destroyedbrick", position,1,1,1)
        {
            position.Y -= 5f;
            velocity.Y = -9f;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            if (velocity.Y < 10)
            {
                velocity.Y += 0.4f;
            }
            rotationAngle += 0.01f;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}