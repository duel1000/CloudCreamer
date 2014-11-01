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

        public FireBall(Vector2 position, bool flipSprite) : base("fireball", position,1,1,1)
        {
            velocity.X = flipSprite ? -12f : 12f;
            velocity.Y = 6.5f;
            _startingPositionX = position.X;
            BoundingBox = new Rectangle((int)position.X, (int)position.Y, 20, 20);
        }

        public override void Update(GameTime gameTime)
        {
            if (position.X - 800 > _startingPositionX || position.X + 800 < _startingPositionX )
            {
                IsDead = true;
                return;
            }

            position += velocity;

            BoundingBox = new Rectangle((int)position.X, (int)position.Y, 20, 20);

            if (velocity.Y < 6.5f)
                velocity.Y += 1f;

            base.Update(gameTime);
        }

        public void Bounce()
        {
            velocity.Y = -9f;
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
