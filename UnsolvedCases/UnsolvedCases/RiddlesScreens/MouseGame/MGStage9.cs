#region File Description
/**
 * MGStag9: Reversed Mouse, Player must click on general image
 * On LoadContent method an external process is executed
 * Process Name: sakasa: A program that Inverts Mouse
 * 
 * On exiting process is terminated
 * 
 * **/
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
    public class MGStage9 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;
        int width, height;

        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;
        int GISpeedX = 6;        //initial speed of the general image by axis X
        int GISpeedY = 6;        //initial speed of the general image by axis Y
        int GIAcceleration = 2;  //after each click on general image,GISpeed increases
        int timesClicked = 0;    //times clicked on general image

        bool finished = false;    //If the general image is clicked many times, the MessageBoxScreen is 
                                  //appeared many times so we need to call MessageBoxScreen only once
        KeyboardState state;

        PostIt postit;

        #endregion

        #region Initialization

        public MGStage9()
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

            Mouse.SetPosition(width / 2, height / 2);     //place mouse on the center of the screen
            Oldmstate = Mouse.GetState();

            //Create postit it visible during the whole game
            postit = new PostIt(content);
            postit.Text = "MIRROR MIRRON ON THE WALL...";
            postit.Width = postit.Width / 2;
            postit.Height = postit.Height / 2;

            postit.Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - postit.Width, 10);
        }

        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        #endregion

        #region Public Methods

        public void ReverseMouse()
        {
            int diffY = 1;
            int diffX = 1;
          //  Oldmstate = mstate;
          //  mstate = Mouse.GetState();

            if ((Oldmstate.Y <= 0) && (mstate.Y <= 0))
                diffY = 1;
            else if ((Oldmstate.Y>=height-100) && (mstate.Y>=height-100))
                diffY = -1;                                 //using screen's original width and heigth the mouse dissapears
                                                            //on edge, so we use a safety distance=100

            if ((Oldmstate.X <= 0) && (mstate.X <= 0))
                diffX = 1;
            else if ((Oldmstate.X >= width-100) && (mstate.X >= width-100))
                diffX = -1;
            
           // if((mstate.X>=0)&&(mstate.X<=1300)&&(mstate.Y>=0)&&(mstate.Y<=700))
            Mouse.SetPosition(Oldmstate.X - (mstate.X - Oldmstate.X) + diffX, Oldmstate.Y - (mstate.Y - Oldmstate.Y) + diffY);
            mstate = Mouse.GetState();
        }

        bool IsMouseInsideWindow()//Viewport contains mouse position
        {
            return ScreenManager.GraphicsDevice.Viewport.Bounds.Contains(mstate.X, mstate.Y);
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            mstate = Mouse.GetState();

          
                if (!(mstate.X < -40 || mstate.X > width + 40 || //Mouse reverses only inside window 
                      mstate.Y < -40 || mstate.Y > height + 40))
                    ReverseMouse();

                    //If mouse is outside window we restore it 
                else if (mstate.X > -50 && mstate.X < 0)  //Restore X 
                {
                    Mouse.SetPosition(ScreenManager.GraphicsDevice.Viewport.X + 100, mstate.Y); //left 
                    AudioManager.PlayCue("Continue");
                }
                else if (mstate.X > width && mstate.X < width + 50)
                {
                    Mouse.SetPosition(ScreenManager.GraphicsDevice.Viewport.Bounds.Right - 100, mstate.Y);//right
                    AudioManager.PlayCue("Continue");
                }
                else if (mstate.Y > height && mstate.Y < height + 50)   //Restore Y
                {
                    Mouse.SetPosition(mstate.X, ScreenManager.GraphicsDevice.Viewport.Bounds.Bottom - 100);//down
                    AudioManager.PlayCue("Continue");
                }
                else if (mstate.Y < -30)
                {
                    Mouse.SetPosition(mstate.X, ScreenManager.GraphicsDevice.Viewport.Bounds.Top + 100); //up
                    AudioManager.PlayCue("Continue");
                }

            //General Image is bouncing around
            GeneralImageRec.X = GeneralImageRec.X + GISpeedX;
            if (((GeneralImageRec.X + GeneralImageRec.Width) > width) || (GeneralImageRec.X < 0))
            {
                GISpeedX = GISpeedX * -1;
            }

            GeneralImageRec.Y = GeneralImageRec.Y + GISpeedY;
            if (((GeneralImageRec.Y + GeneralImageRec.Height) > height) || (GeneralImageRec.Y < 0))
            {
                GISpeedY = GISpeedY * -1;
            }

            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released
               && GeneralImageRec.Contains(mstate.X, mstate.Y) && !finished)
            {
                timesClicked++;

                if (timesClicked <= 3)        //Increase speed on general image
                {
                    GISpeedX = Math.Abs(GISpeedX) + GIAcceleration;
                    GISpeedY += Math.Abs(GISpeedY) + GIAcceleration;
                }

                if (timesClicked > 3)         //To next stage
                {
                    finished = true;
                   
                    ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage10()));
                }
            }

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());

            Oldmstate = Mouse.GetState();
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.DrawString(Fonts.MainFont, "CLICK ON THE 'GENERAL IMAGE' 4 TIMES", new Vector2(0, 
                ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);
            spriteBatch.DrawString(Fonts.MessageFont, "GISpeed: " + GISpeedX, new Vector2(300, 0), Color.White);
            spriteBatch.DrawString(Fonts.MessageFont, "Mouse: " + mstate.X+", "+mstate.Y,  Vector2.Zero, Color.White);
            spriteBatch.Draw(GeneralImageTexture, GeneralImageRec, Color.White);

            postit.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
