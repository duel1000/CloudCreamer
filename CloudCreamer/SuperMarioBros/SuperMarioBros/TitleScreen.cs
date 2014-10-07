using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioBros
{
    public class TitleScreen : GameScreen
    {
        private KeyboardState keyState;
        private SpriteFont font;

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (font == null)
                font = content.Load<SpriteFont>("Font1");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Enter))
                ScreenManager.Instance.AddScreen(new SplashScreen());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "TitleScreen", new Vector2(100, 100), Color.Black);
        }
    }
}
