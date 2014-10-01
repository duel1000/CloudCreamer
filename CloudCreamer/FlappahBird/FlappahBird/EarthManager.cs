using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappahBird
{
    public class EarthManager
    {
        private readonly Texture2D texture;
        private List<Earth> earthList = new List<Earth>();

        public EarthManager(Texture2D texture)
        {
            this.texture = texture;
            CreateEarths();
        }

        private void CreateEarths()
        {
            var position = new Vector2(0, 350);
            var earth = new Earth(texture, position);
            earthList.Add(earth);

            position = new Vector2(241, 350);
            var earthToTheRight = new Earth(texture, position);
            earthList.Add(earthToTheRight);

            position = new Vector2(482, 350);
            var earthToTheRightRight = new Earth(texture, position);
            earthList.Add(earthToTheRightRight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var earth in earthList)
            {
                earth.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var earth in earthList)
            {
                earth.Update(gameTime);

                if (earth.position.X < -250)
                {
                    earth.position.X = 256;
                }
            }
        }
    }
}