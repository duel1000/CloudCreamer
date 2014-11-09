using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperMarioBros3.Managers;

namespace SuperMarioBros3
{
    public class Player : SpriteAnimation
    {
        private int _hitPoints;
        private bool hasJumped = false;
        private bool walking = false;
        private bool jumpKeyReleased;
        private float _powerUpAnimationTimer = 0;
        private float _powerDownAnimationTimer = 0;

        private bool _isDead;
        private bool _isBigMario = false;
        private bool _isSmallMario = true;
        private bool _isFireMario;
        private bool isRunning;
        public bool IsInvulnerable;
        private bool _powerUpAnimationPlayed = false;
        private bool _powerDownAnimationPlayed = false;
        private bool _DeathAnimationPlayed = false;

        private bool _fireFlowerAnimationSetup;

        public bool OnTheFlagPole;
        private bool _flagPoleAnimationSetup;
        private bool _walkIntoCastleAnimationSetup;

        private bool _frozen = false;
        private int _stepsIntoPowerUpAnimation = 0;
        private int _stepsIntoPowerDownAnimation = 0;
        private bool _powerDownFromFireMario;

        private SoundManager _soundManager;
        private bool _takeDamageSoundPlayed;
        private bool _killPlayerSoundPlayed;

        public bool LevelComplete;
        public bool RespawnPlayer { get; set; }

        private bool starMode;
        private float timeInStarMode;
        private float timeSinceStarModeColorChange;

        private int _fireBallCount;
        private float _timeSinceLastFireball;
        private List<FireBall> _fireBalls; 
        public List<FireBall> FireBalls{get { return _fireBalls; }} 

        public bool IsBigMario{get { return _isBigMario; }}

        //hardcoded values for spritesheets
        private int currentSpriteHeight;
        private int currentSpriteWidth;

        public Player(SoundManager soundManager) : base("smallstillmario", new Vector2(64,384),1,3,1) // 64
        {
            this._soundManager = soundManager;
            _fireBalls = new List<FireBall>();
            _hitPoints = 1;
        }

        public override void Update(GameTime gameTime)
        {
            

            for (int i = 0; i < _fireBalls.Count; i++)
            {
                _fireBalls[i].Update(gameTime);

                if (_fireBalls[i].IsDead || _fireBalls[i].Exploded)
                {
                    _fireBalls.Remove(_fireBalls[i]);
                    _fireBallCount--;
                    i--;
                }
            }

            if (OnTheFlagPole)
            {
                RunFlagPoleAnimation();
                base.Update(gameTime);
                return;
            }

            if (starMode)
            {
                timeInStarMode += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                timeSinceStarModeColorChange += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timeInStarMode < 12000)
                {
                    if (timeSinceStarModeColorChange < 50)
                        colorOverlay = Color.Red;
                    else if (timeSinceStarModeColorChange < 100)
                        colorOverlay = Color.Green;
                    else if (timeSinceStarModeColorChange < 150)
                        colorOverlay = Color.Black;
                    else if (timeSinceStarModeColorChange < 200)
                    {
                        colorOverlay = Color.White;
                        timeSinceStarModeColorChange = 0;
                    }
                }
                else if (timeInStarMode > 12000)
                {
                    colorOverlay = Color.White;
                    starMode = false;
                    timeInStarMode = 0;
                    _soundManager.PlayBackgroundMusic();
                }
            }

            if (_isFireMario && !_powerUpAnimationPlayed)
            {
                FireMarioPowerUpAnimation(gameTime);
                BoundingBox = new Rectangle(0, 0, 0, 0);
                base.Update(gameTime);
                return;
            }
            if (_isBigMario && !_powerUpAnimationPlayed)
            {
                PowerUpAnimation(gameTime);
                BoundingBox = new Rectangle(0,0,0,0);
            }
            else if(_isSmallMario && !_powerDownAnimationPlayed && IsInvulnerable)
            {
                PowerDownAnimation(gameTime);
                BoundingBox = new Rectangle(0, 0, 0, 0);
            }
            else if(_isDead)
                KillPlayer();
            else if(!_frozen && !_isDead)
            {
                position += velocity;

                Input(gameTime);

                if (velocity.Y < 10)
                    velocity.Y += 0.45f;

                DestinationRectangle = new Rectangle((int)position.X, (int)position.Y, imageWidth, imageHeight);

                
            }

