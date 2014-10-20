using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    class Score
    {
        private int _points;
        private int _coinCounter;

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

        private readonly SpriteFont spriteFont;
        
        public Score(SoundManager soundManager)
        {
            spriteFont = Content_Manager.GetInstance().SpriteFonts["PointsFont"];
            this.soundManager = soundManager;
        }

        public void Update(GameTime gameTime)
        {
            _elapsedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            _elapsedGameTimeForColorChange += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_elapsedGameTime > 1000)
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawPoints(spriteBatch);
            spriteBatch.DrawString(spriteFont, "MARIO", _gameTextPosition, Color.White);
            DrawCoinCounter(spriteBatch);
            DrawTimer(spriteBatch);
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
    }
}
