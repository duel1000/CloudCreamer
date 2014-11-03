using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    public class Score
    {
        private int _points;
        private int _coinCounter;

        private List<FloatingScoreText> floatingScoreTexts = new List<FloatingScoreText>(); 

        private int _timer = 120;
        private float _elapsedGameTime;
        private float _elapsedGameTimeForColorChange;
        private Vector2 _timerPosition = new Vector2(0,40);

        private Vector2 _scorePosition = new Vector2(10,40);
        private Vector2 _gameTextPosition = new Vector2(20,8);
        private Vector2 _coinCounterPosition = new Vector2(0,40);

        private SoundManager soundManager;
        private bool _runningOutOfTimeSoundEffectPlayed;
        private bool _changeColor;

        private bool _calculateScoreAnimation;

        private readonly SpriteFont spriteFont;
        
        public int Timer{get { return _timer; }}

        public Score(SoundManager soundManager)
        {
            spriteFont = Content_Manager.GetInstance().SpriteFonts["PointsFont"];
            this.soundManager = soundManager;
        }

        public void Update(GameTime gameTime)
        {
            _elapsedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_calculateScoreAnimation)
            {
                RunCalculateScoreAnimation();
                return;
            }
            _elapsedGameTimeForColorChange += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_elapsedGameTime > 1000 && _timer != 0)
            {
                _timer--;
                _elapsedGameTime = 0;
            }

            if (_timer == 30 && !_runningOutOfTimeSoundEffectPlayed)
            {
                soundManager.RunningOutOfTime();
                _runningOutOfTimeSoundEffectPlayed = true;
                _elapsedGameTimeForColorChange = 0;
            }

            for (int i = 0; i < floatingScoreTexts.Count; i++)
            {
                floatingScoreTexts[i].Update(gameTime);

                if (floatingScoreTexts[i].IsDone)
                {
                    floatingScoreTexts.Remove(floatingScoreTexts[i]);
                    i--;
                }
            }
        }

        private void RunCalculateScoreAnimation()
        {
            if (_elapsedGameTime > 24 && _timer != 0)
            {
                soundManager.CoinEffect();
                AddPoint(100);
                _timer--;
                _elapsedGameTime = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawPoints(spriteBatch);
            DrawFloatingScores(spriteBatch);
            spriteBatch.DrawString(spriteFont, "MARIO", _gameTextPosition, Color.White);
            DrawCoinCounter(spriteBatch);
            DrawTimer(spriteBatch);
        }

        private void DrawFloatingScores(SpriteBatch spriteBatch)
        {
            foreach (var floatingScore in floatingScoreTexts)
            {
                floatingScore.Draw(spriteBatch);
            }
        }

        private void DrawTimer(SpriteBatch spriteBatch)
        {
            var color = new Color();
            
            if (_runningOutOfTimeSoundEffectPlayed && _elapsedGameTimeForColorChange < 1000)
            {
                color = Color.Red;
            }
            else if (_runningOutOfTimeSoundEffectPlayed && _elapsedGameTimeForColorChange < 2000)
            {
                color = Color.White;
            }
            else if(_runningOutOfTimeSoundEffectPlayed && _elapsedGameTimeForColorChange < 3000)
            {
                _elapsedGameTimeForColorChange = 0;
            }
            else
            {
                color = Color.White;
            }

            spriteBatch.DrawString(spriteFont, _timer.ToString(), _timerPosition, color);
            spriteBatch.DrawString(spriteFont, "TIME", new Vector2(_timerPosition.X - 14, _timerPosition.Y - 32), color);
        }

        private void DrawCoinCounter(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont,_coinCounter.ToString(), _coinCounterPosition, Color.White);
        }

        private void DrawPoints(SpriteBatch spriteBatch)
        {
            var zeros = "";

            if (_points < 100)
                zeros = "000000";
            else if (_points < 1000)
                zeros = "0000";
            else if (_points < 10000)
                zeros = "0000";
            else if (_points < 100000)
                zeros = "000";
            else if (_points < 1000000)
                zeros = "00";
            else if (_points < 10000000)
                zeros = "0";

            var text = zeros + _points; // Should be in update method?
            spriteBatch.DrawString(spriteFont, text, _scorePosition, Color.White);
        }

        public void AddPoint(int number)
        {
            _points += number;
        }

        public void AddCoin()
        {
            _coinCounter++;
        }

        public void AddPointWithFloatingNumber(int number, Vector2 positionToDraw)
        {
            _points += number;

            floatingScoreTexts.Add(new FloatingScoreText(number, positionToDraw));
        }

        public void FollowCamera(Vector2 centre)
        {
            if (_scorePosition.X < centre.X)
            {
                _scorePosition.X += centre.X - _scorePosition.X - 360; // Hardcoded
                _gameTextPosition.X += centre.X - _gameTextPosition.X - 360;
                _coinCounterPosition.X += centre.X - _coinCounterPosition.X - 160;
                _timerPosition.X += centre.X - _timerPosition.X + 280;
            }
        }

        public void SetScoreStartingPosition()
        {
            _timer = 120; //hardcoded
            _timerPosition = new Vector2(0,40);
            _scorePosition = new Vector2(10,40);
            _gameTextPosition = new Vector2(20,8);
            _coinCounterPosition = new Vector2(0,40);
        }

        public void EndLevel()
        {
            _calculateScoreAnimation = true;
        }
    }

    class FloatingScoreText
    {
        
        private int number;
        private Vector2 position;
        private SpriteFont spriteFont;
        public bool IsDone { get; set; }
        private float timeSinceStart;

        public FloatingScoreText(int number, Vector2 position)
        {
            this.number = number;
            this.position = position;
            this.spriteFont = Content_Manager.GetInstance().SpriteFonts["FloatingScoreFont"];
            this.IsDone = false;
        }

        public void Update(GameTime gameTime)
        {
            timeSinceStart += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            position.Y -= 1.2f;

            if (timeSinceStart > 800)
                IsDone = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, number.ToString(), position, Color.White);
        }
    }
}
