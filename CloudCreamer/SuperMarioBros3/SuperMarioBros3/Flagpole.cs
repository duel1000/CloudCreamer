using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    class Flagpole : SpriteAnimation
    {
        private Texture2D flag;
        private float flagYPosition;
        private Rectangle flagRectangle;
        private bool runFlagEnding;
        private float flagTravelDistance;

        public Flagpole(Vector2 position) : base("flagpole", position, 1,1,1)
        {
            this.BoundingBox = new Rectangle((int)position.X + 20, (int)position.Y, 10, texture.Height);
            flag = Content_Manager.GetInstance().Textures["flagpoleflag"];
            flagYPosition = position.Y + 60;
            flagRectangle = new Rectangle((int)position.X - 66, (int)flagYPosition, 80,80);//Hardcoded

        }

        public override void Update(GameTime gameTime)
        {
            if (runFlagEnding && flagYPosition < flagTravelDistance && flagYPosition < 300)
            {
                flagYPosition += 2.4f;
                flagRectangle = new Rectangle(flagRectangle.X, (int)flagYPosition, 80,80);
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(flag, flagRectangle, Color.White);
            base.Draw(spriteBatch);
        }

        public void RunFlagEndingAnimation(float yPosition)
        {
            runFlagEnding = true;
            flagTravelDistance = 550 - yPosition;
        }
    }
}
