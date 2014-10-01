using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappahBird
{
    public class Tube : Sprite
    {
        private float speed;

        public Tube(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.speed = 30;
        }

        public override void Update(GameTime gameTime)
        {
            Velocity = new Vector2(Velocity.X + speed, Velocity.Y);
            position.X -= 2.3f;

            base.Update(gameTime);
        }
    }
}