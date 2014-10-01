using Microsoft.Xna.Framework;

namespace FlappahBird
{
    public  class CollisionManager
    {
        private readonly PlayerBird flappy;
        private readonly EarthManager earthManager;
        private readonly TubeManager tubeManager;

        public CollisionManager(PlayerBird flappy, EarthManager earthManager, TubeManager tubeManager)
        {
            this.flappy = flappy;
            this.earthManager = earthManager;
            this.tubeManager = tubeManager;
        }

        public void Update(GameTime gameTime)
        {
            CheckCollisions();
        }

        private void CheckCollisions()
        {
            CheckFlappyToEarth();
            CheckFlappyToTubes();
        }

        private void CheckFlappyToTubes()
        {
            foreach (var tube in tubeManager.TubeList)
            {
                //if (tube.BoundingBox.Intersects(flappy.BoundingBox))
                //{
                //    flappy.Hit();
                //}
            }
        }

        private void CheckFlappyToEarth()
        {
            foreach (var earth in earthManager.EarthList)
            {
                //if (earth.BoundingBox.Intersects(flappy.BoundingBox))
                //{
                //    flappy.Hit();
                //}
            }
        }
    }
}