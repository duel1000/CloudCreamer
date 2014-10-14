using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros4.Managers;

namespace SuperMarioBros4.Screens
{
    class TempScreen : Screen
    {
        public TempScreen() : base()
        {
        
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Content_Manager.GetInstance().Textures["spelunkyMan"], new Vector2(200,200), Color.Tomato );
        }
    }
}
