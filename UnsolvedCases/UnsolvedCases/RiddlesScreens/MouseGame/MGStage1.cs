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
    public class MGStage1 : GameScreen
    { 
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        KeyboardState state;

        MouseState mstate;
        MouseState Oldmstate;
        int width, height;

        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;

        bool finished = false;   //If the general image is clicked many times, the MessageBoxScreen is appeared many times
                                 // so we need to call MessageBoxScreen only once
        //Timing
        float currentdelay = 0;     //starting from zero and counting up
        float toFirstMessage = 100f;//delay before first and second message appears
        float FirstMessage = 300f;  //how much the first message stays on screen
        float toSecondMessage = 100f;//delay before second message appears(counting from previous)
        float SecondMessage = 300f;  //how much the second message stays on screen
        float toThirdMessage = 80;//delay before third message appears(counting from previous)
        float ThirdMessage = 200;  //how much the third message stays on screen
        float total;
        string message = "SOLVE THE TASKS AND GET BACK THE STOLEN PAINTINGS";
        string message2 = "ALL YOU HAVE TO DO IS CLICK ON THE GENERAL IMAGE!";
        string message3 = "GOOD LUCK!";

        //Position
        Vector2 messagePos;
        int limitY;

        PostIt postit; 
        #endregion

        #region Initialization

        public MGStage1() 
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
            limitY = height / 10; 
            GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");

            messagePos.X  = width/2- Fonts.MessageFont.MeasureString(message).X/2;//message in the middle by width
            total = toFirstMessage + FirstMessage + toSecondMessage + SecondMessage + toThirdMessage + ThirdMessage;

            Oldmstate = Mouse.GetState();
            GeneralImageRec = new Rectangle(-80, 0, 80, 80);

            postit = new PostIt(content);
            postit.Text = "CLICK ON THE GENERAL IMAGE";
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

            GeneralImageRec.X = -mstate.X;// 1
            GeneralImageRec.Y = -mstate.Y;

            currentdelay++;

            //Message1 slides down
            if (messagePos.Y < limitY && currentdelay > toFirstMessage && currentdelay<toFirstMessage+FirstMessage )
                messagePos.Y++;
            
            //Restore position
            else if (currentdelay == toFirstMessage + FirstMessage)
            {
                messagePos.X = width / 2 - Fonts.MessageFont.MeasureString(message2).X / 2;//message in the middle by width
                messagePos.Y = 0;
            
            }
            //Message2 slides down
            else if (messagePos.Y < limitY && currentdelay > toFirstMessage + FirstMessage + toSecondMessage
                              && currentdelay < toFirstMessage + FirstMessage + toSecondMessage+SecondMessage)
                messagePos.Y++;

            //Restore position
            else if (currentdelay == toFirstMessage + FirstMessage + toSecondMessage+SecondMessage)
            {
                messagePos.X = width / 2 - Fonts.MessageFont.MeasureString(message3).X / 2;//message in the middle by width
                messagePos.Y = 0;
            }

            //Message3 slides down
            else if (messagePos.Y < limitY && currentdelay > toFirstMessage + FirstMessage + toSecondMessage+SecondMessage+toFirstMessage
                              && currentdelay < toFirstMessage + FirstMessage + toSecondMessage + SecondMessage+toThirdMessage+ThirdMessage)
                messagePos.Y++;
           

                if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released
                    && GeneralImageRec.Contains(mstate.X, mstate.Y) && !finished && currentdelay>total) //To Next Stage
                {
                    finished = true;
                    ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage2()));
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

            if (currentdelay > toFirstMessage && currentdelay<toFirstMessage+FirstMessage )
                spriteBatch.DrawString(Fonts.MainFont, message, messagePos, Color.White);

            else if (currentdelay > toFirstMessage + FirstMessage + toSecondMessage 
                 && currentdelay < toFirstMessage + FirstMessage + toSecondMessage+SecondMessage)
                spriteBatch.DrawString(Fonts.MainFont, message2, messagePos, Color.White);

            else if (currentdelay > toFirstMessage + FirstMessage + toSecondMessage + SecondMessage+toThirdMessage
               && currentdelay < toFirstMessage + FirstMessage + toSecondMessage + SecondMessage+toThirdMessage+ThirdMessage)
                spriteBatch.DrawString(Fonts.MainFont, message3, messagePos, Color.White);

            else if(currentdelay > total)
                spriteBatch.DrawString(Fonts.MainFont, "", new Vector2(0, 
                    ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);
          
            spriteBatch.Draw(GeneralImageTexture, GeneralImageRec, Color.White);

            postit.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
