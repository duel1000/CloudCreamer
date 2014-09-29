using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CloudCreamer
{
    public class Enemy : Sprite
    {
        public Enemy(Texture2D texture, Vector2 position, Rectangle bounds) : base(texture, position, bounds)
        {
            Speed = 200;
        }

        public override void Update(GameTime gameTime)
        {
            var random = new Random();
            if (Velocity == Vector2.Zero)
            {
                var direction = random.Next(2);
                Velocity = new Vector2(direction == 0 ? -1 : 1, direction == 0 ? 1 : -1);
            }
            else if (gameTime.TotalGameTime.Seconds%2 == 0)
            {
                if(random.Next(2) == 0)
                    Velocity = new Vector2(-Velocity.X, -Velocity.Y);
            }

            base.Update(gameTime);
        } 
    }
}