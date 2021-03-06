﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    class Map
    {
        private List<EarthTile> earthTiles = new List<EarthTile>();
        private List<HardEarthTile> hardEarthTiles = new List<HardEarthTile>();
        private List<BrickTile> brickTiles = new List<BrickTile>();
        private List<HardTile> hardTiles = new List<HardTile>();
        private List<QuestionMarkTile> questionMarkTiles = new List<QuestionMarkTile>();
        private List<HiddenTile> hiddenTiles = new List<HiddenTile>();
        private List<CoinBrickTile> coinBrickTiles = new List<CoinBrickTile>(); 
        private List<Tube> tubes = new List<Tube>();
        
        private Castle castle;
        public Vector2 CastlePosition { get { return castle.position; } } // For fireworks

        private Flagpole flagpole;
        private ExplosionManager explosionManager;
        private CoinManager coinManager;
        private Player player;
        
        private BackgroundManager backgroundManager;

        public Flagpole Flagpole{get { return flagpole; }}
        
        private SoundManager soundManager;
        private EntityManager entityManager;

        private Score score;
        public List<CoinBrickTile> CoinBrickTiles{get { return coinBrickTiles; }} 
        public List<HiddenTile> HiddenTiles{get { return hiddenTiles; }} 
        public List<HardEarthTile> HardEarthTiles {get { return hardEarthTiles; }}
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

        public Map(ExplosionManager explosionManager, SoundManager soundManager, EntityManager entityManager, CoinManager coinManager, Score score, Player player, BackgroundManager backgroundManager)
        {
            this.explosionManager = explosionManager;
            this.soundManager = soundManager;
            this.entityManager = entityManager;
            this.coinManager = coinManager;
            this.score = score;
            this.player = player;
            this.backgroundManager = backgroundManager;
        }

        public void GenerateMap(int size)
        {
            LoadLevel(1, size);
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

                        if(player.IsBigMario)
                            entityManager.fireFlowers.Add(new FireFlower(new Vector2(BrickTiles[i].Rectangle.X, BrickTiles[i].Rectangle.Y - BrickTiles[i].Rectangle.Height)));
                        else
                            entityManager.mushroomPowerUps.Add(new MushroomPowerUp(new Vector2(BrickTiles[i].Rectangle.X, BrickTiles[i].Rectangle.Y - BrickTiles[i].Rectangle.Height)));
                    }
                    else if (brickTiles[i].IsContaining == "star")
                    {
                        soundManager.PowerUpAppearEffect();
                        entityManager.StarPowerUps.Add(new StarPowerUp(new Vector2(brickTiles[i].Rectangle.X, brickTiles[i].Rectangle.Y - 50)));
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

                        if(player.IsBigMario)
                            entityManager.fireFlowers.Add(new FireFlower(new Vector2(questionMarkTiles[i].Rectangle.X + 3, questionMarkTiles[i].Rectangle.Y - questionMarkTiles[i].Rectangle.Height + 15)));
                        else
                            entityManager.mushroomPowerUps.Add(new MushroomPowerUp(new Vector2(questionMarkTiles[i].Rectangle.X, questionMarkTiles[i].Rectangle.Y - questionMarkTiles[i].Rectangle.Height)));
                    }
                    else if (questionMarkTiles[i].ContainsCoin)
                    {
                        score.AddPointWithFloatingNumber(200, new Vector2(questionMarkTiles[i].Rectangle.Center.X - 20, questionMarkTiles[i].Rectangle.Y - 30));
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
            foreach (var hiddenTile in hiddenTiles)
            {
                if (hiddenTile.IsPunched)
                {
                    hiddenTile.Update(gameTime);

                    if (!hiddenTile.IsEmpty)
                    {
                        hiddenTile.IsEmpty = true;
                        entityManager.mushroomPowerUps.Add(new MushroomPowerUp(new Vector2(hiddenTile.Rectangle.X, hiddenTile.Rectangle.Y - hiddenTile.Rectangle.Height)));
                    }
                        
                }
            }
            foreach (var coinBrick in coinBrickTiles)
            {
                coinBrick.Update(gameTime);
                if (coinBrick.ThrowCoin && coinBrick.IsPunched && !coinBrick.IsEmpty)
                {
                    coinManager.AddCoinAnimation(new Vector2(coinBrick.Rectangle.Center.X - 15, coinBrick.Rectangle.Y - 35)); // Hardcoded 
                    score.AddPointWithFloatingNumber(100, new Vector2(coinBrick.Rectangle.Center.X, coinBrick.Rectangle.Y));
                    coinBrick.TakeCoin();
                    coinBrick.ThrowCoin = false;
                }
            }
            flagpole.Update(gameTime);
            castle.Update(gameTime);
            entityManager.Update(gameTime, soundManager);
            coinManager.Update(gameTime, soundManager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backgroundManager.Draw(spriteBatch);
            flagpole.Draw(spriteBatch);
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
            foreach (HardEarthTile tile in hardEarthTiles)
            {
                tile.Draw(spriteBatch);
            }
            foreach (var hiddenTile in hiddenTiles)
            {
                if(hiddenTile.IsPunched)
                    hiddenTile.Draw(spriteBatch);
            }
            foreach (var coinBrick in coinBrickTiles)
            {
                coinBrick.Draw(spriteBatch);
            }
            
            castle.Draw(spriteBatch);
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
            coinBrickTiles = new List<CoinBrickTile>();
            entityManager.ReLoad();
            GenerateMap(45);
        }

        public void LoadLevel(int number, int size)
        {
            for (int x = 0; x < 232; x++)
            {
                earthTiles.Add(new EarthTile(new Rectangle(x * size, 440, size, size)));

                width = (x + 1) * size;
                height = 480; //Hardcoded
            }

            //Test

            CreateBackground();

            //Level
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(950, 250, 50, 50), true, "PowerUp"));
            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(1000, 50)));
            brickTiles.Add(new BrickTile(new Vector2(1150, 250)));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(1200, 250, 50, 50), true, "", true));
            brickTiles.Add(new BrickTile(new Vector2(1250, 250)));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(1300, 250, 50, 50), true, "", true));
            brickTiles.Add(new BrickTile(new Vector2(1350, 250)));

            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(1250, 50, 50, 50), true, "", true));

            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(1550, 350)));

            tubes.Add(new Tube(new Vector2(1550, 350)));
            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(1800, 350)));
            tubes.Add(new Tube(new Vector2(2100, 300)));
            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(2400, 350)));
            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(2455, 350)));
            tubes.Add(new Tube(new Vector2(2600, 300)));

            hiddenTiles.Add(new HiddenTile(new Vector2(2850, 200)));

            earthTiles.RemoveAt(3400 / 50);
            earthTiles.RemoveAt(3400 / 50);

            brickTiles.Add(new BrickTile(new Vector2(3400, 250)));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(3450, 250, 50, 50), true, "PowerUp"));
            brickTiles.Add(new BrickTile(new Vector2(3500, 250)));

            brickTiles.Add(new BrickTile(new Vector2(3600, 130)));
            brickTiles.Add(new BrickTile(new Vector2(3650, 130)));
            brickTiles.Add(new BrickTile(new Vector2(3700, 130)));
            brickTiles.Add(new BrickTile(new Vector2(3750, 130)));
            brickTiles.Add(new BrickTile(new Vector2(3800, 130)));
            brickTiles.Add(new BrickTile(new Vector2(3850, 130)));
            brickTiles.Add(new BrickTile(new Vector2(3900, 130)));
            brickTiles.Add(new BrickTile(new Vector2(3950, 130)));

            //entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(2400, 350))); //Fix after they actually spawn at the ground. 
            //entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(2455, 350)));

            earthTiles.RemoveAt(4250 / 50);
            earthTiles.RemoveAt(4250 / 50);

            brickTiles.Add(new BrickTile(new Vector2(4100, 130)));
            brickTiles.Add(new BrickTile(new Vector2(4150, 130)));
            brickTiles.Add(new BrickTile(new Vector2(4200, 130)));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(4250, 130, 50, 50), true, "", true));
            coinBrickTiles.Add(new CoinBrickTile(new Vector2(4250, 250), 8)); //Multiple coins box!

            brickTiles.Add(new BrickTile(new Vector2(4500, 250)));
            brickTiles.Add(new BrickTile(new Vector2(4550, 250), true, "star", false)); //Contains a star!

            entityManager.turtles.Add(new Turtle(new Vector2(4600, 200)));

            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(4900, 250, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(5050, 250, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(5200, 250, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(5050, 100, 50, 50), true, "PowerUp", true));

            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(5250, 350)));
            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(5315, 350)));

            brickTiles.Add(new BrickTile(new Vector2(5600, 250)));

            brickTiles.Add(new BrickTile(new Vector2(5800, 130)));
            brickTiles.Add(new BrickTile(new Vector2(5850, 130)));
            brickTiles.Add(new BrickTile(new Vector2(5900, 130)));

            brickTiles.Add(new BrickTile(new Vector2(6100, 130)));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(6150, 130, 50, 50), true, "", true));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(6200, 130, 50, 50), true, "", true));
            brickTiles.Add(new BrickTile(new Vector2(6250, 130)));

            brickTiles.Add(new BrickTile(new Vector2(6150, 270)));
            brickTiles.Add(new BrickTile(new Vector2(6200, 270)));

            AddSmallBlockTriangleLeft(6400);
            AddSmallBlockTriangleRight(6850);

            AddSmallBlockTriangleLeftWithExtraColumn(7040);
            earthTiles.RemoveAt(7900 / 50);
            earthTiles.RemoveAt(7900 / 50);
            AddSmallBlockTriangleRight(7532);

            tubes.Add(new Tube(new Vector2(7850, 330)));

            brickTiles.Add(new BrickTile(new Vector2(8200, 270)));
            brickTiles.Add(new BrickTile(new Vector2(8250, 270)));
            questionMarkTiles.Add(new QuestionMarkTile(new Rectangle(8300, 270, 50, 50), true, "", true));
            brickTiles.Add(new BrickTile(new Vector2(8350, 270)));

            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(8550, 350)));
            entityManager.evilMushrooms.Add(new MushroomEnemy(new Vector2(8605, 350)));

            tubes.Add(new Tube(new Vector2(8830, 330)));
            AddLargeBlockTriangle(8750);

            flagpole = new Flagpole(new Vector2(9814, 20));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(9800, 390)));

            castle = new Castle(new Vector2(10000,140));
        }

        private void CreateBackground()
        {
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(0,300),"biggreenhill"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(2200, 300), "biggreenhill"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(4250, 300), "biggreenhill"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(6720, 300), "biggreenhill"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(9350, 300), "biggreenhill"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(920, 360), "smallgreenhill"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(2750, 360), "smallgreenhill"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(5000, 360), "smallgreenhill"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(7650, 360), "smallgreenhill"));

            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(520, 310), "triplebush"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(3500, 310), "triplebush"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(4680, 310), "triplebush"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(6550, 310), "triplebush"));

            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(1550, 280), "doublebush"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(3100, 280), "doublebush"));

            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(1250, 280), "singlebush"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(4000, 280), "singlebush"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(5800, 280), "singlebush"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(7450, 280), "singlebush"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(8000, 280), "singlebush"));

            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(250, 45), "singlecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(1150, 55), "singlecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(2100, 70), "singlecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(3000, 10), "singlecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(4800, 40), "singlecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(5400, 20), "singlecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(6100, 20), "singlecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(7050, 50), "singlecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(7890, 30), "singlecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(9400, 20), "singlecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(9730, 40), "singlecloud"));

            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(1740, 0), "doublecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(3740, -10), "doublecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(4250, 0), "doublecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(6550, 30), "doublecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(8850, 20), "doublecloud"));

            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(1400, 60), "triplecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(3500, -5), "triplecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(4000, 15), "triplecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(6280, 5), "triplecloud"));
            backgroundManager.AddBackgroundElement(new BackgroundElement(new Vector2(8400, 50), "triplecloud"));
        }

        public void AddSmallBlockTriangleLeft(int distance)
        {
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 50, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 100, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 150, 390)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 50, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 100, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 150, 340)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 100, 290)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 150, 290)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 150, 240)));
        }

        public void AddSmallBlockTriangleLeftWithExtraColumn(int distance)
        {
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 50, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 100, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 150, 390)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 50, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 100, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 150, 340)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 100, 290)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 150, 290)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 150, 240)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 200, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 200, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 200, 290)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 200, 240)));
        }

        public void AddSmallBlockTriangleRight(int distance)
        {
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance - 50, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance - 100, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance - 150, 390)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance - 50, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance - 100, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance - 150, 340)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance - 100, 290)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance - 150, 290)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance - 150, 240)));
        }

        public void AddLargeBlockTriangle(int distance)
        {
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 200, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 250, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 300, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 350, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 400, 390)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 450, 390)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 250, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 300, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 350, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 400, 340)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 450, 340)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 300, 290)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 350, 290)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 400, 290)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 450, 290)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 350, 240)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 400, 240)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 450, 240)));

            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 400, 190)));
            hardEarthTiles.Add(new HardEarthTile(new Vector2(distance + 450, 190)));
        }
    }
}