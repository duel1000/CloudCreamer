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

            position = new Vector2(660, -300);
            var upperTube2 = new Tube(upperTubeTexture, position);
            tubeList.Add(upperTube2);

            position = new Vector2(660, 200);
            var lowerTube2 = new Tube(lowerTubeTexture, position);
            tubeList.Add(lowerTube2);

            position = new Vector2(820, -175);
            var upperTube3 = new Tube(upperTubeTexture, position);
            tubeList.Add(upperTube3);

            position = new Vector2(820, 325);
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
            foreach (var tube in tubeList)
            {
                tube.Update(gameTime);

                
            }
        }
    }
}