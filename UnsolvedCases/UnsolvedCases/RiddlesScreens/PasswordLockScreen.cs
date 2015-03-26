#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnsolvedCases.Toolbox;

#endregion

namespace UnsolvedCases
{
    public class PasswordLockScreen : GameScreen
    {
        #region Fields

        Texture2D background;
        ContentManager content;
        
        Texture2D tableauSprite;        //The background of the calculator
        Rectangle tableauRec;  
        Texture2D displaySprite;        //The display of the calculator
        Rectangle displayRec;
        Texture2D buttonsSprites;       //Helps to know the exact size of the button image
        List<Button> buttons;
        Boolean initialization=true;    //Used to know if initialization of the screen is needed or not
        String password;                //The password the user gives
        Vector2 passwordPosition;       //The possition of the string must be in the display screen
        MouseState mouse;
        MouseState currentMS;
        MouseState previousMS;
        PostIt postit;                  //The postit for the level
        #endregion

        #region Initialization

        /// <summary>
        /// This screen is for the first riddle of the game where the player has to enter a 4digit password in order to move on
        /// </summary>
       
        public PasswordLockScreen()
        {
            displayRec = new Rectangle(10, 10, 400, 400);
            tableauRec = new Rectangle(10, 10, 10, 10);
            buttons = new List<Button>();
            passwordPosition = new Vector2(0f, 0f);
           
        }

        public override void LoadContent()
        {
            ScreenManager.MainGame.IsMouseVisible = true;
           
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
           
            background = content.Load<Texture2D>("Textures/Riddles/Puzzle/puzzle_background");
            tableauSprite = content.Load<Texture2D>("Textures/Riddles/PasswordLock/tableau1");

            //Load in total 12 identical buttons
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));
            buttons.Add(new Button(content, "num_button"));

            //Set the size of the buttons
            foreach(Button b in buttons)
            {
                b.Width = ScreenManager.GraphicsDevice.Viewport.Width /12;
                b.Height = ScreenManager.GraphicsDevice.Viewport.Width / 12;

            }


            buttonsSprites = content.Load<Texture2D>("Textures/Buttons/num_button");
           

            displaySprite = content.Load<Texture2D>("Textures/Riddles/PasswordLock/displayscreen");

            password = "";

            //Create postit it visible during the whole game
            postit = new PostIt(content);
            postit.Text = "INSERT THE PASSWORD TO ENTER THE ROOM...";
            postit.Width = postit.Width / 2;
            postit.Height = postit.Height / 2;
       
