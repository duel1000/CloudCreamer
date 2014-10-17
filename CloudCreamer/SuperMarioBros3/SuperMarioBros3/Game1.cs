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
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Map map;
        private Player player;
        private Camera camera;
        private ExplosionManager explosionManager;
        private SoundManager soundManager;
        private EntityManager entityManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Content_Manager.GetInstance().LoadTextures(Content);
            explosionManager = new ExplosionManager();
            soundManager = new SoundManager(Content);
            entityManager = new EntityManager();
            map = new Map(explosionManager, soundManager, entityManager);
            player = new Player(soundManager);
            soundManager.PlayBackgroundMusic();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            camera = new Camera(GraphicsDevice.Viewport);

            map.GenerateMap(45);
        }

        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            foreach (EarthTile tile in map.EarthTiles)
            {
                player.Collision(tile, map.Width, map.Height);

                foreach (var mushroom in entityManager.evilMushrooms)
                {
                    mushroom.TileCollision(tile);
                    player.EvilMushroomCollision(mushroom);
                }
                camera.Update(player.position, map.Width, map.Height);
            }
            foreach (BrickTile brick in map.BrickTiles)
            {
                player.Collision(brick, map.Width, map.Height);
                foreach (var mushroom in entityManager.evilMushrooms)
                {
                    mushroom.TileCollision(brick);
                }
            }
            foreach (var hardBrick in map.HardTiles)
            {
                player.Collision(hardBrick, map.Width, map.Height);
            }

            explosionManager.Update(gameTime);

            map.Update(gameTime);
                
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend,
                              null,null,null,null,
                              camera.Transform);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            explosionManager.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
