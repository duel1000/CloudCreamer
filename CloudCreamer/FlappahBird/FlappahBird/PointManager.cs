using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace FlappahBird
{
    public class PointManager
    {
        private readonly SoundManager soundManager;
        private readonly SpriteFont spriteFont;
        public int Points { get; set; }
        private int highScore;
        protected float TimeSinceLastPoint { get; set; }

        public PointManager(SoundManager soundManager, SpriteFont spriteFont)
        {
            this.soundManager = soundManager;
            this.spriteFont = spriteFont;
            this.Points = 0;
            highScore = ReadPoints();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var scoreX = 105;
            var scoreY = 30;
            var position = new Vector2(scoreX, scoreY);
            var text = Points < 10 ? "0" + Points : Points.ToString();
            spriteBatch.DrawString(spriteFont, text, position, Color.White);
        }

        public void DrawHighscore(SpriteBatch spriteBatch)
        {
            highScore = highScore < Points ? Points : highScore;
            var scoreX = 25;
            var scoreY = 210;
            var position = new Vector2(scoreX, scoreY);
            var text = highScore < 10 ? "Highscore: 0" + highScore : "Highscore: " + highScore.ToString();
            spriteBatch.DrawString(spriteFont, text, position, Color.Red);
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

        public void ResetPoints()
        {
            Points = 0;
        }

        public void SavePoints()
        {
            if (highScore >= Points)
                return;

            using (var writer = new StreamWriter(@"C:\Test\test.txt"))
            {
                writer.WriteLine(Points);
            }
        }

        public int ReadPoints()
        {
            var line = "";

            using (var reader = new StreamReader(@"C:\Test\test.txt"))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                }
            }
            return Int32.Parse(line);
        }
    }
}