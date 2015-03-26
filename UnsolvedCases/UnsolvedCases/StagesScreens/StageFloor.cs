#region File Description
#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace UnsolvedCases
{
    public class StageFloor : DrawableGameComponent
    {
        #region Fields

        private Game thisGame;
        private SpriteBatch spriteBatch;
        private Texture2D showTexture;                      //the texture the gamer sees
        private Texture2D shadowTexture;                    //the black and white texture

        #endregion

        #region GettersAndSetters

        /// <summary>
        /// The texture the gamer sees
        /// </summary>
        public Texture2D ShowTexture
        {
            get { return showTexture; }
            set { showTexture = value; }
        }
       
        /// <summary>
        /// The texture used for collision detection
        /// </summary>
        public Texture2D ShadowTexture
        {
            get { return shadowTexture; }
            set { shadowTexture = value; }
        }

        #endregion

        #region Initialization

        public StageFloor(Game game)
            : base(game)
        {
            thisGame = game;
            showTexture = thisGame.Content.Load<Texture2D>("Textures/Stages/1st_floor");
            shadowTexture = thisGame.Content.Load<Texture2D>("Textures/Stages/1st_floor_bw");
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            showTexture.Dispose();
            shadowTexture.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();

            spriteBatch.Draw(showTexture, new Vector2(0, 0), Color.White);

            spriteBatch.End();
        }

        #endregion
    }
}
