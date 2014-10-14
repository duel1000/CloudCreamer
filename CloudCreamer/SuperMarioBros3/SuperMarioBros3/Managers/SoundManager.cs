using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace SuperMarioBros3
{
    public class SoundManager
    {
        //private Song backgroundMusic;
        private SoundEffect brickExplosion;

        public SoundManager(ContentManager content)
        {
            brickExplosion = content.Load<SoundEffect>(@"Sounds\brickexplosion");
        }

        //public void PlayBackgroundMusic()
        //{
        //    if (MediaPlayer.GameHasControl)
        //    {
        //        MediaPlayer.Play(backgroundMusic);
        //        MediaPlayer.IsRepeating = true;
        //        MediaPlayer.Volume = 0.1f;
        //    }
        //}

        public void brickExplosionEffect()
        {
            brickExplosion.Play();
        }
    }
}