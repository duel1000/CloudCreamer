using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros2
{
    public class EarthBlock : Sprite
    {
        public EarthBlock(Texture2D texture, Vector2 position) : base(texture, position)
        {

        }

        public EarthBlock(Texture2D texture, Vector2 position, int rows, int columns, double framesPerSecond) : base(texture, position, rows, columns, framesPerSecond)
        {

        }

        public override void Update(GameTime gameTime)
        {
            BoundingBox = new Rectangle((int)position.X, (int)position.Y - 13, 42, 65);
            base.Update(gameTime);
        }
    }
}