            SetupBoundingBox();

            if (Keyboard.GetState().IsKeyDown(Keys.O) && _isDead)
            {
                _isDead = false;
                RespawnPlayer = true;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (LevelComplete)
                return;

            foreach (var fireball in _fireBalls)
            {
                fireball.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }

        private void SetupBoundingBox()
        {
            if (_isSmallMario)
            {
                currentSpriteHeight = 45;
                currentSpriteWidth = 40;
                BoundingBox = new Rectangle(DestinationRectangle.X = flipSprite ? DestinationRectangle.X - 10 : DestinationRectangle.X,
                                            DestinationRectangle.Y, 
                                            currentSpriteWidth, 
                                            currentSpriteHeight);
            }
            else if(_isBigMario || _isFireMario)
            {
                currentSpriteHeight = 70;
                currentSpriteWidth = 40;
                BoundingBox = new Rectangle(DestinationRectangle.X, DestinationRectangle.Y, currentSpriteWidth, currentSpriteHeight);
            }
        }

        private void Input(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                PowerUp();
                _isFireMario = true;
            }

            _timeSinceLastFireball += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_isFireMario && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _timeSinceLastFireball > 108 && _fireBallCount < 2)
            {
                var fireballPosition = flipSprite ? new Vector2(position.X - 24, position.Y + 36) : new Vector2(position.X + 24, position.Y + 36);
                _fireBalls.Add(new FireBall(fireballPosition, flipSprite)); //Hardcoded
                _timeSinceLastFireball = 0;
                _fireBallCount++;
                _soundManager.ShootFireball();

                //  ThrowFireBallAnimation();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    isRunning = true;
                    velocity.X = (float) gameTime.ElapsedGameTime.TotalMilliseconds/2;
                }
                else
                {
                    isRunning = false;
                    velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
                }
                
                flipSprite = false;
                WalkAnimation();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    isRunning = true;
                    velocity.X = -(float) gameTime.ElapsedGameTime.TotalMilliseconds/2;
                }
                else
                {
                    isRunning = false;
                    velocity.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
                }
                    
