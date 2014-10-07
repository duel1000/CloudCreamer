using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioBros2
{
    public class CollisionManager
    {
        private readonly Mario mario;
        private readonly EarthManager earthManager;
        
        public CollisionManager(Mario mario, EarthManager earthManager)
        {
            this.mario = mario;
            this.earthManager = earthManager;
        }

        public void Update(GameTime gameTime)
        {
            CheckCollisions();
        }

        private void CheckCollisions()
        {
            CheckMarioToEarth();
        }

        private void CheckMarioToEarth()
        {
            if (mario.inAir)
            {
                foreach (var earthBlock in earthManager.EarthList)
                {
                    if (earthBlock.BoundingBox.Intersects(mario.BoundingBox) && mario.timeSinceJumpKeyPressed > 200)
                    {
                        mario.VelocityY = 0.0f;
                        mario.inAir = false;
                    }
                }
            }
            else // Håndterer at mario falder ned fra kanten af jorden
            {
                bool isMarioStandingOnTheGround = false;

                foreach (var earthBlock in earthManager.EarthList)
                {
                    if (earthBlock.BoundingBox.Intersects(mario.BoundingBox))
                    {
                        isMarioStandingOnTheGround = true;
                    }
                }

                if (isMarioStandingOnTheGround == false)
                {
                    mario.VelocityY += 0.35f;
                    mario.inAir = true;
                }
            }
            
        }
    }
}