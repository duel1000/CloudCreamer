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
    }
}
