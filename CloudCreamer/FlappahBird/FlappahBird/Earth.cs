using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappahBird
{
    public class Earth : Sprite
    {
        private float Speed;

        public Earth(Texture2D texture, Vector2 position) : base(texture, position)
        {

        }

        public Earth(Texture2D texture, Vector2 position, int rows, int columns, double framesPerSecond) : base(texture, position, rows, columns, framesPerSecond)
        {
            this.Speed = 30;
        }

        public override void Update(GameTime gameTime)
        {
            Velocity = new Vector2(Velocity.X + Speed, Velocity.Y);
            position.X -= 2.3f;

            base.Update(gameTime);
        }
    }
}
