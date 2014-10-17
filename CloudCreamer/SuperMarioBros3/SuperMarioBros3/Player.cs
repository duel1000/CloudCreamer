﻿using System;
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
        private bool hasJumped = false;
        private bool walking = false;
        private bool jumpKeyReleased;
        private float _powerUpAnimationTimer = 0;

        private bool _isBigMario = false;
        private bool _powerUpAnimationPlayed = false;
        private bool _frozen = false;
        private int _stepsIntoPowerUpAnimation = 0;

        private SoundManager _soundManager;

        public Player(SoundManager soundManager) : base("smallstillmario", new Vector2(64,384),1,3,1)
        {
            this._soundManager = soundManager;
        }

        public override void Update(GameTime gameTime)
        {
            if (_isBigMario && !_powerUpAnimationPlayed)
            {
                PowerUpAnimation(gameTime);
            }
            else if(!_frozen)
            {
                position += velocity;

                Input(gameTime);

                if (velocity.Y < 10)
                    velocity.Y += 0.4f;

                DestinationRectangle = new Rectangle((int)position.X, (int)position.Y, imageWidth, imageHeight);

                BoundingBox = DestinationRectangle;
            }

            base.Update(gameTime);
        }

        private void PowerUpAnimation(GameTime gameTime)
        {
            _powerUpAnimationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _frozen = true;
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
                velocity.Y = -9f;
                walking = false;
                hasJumped = true;
                jumpKeyReleased = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
                jumpKeyReleased = true;
        }

        private void PowerUp()
        {
            if (!_isBigMario)
            {
                _powerUpAnimationPlayed = false;
                _soundManager.PowerUpEffect();
                _isBigMario = true;
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

        public void Collision(Tile tile, int xOffset, int yOffset)
        {
            if (BoundingBox.TouchTopOf(tile.BoundingBox))
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
                    velocity.Y = 1f;
                    if (tile.GetType().Name == "BrickTile")
                    {
                        if (tile.TurnsToHardTile)
                            tile.Status = "TurnHard";
                        else
                            tile.Status = "Destroy";
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
            if (position.Y > yOffset - DestinationRectangle.Height) position.Y = yOffset - DestinationRectangle.Height;
        }

        public void EvilMushroomCollision(Mushroom mushroom)
        {
            if (BoundingBox.TouchTopOf(mushroom.BoundingBox) && !mushroom.IsDead)
            {
                mushroom.IsDead = true;
                position.Y -= 3f;
                velocity.Y = -6f;
                hasJumped = true;
            }
        }
    }
}


//TODO

/*Player*/
//Shift key for speed run + superjumpsound
//PlayerDeath

/*Tile*/ 
//Add hard earth tiles
//TileAnimation only for big mario

/*Game*/
//Add coins
//Add lives
//Add timer
//Add Castle
//Add Powerups - currently P powers him up, we need a mushroom to do so
//Add Background

/*Enemies*/
//De skal kunne collide med hinanden

/*Bugs*/
//If you release jumpkey mid air and press it down before he hits the ground he insta-jumps which feels bad
//PowerUp animation starts very quickly in the ground for some reason