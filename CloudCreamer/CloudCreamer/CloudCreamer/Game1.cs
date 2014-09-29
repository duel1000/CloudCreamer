using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CloudCreamer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Sprite background;
        private PlayerCloud playerCloud;
        private SpriteFont gameFont;

        private int score = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = new Sprite(Content.Load<Texture2D>("background"), Vector2.Zero, graphics.GraphicsDevice.Viewport.Bounds);

            var cloudTexture = Content.Load<Texture2D>("singleCloud");

            //Position the cloud at the center of the screen to begin with
            var xPositionOfCloud = graphics.GraphicsDevice.Viewport.Width / 2 - (cloudTexture.Width/8);
            var yPositionOfCloud = (graphics.GraphicsDevice.Viewport.Height / 2) - (cloudTexture.Height/8);

            var playerBounds = new Rectangle(0, -30, graphics.GraphicsDevice.Viewport.Width - 125, graphics.GraphicsDevice.Viewport.Height - 95);
            playerCloud = new PlayerCloud(cloudTexture, new Vector2(xPositionOfCloud,yPositionOfCloud), playerBounds);

            gameFont = Content.Load<SpriteFont>("GameFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            playerCloud.Update(keyboardState, gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            background.Draw(spriteBatch);
            playerCloud.Draw(spriteBatch);

            var scoreText = string.Format("Score: {0}", score);
            var scoreDimensions = gameFont.MeasureString(scoreText); 

            var scoreX = 0 + 10;
            var scoreY = 0 + 5;

            spriteBatch.DrawString(gameFont, scoreText, new Vector2(scoreX, scoreY), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
