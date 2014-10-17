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
                    explosion.position.X += 2.5f;
                    shiftDirection = !shiftDirection;
                }
                    
                else
                {
                    explosion.position.X -= 2.5f;
                    shiftDirection = !shiftDirection;
                }

                explosion.Update(gameTime);
                if (explosion.IsDone())
                    brickExplosions.Remove(explosion);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < brickExplosions.Count; i++)
            {
                brickExplosions[i].Draw(spriteBatch);

                if (brickExplosions[i].position.Y > 3000)
                {
                    brickExplosions.Remove(brickExplosions[i]);
                    i--;
                }
            }
        }

        public void CreateTileExplosion(BrickTile brickTile)
        {
            var topLeft = new Vector2(brickTile.Rectangle.X + 10, brickTile.Rectangle.Y - 10);
            var topRight = new Vector2(brickTile.Rectangle.X - 25 + brickTile.Rectangle.Width, brickTile.Rectangle.Y - 10);
            var bottomLeft = new Vector2(brickTile.Rectangle.X + 10, brickTile.Rectangle.Y + brickTile.Rectangle.Height - 30);
            var bottomRight = new Vector2(brickTile.Rectangle.X - 25 + brickTile.Rectangle.Width, brickTile.Rectangle.Y + brickTile.Rectangle.Height - 30);
            brickExplosions.Add(new BrickExplosion(topLeft));
            brickExplosions.Add(new BrickExplosion(topRight));
            brickExplosions.Add(new BrickExplosion(bottomLeft));
            brickExplosions.Add(new BrickExplosion(bottomRight));
        }
    }

    public class BrickExplosion : SpriteAnimation
    {
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
    }
}