using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappahBird
{
    public class TubeManager
    {
        private readonly Texture2D upperTubeTexture;
        private readonly Texture2D lowerTubeTexture;
        private List<Tube> tubeList = new List<Tube>(); 

        public TubeManager(Texture2D upperTubeTexture, Texture2D lowerTubeTexture)
        {
            this.upperTubeTexture = upperTubeTexture;
            this.lowerTubeTexture = lowerTubeTexture;

            CreateTubes();
        }

        public IEnumerable<Tube> TubeList
        {
            get { return tubeList; }
        } 

        private void CreateTubes()
        {
            var position = new Vector2(500, -250);
            var upperTube = new Tube(upperTubeTexture, position);
            tubeList.Add(upperTube);

            position = new Vector2(500, 250);
            var lowerTube = new Tube(lowerTubeTexture, position);
            tubeList.Add(lowerTube);


            var rand = new Random();
            var randomHeight = rand.Next(205, 355);

            position = new Vector2(660, -randomHeight);
            var upperTube2 = new Tube(upperTubeTexture, position);
            tubeList.Add(upperTube2);

            var fittedHeight = FindFittedNumber(-randomHeight);

            position = new Vector2(660, fittedHeight);
            var lowerTube2 = new Tube(lowerTubeTexture, position);
            tubeList.Add(lowerTube2);

            randomHeight = rand.Next(205, 355);

            position = new Vector2(820, -randomHeight);
            var upperTube3 = new Tube(upperTubeTexture, position);
            tubeList.Add(upperTube3);

            fittedHeight = FindFittedNumber(-randomHeight);

            position = new Vector2(820, fittedHeight);
            var lowerTube3 = new Tube(lowerTubeTexture, position);
            tubeList.Add(lowerTube3);

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tube in tubeList)
            {
                tube.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < tubeList.Count; i++)
            {
                tubeList[i].Update(gameTime);

                if (tubeList[i].position.X < -55)
                {
                    tubeList.RemoveAt(i);
                    tubeList.RemoveAt(i);
                    i++;

                    var rand = new Random();
                    var randomHeight = rand.Next(205, 355);

                    var position = new Vector2(430, -randomHeight);
                    var upperTube = new Tube(upperTubeTexture, position);
                    tubeList.Add(upperTube);

                    var fittedHeight = FindFittedNumber(-randomHeight);

                    position = new Vector2(430, fittedHeight);
                    var lowerTube = new Tube(lowerTubeTexture, position);
                    tubeList.Add(lowerTube); 
                }

                tubeList[i].BoundingBox = new Rectangle((int)tubeList[i].position.X + 5, (int)tubeList[i].position.Y, 52, 410);
            }
        }

        private int FindFittedNumber(int number)
        {
            var result = 0;
            for (int i = 0; i < 500; i++)
            {
                if (number - i == -500)
                {
                    result = i;
                    return result;
                }
            }
            return result;
        }

        public void StopTubes()
        {
            foreach (var tube in tubeList)
            {
                tube.Speed = 0;
            }
        }
    }
}