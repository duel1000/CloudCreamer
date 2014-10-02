using Microsoft.Xna.Framework;

namespace FlappahBird
{
    public  class CollisionManager
    {
        private readonly PlayerBird playerBird;
        private readonly EarthManager earthManager;
        private readonly TubeManager tubeManager;
        private readonly PointManager pointManager;

        public CollisionManager(PlayerBird playerBird, EarthManager earthManager, TubeManager tubeManager, PointManager pointManager)
        {
            this.playerBird = playerBird;
            this.earthManager = earthManager;
            this.tubeManager = tubeManager;
            this.pointManager = pointManager;
        }

        public void Update(GameTime gameTime)
        {
            CheckCollisions(gameTime);
        }

        private void CheckCollisions(GameTime gameTime)
        {
            CheckFlappyToEarth();
            CheckFlappyToHeaven();
            CheckFlappyToTubesAndPoints(gameTime);
        }

        private void CheckFlappyToHeaven()
        {
            if (playerBird.position.Y < -50)
            {
                playerBird.Hit();
                tubeManager.StopTubes();
                earthManager.StopEarth();
            }
        }

        private void CheckFlappyToEarth()
        {
            foreach (var earth in earthManager.EarthList)
            {
                if (earth.BoundingBox.Intersects(playerBird.BoundingBox))
                {
                    playerBird.Hit();
                    tubeManager.StopTubes();
                    earthManager.StopEarth();
                }
            }
        }

        private void CheckFlappyToTubesAndPoints(GameTime gameTime)
        {
            foreach (var tube in tubeManager.TubeList)
            {
                if (tube.BoundingBox.Intersects(playerBird.BoundingBox))
                {
                    playerBird.Hit();
                    tubeManager.StopTubes();
                    earthManager.StopEarth();
                }
                if (tube.position.X < 26 && tube.position.X > 23)
                {
                    pointManager.ScorePoint(gameTime);
                }
            }
        }

        
    }
}