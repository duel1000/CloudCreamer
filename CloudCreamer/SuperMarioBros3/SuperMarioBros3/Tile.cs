using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    public class Tile
    {
        protected Texture2D texture;

        public Rectangle Rectangle { get; protected set; }
        public Rectangle BoundingBox { get; protected set; }

        public string Status = "Alive";
        public bool TurnsToHardTile = false;
        public bool IsPunched;

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, Color.White);
        }
    }

    class EarthTile : Tile
    {
        public EarthTile(Rectangle newRectangle)
        {
            texture = Content_Manager.GetInstance().Textures["singleEarthBlock"];
            this.Rectangle = newRectangle;
            BoundingBox = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height - 10);
        }
    }

    public class BrickTile : Tile
    {
        public string IsContaining { get; set; }
        
        public BrickTile(Vector2 position, bool turnsToHardTile = false, string isContaining = "", bool containsCoin = false)
        {
            texture = Content_Manager.GetInstance().Textures["brick"];
            this.Rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Height, texture.Width);
            BoundingBox = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height - 10);
            this.TurnsToHardTile = turnsToHardTile;
            this.IsContaining = isContaining;
        }
    }

    public class HardTile : Tile
    {
        private Vector2 _startingPosition;
        private float _velocity;
        private float _newPositionY;
        private bool _animationPlayedOnce = false;

        public HardTile(Tile oldTile)
        {
            texture = Content_Manager.GetInstance().Textures["hardbrick"];
            BoundingBox = oldTile.BoundingBox;
            TurnsToHardTile = false;

            _startingPosition = new Vector2(oldTile.Rectangle.X, oldTile.Rectangle.Y);

            this.Rectangle = new Rectangle(oldTile.Rectangle.X, oldTile.Rectangle.Y - 5, oldTile.Rectangle.Width, oldTile.Rectangle.Height); 
            _velocity -= 5f;
            _newPositionY = Rectangle.Y;
        }

        public void Update(GameTime gameTime)
        {
            if (!_animationPlayedOnce) // No reason to update still standing bricks all the time
            {
                IsPunched = true;
                Rectangle = new Rectangle(Rectangle.X, (int)_newPositionY, Rectangle.Width, Rectangle.Height);

                if (Rectangle.Y < _startingPosition.Y)
                {
                    _newPositionY += _velocity;
                    _velocity += 2.5f;
                }
                else
                {
                    _velocity = 0f;
                    _newPositionY = _startingPosition.Y;
                    _animationPlayedOnce = true;
                    Rectangle = new Rectangle(Rectangle.X, (int)_newPositionY, Rectangle.Width, Rectangle.Height);
                }
            }
            else
            {
                IsPunched = false;
            }
        }
    }

    public class QuestionMarkTile : Tile
    {
        public string IsContaining { get; set; }
        public bool ContainsCoin { get; set; }
        private float _timeSinceColorChange = 0;
        private int _frameWidth = 50;
        private int _currentFrame = 0;

        public QuestionMarkTile(Rectangle newRectangle, bool turnsToHardTile = false, string isContaining = "", bool containsCoin = false)
        {
            texture = Content_Manager.GetInstance().Textures["questionmarktile"];
            this.Rectangle = newRectangle;
            this.TurnsToHardTile = turnsToHardTile;
            this.IsContaining = isContaining;
            this.BoundingBox = newRectangle;
            this.ContainsCoin = containsCoin;
        }

        public void Update(GameTime gameTime)
        {
            _timeSinceColorChange += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_timeSinceColorChange < 300)
                _currentFrame = 0;
            else if (_timeSinceColorChange < 500)
                _currentFrame = 1;
            else if (_timeSinceColorChange < 700)
                _currentFrame = 2;
            else if (_timeSinceColorChange < 900)
                _currentFrame = 1;
            else if(_timeSinceColorChange < 1200)
                _timeSinceColorChange = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, Color.White);

            var DestinationRectangle = new Rectangle(Rectangle.X, Rectangle.Y, 50,50);
            var SourceRectangle = new Rectangle(50 * _currentFrame, 0, 50,50);
            spriteBatch.Draw(texture,
                             DestinationRectangle,
                             SourceRectangle,
                             Color.White,
                             0,
                             new Vector2(0,0), 
                             SpriteEffects.None,
                             1);
        }
    }
}
