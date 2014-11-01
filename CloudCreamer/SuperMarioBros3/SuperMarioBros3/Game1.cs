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
        private Score score;
        private Player player;
        private Camera camera;
        private ExplosionManager explosionManager;
        private CoinManager coinManager;
        private SoundManager soundManager;
        private EntityManager entityManager;
        private CollisionManager collisionManager;
        private BackgroundManager backgroundManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Content_Manager.GetInstance().LoadTextures(Content);
            soundManager = new SoundManager(Content);
            explosionManager = new ExplosionManager(soundManager);
            coinManager = new CoinManager(soundManager);
            score = new Score(soundManager);
            entityManager = new EntityManager(score);
            player = new Player(soundManager);
            backgroundManager = new BackgroundManager();
            map = new Map(explosionManager, soundManager, entityManager, coinManager, score, player, backgroundManager);
            soundManager.PlayBackgroundMusic();
            collisionManager = new CollisionManager(explosionManager, soundManager);
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
            if (player.LevelComplete)
            {
                score.EndLevel();
                explosionManager.Update(gameTime);
                score.Update(gameTime);

                if (score.Timer == 0)
                {
                    explosionManager.StartFireworks(map.CastlePosition);
                }

                return;
            }

            player.Update(gameTime);

            if (score.Timer == 0)
                player.TakeDamage();

            collisionManager.PlayerFlagpoleCollision(player, map.Flagpole);
            collisionManager.FireBallEarthCollision(player.FireBalls, map.EarthTiles);
            collisionManager.FireBallBrickCollision(player.FireBalls, map.BrickTiles);
            collisionManager.FireBallQuestionmarkCollision(player.FireBalls, map.QuestionMarkTiles);
            collisionManager.FireBallHardTileCollision(player.FireBalls, map.HardTiles);
            collisionManager.FireBallHardEarthTileCollision(player.FireBalls, map.HardEarthTiles);
            collisionManager.FireBallHiddenTileCollision(player.FireBalls, map.HiddenTiles);
            collisionManager.FireBallTubeCollision(player.FireBalls, map.Tubes);
            collisionManager.FireBallEnemyCollision(player.FireBalls, entityManager.evilMushrooms);

            CheckSpawnPoints();

            foreach (EarthTile tile in map.EarthTiles)
            {
                foreach (var mushroomPowerUp in entityManager.mushroomPowerUps)
                {
                    mushroomPowerUp.TileCollision(tile);
                    player.MushroomPowerUpCollision(mushroomPowerUp);
                }

                foreach (var fireflower in entityManager.fireFlowers)
                {
                    player.FireflowerPowerUpCollision(fireflower);
                }

                player.Collision(tile, map.Width, map.Height);

                foreach (var mushroom in entityManager.evilMushrooms)
                {
                    mushroom.TileCollision(tile);
                }

                foreach (var turtle in entityManager.turtles)
                {
                    turtle.TileCollision(tile);   
                }
               
                camera.Update(player.position, map.Width, map.Height, score);
            }

            foreach (var evilshroom in entityManager.evilMushrooms)
            {
                if(evilshroom.Spawned)
                    player.EvilMushroomCollision(evilshroom);
            }
            foreach (var turtle in entityManager.turtles)
            {
                if (turtle.Spawned)
                    player.TurtleCollision(turtle);
            }

            foreach (CoinBrickTile coinBrick in map.CoinBrickTiles)
            {
                collisionManager.CoinBrickTileMarioCollision(player, coinBrick);
            }

            foreach (BrickTile brick in map.BrickTiles)
            {
                player.Collision(brick, map.Width, map.Height);
                foreach (var mushroom in entityManager.evilMushrooms)
                {
                    mushroom.TileCollision(brick);
                }
                foreach (var turtle in entityManager.turtles)
                {
                    turtle.TileCollision(brick);
                }
                foreach (var mushroomPowerUp in entityManager.mushroomPowerUps)
                {
                    mushroomPowerUp.TileCollision(brick);
                }
            }
            foreach (var hardBrick in map.HardTiles)
            {
                player.Collision(hardBrick, map.Width, map.Height);

                foreach (var mushroomPowerUp in entityManager.mushroomPowerUps)
                {
                    mushroomPowerUp.TileCollision(hardBrick);
                }
                foreach (var turtle in entityManager.turtles)
                {
                    turtle.TileCollision(hardBrick);
                }
            }
            foreach (var questionMarkTile in map.QuestionMarkTiles)
            {
                foreach (var mushroomPowerUp in entityManager.mushroomPowerUps)
                {
                    mushroomPowerUp.TileCollision(questionMarkTile);
                }
                foreach (var turtle in entityManager.turtles)
                {
                    turtle.TileCollision(questionMarkTile);
                }
                player.Collision(questionMarkTile, map.Width, map.Height);
            }
            foreach (var tube in map.Tubes)
            {
                foreach (var evilMushroom in entityManager.evilMushrooms)
                {
                    evilMushroom.SimpleCollision(tube.BoundingBox);
                }
                foreach (var turtle in entityManager.turtles)
                {
                    turtle.SimpleCollision(tube.BoundingBox);
                }
                player.SimpelCollision(tube.BoundingBox);
            }
            foreach (var hardEarthTile in map.HardEarthTiles)
            {
                player.SimpelCollision(hardEarthTile.BoundingBox);
            }
            foreach (var hiddenTile in map.HiddenTiles)
            {
                if(!hiddenTile.IsPunched)
                    player.HiddenTileCollision(hiddenTile);
                else
                    player.Collision(hiddenTile, map.Width, map.Height);

                foreach (var powerUp in entityManager.mushroomPowerUps)
                {
                    powerUp.TileCollision(hiddenTile);
                }
            }

            //Check collision for all evil mushrooms
            foreach (var enemy in entityManager.evilMushrooms)
            {
                var newList = new List<MushroomEnemy>(entityManager.evilMushrooms); //clones the list by value
                newList.Remove(enemy);

                foreach (var anotherEnemy in newList)
                {
                    anotherEnemy.SimpleCollision(enemy.BoundingBox);
                }
            }

            explosionManager.Update(gameTime);
            score.Update(gameTime);
            map.Update(gameTime);

            if (player.RespawnPlayer)
            {
                map.ReGenerate();
                player.Respawn();
                score.SetScoreStartingPosition();
            }

            base.Update(gameTime);
        }

        private void CheckSpawnPoints()
        {
            foreach (var evilMushroom in entityManager.evilMushrooms)
            {
                evilMushroom.CheckIfSpawnPointReached(player.position.X);
            }
            foreach (var turtle in entityManager.turtles)
            {
                turtle.CheckIfSpawnPointReached(player.position.X);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(183,165,255));

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
