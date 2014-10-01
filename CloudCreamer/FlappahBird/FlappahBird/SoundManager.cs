using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace FlappahBird
{
    public class SoundManager
    {
        private Song backgroundMusic;
        private SoundEffect jumpSoundEffect;

        public SoundManager(ContentManager content)
        {
            backgroundMusic = content.Load<Song>(@"sounds\loop");
            jumpSoundEffect = content.Load<SoundEffect>(@"sounds\flap");
        }

        public void PlayBackgroundMusic()
        {
            if (MediaPlayer.GameHasControl)
            {
                MediaPlayer.Play(backgroundMusic);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.1f;
            }
        }

        public void PlayJumpEffect()
        {
            jumpSoundEffect.Play();
        }
    }
}