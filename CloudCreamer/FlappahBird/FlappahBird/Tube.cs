using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappahBird
{
    public class Tube : Sprite
    {
        public Tube(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            this.Speed = 2.3f;
        }

        public override void Update(GameTime gameTime)
        {
            position.X -= Speed;

            if (position.X < -55)
            {
                position.X = 430;
            }

            BoundingBox = new Rectangle((int)position.X + 5, (int)position.Y, 52, 410);

            base.Update(gameTime);
        }
    }
}