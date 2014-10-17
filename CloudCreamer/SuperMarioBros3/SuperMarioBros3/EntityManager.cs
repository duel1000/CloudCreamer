using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3
{
    public class EntityManager
    {
        public List<Mushroom> evilMushrooms = new List<Mushroom>();

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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var mushroom in evilMushrooms)
            {
                mushroom.Draw(spriteBatch);
            }
        }
    }
}