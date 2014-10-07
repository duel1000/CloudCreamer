using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros2
{
    public class EarthManager
    {
        private readonly Texture2D texture;
        private readonly List<EarthBlock> earthBlockList = new List<EarthBlock>(); 

        public EarthManager(Texture2D texture)
        {
            this.texture = texture;
            CreateEarths();
        }

        public IEnumerable<EarthBlock> EarthList
        {
            get { return earthBlockList; }
        }

        private void CreateEarths()
        {
            for (var i = 0; i < 500; i++)
            {
                var earthToAdd = new EarthBlock(texture, new Vector2(i * 42, 600 - 65));
                earthBlockList.Add(earthToAdd);
            }
            earthBlockList.RemoveAt(15);
            earthBlockList.RemoveAt(15);
            earthBlockList.RemoveAt(15);
            earthBlockList.RemoveAt(15);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var earthBlock in earthBlockList)
            {
                earthBlock.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var earthBlock in earthBlockList)
            {
                earthBlock.Draw(spriteBatch);
            }
        }
    }
}