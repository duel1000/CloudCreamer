using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappahBird
{
    public class PlayerBird : Sprite
    {

        public float gravitySpeed = 0.0f;

        public PlayerBird(Texture2D texture, Vector2 position) : base(texture, position)
        {
           
        }

        public override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                position.Y -= 5;
            }

            base.Update(gameTime);
        }
    }
}