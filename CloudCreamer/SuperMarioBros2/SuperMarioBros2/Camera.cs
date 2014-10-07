using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros2
{
    public class Camera
    {
        public Matrix transform;
        private Viewport view;
        private Vector2 centre;

        public Camera(Viewport newView)
        {
            view = newView;
        }

        public void Update(GameTime gameTime, Mario mario)
        {
            centre = new Vector2(mario.position.X + (mario.BoundingBox.Width / 2) - 300, 0);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                        Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));

        }
    }
}
