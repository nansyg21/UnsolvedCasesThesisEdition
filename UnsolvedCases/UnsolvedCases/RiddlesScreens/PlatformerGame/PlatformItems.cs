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
    public class PlatformItems : DrawableGameComponent
    {
        #region Fields

        //Type of platform
        const int RED = 1;
        const int BLUE = 2;
        const int GREEN = 3;

        Platform platform;

        private Game _game;

        private SpriteBatch _spriteBatch;
        private Texture2D _texture;                 //platform texture
        private Vector2 _position;                  //platform position
        private Rectangle _rect;                    //platform rectangle

        int type;

        #endregion

        #region GettersAndSetters

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public Rectangle Rectangle
        {
            get { return _rect; }
            set { _rect = value; }
        }
      
        #endregion


        #region Constructor

        /// <summary>
        /// Constractor of platforms
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="num">The number defines the type of platform</param>
        /// 

        public PlatformItems(Game game, int num, Platform p)
            : base(game)
        {

            _position = new Vector2(0f, 0f);
            _game = game;

            if (num == RED)
            {
                _texture = _game.Content.Load<Texture2D>("Textures/Riddles/Platformer/fireball");
                type = 1;
            }
            else if (num == BLUE)
            {
                _texture = _game.Content.Load<Texture2D>("Textures/Riddles/Platformer/blueball");
                type = 2;
            }
            else if (num == GREEN)
            {
                _texture = _game.Content.Load<Texture2D>("Textures/Riddles/Platformer/greenball");
                type = 3;
            }

            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));           

            platform = p;

            _rect = new Rectangle(0,0,0,0);
            _rect.X = platform.Rectangle.X + (platform.Rectangle.Width / 2) - 15;
            _rect.Y = platform.Rectangle.Y - 30;
            _rect.Width = 30;
            _rect.Height = 30;

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

           

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, _rect, Color.White);
            _spriteBatch.End();

        }

        #endregion
    }
}