                flipSprite = true;
                WalkAnimation();
            }

            else
            {
                isRunning = false;
                StopWalkAnimation();
                velocity.X = 0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && hasJumped == false && jumpKeyReleased && velocity.Y <= 0)
            {
                if(_isSmallMario)
                    texture = Content_Manager.GetInstance().Textures["smalljumpmario"];
                else if(_isBigMario && !_isFireMario)
                    texture = Content_Manager.GetInstance().Textures["bigmariojumping"];
                else if (_isFireMario)
                    texture = Content_Manager.GetInstance().Textures["firemariojumping"];

                currentFrame = 1; 
                rows = 1;
                columns = 1;
                framesPerSecond = 0;
                endFrame = 1;

                _soundManager.SmallJumpEffect();

                if (isRunning)
                    velocity.Y = -15.2f;
                else
                    velocity.Y = -13.2f;

                position.Y -= 5f;
                walking = false;
                hasJumped = true;
                jumpKeyReleased = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                if(!jumpKeyReleased && velocity.Y < -4)
                    velocity.Y = -2.5f;
                jumpKeyReleased = true;
                
            }
        }

        private void PowerUp()
        {
            if (!_isBigMario)
            {
                _hitPoints = 2;
                _powerUpAnimationPlayed = false;
                _soundManager.PowerUpEffect();
                _isBigMario = true;
                _isSmallMario = false;
                position.Y -= 30;
            }
        }

        private void FireFlowerPowerUp()
        {
            if (!_isFireMario)
            {
                _hitPoints = 2;
                _powerUpAnimationPlayed = false;
                _soundManager.PowerUpEffect();
                _isBigMario = true;
                _isFireMario = true;
                _isSmallMario = false;
            }
        }

        private void PowerDown()
        {
            if (!_takeDamageSoundPlayed)
            {
                IsInvulnerable = true;
                _soundManager.PowerDown();
                _takeDamageSoundPlayed = true;

                if (_isFireMario)
                    _powerDownFromFireMario = true;

                _isBigMario = false;
                _isSmallMario = true;
                _isFireMario = false;
                _powerUpAnimationPlayed = false;
            }
        }

        public void TakeDamage()
        {
            if(!IsInvulnerable)
                _hitPoints--;

            if (_hitPoints == 0)
                _isDead = true;
            else if (_hitPoints == 1)
                PowerDown();
        }

        public void KillPlayer()
        {
            if (!_killPlayerSoundPlayed)
            {
                _soundManager.PlayerDeath();
                _killPlayerSoundPlayed = true;
            }
            RunDeathAnimation();
        }

        private void RunDeathAnimation()
        {
            if (!_DeathAnimationPlayed)
            {
                texture = Content_Manager.GetInstance().Textures["death"];
                rows = 1;
                framesPerSecond = 0;
                columns = 1;
                currentFrame = 1;
                endFrame = 1;
                velocity.X = (velocity.X *-1)/2;
                position.Y -= 15;
                velocity.Y -= 10f;
                _DeathAnimationPlayed = true;
            }

            if (position.Y < 1000)
            {
                position += velocity;

                if (velocity.Y < 10)
                    velocity.Y += 0.45f;
            }
            else
            {
                position = position;
            }
        }

        private void PowerUpAnimation(GameTime gameTime)
        {
            _powerUpAnimationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _frozen = true;
            walking = false;
            currentFrame = 1;
            rows = 1;
            columns = 1;
            framesPerSecond = 0;
            endFrame = 1;

            if (_powerUpAnimationTimer < 200)
            {
                if (_stepsIntoPowerUpAnimation == 0)
                {
                    position.Y -= 25; //Hardcoded value for big mario height
                    _stepsIntoPowerUpAnimation++;
                }
                texture = Content_Manager.GetInstance().Textures["bigmariostanding"];
            }
            else if (_powerUpAnimationTimer < 400)
            {
                if (_stepsIntoPowerUpAnimation == 1)
                {
                    position.Y += 25;
                    _stepsIntoPowerUpAnimation++;
                }
                texture = Content_Manager.GetInstance().Textures["smallstillmario"];
            }
            else if (_powerUpAnimationTimer < 600)
            {
                if (_stepsIntoPowerUpAnimation == 2)
                {
                    position.Y -= 25;
                    _stepsIntoPowerUpAnimation++;
                }
                texture = Content_Manager.GetInstance().Textures["bigmariostanding"];
            }
            else if (_powerUpAnimationTimer < 800)
            {
                if (_stepsIntoPowerUpAnimation == 3)
                {
                    position.Y += 25;
                    _stepsIntoPowerUpAnimation++;
                }
                texture = Content_Manager.GetInstance().Textures["smallstillmario"];
            }
            else if (_powerUpAnimationTimer < 1000)
            {
                if (_stepsIntoPowerUpAnimation == 4)
                {
                    position.Y -= 25;
                    _stepsIntoPowerUpAnimation++;
                }
                texture = Content_Manager.GetInstance().Textures["bigmariostanding"];
            }
            else if (_powerUpAnimationTimer < 1200)
            {
                _frozen = false;
                _powerUpAnimationPlayed = true;
                _powerUpAnimationTimer = 0;
            }
        }

        private void FireMarioPowerUpAnimation(GameTime gameTime)
        {
            _powerUpAnimationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (!_fireFlowerAnimationSetup)
            {
                texture = Content_Manager.GetInstance().Textures["fireflowerpowerupanimation"];

                _frozen = true;
                walking = false;
                currentFrame = 1;
                rows = 1;
                columns = 4;
                framesPerSecond = 20;
                endFrame = 4;
                _fireFlowerAnimationSetup = true;
            }

            if (_powerUpAnimationTimer > 600)
            {
                _powerUpAnimationTimer = 0;
                _powerUpAnimationPlayed = true;
                _frozen = false;
                StopWalkAnimation();
            }

        }

        private void PowerDownAnimation(GameTime gameTime)
        {
            _powerDownAnimationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _frozen = true;
            walking = false;
            currentFrame = 1;
            rows = 1;
            columns = 1;
            framesPerSecond = 0;
            endFrame = 1;

            var textureToUse = _powerDownFromFireMario ? "firemariostanding" : "bigmariostanding"; //Sorta hardcoded

            if (_powerDownAnimationTimer < 200)
            {
                if (_stepsIntoPowerDownAnimation == 0)
                {
                    position.Y += 25; //Hardcoded value for big mario height
                    _stepsIntoPowerDownAnimation++;
                }
                texture = Content_Manager.GetInstance().Textures["smallstillmario"];
            }
            else if (_powerDownAnimationTimer < 400)
            {
                if (_stepsIntoPowerDownAnimation == 1)
                {
                    position.Y -= 25;
                    _stepsIntoPowerDownAnimation++;
                }
                texture = Content_Manager.GetInstance().Textures[textureToUse];
            }
            else if (_powerDownAnimationTimer < 600)
            {
                if (_stepsIntoPowerDownAnimation == 2)
                {
                    position.Y += 25;
                    _stepsIntoPowerDownAnimation++;
                }
                texture = Content_Manager.GetInstance().Textures["smallstillmario"];
            }
            else if (_powerDownAnimationTimer < 800)
            {
                if (_stepsIntoPowerDownAnimation == 3)
                {
                    position.Y -= 25;
                    _stepsIntoPowerDownAnimation++;
                }
                texture = Content_Manager.GetInstance().Textures[textureToUse];
            }
            else if (_powerDownAnimationTimer < 1000)
            {
                if (_stepsIntoPowerDownAnimation == 4)
                {
                    position.Y += 25;
                    _stepsIntoPowerDownAnimation++;
                }
                texture = Content_Manager.GetInstance().Textures["smallstillmario"];
            }
            else if (_powerDownAnimationTimer < 1200)
            {
                _frozen = false;
                _powerDownAnimationPlayed = true;
                _powerDownAnimationTimer = 0;
                IsInvulnerable = false;
                _powerDownFromFireMario = false;
            }
        }

        private void StopWalkAnimation()
        {
            if (!hasJumped)
            {
                if(_isSmallMario)
                    texture = Content_Manager.GetInstance().Textures["smallstillmario"];
                else if(_isBigMario && !_isFireMario)
                    texture = Content_Manager.GetInstance().Textures["bigmariostanding"]; 
                else if(_isFireMario)
                    texture = Content_Manager.GetInstance().Textures["firemariostanding"]; 
                
                walking = false;
                currentFrame = 1;
                rows = 1;
                columns = 1;
                framesPerSecond = 0;
                endFrame = 1;
            }
        }

        private void WalkAnimation()
        {
            if (walking == false && !hasJumped)
            {
                if(_isSmallMario)
                    texture = Content_Manager.GetInstance().Textures["oldmario"];
                else if(_isBigMario && !_isFireMario)
                    texture = Content_Manager.GetInstance().Textures["bigmariowalking"];
                else if(_isFireMario)
                    texture = Content_Manager.GetInstance().Textures["firemariowalking"];
                currentFrame = 1;
                rows = 1;
                columns = 3;
                framesPerSecond = 13;
                endFrame = 3;
                walking = true;
            }
        }

        public void SimpelCollision(Rectangle objectBoundingBox)
        {
            if (BoundingBox.TouchTopOf(objectBoundingBox))
            {
                position.Y = objectBoundingBox.Y - DestinationRectangle.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }
            else if (BoundingBox.TouchLeftOf(objectBoundingBox))
            {
                position.X = objectBoundingBox.X - DestinationRectangle.Width - 2;
            }
            else if (BoundingBox.TouchRightOf(objectBoundingBox))
            {
                position.X = objectBoundingBox.X + objectBoundingBox.Width + 2;
            }
        }

        public void Collision(Tile tile, int xOffset, int yOffset)
        {
            if (BoundingBox.TouchTopOf(tile.BoundingBox) && !_isDead)
            {
                if (velocity.Y > 0)
                {
                    position.Y = tile.BoundingBox.Y - currentSpriteHeight;
                    velocity.Y = 0f;
                    hasJumped = false;
                }
            }
            if (BoundingBox.TouchBottomOf(tile.BoundingBox))
            {
                if (velocity.Y < 0)
                {
                    tile.IsPunched = true;
                    velocity.Y = 2.5f;
                    if (tile.GetType().Name == "BrickTile")
                    {
                        if (tile.TurnsToHardTile && _isBigMario)
                            tile.Status = "TurnHard";
                        else if(_isBigMario)
                            tile.Status = "Destroy";
                    }
                    if (tile.GetType().Name == "QuestionMarkTile")
                    {
                        if (tile.TurnsToHardTile)
                        {
                            tile.Status = "TurnHard";
                        }
                            
                    }
                    if (tile.GetType().Name == "HiddenTile")
                    {
                        tile.IsPunched = true;
                    }
                }
            }
            else if (BoundingBox.TouchLeftOf(tile.BoundingBox))
            {
                position.X = tile.BoundingBox.X - currentSpriteWidth - 2;
            }
            else if (BoundingBox.TouchRightOf(tile.BoundingBox))
            {
                position.X = tile.BoundingBox.X + tile.BoundingBox.Width + 2;
            }

            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - currentSpriteWidth) position.X = xOffset - currentSpriteWidth;
            //if (position.Y < 0) velocity.Y = 1f; Makes mario able to jump out top of the map
            if (position.Y > yOffset && !_isDead)
                _isDead = true; //;position.Y = yOffset - DestinationRectangle.Height;
        }

        public void HiddenTileCollision(HiddenTile tile)
        {
            if (BoundingBox.TouchBottomOf(tile.BoundingBox) && velocity.Y < 0)
            {
                    tile.IsPunched = true;
                    velocity.Y = 2.5f;
                    _soundManager.PowerUpAppearEffect();
            }
        }

        public void EvilMushroomCollision(MushroomEnemy mushroom)
        {
            if(mushroom.IsDead)
                return;

            if (BoundingBox.TouchTopOf(mushroom.BoundingBox) && !mushroom.IsDead && velocity.Y > 0)
            {
                if (starMode)
                {
                    mushroom.IsDead = true;
                }
                else
                {
                    mushroom.SquishEnemy();
                    position.Y -= 3f;
                    velocity.Y = -6f;
                    hasJumped = true;
                }
            }
            else if(BoundingBox.TouchLeftOf(mushroom.BoundingBox) ||
                    BoundingBox.TouchRightOf(mushroom.BoundingBox) ||
                    BoundingBox.TouchBottomOf(mushroom.BoundingBox))
            {
                if (starMode)
                    mushroom.IsDead = true;
                else
                    TakeDamage();
            }
        }

        public void TurtleCollision(Turtle turtle)
        {
            if (BoundingBox.TouchTopOf(turtle.BoundingBox) && !turtle.IsDead && velocity.Y > 0)
            {
                if (starMode)
                {
                    turtle.IsDead = true;
                }
                else
                {
                    turtle.IsDead = true;
                    position.Y -= 3f;
                    velocity.Y = -6f;
                    hasJumped = true;
                }
            }
            else if (BoundingBox.TouchLeftOf(turtle.BoundingBox) ||
                    BoundingBox.TouchRightOf(turtle.BoundingBox) ||
                    BoundingBox.TouchBottomOf(turtle.BoundingBox))
            {
                if (starMode)
                    turtle.IsDead = true;
                else
                    TakeDamage();
            }
        }

        public void MushroomPowerUpCollision(MushroomPowerUp mushroomPowerUp)
        {
            if (BoundingBox.TouchTopOf(mushroomPowerUp.BoundingBox) ||
                BoundingBox.TouchLeftOf(mushroomPowerUp.BoundingBox) ||
                BoundingBox.TouchRightOf(mushroomPowerUp.BoundingBox) ||
                BoundingBox.TouchBottomOf(mushroomPowerUp.BoundingBox) && 
                !mushroomPowerUp.IsEaten)
            {
                mushroomPowerUp.IsEaten = true;
                PowerUp();
            }
        }

        public void FireflowerPowerUpCollision(FireFlower fireflower)
        {
            if (BoundingBox.TouchTopOf(fireflower.BoundingBox) ||
                BoundingBox.TouchLeftOf(fireflower.BoundingBox) ||
                BoundingBox.TouchRightOf(fireflower.BoundingBox) ||
                BoundingBox.TouchBottomOf(fireflower.BoundingBox) &&
                !fireflower.IsEaten)
            {
                fireflower.IsEaten = true;
                FireFlowerPowerUp();
            }
        }

        //resets all his stats
        public void Respawn()
        {
            texture = Content_Manager.GetInstance().Textures["smallstillmario"];
            
            position = new Vector2(64,384);

            ResetAllPlayerBooleans();

            _hitPoints = 1;
            rows = 1;
            framesPerSecond = 0;
            columns = 1;
            currentFrame = 1;
            endFrame = 1;
        }

        private void ResetAllPlayerBooleans()
        {
            flipSprite = false;
            RespawnPlayer = false;
            hasJumped = false;
            walking = false;
            jumpKeyReleased = true;
            _isDead = false;
            _isSmallMario = true;
            _isBigMario = false;
            _isFireMario = false;
            isRunning = false;
            IsInvulnerable = false;
            _powerDownAnimationPlayed = false;
            _powerUpAnimationPlayed = false;
            _DeathAnimationPlayed = false;
            _fireFlowerAnimationSetup = false;
            OnTheFlagPole = false;
            _flagPoleAnimationSetup = false;
            _walkIntoCastleAnimationSetup = false;
            _frozen = false;
            _stepsIntoPowerDownAnimation = 0;
            _stepsIntoPowerUpAnimation = 0;
            _powerDownFromFireMario = false;
            _takeDamageSoundPlayed = false;
            _killPlayerSoundPlayed = false;
            LevelComplete = false;
            starMode = false;
        }

        public void RunFlagPoleAnimation()
        {
            if (position.Y < 440 - texture.Height)
            {
                if (!_flagPoleAnimationSetup)
                {
                    _soundManager.FlagpoleEffect();
                    texture = _isBigMario ? Content_Manager.GetInstance().Textures["bigmariopole"] : Content_Manager.GetInstance().Textures["smallflagpolemario"];
                    
                    if(_isFireMario)
                        texture = Content_Manager.GetInstance().Textures["firemarioflagpole"];//Hardcoded somewhat

                    rows = 1;
                    columns = 2; 
                    currentFrame = 1;
                    framesPerSecond = 10;
                    endFrame = 2;
                    velocity.X = 0;
                    velocity.Y = 3f;
                    _flagPoleAnimationSetup = true;
                    BoundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width / 2, texture.Height);
                }
                position.Y += velocity.Y;
            }
            else
            {
                if (!_walkIntoCastleAnimationSetup)
                {
                    _soundManager.StageClear();
                    flipSprite = false;
                    hasJumped = false;
                    walking = false;
                    WalkAnimation();
                    velocity.Y = 0;
                    velocity.X = 3f;
                    _walkIntoCastleAnimationSetup = true;
                }
                position.X += velocity.X;

                if (position.X >= 10115)
                {
                    LevelComplete = true;
                }
            }
        }

        public void HitACeiling()
        {
            velocity.Y = 2.5f;
        }

        public bool IsMovingUpwards{
            get 
            { 
                if (velocity.Y < 0) 
                    return true;
                return false;
            }
        }

        public void HitTheGround(int yPosition)
        {
            if (velocity.Y > 0)
            {
                position.Y = yPosition - DestinationRectangle.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }
        }

        public void HitWallRightSide(int xPosition)
        {
            position.X = xPosition - DestinationRectangle.Width - 2; // Hardcoded
        }

        public void HitWallLeftSide(int xPosition)
        {
            position.X = xPosition + 2; // Hardcoded
        }

        public void StarMode()
        {
            starMode = true;
            _soundManager.PlayStarPowerSong();
        }
    }
}

/*Player*/
//Tweak mario boundingbox

/*Game*/
//Add lives

/*Enemies*/
//Check om fjender spawner de korrekte steder

/*Bugs*/
//If you release jumpkey mid air and press it down before he hits the ground he insta-jumps which feels bad
//PowerUp animation starts very quickly in the ground for some reason
//When PowerUp hits the ground its first frame is in the ground for some reason
//Prøvede at respawne hvor den var "store-mario" og så døde den bare hele tiden..
//Ved powerup ryger mario gennem jorden nogle gange
//Fjender spawner mega højt oppe i luften?
//BigMario kan hoppe igennem nogle bricks?
//Fjender lander nede i jorden og skifter retning randomly når de rammer
//efter respawn ved powerup falder han gennem jorden

/*???*/
//Hvorfor får GenerateMap 45 ind?

/*OPTIONAL*/
//Add Menu?
//Add highscore?
//Add Game Over?