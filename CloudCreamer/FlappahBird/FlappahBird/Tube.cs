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

            base.Update(gameTime);
        }
    }
}