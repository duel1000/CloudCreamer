using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3
{
    public class EntityManager
    {
        public List<MushroomEnemy> evilMushrooms = new List<MushroomEnemy>();
        public List<MushroomPowerUp> mushroomPowerUps = new List<MushroomPowerUp>(); 
        public List<FireFlower> fireFlowers = new List<FireFlower>(); 

        public EntityManager()
        {
        }

        public void Update(GameTime gameTime, SoundManager soundManager)
        {
            for (int i = 0; i < evilMushrooms.Count; i++)
            {
                if (evilMushrooms[i].IsDead)
                {
                    soundManager.StompEffect();
                    evilMushrooms.Remove(evilMushrooms[i]);
                    i--;
                }
                else
                {
                    evilMushrooms[i].Update(gameTime);
                }
            }

            for (int i = 0; i < mushroomPowerUps.Count; i++)
            {
                if (mushroomPowerUps[i].IsEaten)
                {
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
                    fireFlowers.Remove(fireFlowers[i]);
                    i--;
                }
                else
                {
                    fireFlowers[i].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var mushroom in evilMushrooms)
            {
                mushroom.Draw(spriteBatch);
            }
            foreach (var mushroomPowerUp in mushroomPowerUps)
            {
                mushroomPowerUp.Draw(spriteBatch);
            }
            foreach (var fireflower in fireFlowers)
            {
                fireflower.Draw(spriteBatch);
            }
        }

        public void ReLoad()
        {
            evilMushrooms = new List<MushroomEnemy>();
            mushroomPowerUps = new List<MushroomPowerUp>();
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

            BoundingBox = new Rectangle(DestinationRectangle.X + 5, DestinationRectangle.Y, DestinationRectangle.Width - 10, DestinationRectangle.Height);

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