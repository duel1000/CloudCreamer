using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3
{
    public class EntityManager
    {
        public List<MushroomEnemy> evilMushrooms = new List<MushroomEnemy>();
        public List<MushroomPowerUp> mushroomPowerUps = new List<MushroomPowerUp>(); 

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
                    soundManager.StompEffect();
                    mushroomPowerUps.Remove(mushroomPowerUps[i]);
                    i--;
                }
                else
                {
                    mushroomPowerUps[i].Update(gameTime);
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
        }
    }

    public class MushroomPowerUp : SpriteAnimation
    {
        public bool IsEaten { get; set; }
        private bool _goingLeft;

        public MushroomPowerUp(Vector2 position) : base("mushroompowerup", position, 1, 1, 1)
        {
            velocity.X = 2f;
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
            if (BoundingBox.TouchTopOf(tile.BoundingBox))
            {
                position.Y = tile.BoundingBox.Y - DestinationRectangle.Height;
                velocity.Y = 0f;
            }
            else if (BoundingBox.TouchLeftOf(tile.BoundingBox))
            {
                if (!_goingLeft)
                {
                    velocity.X -= 4;
                    _goingLeft = true;
                    flipSprite = true;
                }
            }
            else if (BoundingBox.TouchRightOf(tile.BoundingBox))
            {
                if (_goingLeft)
                {
                    velocity.X += 4;
                    _goingLeft = false;
                    flipSprite = false;
                }
            }
        }
    }
}