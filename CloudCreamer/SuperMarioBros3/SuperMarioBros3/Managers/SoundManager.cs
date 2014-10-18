using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace SuperMarioBros3
{
    public class SoundManager
    {
        private SoundEffect brickExplosion;
        private SoundEffect hardBrickBump;
        private SoundEffect stomp;
        private SoundEffect smallJump;
        private SoundEffect _powerUp;
        private SoundEffect _powerUpAppear;
        private SoundEffect _powerDown;
        private SoundEffect _playerDeath;
        private Song backgroundMusic;

        public SoundManager(ContentManager content)
        {
            brickExplosion = content.Load<SoundEffect>(@"Sounds\brickexplosion");
            hardBrickBump = content.Load<SoundEffect>(@"Sounds\hardbrickbump");
            stomp = content.Load<SoundEffect>(@"Sounds\stomp");
            smallJump = content.Load<SoundEffect>(@"Sounds\smalljump");
            _powerUp = content.Load<SoundEffect>(@"Sounds\powerup");
            _powerUpAppear = content.Load<SoundEffect>(@"Sounds\powerupappear");
            _powerDown = content.Load<SoundEffect>(@"Sounds\powerdown");
            _playerDeath = content.Load<SoundEffect>(@"Sounds\mariodeath");
            backgroundMusic = content.Load<Song>(@"Sounds\theme");
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

        public void BrickExplosionEffect()
        {
            brickExplosion.Play();
        }
        public void HardBrickBumpEffect()
        {
            hardBrickBump.Play();
        }
        public void StompEffect()
        {
            stomp.Play();
        }
        public void SmallJumpEffect()
        {
            smallJump.Play();
        }
        public void PowerUpEffect()
        {
            _powerUp.Play();
        }
        public void PowerUpAppearEffect()
        {
            _powerUpAppear.Play();
        }
        public void PowerDown()
        {
            _powerDown.Play();
        }

        public void PlayerDeath()
        {
            _playerDeath.Play();
        }
    }
}