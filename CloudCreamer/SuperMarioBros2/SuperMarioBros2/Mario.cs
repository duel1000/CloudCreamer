using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioBros2
{
    public class Mario : Sprite
    {
        private readonly SoundManager soundManager;
        public float timeSinceJumpKeyPressed = 0;

        public Mario(Texture2D texture, Vector2 position, SoundManager soundManager) : base(texture, position, 1, 9, 10)
        {
            this.soundManager = soundManager;
            inAir = true;
        }

        public override void Update(GameTime gameTime)
        {
            position.Y = position.Y;
            BoundingBox = new Rectangle((int)position.X, (int)position.Y, 20, 40); // HEST hvor bliver en rectangel tegnet fra - marioright kan stå i luften?
            position += velocity;
            HandleInput(gameTime);
            base.Update(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = 3f;
                MovingLeft = false;
                AnimationOn = true;

                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    velocity.X = 4.5f;
                    framesPerSecond = 16;
                }
                else
                {
                    framesPerSecond = 10;
                }
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X = -3f;
                MovingLeft = true;
                AnimationOn = true;

                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    velocity.X = -4.5f;
                    framesPerSecond = 16;
                }
                else
                {
                    framesPerSecond = 10;
                }
            }
            else
            {
                velocity.X = 0f;
                currentFrame = 0;
                AnimationOn = false;
            }

            timeSinceJumpKeyPressed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && inAir == false) // HEST håndter at man ikke kan hoppe LIGE efter ét hop
            {
                soundManager.PlayJumpEffect();
                timeSinceJumpKeyPressed = 0;
                inAir = true;
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    position.Y -= 15f;
                    velocity.Y = -14f;
                }
                else
                {
                    position.Y -= 10f;
                    velocity.Y = -10f;
                }
            }

            if (inAir == true)
            {
                velocity.Y += 0.4f; // Tallet skal åbenbart gå op i et eller andet for at han lander på jorden samme sted
            }
        }
    }
}
