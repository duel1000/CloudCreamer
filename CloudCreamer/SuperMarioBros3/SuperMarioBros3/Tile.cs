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

        public void Draw(SpriteBatch spriteBatch)
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
        public BrickTile(Rectangle newRectangle, bool turnsToHardTile = false)
        {
            texture = Content_Manager.GetInstance().Textures["brick"];
            this.Rectangle = newRectangle;
            BoundingBox = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height - 10);
            this.TurnsToHardTile = turnsToHardTile;
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
        }
    }
}
