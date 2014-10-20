using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3
{
    class CoinManager
    {
        private List<CoinAnimation> coinAnimations = new List<CoinAnimation>();
        private SoundManager _soundManager;

        public CoinManager(SoundManager soundManager)
        {
            this._soundManager = soundManager;
        }

        public void AddCoinAnimation(Vector2 position)
        {
            coinAnimations.Add(new CoinAnimation(position));
        }

        public void Update(GameTime gameTime, SoundManager soundManager)
        {
            for (int i = 0; i < coinAnimations.Count; i++)
            {
                if (!coinAnimations[i]._soundEffectPlayed)
                {
                    soundManager.CoinEffect();
                    coinAnimations[i]._soundEffectPlayed = true;
                }

                coinAnimations[i].Update(gameTime);
                if (coinAnimations[i].IsDone())
                {
                    coinAnimations.Remove(coinAnimations[i]);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var coin in coinAnimations)
            {
                coin.Draw(spriteBatch);
            }
        }
    }

    class CoinAnimation : SpriteAnimation
    {
        private Vector2 _startingPosition;
        public bool _soundEffectPlayed;

        public CoinAnimation(Vector2 startingPosition)
            : base("coinspin", startingPosition, 1, 7, 26)
        {
            this._startingPosition = startingPosition;
            velocity.Y = -14f;
        }

        public bool IsDone()
        {
            bool isDone = position.Y > _startingPosition.Y ? true : false;
            return isDone;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;

            if (velocity.Y < 10)
                velocity.Y += 0.5f;

            base.Update(gameTime);
        }
    }
}
