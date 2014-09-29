using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CloudCreamer
{
    public class PlayerCloud : Sprite
    {
        public PlayerCloud(Texture2D texture, Vector2 position, Rectangle movementBounds)
            : base(texture, position, movementBounds)
        {
            Speed = 300;
        }

        public override void Update(KeyboardState keyboardState, GameTime gameTime)
        {
            UpdateVelocity(keyboardState);
            base.Update(keyboardState, gameTime);
        }

        private void UpdateVelocity(KeyboardState keyboardState)
        {
            var keyDictionary = new Dictionary<Keys, Vector2>
                                    {
                                        { Keys.Left, new Vector2(-1, 0) },
                                        { Keys.Right, new Vector2(1, 0) },
                                        { Keys.Up, new Vector2(0, -1) },
                                        { Keys.Down, new Vector2(0, 1) },
                                    };

            var velocity = Vector2.Zero;

            foreach (var key in keyDictionary)
            {
                if (keyboardState.IsKeyDown(key.Key))
                    velocity += key.Value;
            }

            if (velocity != Vector2.Zero)
                velocity.Normalize(); // If its a diagonal movement it wont be faster

            Velocity = velocity * Speed;
        }
    }
}
