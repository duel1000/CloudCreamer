using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3
{
    public class ExplosionManager
    {
        private List<BrickExplosion> brickExplosions = new List<BrickExplosion>();
        private List<FireballExplosion> fireballExplosions = new List<FireballExplosion>();
        private List<Fireworks> fireworks = new List<Fireworks>();
        private int _fireworksCount;
        private bool shiftDirection = false;
        private float _elapsedGameTime;

        private SoundManager soundManager;

        public ExplosionManager(SoundManager soundManager)
        {
            this.soundManager = soundManager;
        }

        public void Update(GameTime gameTime)
        {
            _elapsedGameTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            foreach (var explosion in brickExplosions)
            {
                if (shiftDirection)
                {
                    explosion.position.X += 2.5f;
                    shiftDirection = !shiftDirection;
                }
                    
                else
                {
                    explosion.position.X -= 2.5f;
                    shiftDirection = !shiftDirection;
                }

                explosion.Update(gameTime);
                if (explosion.IsDone())
                    brickExplosions.Remove(explosion);
            }

            foreach (var fireballExplosion in fireballExplosions)
            {
                fireballExplosion.Update(gameTime);
            }

            foreach (var firework in fireworks)
            {
                firework.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < brickExplosions.Count; i++)
            {
                brickExplosions[i].Draw(spriteBatch);

                if (brickExplosions[i].position.Y > 3000)
                {
                    brickExplosions.Remove(brickExplosions[i]);
                    i--;
                }
            }

            for (int i = 0; i < fireballExplosions.Count; i++)
            {
                fireballExplosions[i].Draw(spriteBatch);

                if (fireballExplosions[i].IsDone())
                {
                    fireballExplosions.Remove(fireballExplosions[i]);
                    i--;
                }
            }

            for (int i = 0; i < fireworks.Count; i++)
            {
                fireworks[i].Draw(spriteBatch);

                if (fireworks[i].IsDone())
                {
                    fireworks.Remove(fireworks[i]);
                    i--;
                }
            }
        }

        public void CreateTileExplosion(BrickTile brickTile)
        {
            var topLeft = new Vector2(brickTile.Rectangle.X + 10, brickTile.Rectangle.Y - 10);
            var topRight = new Vector2(brickTile.Rectangle.X - 25 + brickTile.Rectangle.Width, brickTile.Rectangle.Y - 10);
            var bottomLeft = new Vector2(brickTile.Rectangle.X + 10, brickTile.Rectangle.Y + brickTile.Rectangle.Height - 30);
            var bottomRight = new Vector2(brickTile.Rectangle.X - 25 + brickTile.Rectangle.Width, brickTile.Rectangle.Y + brickTile.Rectangle.Height - 30);
            brickExplosions.Add(new BrickExplosion(topLeft));
            brickExplosions.Add(new BrickExplosion(topRight));
            brickExplosions.Add(new BrickExplosion(bottomLeft));
            brickExplosions.Add(new BrickExplosion(bottomRight));
        }

        public void CreateFireballExplosion(Vector2 position)
        {
            fireballExplosions.Add(new FireballExplosion(position));
        }

        //Hardcoded values based on castleposition
        public void StartFireworks(Vector2 castlePosition)
        {
            if (_elapsedGameTime > 3200) // Waiting
                _elapsedGameTime = 0;

            if (_elapsedGameTime > 600 && _fireworksCount == 0)
            {
                fireworks.Add(new Fireworks(new Vector2(castlePosition.X + 170, castlePosition.Y - 57)));
                _fireworksCount++;
                soundManager.FireworksEffect();
                _elapsedGameTime = 0;
            }
            if (_elapsedGameTime > 620 && _fireworksCount == 1)
            {
                fireworks.Add(new Fireworks(new Vector2(castlePosition.X, castlePosition.Y - 40)));
                _fireworksCount++;
                soundManager.FireworksEffect();
                _elapsedGameTime = 0;
            }
            if (_elapsedGameTime > 620 && _fireworksCount == 2)
            {
                fireworks.Add(new Fireworks(new Vector2(castlePosition.X - 60, castlePosition.Y - 63)));
                _fireworksCount++;
                soundManager.FireworksEffect();
                _elapsedGameTime = 0;
            }
            if (_elapsedGameTime > 660 && _fireworksCount == 3)
            {
                fireworks.Add(new Fireworks(new Vector2(castlePosition.X + 190, castlePosition.Y - 40)));
                _fireworksCount++;
                soundManager.FireworksEffect();
                _elapsedGameTime = 0;
            }
            if (_elapsedGameTime > 680 && _fireworksCount == 4)
            {
                fireworks.Add(new Fireworks(new Vector2(castlePosition.X + 85, castlePosition.Y - 55)));
                _fireworksCount++;
                soundManager.FireworksEffect();
                _elapsedGameTime = 0;
            }
            if (_elapsedGameTime > 600 && _fireworksCount == 5)
            {
                fireworks.Add(new Fireworks(new Vector2(castlePosition.X + 260, castlePosition.Y - 15)));
                _fireworksCount++;
                soundManager.FireworksEffect();
                _elapsedGameTime = 0;
            }
            if (_elapsedGameTime > 600 && _fireworksCount == 6)
            {
                fireworks.Add(new Fireworks(new Vector2(castlePosition.X + 60, castlePosition.Y - 45)));
                _fireworksCount++;
                soundManager.FireworksEffect();
                _elapsedGameTime = 0;
            }
            if (_elapsedGameTime > 580 && _fireworksCount == 7)
            {
                fireworks.Add(new Fireworks(new Vector2(castlePosition.X + 200, castlePosition.Y - 25)));
                _fireworksCount++;
                soundManager.FireworksEffect();
                _elapsedGameTime = 0;

                //Begin next level TODO
            }
        }
    }

    class Fireworks : SpriteAnimation
    {
        public bool IsDone()
        {
            return animationPlayedOnce;
        }

        public Fireworks(Vector2 position) : base("fireworks", position, 1,6,15)
        {
            
        }
    }

    public class FireballExplosion : SpriteAnimation
    {
        public bool IsDone()
        {
            return animationPlayedOnce;
        }

        public FireballExplosion(Vector2 position): base("fireballexplosion", position,1,3,18)
        {
            
        }
    }

    public class BrickExplosion : SpriteAnimation
    {
        public bool IsDone()
        {
            return animationPlayedOnce;
        }

        public BrickExplosion(Vector2 position) : base("destroyedbrick", position,1,1,1)
        {
            position.Y -= 5f;
            velocity.Y = -9f;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            if (velocity.Y < 10)
            {
                velocity.Y += 0.4f;
            }
            rotationAngle += 0.01f;
        }
    }
}