using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarioBros3
{
    class CollisionManager
    {
        public CollisionManager()
        {
        }

        public void PlayerFlagpoleCollision(Player player, Flagpole flagpole)
        {
            if (player.BoundingBox.TouchTopOf(flagpole.BoundingBox))
            {
                player.OnTheFlagPole = true;
                flagpole.RunEndingAnimation(player.position);
            }
            else if (player.BoundingBox.TouchLeftOf(flagpole.BoundingBox))
            {
                player.OnTheFlagPole = true;
                flagpole.RunEndingAnimation(player.position);
            }
        }

        public void FireBallEarthCollision(List<FireBall> fireBalls, List<EarthTile> earthTiles)
        {
            foreach (var fireball in fireBalls)
            {
                foreach (var earthTile in earthTiles)
                {
                    if (fireball.BoundingBox.TouchTopOf(earthTile.BoundingBox))
                        fireball.Bounce();
                    else if(fireball.BoundingBox.TouchLeftOf(earthTile.BoundingBox))
                        fireball.Reverse();
                }
            }
        }
    }
}
