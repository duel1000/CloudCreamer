using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SuperMarioBros2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private EarthManager earthManager;
        private Mario mario;
        private Camera camera;
        private CollisionManager collisionManager;
        private SoundManager soundManager;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this) {PreferredBackBufferHeight = 600, PreferredBackBufferWidth = 800};
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
            camera = new Camera(GraphicsDevice.Viewport);
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
            soundManager = new SoundManager(Content);
            mario = new Mario(Content.Load<Texture2D>("mario"), new Vector2(100,400), soundManager);
            earthManager = new EarthManager(Content.Load<Texture2D>("singleEarthBlock"));
            collisionManager = new CollisionManager(mario, earthManager);
            
            soundManager.PlayBackgroundMusic();
            // TODO: use this.Content to load your game content here
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
            mario.Update(gameTime);
            // TODO: Add your update logic here
            earthManager.Update(gameTime); // De står stille? HEST
            collisionManager.Update(gameTime);
            camera.Update(gameTime, mario);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(183,165,255));

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
            earthManager.Draw(spriteBatch);
            mario.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

//BUGS:
// Hvorfor lander Mario nede i jorden?

//GENERAL:
// Få kameraet til kun at følge mario i kanterne af skærmen
// Lav jordstykker som i videoen
// Implementer et "hoppe" spritesheet
// Implementer en funktion i Mario der giver ham powerUp
// Implementer funktion HIT() i mario som gør at han dør hvis ikke han er poweredUp
// Mario skal dø når han ryger ud af skærmen

//IDEAS
//Levelmanager
