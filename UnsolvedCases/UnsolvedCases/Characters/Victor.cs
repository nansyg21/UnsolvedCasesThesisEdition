#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#endregion

namespace UnsolvedCases
{
    public class Victor : DrawableGameComponent
    {
        #region Fields

        private Vector2 victorPosition;
        int direction = 1;                          //0=stand 1=right 2=left 3=up 4=down
        float elapsed;                              //counts the elapsed time
        float delay = 120f;                         //delay between frame change
        int frame = 0; 
        Boolean move = false;                       //shows if the character can move or not

        private Game _game;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;                 //characters texture
        private Vector2 _position;                  //characters position
        private float _speed = 0.0000001f;          //initial speed to appear stopped
        private float _speedStep = 0.1f;            //acceleration
        private float _rotationAngle;               //orientation of the texture's rectagnle
        private float _rotationStep = 1.57f;
        private Vector2 _textureReferencePoint;

        KeyboardState KeyState;
        KeyboardState PrevState;
        Vector2 _velocity;

        Texture2D[] victorDown;                     //down move images
        Texture2D[] victorUp;                       //up move images
        Texture2D[] victorLeft;                     //left move images
        Texture2D[] victorRight;                    //right move images

        #endregion

        #region GettersAndSetters

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public float SpeedStep
        {
            get { return _speedStep; }
            set { _speedStep = value; }
        }

        public float RotationAngle
        {
            get { return _rotationAngle; }
            set { _rotationAngle = value; }
        }

        public float RotationStep
        {
            get { return _rotationStep; }
            set { _rotationStep = value; }
        }

        public Vector2 TextureReferencePoint
        {
            get { return _textureReferencePoint; }
            set { _textureReferencePoint = value; }
        }

        #endregion

        #region Bounding Rectangle

        public Vector2 RectUpperLeftCorner
        {
            get { return new Vector2(_position.X-_texture.Width/2,_position.Y-_texture.Height/2); }
        }

        public int RectWidth
        {
            get { return _texture.Width; }
        }

        public int RectHeight
        {
            get { return _texture.Height; }
        }