            //Initialize mouse states
            currentMS = Mouse.GetState();
            previousMS=Mouse.GetState();

         }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Used to set all the labels location on the screen
        /// </summary>
        protected virtual void UpdateLabelLocations()
        {
            tableauRec.Width=ScreenManager.GraphicsDevice.Viewport.Height-(ScreenManager.GraphicsDevice.Viewport.Height/20);
            tableauRec.Height=ScreenManager.GraphicsDevice.Viewport.Height-(ScreenManager.GraphicsDevice.Viewport.Height/30);
            tableauRec.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - ((ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 2);
            tableauRec.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - ((ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 2);

            displayRec.Width = ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 5);
            displayRec.Height = ScreenManager.GraphicsDevice.Viewport.Height / 10;
            displayRec.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (3*(ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 7);
            displayRec.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - ((ScreenManager.GraphicsDevice.Viewport.Height - (2*(ScreenManager.GraphicsDevice.Viewport.Height / 3))) );

            passwordPosition.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (3 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 4)) / 7);
            passwordPosition.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - ((ScreenManager.GraphicsDevice.Viewport.Height - (5 * (ScreenManager.GraphicsDevice.Viewport.Height / 7))));

            
            postit.Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width-postit.Width,10);

            buttons[2].Position = new Vector2((tableauRec.X + ((ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 2) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 2 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);
            buttons[5].Position = new Vector2((tableauRec.X + ((ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 2) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 3 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);
            buttons[8].Position = new Vector2((tableauRec.X + ((ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 2) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 4 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);
            buttons[0].Position = new Vector2((tableauRec.X + ((ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 2) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 5 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);

            buttons[1].Position = new Vector2((tableauRec.X + ((ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 4) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 2 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);
            buttons[4].Position = new Vector2((tableauRec.X + ((ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 4) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 3 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);
            buttons[7].Position = new Vector2((tableauRec.X + ((ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 4) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 4 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);
            buttons[10].Position = new Vector2((tableauRec.X + ((ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 4) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 5 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);

            buttons[3].Position = new Vector2((tableauRec.X + (3 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 4) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 2 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);
            buttons[6].Position = new Vector2((tableauRec.X + (3 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 4) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 3 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);
            buttons[9].Position = new Vector2((tableauRec.X + (3 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 4) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 4 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);
            buttons[11].Position = new Vector2((tableauRec.X + (3 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 4) - (ScreenManager.GraphicsDevice.Viewport.Width / 20)), 5 * (ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height / 20)) / 6);
           
            //Set text to the buttons
            for (int i = 0; i < 10; i++)
            {
                buttons[i].Text = i.ToString();

            }

            buttons[10].Text = "OK";
            buttons[11].Text = "DEL";

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
           
       
            base.Update(gameTime, otherScreenHasFocus, false);

            //Calculate positions only if initialization is needed
            if (initialization)
            {
                initialization = false;
                UpdateLabelLocations();
            }
            mouse = Mouse.GetState();

            buttons[0].PressedButtonIllusion(mouse);
            buttons[1].PressedButtonIllusion(mouse);
            buttons[2].PressedButtonIllusion(mouse);
            buttons[3].PressedButtonIllusion(mouse);
            buttons[4].PressedButtonIllusion(mouse);
            buttons[5].PressedButtonIllusion(mouse);
            buttons[6].PressedButtonIllusion(mouse);
            buttons[7].PressedButtonIllusion(mouse);
            buttons[8].PressedButtonIllusion(mouse);
            buttons[9].PressedButtonIllusion(mouse);
            buttons[10].PressedButtonIllusion(mouse);
            buttons[11].PressedButtonIllusion(mouse);
        }

        public override void HandleInput()
        {
            currentMS = Mouse.GetState();
           
            /** Check if the player clicked on any button
             * If it is a number button add it to the password - only once per click
             * If it is delete then delete the last number of the password
             * If it is ok then check if the password is correct
             * */
            if ((currentMS.LeftButton == ButtonState.Pressed) && (previousMS.LeftButton == ButtonState.Released))
            {
                if (buttons[0].ButtonPressed)
                    password = password + "0";
                else if (buttons[1].ButtonPressed)
                    password = password + "1";
                else if (buttons[1].ButtonPressed)
                    password = password + "1";
                else if (buttons[2].ButtonPressed)
                    password = password + "2";
                else if (buttons[3].ButtonPressed)
                    password = password + "3";
                else if (buttons[4].ButtonPressed)
                    password = password + "4";
                else if (buttons[5].ButtonPressed)
                    password = password + "5";
                else if (buttons[6].ButtonPressed)
                    password = password + "6";
                else if (buttons[7].ButtonPressed)
                    password = password + "7";
                else if (buttons[8].ButtonPressed)
                    password = password + "8";
                else if (buttons[9].ButtonPressed)
                    password = password + "9";
                else if (buttons[11].ButtonPressed)
                {
                    if (password.Length > 0)
                        password = password.Remove(password.Length - 1, 1);
                }
                else if (buttons[10].ButtonPressed)
                {
                    if (password.Equals("1924"))
                    {
                        ScreenManager.AddScreen(new MessageBoxScreen("DOOR'S UNLOCKED! \nWELL DONE!"));
                    }
                    else
                    {
                        ScreenManager.AddScreen(new MessageBoxScreen("WRONG PASSWORD \nKEEP TRYING!"));
                        password = "";
                    }
                }
                
            }
            previousMS = currentMS;
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            UpdateLabelLocations();

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(tableauSprite,tableauRec,Color.White);
            buttons[2].Draw(this, gameTime);
            buttons[5].Draw(this, gameTime);
            buttons[8].Draw(this, gameTime);
            buttons[0].Draw(this, gameTime);

            buttons[1].Draw(this, gameTime);
            buttons[4].Draw(this, gameTime);
            buttons[7].Draw(this, gameTime);
            buttons[10].Draw(this, gameTime);

            buttons[3].Draw(this, gameTime);
            buttons[6].Draw(this, gameTime);
            buttons[9].Draw(this, gameTime);
            buttons[11].Draw(this, gameTime);

            spriteBatch.Draw(displaySprite,displayRec,Color.White);

            postit.Draw(this,gameTime);

            spriteBatch.DrawString(Fonts.MainFont, password, passwordPosition, Fonts.MainColor);
            
            spriteBatch.End();
        }
        
        #endregion

    }
}

        



       

       

       