#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace UnsolvedCases
{
    public class MGStage6 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;

        int width, height;

        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;

        Label txtLabel1;
        Label txtLabel2;

        KeyboardState state;
        
        #endregion

        #region Initialization

        public MGStage6()
        {
        }

        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            width = ScreenManager.GraphicsDevice.Viewport.Width;
            height = ScreenManager.GraphicsDevice.Viewport.Height;

            GeneralImageRec = new Rectangle(80, 80, 80, 80);
            GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");

            txtLabel1 = new Label(content, "CLICK ON: ", Fonts.MainFont);
            txtLabel2 = new Label(content, "THE GENERAL IMAGE", Fonts.MainFont);
            txtLabel1.Position = new Vector2(0, height / 2);
            txtLabel2.Position =new Vector2( txtLabel1.Position.X + Fonts.MainFont.MeasureString(txtLabel1.Text).X, 
                txtLabel1.Position.Y);
 
            Oldmstate = Mouse.GetState();
        }

        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            mstate = Mouse.GetState();
            
            if(txtLabel2.GetRectagnle(this).Contains(mstate.X, mstate.Y) && mstate.LeftButton == ButtonState.Pressed 
                && Oldmstate.LeftButton == ButtonState.Released)
                ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage7()));

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());

            Oldmstate = mstate;
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.DrawString(Fonts.MessageFont, "Mouse: " + mstate.X + ", " + mstate.Y, Vector2.Zero, Color.White);
            txtLabel1.Draw(this, gameTime);
            txtLabel2.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