        public List<Vector2> RectanglePoints
        {
            get
            {
                return new List<Vector2>()
                {
                     Rotations.RotatePoint((_position-TextureReferencePoint),_position, RotationAngle),
                     Rotations.RotatePoint(new Vector2() { X = (_position-TextureReferencePoint).X + RectWidth, Y = 
                         (_position-TextureReferencePoint).Y }, _position, RotationAngle),
                     Rotations.RotatePoint(new Vector2() { X = (_position-TextureReferencePoint).X + RectWidth, Y = 
                         (_position-TextureReferencePoint).Y + RectHeight }, _position, RotationAngle),
                     Rotations.RotatePoint(new Vector2() { X = (_position-TextureReferencePoint).X, Y = 
                         (_position-TextureReferencePoint).Y + RectHeight }, _position, RotationAngle)
                };
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constractor of the main character
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="SpriteTexture">The initial sprite of the character</param>
        /// <param name="InitialPosition">The initial position of the character</param>
        public Victor(Game game, string SpriteTexture, Vector2 InitialPosition)
            : base(game)
        {
            _position = InitialPosition;
            _game = game;
            _texture = _game.Content.Load<Texture2D>(SpriteTexture);
            victorDown = new Texture2D[4];
            victorUp = new Texture2D[4];
            victorLeft = new Texture2D[4];
            victorRight = new Texture2D[4];

            victorDown[0] = _game.Content.Load<Texture2D>("Characters/Victor/victor_down_xsmall_1");
            victorDown[1] = _game.Content.Load<Texture2D>("Characters/Victor/victor_down_xsmall_2");
            victorDown[2] = _game.Content.Load<Texture2D>("Characters/Victor/victor_down_xsmall_3");
            victorDown[3] = _game.Content.Load<Texture2D>("Characters/Victor/victor_down_xsmall_4");

            victorUp[0] = _game.Content.Load<Texture2D>("Characters/Victor/victor_up_xsmall_1");
            victorUp[1] = _game.Content.Load<Texture2D>("Characters/Victor/victor_up_xsmall_2");
            victorUp[2] = _game.Content.Load<Texture2D>("Characters/Victor/victor_up_xsmall_3");
            victorUp[3] = _game.Content.Load<Texture2D>("Characters/Victor/victor_up_xsmall_4");

            victorLeft[0] = _game.Content.Load<Texture2D>("Characters/Victor/victor_left_xsmall_1");
            victorLeft[1] = _game.Content.Load<Texture2D>("Characters/Victor/victor_left_xsmall_2");
            victorLeft[2] = _game.Content.Load<Texture2D>("Characters/Victor/victor_left_xsmall_3");
            victorLeft[3] = _game.Content.Load<Texture2D>("Characters/Victor/victor_left_xsmall_4");

            victorRight[0] = _game.Content.Load<Texture2D>("Characters/Victor/victor_right_xsmall_1");
            victorRight[1] = _game.Content.Load<Texture2D>("Characters/Victor/victor_right_xsmall_2");
            victorRight[2] = _game.Content.Load<Texture2D>("Characters/Victor/victor_right_xsmall_3");
            victorRight[3] = _game.Content.Load<Texture2D>("Characters/Victor/victor_right_xsmall_4");

            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            _textureReferencePoint = new Vector2(Texture.Width/2, Texture.Height/2);
            _rotationAngle = 0;
            victorPosition = new Vector2(10f, 10f);
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            _texture.Dispose();
           
            base.Dispose(disposing);
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            KeyState = Keyboard.GetState();
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            /* Change the rotation angle according to the key pressed rotation angle is mesured in rads
             If speed is less than 0.7 increase it by the speedStep */

            if (KeyState.IsKeyDown(Keys.Up))
            {
                direction=3;
                move = true;
                _rotationAngle = (float)4.712389;

                if(_speed < 0.7)
                _speed += _speedStep;
            }
            else if (KeyState.IsKeyDown(Keys.Down))
            {
                move = true;
                direction=4;
                _rotationAngle = (float)1.5707963;

                if (_speed < 0.7)
                _speed += _speedStep;
            }
            else if (KeyState.IsKeyDown(Keys.Left))
            {
                direction=2;
                move = true;
                _rotationAngle = (float)3.1415927;

                if (_speed < 0.7)
                _speed += _speedStep;
            }
            else if (KeyState.IsKeyDown(Keys.Right))
            {
                direction=1;
                move = true;
                _rotationAngle = 0;

                if (_speed < 0.7)
                _speed += _speedStep;
            }
            else
            {
                move = false;
                _speed = 0;
            }
            /* If the character is moving then change the frames of the animation every time the current delay has elapsed
             If the frame is the last of 4 then the next is the first again
             If the character is not moving then the frame to display is the first one in witch the char is appear standing*/
            if (move)
            {
                if (elapsed >= delay)
                {
                    if (frame >= 3)
                    {
                        frame = 0;
                    }
                    else
                    {
                        if (direction == 1)
                        {
                            //if (_position.X < (1366 - (_texture.Width)-10))
                            //{
                            _texture = victorRight[frame];
                            //}
                        }
                        else if (direction == 2)
                        {
                            //if (_position.X >= 10)
                            //{
                            _texture = victorLeft[frame];
                            //}
                        }
                        else if (direction == 3)
                        {
                            //if (_position.Y >= 10)
                            //{
                            _texture = victorUp[frame];
                            //}
                        }
                        else if (direction == 4)
                        {
                            //if (_position.Y < (760 - _texture.Height-10))
                            //{
                            _texture = victorDown[frame];
                            //}
                        }
                       
                        frame++;
                    }
                    
                    elapsed = 0;
                }
            }
            else
            {
                if (direction == 1)
                {
                    //if (victorPosition.X < (1366 - (_texture.Width)))
                    //{
                    _texture = victorRight[0];
                    //}
                }
                else if (direction == 2)
                {
                    //if (victorPosition.X >= 0)
                    //{
                    _texture = victorLeft[0];
                    //}
                }
                else if (direction == 3)
                {
                    //if (victorPosition.Y >= 0)
                    //{
                    _texture = victorUp[0];
                    //}
                }
                else if (direction == 4)
                {
                    //if (victorPosition.Y < (760 - _texture.Height))
                    //{
                    _texture = victorDown[0];
                    //}
                }
            }
            
            //Update the velocity according to the current speed and update the position according to the current velocity
            _velocity = new Vector2(_speed * (float)Math.Cos(RotationAngle), _speed * (float)Math.Sin(RotationAngle));
            _position += _velocity;
            PrevState = KeyState;
            
            base.Update(gameTime);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            //Draw the spritebatch with the same orientation even if the orientation of the rectangle changes
            _spriteBatch.Draw(_texture, _position, null, Color.White, 0, TextureReferencePoint, 1.0f, SpriteEffects.None, 0f);
            //Draw the spritebatch with the same orientation with the rectangle
            //_spriteBatch.Draw(_texture, _position, null, Color.White, RotationAngle, TextureReferencePoint, 1.0f,
            //SpriteEffects.None, 0f);
            _spriteBatch.End();

        }

        #endregion
    }
}