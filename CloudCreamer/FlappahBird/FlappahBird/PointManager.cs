using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappahBird
{
    public class PointManager
    {
        private readonly SoundManager soundManager;
        private readonly SpriteFont spriteFont;
        protected int Points { get; set; }
        protected float TimeSinceLastPoint { get; set; }

        public PointManager(SoundManager soundManager, SpriteFont spriteFont)
        {
            this.soundManager = soundManager;
            this.spriteFont = spriteFont;
            this.Points = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var scoreX = 105;
            var scoreY = 30;
            var position = new Vector2(scoreX, scoreY);
            var text = Points < 10 ? "0" + Points : Points.ToString();

            spriteBatch.DrawString(spriteFont, text, position, Color.Black);
        }

        public void ScorePoint(GameTime gameTime)
        {
            TimeSinceLastPoint += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (TimeSinceLastPoint > 30)
            {
                soundManager.PlayScorePointEffect();
                Points++;
                TimeSinceLastPoint = 0;
            }
            
        }
    }
}