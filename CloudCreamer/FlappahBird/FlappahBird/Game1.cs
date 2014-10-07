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
        private Sprite startScreen;
        private Sprite gameOverScreen;
        private Sprite getReadySign;
        private Sprite background;
        private Sprite whiteSmack;
        private PlayerBird playerBird;
        private TubeManager tubeManager;
        private EarthManager earthManager;
        private SoundManager soundManager;
        private CollisionManager collisionManager;
        private PointManager pointManager;
        private SpriteFont pointFont;
        private float elapsedGameTime;

        private GameState gameState;

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
            gameState = new StartScreenState(this);
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
            startScreen = new Sprite(Content.Load<Texture2D>("pressSpace"), new Vector2(80, 95));
            gameOverScreen = new Sprite(Content.Load<Texture2D>("gameOver"), new Vector2(26,102));
            background = new Sprite(Content.Load<Texture2D>("fBackground"), new Vector2(0,-45));
            getReadySign = new Sprite(Content.Load<Texture2D>("getReady"), new Vector2(26, 102));
            whiteSmack = new Sprite(Content.Load<Texture2D>("whiteSmack"), new Vector2(0,0));
            soundManager = new SoundManager(Content);
            earthManager = new EarthManager(Content.Load<Texture2D>("earth"));

            var upperTube = Content.Load<Texture2D>("upperTube");
            var lowerTube = Content.Load<Texture2D>("lowerTube");
            tubeManager = new TubeManager(upperTube,lowerTube);

            var flappyTexture = Content.Load<Texture2D>("bird");
            var flappyXPosition = graphics.GraphicsDevice.Viewport.Width/5;
            var flappyYPosition = -25;
            playerBird = new PlayerBird(flappyTexture, new Vector2((int)flappyXPosition, (int)flappyYPosition), soundManager);
            pointFont = Content.Load<SpriteFont>("PointsFont");
            pointManager = new PointManager(soundManager, pointFont);
            collisionManager = new CollisionManager(playerBird, earthManager, tubeManager, pointManager);

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
            gameState.Update(gameTime);
            
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
            tubeManager.Draw(spriteBatch);
            earthManager.Draw(spriteBatch);
            pointManager.Draw(spriteBatch);

            gameState.Draw(spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        public class StartScreenState : GameState
        {
            public StartScreenState(Game1 game) : base(game)
            {
                game.earthManager = new EarthManager(game.Content.Load<Texture2D>("earth"));
                game.soundManager = new SoundManager(game.Content);
                var upperTube = game.Content.Load<Texture2D>("upperTube");
                var lowerTube = game.Content.Load<Texture2D>("lowerTube");
                game.tubeManager = new TubeManager(upperTube, lowerTube);
                var flappyTexture = game.Content.Load<Texture2D>("bird");
                var flappyXPosition = game.graphics.GraphicsDevice.Viewport.Width / 5;
                var flappyYPosition = -25;
                game.playerBird = new PlayerBird(flappyTexture, new Vector2((int)flappyXPosition, (int)flappyYPosition), game.soundManager);
                game.pointManager = new PointManager(game.soundManager, game.pointFont);
                game.collisionManager = new CollisionManager(game.playerBird, game.earthManager, game.tubeManager, game.pointManager);

                game.pointManager.ResetPoints();
            }

            public override void Update(GameTime gameTime)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    game.gameState = new PlayingState(game);
                    game.playerBird.Update(gameTime);
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                game.startScreen.Draw(spriteBatch);
                game.playerBird.Draw(spriteBatch);
            }
        }

        public class PlayingState : GameState
        {
            public PlayingState(Game1 game)
                : base(game)
            {

            }

            public override void Update(GameTime gameTime)
            {
                game.playerBird.Update(gameTime);
                game.earthManager.Update(gameTime);
                game.tubeManager.Update(gameTime);
                game.collisionManager.Update(gameTime);

                game.elapsedGameTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (game.playerBird.IsDead)
                {
                    game.elapsedGameTime = 0;
                    game.gameState = new GameOverState(game);
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                if (!game.playerBird.IsDead)
                {
                    if (game.elapsedGameTime > 1000 && game.elapsedGameTime < 2200)
                        game.getReadySign.Draw(spriteBatch);

                    game.playerBird.Draw(spriteBatch);
                }
            }
        }

        public class GameOverState : GameState
        {
            public GameOverState(Game1 game) : base(game)
            {
                
            }

            public override void Update(GameTime gameTime)
            {
                game.playerBird.AnimationOn = false;
                game.playerBird.Update(gameTime);
                game.earthManager.Update(gameTime);
                game.tubeManager.Update(gameTime);
                game.elapsedGameTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (Keyboard.GetState().IsKeyDown(Keys.X))
                {
                    game.gameState = new StartScreenState(game);
                    game.elapsedGameTime = 0;
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                if (game.elapsedGameTime < 2)
                {
                    game.whiteSmack.Draw(spriteBatch);
                }
                game.playerBird.Draw(spriteBatch);
                game.gameOverScreen.Draw(spriteBatch);

                var scoreX = 60;
                var scoreY = 390;
                var position = new Vector2(scoreX, scoreY);
                spriteBatch.DrawString(game.pointFont, "PRESS X...", position, Color.White);
            }
        }
    }

    
}

// Highscore vises?