using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappahBird
{
    public class PlayerBird : Sprite
    {
        private readonly SoundManager soundManager;
        public float gravity = 0.4f;
        public float timeSinceLastJump = 0;

        public PlayerBird(Texture2D texture, Vector2 position, SoundManager soundManager) : base(texture, position, 1, 3, 14)
        {
            this.soundManager = soundManager;
            Origin = new Vector2(texture.Width/6,texture.Height/2);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsDead)
            {
                position.Y = position.Y + Velocity.Y;
                Velocity = new Vector2(Velocity.X, Velocity.Y + gravity);

                if (timeSinceLastJump > 180 && RotationAngle < 1.2)
                    RotationAngle += 0.1f;

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && timeSinceLastJump > 70)
                {
                    soundManager.PlayJumpEffect();
                    Velocity = new Vector2(0, -6);
                    RotationAngle = -0.4f;
                    timeSinceLastJump = 0;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Space))
                {
                    timeSinceLastJump += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }

            BoundingBox = new Rectangle((int)position.X, (int)position.Y, 26, 18);

            base.Update(gameTime);
        }

        public void Hit()
        {
            IsDead = true;
            soundManager.PlayHitEffect();
            position.X = -5000;
        }

        public bool IsDead { get; private set; }
    }
}