#region File Description
#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnsolvedCases.Toolbox;

#endregion

namespace UnsolvedCases
{
    public class MGStage2 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;

        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;

        KeyboardState state;

        bool GeneralImageIsVisible = false;     //after the user completes the task the general image appears
        int MouseCounter = 0;               //mouse dissapear times counter
        int CounterLimit = 5;               //if mouse is dissapeared CounterLimit times, we go to next page

        bool finished = false;          //If the general image is clicked many times, the MessageBoxScreen 
                                        //is appeared many times so we need to call MessageBoxScreen only once
        PostIt postit;     

        #endregion

        #region Initialization

        public MGStage2()
        {
        }

        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");

            Oldmstate = Mouse.GetState();
            GeneralImageRec = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 40, 0, 80, 80);

            //Create postit it visible during the whole game
            postit = new PostIt(content);
            postit.Text = "CLICK ME IF YOU CAN...";
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

            if (!finished)
            {
                //mouse visible &&  user clicked -> mouse invisible, MouseCounter++ 
                if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released)
                    if (ScreenManager.GetCursor.Visible)
                    {
                        ScreenManager.GetCursor.Visible = false;
                        MouseCounter++;                                             //mouse is set invisible one more time
                    }
                    else                                                            //mouse invisible &&  user clicked -> mouse invisible
                        ScreenManager.GetCursor.Visible = true;

                if (MouseCounter > CounterLimit)                                    //user completed the task, the general image appears
                    GeneralImageIsVisible = true;
            }

            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released && GeneralImageIsVisible 
                && GeneralImageRec.Contains(mstate.X, mstate.Y)&& !finished)        //To Next Stage
            {
                finished = true;
                ScreenManager.GetCursor.Visible = true;
                ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage3()));
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

            if (GeneralImageIsVisible)
                spriteBatch.Draw(GeneralImageTexture, GeneralImageRec, Color.White);

            postit.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
