using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3
{
    class Map
    {
        private List<EarthTile> earthTiles = new List<EarthTile>();
        private List<BrickTile> brickTiles = new List<BrickTile>();
        private List<HardTile> hardTiles = new List<HardTile>();
        private List<QuestionMarkTile> questionMarkTiles = new List<QuestionMarkTile>();
        private List<Tube> tubes = new List<Tube>();
        private ExplosionManager explosionManager;
        private CoinManager coinManager;

        private SoundManager soundManager;
        private EntityManager entityManager;

        private Score score;

        public List<EarthTile> EarthTiles
        {
            get { return earthTiles; }
        }
        public List<BrickTile> BrickTiles
        {
            get { return brickTiles; }
        }
        public List<Tube> Tubes{get { return tubes; }} 
        public List<HardTile> HardTiles
        {
            get { return hardTiles; }
        }

        public List<QuestionMarkTile> QuestionMarkTiles
        {
            get { return questionMarkTiles; }
        } 

        private int width, height;

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public Map(ExplosionManager explosionManager, SoundManager soundManager, EntityManager entityManager, CoinManager coinManager, Score score)
        {
            this.explosionManager = explosionManager;
            this.soundManager = soundManager;
            this.entityManager = entityManager;
            this.coinManager = coinManager;
            this.score = score;
        }

        public void GenerateMap(int size)
        {
            for (int x = 0; x < 500; x++)
            {
                earthTiles.Add(new EarthTile(new Rectangle(x * size, 440, size, size)));

                width = (x + 1)*size;
                height = 480; //Hardcoded
            }

            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(350, 250, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(450, 250, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(550, 250, 50, 50), true, "", true));

            //
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(650, 250, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(750, 250, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(850, 250, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(950, 250, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(1050, 250, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(1100, 250, 50, 50), true, "", true));
            //

            earthTiles.RemoveAt(95);
            earthTiles.RemoveAt(95);

            earthTiles.RemoveAt(109);
            earthTiles.RemoveAt(109);

            brickTiles.Add(new BrickTile(new Vector2(1300, 250)));
            brickTiles.Add(new BrickTile(new Vector2(1400, 250)));
            brickTiles.Add(new BrickTile(new Vector2(1500, 250)));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(1350, 250, 50, 50), true,"",true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(1450, 250, 50, 50), true));

            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(1400, 50, 50, 50), true));

            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(800, 350)));
            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(800, 350)));

            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(1000,250,50,50), true, "PowerUp"));

            //Tube
            tubes.Add(new Tube(new Vector2(1700, 350)));
            tubes.Add(new Tube(new Vector2(2700, 300)));
            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(3000, 350)));
            tubes.Add(new Tube(new Vector2(3100, 240)));
            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(3200, 350)));
            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(3500, 350)));
            tubes.Add(new Tube(new Vector2(3600, 240)));


            brickTiles.Add(new BrickTile(new Vector2(4800, 250)));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(4850, 250, 50, 50), true));
            brickTiles.Add(new BrickTile(new Vector2(4900, 250)));
            brickTiles.Add(new BrickTile(new Vector2(4950, 50)));
            brickTiles.Add(new BrickTile(new Vector2(5000, 50)));
            brickTiles.Add(new BrickTile(new Vector2(5050, 50)));
            brickTiles.Add(new BrickTile(new Vector2(5100, 50)));
            brickTiles.Add(new BrickTile(new Vector2(5150, 50)));
            brickTiles.Add(new BrickTile(new Vector2(5200, 50)));
            brickTiles.Add(new BrickTile(new Vector2(5250, 50)));
            brickTiles.Add(new BrickTile(new Vector2(5300, 50)));
            brickTiles.Add(new BrickTile(new Vector2(5350, 50)));

            //Lav sections?


            //Hardbrick
            //brickTiles.Add(new BrickTile(new Rectangle(1100, 250, 50, 50), true));


            //brickTiles.Add(new BrickTile(new Rectangle(500, 350, 50, 50)));
            //brickTiles.Add(new BrickTile(new Rectangle(550, 350, 50, 50)));
            //brickTiles.Add(new BrickTile(new Rectangle(600, 300, 50, 50)));
            //brickTiles.Add(new BrickTile(new Rectangle(750, 300, 50, 50)));
            //brickTiles.Add(new BrickTile(new Rectangle(900, 250, 50, 50)));
            //brickTiles.Add(new BrickTile(new Rectangle(950, 200, 50, 50)));
            //brickTiles.Add(new BrickTile(new Rectangle(1000, 150, 50, 50)));
            //brickTiles.Add(new BrickTile(new Rectangle(1150, 100, 50, 50)));
            //brickTiles.Add(new BrickTile(new Rectangle(1500, 350, 50, 50)));
            //brickTiles.Add(new BrickTile(new Rectangle(2000, 350, 50, 50)));

            //TEST
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(7);
            //earthTiles.RemoveAt(15);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < brickTiles.Count(); i++)
            {
                if (brickTiles[i].Status == "Destroy")
                {
                    explosionManager.CreateTileExplosion(brickTiles[i]);
                    soundManager.BrickExplosionEffect();
                    brickTiles.Remove(brickTiles[i]);
                    i--;
                }
                else if (BrickTiles[i].Status == "TurnHard")
                {
                    if (BrickTiles[i].IsContaining == "PowerUp")
                    {
                        soundManager.PowerUpAppearEffect();
                        entityManager.mushroomPowerUps.Add(new MushroomPowerUp(new Vector2(BrickTiles[i].Rectangle.X, BrickTiles[i].Rectangle.Y - BrickTiles[i].Rectangle.Height)));
                    }
                    else
                        soundManager.HardBrickBumpEffect();
                    
                    var newHardTile = new HardTile(brickTiles[i]);
                    hardTiles.Add(newHardTile);
                    brickTiles.Remove(brickTiles[i]);
                }
            }
            foreach (var hardBrick in hardTiles)
            {
                hardBrick.Update(gameTime);
            }

            for (int i = 0; i < questionMarkTiles.Count; i++)
            {
                questionMarkTiles[i].Update(gameTime);

                if (questionMarkTiles[i].Status == "TurnHard")
                {
                    if (questionMarkTiles[i].IsContaining == "PowerUp")
                    {
                        soundManager.PowerUpAppearEffect();
                        entityManager.mushroomPowerUps.Add(new MushroomPowerUp(new Vector2(questionMarkTiles[i].Rectangle.X, questionMarkTiles[i].Rectangle.Y - questionMarkTiles[i].Rectangle.Height)));
                    }
                    else if (questionMarkTiles[i].ContainsCoin)
                    {
                        score.AddPoint(200);
                        score.AddCoin();
                        soundManager.CoinEffect();
                        coinManager.AddCoinAnimation(new Vector2(questionMarkTiles[i].Rectangle.Center.X - 15, questionMarkTiles[i].Rectangle.Y - 35)); // Hardcoded 
                    }
                    else 
                        soundManager.HardBrickBumpEffect();

                    var newHardTile = new HardTile(questionMarkTiles[i]);
                    hardTiles.Add(newHardTile);
                    questionMarkTiles.Remove(questionMarkTiles[i]);
                }
            }
            
            entityManager.Update(gameTime, soundManager);
            coinManager.Update(gameTime, soundManager);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            entityManager.Draw(spriteBatch);
            foreach (var tube in tubes)
            {
                tube.Draw(spriteBatch);
            }
            foreach (EarthTile tile in earthTiles)
            {
                tile.Draw(spriteBatch);
            }
            foreach (BrickTile brick in brickTiles)
            {
                brick.Draw(spriteBatch);
            }
            foreach (HardTile hardTile in hardTiles)
            {
                hardTile.Draw(spriteBatch);                
            }
            foreach (QuestionMarkTile tile  in questionMarkTiles)
            {
                tile.Draw(spriteBatch);
            }
            
            coinManager.Draw(spriteBatch);
            score.Draw(spriteBatch);
        }

        public void ReGenerate()
        {
            earthTiles = new List<EarthTile>();
            brickTiles = new List<BrickTile>();
            hardTiles = new List<HardTile>();
            questionMarkTiles = new List<QuestionMarkTile>();
            tubes = new List<Tube>();
            entityManager.ReLoad();
            GenerateMap(45);
        }
    }
}