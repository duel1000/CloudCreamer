using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3
{
    public class EntityManager
    {
        public List<MushroomEnemy> evilMushrooms = new List<MushroomEnemy>();
        public List<Turtle> turtles = new List<Turtle>(); 
        public List<MushroomPowerUp> mushroomPowerUps = new List<MushroomPowerUp>(); 
        public List<StarPowerUp> StarPowerUps = new List<StarPowerUp>(); 
        public List<FireFlower> fireFlowers = new List<FireFlower>();
        private Score score;
        private float _timeSinceLastScore;
        private int _enemiesKilledInARow;

        public EntityManager(Score score)
        {
            this.score = score;
        }

        public void Update(GameTime gameTime, SoundManager soundManager)
        {
            _timeSinceLastScore += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            for (int i = 0; i < evilMushrooms.Count; i++)
            {
                if (evilMushrooms[i].IsDead)
                {
                    soundManager.StompEffect();
                    ScorePoints(new Vector2(evilMushrooms[i].BoundingBox.X, evilMushrooms[i].BoundingBox.Y));
                    evilMushrooms.Remove(evilMushrooms[i]);
                    i--;
                }
                else
                {
                    evilMushrooms[i].Update(gameTime);
                }
            }

            for (int i = 0; i < turtles.Count; i++)
            {
                if (turtles[i].IsDead)
                {
                    soundManager.StompEffect();
                    ScorePoints(new Vector2(turtles[i].BoundingBox.X, turtles[i].BoundingBox.Y));
                    turtles.Remove(turtles[i]);
                    i--;
                }
                else
                {
                    turtles[i].Update(gameTime);
                }
            }

            for (int i = 0; i < mushroomPowerUps.Count; i++)
            {
                if (mushroomPowerUps[i].IsEaten)
                {
                    score.AddPointWithFloatingNumber(500, mushroomPowerUps[i].position);
                    mushroomPowerUps.Remove(mushroomPowerUps[i]);
                    i--;
                }
                else
                {
                    mushroomPowerUps[i].Update(gameTime);
                }
            }

            for (int i = 0; i < fireFlowers.Count; i++)
            {
                if (fireFlowers[i].IsEaten)
                {
                    score.AddPointWithFloatingNumber(1000, fireFlowers[i].position);
                    fireFlowers.Remove(fireFlowers[i]);
                    i--;
                }
                else
                {
                    fireFlowers[i].Update(gameTime);
                }
            }

            for (int i = 0; i < StarPowerUps.Count; i++)
            {
                if (StarPowerUps[i].IsEaten)
                {
                    score.AddPointWithFloatingNumber(1000, StarPowerUps[i].position);
                    StarPowerUps.Remove(StarPowerUps[i]);
                    i--;
                }
                else
                {
                    StarPowerUps[i].Update(gameTime);
                }
            }
        }

        //You have a second to multiply score or get a life
        private void ScorePoints(Vector2 position)
        {
            if (_timeSinceLastScore > 1000)
                _enemiesKilledInARow = 0;

            if (_enemiesKilledInARow == 0)
            {
                score.AddPointWithFloatingNumber(100, position);
                _enemiesKilledInARow++;
                _timeSinceLastScore = 0;
                return;
            }
            if (_timeSinceLastScore < 1000 && _enemiesKilledInARow == 1)
            {
                score.AddPointWithFloatingNumber(200, position);
                _enemiesKilledInARow++;
                _timeSinceLastScore = 0;
                return;
            }
            if (_timeSinceLastScore < 1000 && _enemiesKilledInARow == 2)
            {
                score.AddPointWithFloatingNumber(400, position);
                _enemiesKilledInARow++;
                _timeSinceLastScore = 0;
                return;
            }
            if (_timeSinceLastScore < 1000 && _enemiesKilledInARow == 3)
            {
                score.AddPointWithFloatingNumber(800, position);
                _enemiesKilledInARow++;
                _timeSinceLastScore = 0;
                return;
            }
            if (_timeSinceLastScore < 1000 && _enemiesKilledInARow == 4)
            {
                //Add life
                _enemiesKilledInARow++;
                _timeSinceLastScore = 0;
                return;
            }
            if (_timeSinceLastScore < 1000 && _enemiesKilledInARow == 5)
            {
                score.AddPointWithFloatingNumber(1000, position);
                _enemiesKilledInARow++;
                _timeSinceLastScore = 0;
                return;
            }
            if (_timeSinceLastScore < 1000 && _enemiesKilledInARow == 6)
            {
                score.AddPointWithFloatingNumber(1000, position);
                _enemiesKilledInARow++;
                _timeSinceLastScore = 0;
                return;
            }
            if (_timeSinceLastScore < 1000 && _enemiesKilledInARow == 7)
            {
                score.AddPointWithFloatingNumber(1000, position);
                _enemiesKilledInARow++;
                _timeSinceLastScore = 0;
                return;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var mushroom in evilMushrooms)
            {
                mushroom.Draw(spriteBatch);
            }
            foreach (var turtle in turtles)
            {
             turtle.Draw(spriteBatch);   
            }
            foreach (var mushroomPowerUp in mushroomPowerUps)
            {
                mushroomPowerUp.Draw(spriteBatch);
            }
            foreach (var fireflower in fireFlowers)
            {
                fireflower.Draw(spriteBatch);
            }
            foreach (var star in StarPowerUps)
            {
                star.Draw(spriteBatch);
            }
        }

        public void ReLoad()
        {
            evilMushrooms = new List<MushroomEnemy>();
            turtles = new List<Turtle>();
            mushroomPowerUps = new List<MushroomPowerUp>();
        }
    }

    public class StarPowerUp : SpriteAnimation
    {
        public bool IsEaten { get; set; }

        public StarPowerUp(Vector2 position) : base("star", position, 1, 4, 24)
        {
            velocity.X = 3.2f;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;

            if (velocity.Y < 10)
                velocity.Y += 0.45f;

            BoundingBox = new Rectangle((int)position.X, (int)position.Y, imageWidth, imageHeight);

            base.Update(gameTime);
        }

        public void TileCollision(Tile tile)
        {
            if (BoundingBox.TouchTopOf(tile.BoundingBox) && tile.IsPunched)
            {
                position.Y -= 10;
                velocity.Y = -15f;
            }
            else if (BoundingBox.TouchTopOf(tile.BoundingBox) && !tile.IsPunched)
            {
                position.Y = tile.BoundingBox.Y - DestinationRectangle.Height;
                velocity.Y = -13f;
            }
            else if (BoundingBox.TouchLeftOf(tile.BoundingBox))
            {
                if (velocity.Y != 10)
                {
                    velocity.X = -3.2f;
                }
            }
            else if (BoundingBox.TouchRightOf(tile.BoundingBox))
            {
                if (velocity.Y != 10)
                {
                    velocity.X = 3.2f;
                }
            }
            else if (BoundingBox.TouchBottomOf(tile.BoundingBox))
            {
                velocity.Y = 7f;
            }
        }

        public void SimpleCollision(Rectangle boundingBox)
        {
            if (BoundingBox.TouchTopOf(boundingBox))
            {
                position.Y = boundingBox.Y - DestinationRectangle.Height;
                velocity.Y = -13f;
            }
            else if (BoundingBox.TouchLeftOf(boundingBox))
            {
                if (velocity.Y != 10)
                {
                    velocity.X = -3.2f;
                }
            }
            else if (BoundingBox.TouchRightOf(boundingBox))
            {
                if (velocity.Y != 10)
                {
                    velocity.X = 3.2f;
                }
            }
        }
    }

    public class MushroomPowerUp : SpriteAnimation
    {
        public bool IsEaten { get; set; }
        private bool _goingLeft;

        public MushroomPowerUp(Vector2 position) : base("mushroompowerup", position, 1, 1, 1)
        {
            velocity.X = 2f;
            velocity.Y = 0;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;

            if (velocity.Y < 10)
                velocity.Y += 0.45f;

            BoundingBox = new Rectangle((int)position.X, (int)position.Y, DestinationRectangle.Width, DestinationRectangle.Height);

            base.Update(gameTime);
        }

        public void TileCollision(Tile tile)
        {
            if (BoundingBox.TouchTopOf(tile.BoundingBox) && tile.IsPunched)
            {
                position.Y -= 10;
                velocity.Y = -11;
            }
            if (BoundingBox.TouchTopOf(tile.BoundingBox) && !tile.IsPunched)
            {
                position.Y = tile.BoundingBox.Y - DestinationRectangle.Height;
                velocity.Y = 0f;
            }
            else if (BoundingBox.TouchLeftOf(tile.BoundingBox))
            {
                if (!_goingLeft && velocity.Y != 10)
                {
                    velocity.X -= 4;
                    _goingLeft = true;
                    flipSprite = true;
                }
            }
            else if (BoundingBox.TouchRightOf(tile.BoundingBox))
            {
                if (_goingLeft && velocity.Y != 10)
                {
                    velocity.X += 4;
                    _goingLeft = false;
                    flipSprite = false;
                }
            }
        }
    }

    public class FireFlower : SpriteAnimation
    {
        public bool IsEaten { get; set; }

        public FireFlower(Vector2 position) : base("fireflower", position,1,4,20)
        {
            BoundingBox = new Rectangle((int)position.X + 10, (int)position.Y, 15, 35);
        }
    }
}