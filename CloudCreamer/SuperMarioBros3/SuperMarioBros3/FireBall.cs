using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    public  class FireBall : SpriteAnimation
    {
        private float _startingPositionX;
        public bool IsDead { get; set; }
        private float timeSinceLastBounce;

        public FireBall(Vector2 position, bool flipSprite) : base("fireball", position,1,1,1)
        {
            velocity.X = flipSprite ? -12f : 12f;
            velocity.Y = 6.5f;
            _startingPositionX = position.X;
            BoundingBox = new Rectangle((int)position.X, (int)position.Y, 20, 20);
        }

        public override void Update(GameTime gameTime)
        {
            if (position.X < _startingPositionX)
                flipSprite = true;

            if (position.X - 800 > _startingPositionX || position.X + 800 < _startingPositionX )
            {
                IsDead = true;
                return;
            }

            position += velocity;

            BoundingBox = new Rectangle((int)position.X, (int)position.Y, 20, 20);

            if (velocity.Y < 6.5f)
                velocity.Y += 1f;

            timeSinceLastBounce += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeSinceLastBounce > 180 && rotationAngle < 1.2f)
            {
                rotationAngle -= 0.01f;
            }

            base.Update(gameTime);
        }

        public void Bounce()
        {
            velocity.Y = -9f;
            rotationAngle = -0.4f;
            timeSinceLastBounce = 0;
        }

        public void Reverse()
        {
            velocity.X = velocity.X * -1;
        }

        public bool Exploded { get; set; }
        public void Explode()
        {
            Exploded = true;
        }

        
    }
}
