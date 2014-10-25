using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarioBros3
{
    class Flagpole : SpriteAnimation
    {
        public Flagpole(Vector2 position) : base("flagpole", position, 1,1,1)
        {
            this.BoundingBox = new Rectangle((int)position.X + 20, (int)position.Y, 10, texture.Height);
        }

        public void RunEndingAnimation(Vector2 p0)
        {
            //Start animation somehow
        }
    }
}
