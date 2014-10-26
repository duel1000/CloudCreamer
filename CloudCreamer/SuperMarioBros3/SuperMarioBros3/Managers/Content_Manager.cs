using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros3.Managers
{
    class Content_Manager
    {
        private static Content_Manager _instance;
        private ContentManager CM;

        public Dictionary<String, Texture2D> Textures;
        public Dictionary<String, SpriteFont> SpriteFonts; 

        private Content_Manager()
        {
            Textures = new Dictionary<String, Texture2D>();
            SpriteFonts = new Dictionary<String, SpriteFont>();
        }

        public static Content_Manager GetInstance()
        {
            return _instance ?? (_instance = new Content_Manager());
        }

        public void LoadTextures(ContentManager content)
        {
            CM = content;
            AddTexture("oldmario");
            AddTexture("smallstillmario");
            AddTexture("smalljumpmario");
            AddTexture("bigmariowalking");
            AddTexture("bigmariostanding");
            AddTexture("bigmariojumping");
            AddTexture("spelunkyMan");
            AddTexture("mario");
            AddTexture("brick");
            AddTexture("death");
            AddTexture("coinspin");
            AddTexture("destroyedbrick");
            AddTexture("singleEarthBlock");
            AddTexture("hardbrick");
            AddTexture("questionmarktile");
            AddTexture("evilmushroom");
            AddTexture("mushroompowerup");
            AddTexture("tube");
            AddTexture("hardearthtile");
            AddTexture("castle");
            AddTexture("flagpole");
            AddTexture("smallflagpolemario");
            AddTexture("bigmariopole");
            AddTexture("fireflower");
            AddTexture("fireflowerpowerupanimation");
            AddTexture("firemariostanding");
            AddTexture("firemariowalking");
            AddTexture("firemariojumping");
            AddTexture("fireball");

            AddSpriteFont("PointsFont");
        }

        public void AddTexture(String file, String name = "")
        {
            var newTexture = CM.Load<Texture2D>(file);
            
            if(name == "")
                Textures.Add(file, newTexture);
            else
                Textures.Add(name, newTexture);
        }

        public void AddSpriteFont(String file)
        {
            var newSpriteFont = CM.Load<SpriteFont>(file);

            SpriteFonts.Add(file, newSpriteFont);
        }
    }
}
