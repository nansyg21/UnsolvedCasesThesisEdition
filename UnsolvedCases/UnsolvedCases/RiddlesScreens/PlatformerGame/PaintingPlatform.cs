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
    public class PaintingPlatform : DrawableGameComponent
    {
        #region Fields

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

        public Rectangle Rectangle
        {
            get { return _rect; }
            set { _rect = value; }
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

        public PaintingPlatform(Game game, int num)
            : base(game)
        {
            _position = new Vector2(0f, 0f);
            _game = game;

            /*Load the apropriate texture according to the type
             *Set the delay likewise*/
            if (num == 1)
            {
                _texture = _game.Content.Load<Texture2D>("Textures/Riddles/Platformer/ice_platform");
                type = 1;
            }
            else if (num == 2)
            {
                _texture = _game.Content.Load<Texture2D>("Textures/Riddles/Platformer/wooden_platform");
                type = 2;
            }
            else if (num == 3)
            {
                _texture = _game.Content.Load<Texture2D>("Textures/Riddles/Platformer/metal_platform");
                type = 3;
            }

            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
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