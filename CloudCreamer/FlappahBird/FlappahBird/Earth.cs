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
        public Earth(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            this.Speed = 2.3f;
        }

        public Earth(Texture2D texture, Vector2 position, int rows, int columns, double framesPerSecond) : base(texture, position, rows, columns, framesPerSecond)
        {
            this.Speed = 2.3f;
        }

        public override void Update(GameTime gameTime)
        {
            position.X -= Speed;

            BoundingBox = new Rectangle((int)position.X, (int)position.Y + 5, 250, 100);

            base.Update(gameTime);
        }
    }
}
