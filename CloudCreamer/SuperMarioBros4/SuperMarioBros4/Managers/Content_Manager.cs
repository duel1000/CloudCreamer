﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros4.Managers
{
    class Content_Manager
    {
        private static Content_Manager _instance;
        private ContentManager CM;

        public Dictionary<String, Texture2D> Textures;

        private Content_Manager()
        {
            Textures = new Dictionary<String, Texture2D>();
        }

        public static Content_Manager GetInstance()
        {
            return _instance ?? (_instance = new Content_Manager());
        }

        public void LoadTextures(ContentManager content)
        {
            CM = content;
            AddTexture("spelunkyMan");
        }

        public void AddTexture(String file, String name = "")
        {
            var newTexture = CM.Load<Texture2D>(file);
            
            if(name == "")
                Textures.Add(file, newTexture);
            else
                Textures.Add(name, newTexture);
        }
    }
}
