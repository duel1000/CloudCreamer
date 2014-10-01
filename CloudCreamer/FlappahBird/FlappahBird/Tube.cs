using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappahBird
{
    public class Tube : Sprite
    {
        public Tube(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        public override void Update(GameTime gameTime)
        {
            position.X -= 2.3f;

            if (position.X < -55)
            {
                position.X = 430;
            }

            base.Update(gameTime);
        }
    }
}