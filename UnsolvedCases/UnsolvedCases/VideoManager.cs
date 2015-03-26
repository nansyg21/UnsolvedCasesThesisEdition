#region File Description
#endregion

#region Using Statements

using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

#endregion

namespace UnsolvedCases
{
    public class VideoManager : GameScreen, IDisposable
    {
        #region Fields

        Video video;
        VideoPlayer player;
        Texture2D videoTexture;
        SpriteBatch spriteBatch;
        string name;
        GameScreen nextScreen;
      
        #endregion

        #region Initialization

        public VideoManager(string name, GameScreen nextScreen)
            : base()
        {
            this.name = name;
            this.nextScreen = nextScreen;
        }

        /// <summary>
        /// Loads the graphics content for this screen
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            ContentManager content = ScreenManager.Game.Content;
          
            video = content.Load<Video>(@"Videos\" + name);
            player = new VideoPlayer();
            player.Play(video);
        }

        #endregion

        #region Updating

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (player.State == MediaState.Stopped || Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                player.Stop();
                LoadingScreen.Load(ScreenManager, false, null, nextScreen);
            }
            else
            {
                videoTexture = player.GetTexture();
            }

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        #endregion

        #region Drawing

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch = ScreenManager.SpriteBatch;

            if (videoTexture != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(videoTexture, spriteBatch.GraphicsDevice.Viewport.Bounds, Color.White);
                spriteBatch.DrawString(Fonts.LoginFont, "PRESS SPACE TO SKIP VIDEO", Vector2.Zero, Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        #endregion

        #region Public Methods

        public void Dispose() { }

        #endregion
    }
}
