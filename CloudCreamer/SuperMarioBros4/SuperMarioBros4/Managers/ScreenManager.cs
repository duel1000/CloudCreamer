using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros4.Screens;

namespace SuperMarioBros4.Managers
{
    class ScreenManager
    {
        ArrayList _screens;
        Screen _currentScreen;

        public ScreenManager()
        {
            _screens = new ArrayList();
            _screens.Add(new TempScreen());

            _currentScreen = (Screen)_screens[0];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScreen.Draw(spriteBatch);
        }
    }
}
