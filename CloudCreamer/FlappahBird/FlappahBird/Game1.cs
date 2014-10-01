using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FlappahBird
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Sprite background;
        private PlayerBird flappy;
        private TubeManager tubeManager;
        private EarthManager earthManager;
        private SoundManager soundManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 450;
            graphics.PreferredBackBufferWidth = 250;
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
            
            background = new Sprite(Content.Load<Texture2D>("fBackground"), new Vector2(0,-45));
            soundManager = new SoundManager(Content);
            earthManager = new EarthManager(Content.Load<Texture2D>("earth"));

            var upperTube = Content.Load<Texture2D>("upperTube");
            var lowerTube = Content.Load<Texture2D>("lowerTube");
            tubeManager = new TubeManager(upperTube,lowerTube);

            var flappyTexture = Content.Load<Texture2D>("bird");
            var flappyXPosition = graphics.GraphicsDevice.Viewport.Width/5;
            var flappyYPosition = graphics.GraphicsDevice.Viewport.Height / 8;
            flappy = new PlayerBird(flappyTexture, new Vector2((int)flappyXPosition, (int)flappyYPosition), soundManager);

            soundManager.PlayBackgroundMusic();
            // use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            flappy.Update(gameTime);
            earthManager.Update(gameTime);
            tubeManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            background.Draw(spriteBatch);
            earthManager.Draw(spriteBatch);
            tubeManager.Draw(spriteBatch);
            flappy.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
