#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnsolvedCases.Toolbox;

#endregion

namespace UnsolvedCases
{
    public class MGStage7 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;

        int width, height;

        Color generalImageColor;    //Color with witch we are going to Draw the general_image
        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;
        
        bool finished = false;          //If the general image is clicked many times, the MessageBoxScreen 
                                        //is appeared many times so we need to call MessageBoxScreen only once

        KeyboardState state;

        PostIt postit;  

        #endregion

        #region Initialization

        public MGStage7()
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

            GeneralImageRec = new Rectangle(width / 2 - 40, height / 2 - 40, 80, 80);
            GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");

            generalImageColor = Color.White; //initially Aplha =0; with every click Alpha will increase
            generalImageColor.A = 0;
            Oldmstate = Mouse.GetState();

            //Create postit it visible during the whole game
            postit = new PostIt(content);
            postit.Text = "CLICKING MAKES ME STRONGER...";
            postit.Width = postit.Width / 2;
            postit.Height = postit.Height / 2;

            postit.Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - postit.Width, 10);
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
            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released 
                && !GeneralImageRec.Contains(mstate.X, mstate.Y))
                generalImageColor.A += 10;


            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released
               && GeneralImageRec.Contains(mstate.X, mstate.Y) && generalImageColor.A >= 240 && !finished)
            {
                finished = true;
                Console.WriteLine("Aplha is: "+generalImageColor.A);
                ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage8()));
            }

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
          //  spriteBatch.DrawString(Fonts.MainFont, "CLICK ON THE 'GENERAL IMAGE'", new Vector2(0, 
          //      ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);
            spriteBatch.Draw(GeneralImageTexture, GeneralImageRec, generalImageColor);

            postit.Draw(this, gameTime);
            
            spriteBatch.End();
        }

        #endregion
    }
}
