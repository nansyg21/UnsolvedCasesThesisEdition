#region File Description
#endregion

#region Using Statements

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace UnsolvedCases.Tools
{
    public class MouseCursor : DrawableGameComponent
    {
        #region Fields

        private Texture2D mouseTexture;
        private Vector2 position;

        private SpriteBatch spriteBatch;

        #endregion

        #region Initialization

        public MouseCursor(Game game, SpriteBatch _spriteBatch)
            : base(game)
        {
            spriteBatch = _spriteBatch;
            mouseTexture = game.Content.Load<Texture2D>(@"Misc\cursor_small");
        }

        public Texture2D getTexture()
        {
            return mouseTexture;
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {

            MouseState State = Mouse.GetState();
            position = new Vector2(State.X, State.Y);
            base.Update(gameTime);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mouseTexture, position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            mouseTexture.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}
