using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappahBird
{
    public class PlayerBird : Sprite
    {
        public float gravity = 0.4f;
        public float timeSinceLastJump = 0;

        public PlayerBird(Texture2D texture, Vector2 position) : base(texture, position, 1, 3, 8)
        {
            Origin = new Vector2(texture.Width/6,texture.Height/2);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y = position.Y + Velocity.Y;
            Velocity = new Vector2(Velocity.X, Velocity.Y + gravity);
            
            if(timeSinceLastJump > 180 && RotationAngle < 1.2)
                RotationAngle += 0.1f;

            if(Keyboard.GetState().IsKeyDown(Keys.Space) && timeSinceLastJump > 70)
            {
                Velocity = new Vector2(0, -6);
                RotationAngle = -0.4f;
                timeSinceLastJump = 0;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                timeSinceLastJump += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            base.Update(gameTime);
        }
    }
}