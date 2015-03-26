#region File Description
/**
 * MGStage8: The mouse speed is increasing and decreasing
 *           The GeneralImage speed is increasing and decreasing
 *           After each click things speed up a little
 * The user must click on the general image wich is moving around, 3 times
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
    public class MGStage8 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;

        int width, height;
       
        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;

        float MouseSpeed = 1.2f;                            //initial speed of the mouse
        int GISpeedX = 7;                                   //initial speed of the general image by axis X
        int GISpeedY = 7;                                   //initial speed of the general image by axis Y
        int timesClicked = 0;                               //times clicked on general image

        int GIAcceleration = 10;                            //after each click on general image,GISpeed increases
        float    MouseAcceleration = 0.1f;                  //after each click on general image,mouse speed increases

        bool finished = false;                              //If the general image is clicked many times, the MessageBoxScreen is 
                                                            //appeared many times so we need to call MessageBoxScreen only once
        KeyboardState state;

        PostIt postit;

        #endregion

        #region Initialization

        public MGStage8()
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

            Mouse.SetPosition(width / 2, height / 2);                   //place mouse on the center of the screen
            Oldmstate = Mouse.GetState();

            //Create postit it visible during the whole game
            postit = new PostIt(content);
            postit.Text = "TRY TO BE FAST...";
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

        public void IncreaseMouseSpeed(float speed)
        {
            int Xdist = mstate.X - Oldmstate.X;
            int Ydist = mstate.Y - Oldmstate.Y;
 
            int newX = mstate.X + (int)speed * Xdist;
            int newY = mstate.Y + (int)speed * Ydist;

            Mouse.SetPosition(newX, newY);
        }

        bool IsMouseInsideWindow()//Viewport contains mouse position
        {
            return  ScreenManager.GraphicsDevice.Viewport.Bounds.Contains(mstate.X,mstate.Y);
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            mstate = Mouse.GetState();

           

            if(IsMouseInsideWindow())   //Mouse goes crazy only inside viewport 
                IncreaseMouseSpeed(MouseSpeed);

            //General Image is bouncing around
            GeneralImageRec.X = GeneralImageRec.X + GISpeedX;

            if (((GeneralImageRec.X + GeneralImageRec.Width) > width) || (GeneralImageRec.X < 0))
            {
                GISpeedX = GISpeedX * -1;
            }

            GeneralImageRec.Y = GeneralImageRec.Y + GISpeedY;

            if (((GeneralImageRec.Y + GeneralImageRec.Height) >  height) || (GeneralImageRec.Y < 0))
            {
                GISpeedY = GISpeedY * -1;
            }
            
            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released
               && GeneralImageRec.Contains(mstate.X, mstate.Y) &&  !finished)
            {
                if (timesClicked < 3)
                {
                    MouseSpeed += MouseAcceleration;
                    GISpeedX += GIAcceleration;
                    GISpeedY += GIAcceleration;
                    timesClicked++;
                }

                if(timesClicked >= 3)
                {
                    finished = true;
                    ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage9()));
                }
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

          //  spriteBatch.DrawString(Fonts.MainFont, "CLICK ON THE 'GENERAL IMAGE' 3 TIMES", new Vector2(0, 
          //      ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);
            //spriteBatch.DrawString(Fonts.MessageFont, "Mouse Speed: " + MouseSpeed+", GISpeed: "+GISpeedX, 
            //new Vector2 (300,0), Color.White);
            spriteBatch.DrawString(Fonts.MainFont, "CLICKED: " + timesClicked, new Vector2(0, 300), Color.White);
            spriteBatch.Draw(GeneralImageTexture, GeneralImageRec, Color.White);

            postit.Draw(this, gameTime);
            spriteBatch.End();
        }

        #endregion
    }
}
