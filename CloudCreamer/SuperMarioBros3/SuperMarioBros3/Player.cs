using System;
using System.Collections.Generic;
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
        private bool _isSmallMario;
        private bool _isInvulnerable;
        private bool _powerUpAnimationPlayed = false;
        private bool _powerDownAnimationPlayed = false;
        private bool _DeathAnimationPlayed = false;

        private bool _frozen = false;
        private int _stepsIntoPowerUpAnimation = 0;
        private int _stepsIntoPowerDownAnimation = 0;

        private SoundManager _soundManager;
        private bool _takeDamageSoundPlayed;
        private bool _killPlayerSoundPlayed;

        public bool RespawnPlayer { get; set; }

        public Player(SoundManager soundManager) : base("smallstillmario", new Vector2(64,384),1,3,1)
        {
            this._soundManager = soundManager;
            _hitPoints = 1;
        }

        public override void Update(GameTime gameTime)
        {
            if (_isBigMario && !_powerUpAnimationPlayed)
            {
                PowerUpAnimation(gameTime);
                BoundingBox = new Rectangle(0,0,0,0);
            }
            else if(_isSmallMario && !_powerDownAnimationPlayed)
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

                BoundingBox = DestinationRectangle;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.O) && _isDead)
            {
                _isDead = false;
                RespawnPlayer = true;
            }

            base.Update(gameTime);
        }

        private void Input(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                PowerUp();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
                flipSprite = false;
                WalkAnimation();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X = -(float) gameTime.ElapsedGameTime.TotalMilliseconds/3;
                flipSprite = true;
                WalkAnimation();
            }

            else
            {
                StopWalkAnimation();
                velocity.X = 0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && hasJumped == false && jumpKeyReleased && velocity.Y <= 0)
            {
                if(_isBigMario)
                    texture = Content_Manager.GetInstance().Textures["bigmariojumping"];
                else    
                    texture = Content_Manager.GetInstance().Textures["smalljumpmario"];
                
                currentFrame = 1;
                rows = 1;
                columns = 1;
                framesPerSecond = 0;
                endFrame = 1;

                _soundManager.SmallJumpEffect();
                position.Y -= 5f;
                velocity.Y = -13.2f;
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
            }
        }

        private void PowerDown()
        {
            if (!_takeDamageSoundPlayed)
            {
                _isInvulnerable = true;
                _soundManager.PowerDown();
                _takeDamageSoundPlayed = true;
                _isBigMario = false;
                _isSmallMario = true;
                _powerUpAnimationPlayed = false;
            }
        }

        private void TakeDamage()
        {
            if(!_isInvulnerable)
                _hitPoints--;

            if (_hitPoints == 0)
                _isDead = true;
            else if (_hitPoints == 1)
                PowerDown();
        }

        private void KillPlayer()
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
                texture = Content_Manager.GetInstance().Textures["bigmariostanding"];
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
                texture = Content_Manager.GetInstance().Textures["bigmariostanding"];
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
                _isInvulnerable = false;
            }
        }

        private void StopWalkAnimation()
        {
            if (!hasJumped)
            {
                if(_isBigMario)
                    texture = Content_Manager.GetInstance().Textures["bigmariostanding"];
                else
                    texture = Content_Manager.GetInstance().Textures["smallstillmario"]; 
                
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
                if(_isBigMario)
                    texture = Content_Manager.GetInstance().Textures["bigmariowalking"];
                else
                    texture = Content_Manager.GetInstance().Textures["oldmario"];

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
                    position.Y = tile.BoundingBox.Y - DestinationRectangle.Height;
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
                }
            }
            else if (BoundingBox.TouchLeftOf(tile.BoundingBox))
            {
                position.X = tile.BoundingBox.X - DestinationRectangle.Width - 2;
            }
            else if (BoundingBox.TouchRightOf(tile.BoundingBox))
            {
                position.X = tile.BoundingBox.X + tile.BoundingBox.Width + 2;
            }

            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - DestinationRectangle.Width) position.X = xOffset - DestinationRectangle.Width;
            //if (position.Y < 0) velocity.Y = 1f; Makes mario able to jump out top of the map
            if (position.Y > yOffset && !_isDead)
                _isDead = true; //;position.Y = yOffset - DestinationRectangle.Height;
        }

        public void EvilMushroomCollision(MushroomEnemy mushroom)
        {
            if (BoundingBox.TouchTopOf(mushroom.BoundingBox) && !mushroom.IsDead && velocity.Y > 0)
            {
                mushroom.IsDead = true;
                position.Y -= 3f;
                velocity.Y = -6f;
                hasJumped = true;
            }
            else if(BoundingBox.TouchLeftOf(mushroom.BoundingBox) ||
                    BoundingBox.TouchRightOf(mushroom.BoundingBox) ||
                    BoundingBox.TouchBottomOf(mushroom.BoundingBox))
            {
                TakeDamage();
            }
        }

        public void PowerUpCollision(MushroomPowerUp mushroomPowerUp)
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

        //resets all his stats
        public void Respawn()
        {
            texture = Content_Manager.GetInstance().Textures["smallstillmario"];
            RespawnPlayer = false;
            position = new Vector2(64,384);
            _isDead = false;
            _frozen = false;
            _hitPoints = 1;
            _isInvulnerable = false;
            _DeathAnimationPlayed = false;
            _killPlayerSoundPlayed = false;
            rows = 1;
            framesPerSecond = 0;
            columns = 1;
            currentFrame = 1;
            endFrame = 1;
        }
    }
}

/*Player*/
//Shift key for speed run + superjumpsound

/*Tile*/ 
//Add hard earth tiles

/*Game*/
//Layout Map - create a sectionmanager + level manager?
//Add lives
//Add Castle
//Add Background

/*Enemies*/
//De skal kunne collide med hinanden
//Lav animation på dem
//Spawn point -> de spawner først når mario kommer tæt på dem

/*Bugs*/
//If you release jumpkey mid air and press it down before he hits the ground he insta-jumps which feels bad
//PowerUp animation starts very quickly in the ground for some reason
//When PowerUp hits the ground its first frame is in the ground for some reason
//Prøvede at respawne hvor den var "store-mario" og så døde den bare hele tiden..

/*???*/
//Hvorfor får GenerateMap 45 ind?

/*OPTIONAL*/
//Add Menu?
//Add highscore?
//Add Game Over?