#region File Description
/**
 * MGStag10: Initially all buttons are open
 * By clicking on a button some buttons change
 * Our goal is to close all buttons
 * 
 * Open button: drawn with small Alpha 
 * Closed button: drawn NORMAL
 * 
 * The general image is clickable only inside the square
 * If all the buttons are closed the general gets inside the square
 * 
 * SOLUTION: Click all the buttons once
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
    public class MGStage10 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;
        int width, height;

        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;

        Texture2D squareTex;//square in the center
        Rectangle sqRec;

        Texture2D button1;
        Texture2D button2;
        Texture2D button3;
        Texture2D button4;


        static int rows = 3;     //number of rows
        static int collumns = 4;//number of collumns

        Rectangle[] buttonRecs = new Rectangle[rows * collumns];//array for all the buttons rectangles;
        bool[] closedButton = new bool[rows * collumns];        //array to hold closed buttons

        int bWidth = 100;
        int bHeigth = 40;
        int completedButtons = 0;//how many buttons are open
        int step;               //step which moves the general image


        bool finished = false;      //If the general image is clicked many times, the MessageBoxScreen is appeared many times 
        //so we need to call MessageBoxScreen only once
        KeyboardState state;

        PostIt postit; 

        #endregion

        #region Initialization

        public MGStage10()
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
            step = width / 20;

            GeneralImageRec = new Rectangle(width / 2 + width / 12, height / 2, 80, 80);
            GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");

            sqRec = new Rectangle(width / 2, height / 2, GeneralImageRec.Width, GeneralImageRec.Height);
            squareTex = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage10\square");

            button1 = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage10\button1");
            button2 = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage10\button2");
            button3 = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage10\button3");
            button4 = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage10\button4");

            buttonRecs[0] = new Rectangle(10, 10, bWidth, bHeigth);
            buttonRecs[1] = new Rectangle(buttonRecs[0].X + buttonRecs[0].Width + bWidth, buttonRecs[0].Y, bWidth, bHeigth);
            buttonRecs[2] = new Rectangle(buttonRecs[1].X + buttonRecs[1].Width + bWidth, buttonRecs[1].Y, bWidth, bHeigth);
            buttonRecs[3] = new Rectangle(buttonRecs[2].X + buttonRecs[2].Width + bWidth, buttonRecs[2].Y, bWidth, bHeigth);

            buttonRecs[4] = new Rectangle(buttonRecs[0].X, buttonRecs[0].Y + buttonRecs[0].Height + bHeigth, bWidth, bHeigth);
            buttonRecs[5] = new Rectangle(buttonRecs[4].X + buttonRecs[4].Width + bWidth, buttonRecs[4].Y, bWidth, bHeigth);
            buttonRecs[6] = new Rectangle(buttonRecs[5].X + buttonRecs[5].Width + bWidth, buttonRecs[5].Y, bWidth, bHeigth);
            buttonRecs[7] = new Rectangle(buttonRecs[6].X + buttonRecs[6].Width + bWidth, buttonRecs[6].Y, bWidth, bHeigth);

            buttonRecs[8] = new Rectangle(buttonRecs[4].X, buttonRecs[4].Y + buttonRecs[4].Height + bHeigth, bWidth, bHeigth);
            buttonRecs[9] = new Rectangle(buttonRecs[8].X + buttonRecs[8].Width + bWidth, buttonRecs[8].Y, bWidth, bHeigth);
            buttonRecs[10] = new Rectangle(buttonRecs[9].X + buttonRecs[9].Width + bWidth, buttonRecs[9].Y, bWidth, bHeigth);
            buttonRecs[11] = new Rectangle(buttonRecs[10].X + buttonRecs[10].Width + bWidth, buttonRecs[10].Y, bWidth, bHeigth);

            for (int i = 0; i < rows * collumns; i++)
                closedButton[i] = false;

            Oldmstate = Mouse.GetState();

            //Create postit it visible during the whole game
            postit = new PostIt(content);
            postit.Text = "TURN ALL THE LIGHTS ON...";
            postit.Width = postit.Width / 2;
            postit.Height = postit.Height / 2;

            postit.Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - postit.Width, 10);
        }

        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        #endregion

        #region Private Methods

        private int FindClickedButton()//Uses a loop to find the buttonn the user clicked on
        {
            int i = 0;
            foreach (Rectangle rec in buttonRecs)
            {
                if (rec.Contains(mstate.X, mstate.Y))
                    return i;
                else
                    i++;
            }
            return -1;
        }

        private void CloseButton(int pos)//Closes the button (starting from 0)
        {
            try
            {
                if (!closedButton[pos])
                {
                    closedButton[pos] = true;  //if it is open: close button
                    completedButtons++;
                }
            }
            catch (Exception e) { }//do nothing
        }

        private void OpenButton(int pos) //Opens the button (starting from 0)
        {
            try
            {

                if (closedButton[pos])
                {
                    closedButton[pos] = false; //if it is open: close button
                    completedButtons--;
                }
            }
            catch (Exception e) { }//do nothing
        }

        private void SwitchButton(int pos)//If button is closed: close it, else open it (starting from 0)
        {
            try
            {
                if (!closedButton[pos])
                    CloseButton(pos);
                else
                    OpenButton(pos);
            }
            catch (Exception e) { Console.WriteLine(pos + "Wrong index"); }
        }

        private bool AllButtonsAreClosed() //Uses a for loop to see if all buttons are closed
        {
            //returns true: if everything in closedButton is true 
            //returns false: if we find at least one false

            foreach (bool b in closedButton)
            {

                if (!b)
                    return false;
            }
            return true;
        }

        private void CheckConnectionsBetweenButtons(int pos)//Here we determine the connections between buttons(ex:1 switches button 5)
        {
            if (pos == 0)
            {
                SwitchButton(5);
            }
            else if (pos == 1)
            {
                SwitchButton(7);
            }
            else if (pos == 2)
            {
                SwitchButton(5);
                SwitchButton(4);
            }
            else if (pos == 3)
            {
                SwitchButton(2);
            }
            else if (pos == 4)
            {
                SwitchButton(9);
            }
            else if (pos == 5)
            {
                SwitchButton(11);
            }
            else if (pos == 6)
            {
                SwitchButton(0);
            }
            else if (pos == 7)
            {

                SwitchButton(8);
            }
            else if (pos == 8)
            {
                SwitchButton(5);
                SwitchButton(6);
            }
            else if (pos == 9)
            {
                SwitchButton(1);
            }
            else if (pos == 10)
            {
                SwitchButton(5);
                SwitchButton(3);
            }
            else if (pos == 11)
            {
                SwitchButton(5);
                SwitchButton(10);
            }
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            mstate = Mouse.GetState();

            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released)
            {
                int k;

                if ((k = FindClickedButton()) != -1)//clicked on button
                {

                    /**kleinei ta geitonika
                    int curntrow = k / collumns;//find the row (starting from zero)
                    SwitchButton(k);

                    if (k + 1 <= curntrow * collumns + collumns - 1)
                        SwitchButton(k + 1);

                    if (k - 1 >= curntrow * collumns)
                        SwitchButton(k - 1);

                    SwitchButton(k - collumns);
                    SwitchButton(k + collumns);
                    **/

                    CheckConnectionsBetweenButtons(k);
                }
            }


            //Check if stage is over
            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released
                && completedButtons == 12 && GeneralImageRec.Contains(mstate.X, mstate.Y) && !finished)
            {
                finished = true;
                ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage11()));
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

            Color clr = Color.Brown;
            clr.A = 100;

            for (int i = 0; i < rows * collumns; i++)
            {
                if (!closedButton[i])
                    spriteBatch.Draw(button1, buttonRecs[i], clr);
                else
                    spriteBatch.Draw(button1, buttonRecs[i], Color.White);
            }

            spriteBatch.DrawString(Fonts.MainFont, "CLICK ON THE 'GENERAL IMAGE'",
                  new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);


            GeneralImageRec.X = width / 2 + ((rows * collumns) - completedButtons) * step;//percentage completed
            spriteBatch.Draw(GeneralImageTexture, GeneralImageRec, Color.White);


            spriteBatch.Draw(squareTex, sqRec, Color.White);

            postit.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
