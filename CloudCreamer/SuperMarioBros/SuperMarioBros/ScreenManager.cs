using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros
{
    public class ScreenManager
    {
        private ContentManager content;
        private GameScreen currentScreen;
        private GameScreen newScreen;
        private static ScreenManager _instance;
        Stack<GameScreen> screenStack = new Stack<GameScreen>();
        private Vector2 screenDimensions;
        private bool transition;
        private FadeAnimation fade;

        private Texture2D fadeTexture;

        public static ScreenManager Instance
        {
            get { return _instance ?? (_instance = new ScreenManager()); }
        }

        public Vector2 ScreenDimensions
        {
            get { return screenDimensions; }
            set { screenDimensions = value; }
        }

        public void AddScreen(GameScreen screen)
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.Alpha = 1.0f;
            fade.ActivateValue = 1.0f;    
        }

        public void Initialize()
        {
            currentScreen = new SplashScreen();
            fade = new FadeAnimation();
        }

        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(Content);

            fadeTexture = content.Load<Texture2D>("fade");
            fade.LoadContent(content, fadeTexture,"", Vector2.Zero);
            fade.Scale = ScreenDimensions.X;
        }

        public void Update(GameTime gameTime)
        {
            if(!transition)
                currentScreen.Update(gameTime);
            else
                Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if(transition)
                fade.Draw(spriteBatch);
        }

        private void Transition(GameTime gameTime)
        {
            fade.Update(gameTime);

            if (fade.Alpha == 0.0f && fade.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(newScreen);
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent(content);
            }
            else if (fade.Alpha == 0.0f)
            {
                transition = false;
                fade.IsActive = false;
            }
        }
    }
}
