using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CloudCreamer
{
    public class EnemyManager
    {
        private readonly Texture2D texture;
        private readonly Rectangle bounds;
        private List<Enemy> enemies = new List<Enemy>(); 

        public EnemyManager(Texture2D texture, Rectangle bounds)
        {
            this.texture = texture;
            this.bounds = bounds;

            CreateEnemy();
        }

        private void CreateEnemy()
        {
            var position = RandomPosition();
            var enemy = new Enemy(texture, position, bounds);
            enemies.Add(enemy);
        }

        private Vector2 RandomPosition()
        {
            var random = new Random();
            var xPosition = random.Next(bounds.Width - texture.Width + 1);
            var yPosition = random.Next(bounds.Height);
            return new Vector2(xPosition, yPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
            }
        }
    }
}