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
    public class Character : DrawableGameComponent
    {
        #region Fields

        float gravity = 1.3f;                       //Shows the speed the character goes down after a jump
      
        /*Animation*/
        int direction = 1;                          //0=stand 1=right 2=left 3=up 4=down
        float elapsed;                              //counts the elapsed time
        float delay = 60f;                          //delay between frame change
        int frame = 0;                              //initial frame
        Boolean animation = false;                  //If true perform the animation

        /*Tables to hold the animation images*/
        Texture2D[] victorDown;                     //down move images
        Texture2D[] victorUp;                       //up move images
        Texture2D[] victorLeft;                     //left move images
        Texture2D[] victorRight;                    //right move images
        
        private Game _game;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;                 //characters texture
        private Vector2 _position;                  //characters position
        private Vector2 previous_position;                  //characters previous position



        private Vector2 _textureReferencePoint;
        
        private Vector2 victorPosition;

        List<Platform> platforms;                   //Platforms on the board
        int[] items;                   //Platforms on the board

        public Vector2 Movement;                    //Used to define movement distance

        Boolean newPlatforms = true;
        Platform currentPlatform;
        
        /*Keyboard States*/
        KeyboardState KeyState;
        KeyboardState PrevState;
        
        /*Width and height of board*/
        int widthRange;
        int heightRange;

        Rectangle collisionRect;                    //A rectangle with the size of the sprite used to forsee collision

        /*Direction and speed of movement*/
        Vector2 mDirection = Vector2.Zero;          
        Vector2 mSpeed = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        Boolean jumping = false;
        public Boolean falling = false;    //character is falling down

        

        enum State
        {
            Walking,
            Jumping
        }

        State currentState = State.Walking;

        #endregion

        #region GettersAndSetters

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Boolean IsJumping
        {
            get { return jumping; }
            set { jumping= value; }
        }

        public Platform CurrentPlatform
        {
            get { return currentPlatform; }
            set { currentPlatform = value; }
        }

        public Boolean NeedPlatforms
        {
            get { return newPlatforms; }
            set { newPlatforms = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Rectangle Rectangle
        {
            get { return collisionRect; }
            set { collisionRect = value; }
        }

        public int WidthRange
        {
            get { return widthRange; }
            set { widthRange = value; }
        }

        public int HeightRange
        {
            get { return heightRange; }
            set { heightRange = value; }
        }

        public Vector2 TextureReferencePoint
        {
            get { return _textureReferencePoint; }
            set { _textureReferencePoint = value; }
        }

        public int[] HowManyItems
        {
            get { return items; }
            set { items = value; }
        }

        #endregion

        #region Bounding Rectangle

        public int RectWidth
        {
            get { return _texture.Width; }
        }

        public int RectHeight
        {
            get { return _texture.Height; }
        }

        public List<Platform> Platforms
        {
            set { platforms = value; }
            get { return platforms; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constractor of the main character
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="SpriteTexture">The initial sprite of the character</param>
        /// <param name="InitialPosition">The initial position of the character</param>
        public Character(Game game, string SpriteTexture, Vector2 InitialPosition)
            : base(game)
        {
 
            _position = InitialPosition;
            previous_position = _position;
            _game = game;
          
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
            
            victorPosition = new Vector2(10f, 10f);

            _texture = victorRight[0];
            collisionRect = new Rectangle((int)_position.X+(victorRight[0].Width/4),(int) _position.Y,(int) 3*(victorRight[0].Width/4),(int) victorRight[0].Height);

            widthRange = 0;
            heightRange = 0;

            platforms = new List<Platform>();
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            _texture.Dispose();

            base.Dispose(disposing);
        }

        #endregion

        #region Movement
        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;
                /*Move left and enable animation*/
                if (aCurrentKeyboardState.IsKeyDown(Keys.Left))
                {
                    if (_position.X >= _texture.Width)
                    {
                        Movement += new Vector2(-1, 0);
                        direction = 2;
                        this.animation = true;
                    }
                }
                /*Move right and enable animation*/
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right))
                {
                    if (_position.X < (1366 - (_texture.Width - 10)))
                    {
                        Movement += new Vector2(1, 0);
                        direction = 1;
                        this.animation = true;
                    }
                }
                /*Disable animation*/
                else
                    this.animation = false;

                /*Jump if key up is pressed and the character is on ground*/
                if (aCurrentKeyboardState.IsKeyDown(Keys.Up))
                {
                    
                    if ((_position.Y >= 10)&&(this.IsOnFirmGround()))
                    {
                        
                        Movement = -Vector2.UnitY * 100; //Change 100 to jump higher or lower                   
                    }
                    this.jumping = true;
                }
            /*    else if (aCurrentKeyboardState.IsKeyDown(Keys.Down))
                {
                    if (_position.Y < (760 - _texture.Height - 10))
                    {
                        Movement += new Vector2(0, 1);
                    }
                }*/

              
        }
        
        /*Movement speed and position update
         *Update collision rectangle position*/
        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            Movement -= Movement * new Vector2(.30f, .30f);
            this._position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
            collisionRect.X = (int)_position.X;
            collisionRect.Y = (int)_position.Y;
        }

        /*The gravity effect drows the character down when not on solid ground
         *Change gravity variable to make the character fall faster or slower*/
        private void AffectWithGravity()
        {
            Movement += Vector2.UnitY * gravity;
        }

        /*Move only if you can
         *Know the last possible position and check if you can go to the new one
         *If so, then move else return to the last possible position*/
        private void MoveIfPossible(KeyboardState aCurrentKeyboardState, GameTime gameTime)
        {
            Vector2 oldPosition = this._position;

            UpdatePositionBasedOnMovement(gameTime);
            MovementAnimation(animation, elapsed);
            collisionRect.X = (int)_position.X;
            collisionRect.Y = (int)_position.Y;

            this._position = PlatformerLevel.CurrentBoard.WhereCanIGetTo(oldPosition, _position, collisionRect); //Static method from PlatformerLevel - Improves movement in order not to float
            collisionRect.X = (int)_position.X;
            collisionRect.Y = (int)_position.Y;
        }

        /*Check if character is on ground or in the air
       * A character can jump only if he is on firm ground
       * Checks the collision with every platform on the screen*/
        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = collisionRect;
            onePixelLower.Offset(0, 1);
            if (platforms.Count != 0)
            {
                foreach (Platform p in platforms)
                {
                    if (onePixelLower.Intersects(p.Rectangle))
                    {
                       
                        this.jumping = false;
                       
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion


        /*Method for animation*/
        public void MovementAnimation(Boolean animation,float elapsed)
        {
            if (animation)
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
                            _texture = victorRight[frame];
                        }
                        else if (direction == 2)
                        {
                            _texture = victorLeft[frame];
                        }
                        else if (direction == 3)
                        {
                            _texture = victorUp[frame];
                        }
                        else if (direction == 4)
                        {
                            _texture = victorDown[frame];
                        }

                        frame++;
                    }
                    this.elapsed = 0;
                    
                }
            }
            else
            {
                if (direction == 1)
                {
                    _texture = victorRight[0];
                }
                else if (direction == 2)
                {
                    _texture = victorLeft[0];
                }
                else if (direction == 3)
                {
                    _texture = victorUp[0];
                }
                else if (direction == 4)
                {
                    _texture = victorDown[0];
                }
            }
        
        }
        
        #region Update

        public override void Update(GameTime gameTime)
        {
            KeyState = Keyboard.GetState();
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            /*Keep the character above the lower part of screen*/
            if (_position.Y > 730)
            {
                 _position.Y = 730;
            }

            /*Perform movement*/ 
            UpdateMovement(aCurrentKeyboardState);
            AffectWithGravity();
            MoveIfPossible(aCurrentKeyboardState,gameTime);

            //Check collision with head with platform
            if (previous_position.Y < _position.Y)
                falling = true;
            else
                falling = false;
            previous_position = _position;
              
            mPreviousKeyboardState = aCurrentKeyboardState;
            PrevState = KeyState;

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
          
            _spriteBatch.Draw(_texture, _position, null, Color.White, 0, TextureReferencePoint, 1.0f, SpriteEffects.None, 0f);
            
            _spriteBatch.End();

        }

    
        
        #endregion
    }
}