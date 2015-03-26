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
    public class Platform : DrawableGameComponent
    {
        #region Fields

        //Type of platform
        const int STARTER = 0; 
        const int GRASS = 1;
        const int WOOD = 2;
        const int METAL = 3;


        float elapsed;                              //counts the elapsed time
        float delay;                         //delay to count when the platform must disappear
       
        private Game _game;
        
        /*Different delays according to type of platform*/
        float _metalDelay=7000f;
        float _woodDelay=6000f;
        float _grassDelay=5000f;

        private SpriteBatch _spriteBatch;
        private Texture2D _texture;                 //platform texture
        private Vector2 _position;                  //platform position
        private Rectangle _rect;                    //platform rectangle
      
        int widthRange;
        int heightRange;

        int type;

        Boolean hide=false;                         //True when is time to hide the platform

        Boolean item = false;

        Boolean starter = false;

        PlatformItems platformItem;

        Random itemType;

        Boolean itemCheck = false;

        #endregion

        #region GettersAndSetters

        public PlatformItems PlatformItem
        {
            get { return platformItem; }
            set { platformItem = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float Elapsed
        {
            get { return elapsed; }
            set { elapsed = value; }
        }

        public Boolean TimeToHide
        {
            get { return hide; }
            set { hide = value; }
        }

        public Boolean HasItem
        {
            get { return item; }
            set { item = value; }
        }

        public Boolean IsStarterPlatform
        {
            get { return starter; }
            set { starter = value; }
        }

        public Rectangle Rectangle
        {
            get { return _rect; }
            set { _rect = value; }
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

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        #endregion

       
        #region Constructor

        /// <summary>
        /// Constractor of platforms
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="num">The number defines the type of platform</param>
        /// 
      
        public Platform(Game game, int num)
            : base(game)
        {
            _position = new Vector2(0f,0f);
            _game = game;
            elapsed = 0;
            /*Load the apropriate texture according to the type
             *Set the delay likewise*/
            if(num==GRASS)
            {
                _texture = _game.Content.Load<Texture2D>("Textures/Riddles/Platformer/grass_platform");
                delay = _grassDelay;
            }
            else if(num==WOOD)
            {
                _texture = _game.Content.Load<Texture2D>("Textures/Riddles/Platformer/wooden_platform");
                delay = _woodDelay;
            }
            else if (num == METAL)
            {
                _texture = _game.Content.Load<Texture2D>("Textures/Riddles/Platformer/metal_platform");
                delay=_metalDelay;
            }
            else if(num==STARTER)
            {
                _texture = game.Content.Load<Texture2D>("Textures/Riddles/Platformer/starter_platform");
                delay = 1000000000000000;
                starter = true;
            }
          
            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));           
            
            widthRange = 0;
            heightRange = 0;

            itemType = new Random();
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

            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

          //  if ((HasItem)&&(!itemCheck))
          //  {
          //      AddItem();
          //  }

            /*Check if it is time to vanish platform*/
            if (elapsed > delay)
            {
                hide = true;
                elapsed = 0;
            }
            
            base.Update(gameTime);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture,_rect,Color.White);
            _spriteBatch.End();

        }

        #endregion

        public void AddItem()
        {
            
            int type = itemType.Next(0,7);
            if (type < 2)
                platformItem=new PlatformItems(_game, 1, this);
            else if (type < 4)
                platformItem = new PlatformItems(_game, 2, this);
            else
                platformItem = new PlatformItems(_game, 3, this);

            itemCheck = true;
           
        }
    }
}