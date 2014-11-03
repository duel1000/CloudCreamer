using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarioBros3
{
    class CollisionManager
    {
        private ExplosionManager explosionManager;
        private SoundManager soundManager;

        public CollisionManager(ExplosionManager explosionManager, SoundManager soundManager)
        {
            this.explosionManager = explosionManager;
            this.soundManager = soundManager;
        }

        public void PlayerFlagpoleCollision(Player player, Flagpole flagpole)
        {
            if (player.BoundingBox.TouchTopOf(flagpole.BoundingBox))
            {
                player.OnTheFlagPole = true;
                flagpole.RunFlagEndingAnimation(player.position.Y);
            }
            else if (player.BoundingBox.TouchLeftOf(flagpole.BoundingBox))
            {
                player.OnTheFlagPole = true;
                flagpole.RunFlagEndingAnimation(player.position.Y);
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
                    else if (fireball.BoundingBox.TouchRightOf(earthTile.BoundingBox))
                        fireball.Reverse();
                }
            }
        }

        public void FireBallBrickCollision(List<FireBall> fireBalls, List<BrickTile> brickTiles)
        {
            foreach (var fireball in fireBalls)
            {
                foreach (var brick in brickTiles)
                {
                    if (fireball.BoundingBox.TouchTopOf(brick.BoundingBox))
                        fireball.Bounce();
                    else if (fireball.BoundingBox.TouchLeftOf(brick.BoundingBox))
                        fireball.Reverse();
                    else if (fireball.BoundingBox.TouchRightOf(brick.BoundingBox))
                        fireball.Reverse();
                }
            }
        }

        public void FireBallQuestionmarkCollision(List<FireBall> fireBalls, List<QuestionMarkTile> questionmarkTiles)
        {
            foreach (var fireball in fireBalls)
            {
                foreach (var questionmarkTile in questionmarkTiles)
                {
                    if (fireball.BoundingBox.TouchTopOf(questionmarkTile.BoundingBox))
                        fireball.Bounce();
                    else if (fireball.BoundingBox.TouchLeftOf(questionmarkTile.BoundingBox))
                        fireball.Reverse();
                    else if (fireball.BoundingBox.TouchRightOf(questionmarkTile.BoundingBox))
                        fireball.Reverse();
                }
            }
        }

        public void FireBallHardTileCollision(List<FireBall> fireBalls, List<HardTile> hardTiles)
        {
            foreach (var fireball in fireBalls)
            {
                foreach (var hardTile in hardTiles)
                {
                    if (fireball.BoundingBox.TouchTopOf(hardTile.BoundingBox))
                        fireball.Bounce();
                    else if (fireball.BoundingBox.TouchLeftOf(hardTile.BoundingBox))
                        fireball.Reverse();
                    else if (fireball.BoundingBox.TouchRightOf(hardTile.BoundingBox))
                        fireball.Reverse();
                }
            }
        }

        public void FireBallHardEarthTileCollision(List<FireBall> fireBalls, List<HardEarthTile> hardEarthTiles)
        {
            foreach (var fireball in fireBalls)
            {
                foreach (var hardEarthTile in hardEarthTiles)
                {
                    if (fireball.BoundingBox.TouchTopOf(hardEarthTile.BoundingBox))
                        fireball.Bounce();
                    else if (fireball.BoundingBox.TouchLeftOf(hardEarthTile.BoundingBox))
                        fireball.Reverse();
                    else if (fireball.BoundingBox.TouchRightOf(hardEarthTile.BoundingBox))
                        fireball.Reverse();
                }
            }
        }

        public void FireBallHiddenTileCollision(List<FireBall> fireBalls, List<HiddenTile> hiddenTiles)
        {
            foreach (var fireball in fireBalls)
            {
                foreach (var hiddenTile in hiddenTiles)
                {
                    if (fireball.BoundingBox.TouchTopOf(hiddenTile.BoundingBox) && hiddenTile.IsPunched)
                        fireball.Bounce();
                    else if (fireball.BoundingBox.TouchLeftOf(hiddenTile.BoundingBox) && hiddenTile.IsPunched)
                        fireball.Reverse();
                    else if (fireball.BoundingBox.TouchRightOf(hiddenTile.BoundingBox) && hiddenTile.IsPunched)
                        fireball.Reverse();
                }
            }
        }

        public void FireBallTubeCollision(List<FireBall> fireBalls, List<Tube> tubes)
        {
            foreach (var fireball in fireBalls)
            {
                foreach (var tube in tubes)
                {
                    if (fireball.BoundingBox.TouchTopOf(tube.BoundingBox) ||
                        fireball.BoundingBox.TouchLeftOf(tube.BoundingBox) ||
                        fireball.BoundingBox.TouchRightOf(tube.BoundingBox))
                    {
                        fireball.Explode();
                        explosionManager.CreateFireballExplosion(fireball.position);
                        soundManager.HardBrickBumpEffect();
                    }
                }
            }
        }

        public void FireBallMushroomEnemyCollision(List<FireBall> fireballs, List<MushroomEnemy> enemies)
        {
            foreach (var fireball in fireballs)
            {
                foreach (var enemy in enemies)
                {
                    if (fireball.BoundingBox.TouchTopOf(enemy.BoundingBox) ||
                        fireball.BoundingBox.TouchBottomOf(enemy.BoundingBox) ||
                        fireball.BoundingBox.TouchLeftOf(enemy.BoundingBox) ||
                        fireball.BoundingBox.TouchRightOf(enemy.BoundingBox))
                    {
                         enemy.KillEnemy();
                         fireball.Explode();
                         explosionManager.CreateFireballExplosion(fireball.position);
                    }
                }
            }
        }

        public void FireBallTurtleCollision(List<FireBall> fireballs, List<Turtle> enemies)
        {
            foreach (var fireball in fireballs)
            {
                foreach (var enemy in enemies)
                {
                    if (fireball.BoundingBox.TouchTopOf(enemy.BoundingBox) ||
                        fireball.BoundingBox.TouchBottomOf(enemy.BoundingBox) ||
                        fireball.BoundingBox.TouchLeftOf(enemy.BoundingBox) ||
                        fireball.BoundingBox.TouchRightOf(enemy.BoundingBox))
                    {
                        enemy.IsDead = true;
                        fireball.Explode();
                        explosionManager.CreateFireballExplosion(fireball.position);
                    }
                }
            }
        }

        public void CoinBrickTileMarioCollision(Player player, CoinBrickTile coinBrickTile)
        {
            if (player.BoundingBox.TouchTopOf(coinBrickTile.BoundingBox))
            {
                player.HitTheGround(coinBrickTile.BoundingBox.Y);
            }
            if (player.BoundingBox.TouchBottomOf(coinBrickTile.BoundingBox) && player.IsMovingUpwards)
            {
                coinBrickTile.Punch();
                player.HitACeiling();
            }
            else if (player.BoundingBox.TouchLeftOf(coinBrickTile.BoundingBox))
            {
                player.HitWallRightSide(coinBrickTile.BoundingBox.X);
            }
            else if (player.BoundingBox.TouchRightOf(coinBrickTile.BoundingBox))
            {
                player.HitWallLeftSide(coinBrickTile.BoundingBox.X + coinBrickTile.BoundingBox.Width);
            }
        }

        public void StarPowerUpPlayerCollision(Player player, List<StarPowerUp> stars)
        {
            foreach (var star in stars)
            {
                if (player.BoundingBox.TouchTopOf(star.BoundingBox) ||
                        player.BoundingBox.TouchBottomOf(star.BoundingBox) ||
                        player.BoundingBox.TouchLeftOf(star.BoundingBox) ||
                        player.BoundingBox.TouchRightOf(star.BoundingBox))
                {
                    player.StarMode();
                    star.IsEaten = true;
                }
            }
        }
    }
}
