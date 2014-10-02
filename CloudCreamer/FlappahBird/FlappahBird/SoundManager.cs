using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace FlappahBird
{
    public class SoundManager
    {
        private Song backgroundMusic;
        private SoundEffect jumpSoundEffect;
        private SoundEffect hitSoundEffect;
        private SoundEffect scorePointEffect;

        public SoundManager(ContentManager content)
        {
            backgroundMusic = content.Load<Song>(@"sounds\loop");
            jumpSoundEffect = content.Load<SoundEffect>(@"sounds\flap");
            hitSoundEffect = content.Load<SoundEffect>(@"sounds\hitSound");
            scorePointEffect = content.Load<SoundEffect>(@"sounds\point");
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

        public void PlayHitEffect()
        {
            hitSoundEffect.Play();
        }

        public void PlayScorePointEffect()
        {
            scorePointEffect.Play();
        }
    }
}