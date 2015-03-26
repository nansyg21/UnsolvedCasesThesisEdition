#region File Description
#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

#endregion

namespace UnsolvedCases
{
    public class Textfield
    {
        #region Fields

        string text;
        string currentText;

        ContentManager content;

        SpriteFont font;

        Vector2 position;

        Color color;

        bool isClicked;
        bool buttonPressed;
        bool firstLetter;
        bool capsLock;

        #endregion

        #region Constants

        private const string start = "[ CLICK HERE TO ENTER YOUR USERNAME ]";

        #endregion

        #region Properties

        public bool Clicked
        {
            get { return isClicked; }
            set { isClicked = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion

        #region Initialization

        public Textfield(ContentManager content, SpriteFont font, string text, Color color)
        {
            this.content = content;
            this.text = text;
            this.font = font;
            this.color = color;
            isClicked = false;
            buttonPressed = false;
            firstLetter = false;
        }

        #endregion

        #region Updating

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        public void HandleLeftMouseClick()
        {
             if (Clicked == false)
             {
                 Clicked = true;
                 this.text = "_";
                 firstLetter = true;
              }
         }

         public virtual void Update(GameScreen screen, GameTime gameTime, KeyboardState previousState, KeyboardState currentState)
         {
             capsLock = Console.CapsLock;

             if (Clicked == true)
             {
                 if (currentState.IsKeyDown(Keys.A))
                 {
                     if (!previousState.IsKeyDown(Keys.A))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "A";
                         else
                             currentText = "a";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.B))
                 {
                     if (!previousState.IsKeyDown(Keys.B))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "B";
                         else
                             currentText = "b";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.C))
                 {
                     if (!previousState.IsKeyDown(Keys.C))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "C";
                         else
                             currentText = "c";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D))
                 {
                     if (!previousState.IsKeyDown(Keys.D))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "D";
                         else
                             currentText = "d";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.E))
                 {
                     if (!previousState.IsKeyDown(Keys.E))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "E";
                         else
                             currentText = "e";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.F))
                 {
                     if (!previousState.IsKeyDown(Keys.F))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "F";
                         else
                             currentText = "f";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.G))
                 {
                     if (!previousState.IsKeyDown(Keys.G))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "G";
                         else
                             currentText = "g";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.H))
                 {
                     if (!previousState.IsKeyDown(Keys.H))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "H";
                         else
                             currentText = "h";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.I))
                 {
                     if (!previousState.IsKeyDown(Keys.I))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "I";
                         else
                             currentText = "i";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.J))
                 {
                     if (!previousState.IsKeyDown(Keys.J))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "J";
                         else
                             currentText = "j";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.K))
                 {
                     if (!previousState.IsKeyDown(Keys.K))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "K";
                         else
                             currentText = "k";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.L))
                 {
                     if (!previousState.IsKeyDown(Keys.L))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "L";
                         else
                             currentText = "l";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.M))
                 {
                     if (!previousState.IsKeyDown(Keys.M))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "M";
                         else
                             currentText = "m";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.N))
                 {
                     if (!previousState.IsKeyDown(Keys.N))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "N";
                         else
                             currentText = "n";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.O))
                 {
                     if (!previousState.IsKeyDown(Keys.O))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "O";
                         else
                             currentText = "o";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.P))
                 {
                     if (!previousState.IsKeyDown(Keys.P))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "P";
                         else
                             currentText = "p";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.Q))
                 {
                     if (!previousState.IsKeyDown(Keys.Q))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "Q";
                         else
                             currentText = "q";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.R))
                 {
                     if (!previousState.IsKeyDown(Keys.R))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "R";
                         else
                             currentText = "r";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.S))
                 {
                     if (!previousState.IsKeyDown(Keys.S))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "S";
                         else
                             currentText = "s";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.T))
                 {
                     if (!previousState.IsKeyDown(Keys.T))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "T";
                         else
                             currentText = "t";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.U))
                 {
                     if (!previousState.IsKeyDown(Keys.U))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "U";
                         else
                             currentText = "u";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.V))
                 {
                     if (!previousState.IsKeyDown(Keys.V))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "V";
                         else
                             currentText = "v";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.W))
                 {
                     if (!previousState.IsKeyDown(Keys.W))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "W";
                         else
                             currentText = "w";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.X))
                 {
                     if (!previousState.IsKeyDown(Keys.X))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "X";
                         else
                             currentText = "x";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.Y))
                 {
                     if (!previousState.IsKeyDown(Keys.Y))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "Y";
                         else
                             currentText = "y";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.Z))
                 {
                     if (!previousState.IsKeyDown(Keys.Z))
                     {
                         buttonPressed = true;

                         if (capsLock)
                             currentText = "Z";
                         else
                             currentText = "z";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D0) || currentState.IsKeyDown(Keys.NumPad0))
                 {
                     if (!previousState.IsKeyDown(Keys.D0) && !previousState.IsKeyDown(Keys.NumPad0))
                     {
                         buttonPressed = true;
                         currentText = "0";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D1) || currentState.IsKeyDown(Keys.NumPad1))
                 {
                     if (!previousState.IsKeyDown(Keys.D1) && !previousState.IsKeyDown(Keys.NumPad1))
                     {
                         buttonPressed = true;
                         currentText = "1";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D2) || currentState.IsKeyDown(Keys.NumPad2))
                 {
                     if (!previousState.IsKeyDown(Keys.D2) && !previousState.IsKeyDown(Keys.NumPad2))
                     {
                         buttonPressed = true;
                         currentText = "2";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D3) || currentState.IsKeyDown(Keys.NumPad3))
                 {
                     if (!previousState.IsKeyDown(Keys.D3) && !previousState.IsKeyDown(Keys.NumPad3))
                     {
                         buttonPressed = true;
                         currentText = "3";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D4) || currentState.IsKeyDown(Keys.NumPad4))
                 {
                     if (!previousState.IsKeyDown(Keys.D4) && !previousState.IsKeyDown(Keys.NumPad4))
                     {
                         buttonPressed = true;
                         currentText = "4";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D5) || currentState.IsKeyDown(Keys.NumPad5))
                 {
                     if (!previousState.IsKeyDown(Keys.D5) && !previousState.IsKeyDown(Keys.NumPad5))
                     {
                         buttonPressed = true;
                         currentText = "5";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D6) || currentState.IsKeyDown(Keys.NumPad6))
                 {
                     if (!previousState.IsKeyDown(Keys.D6) && !previousState.IsKeyDown(Keys.NumPad6))
                     {
                         buttonPressed = true;
                         currentText = "6";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D7) || currentState.IsKeyDown(Keys.NumPad7))
                 {
                     if (!previousState.IsKeyDown(Keys.D7) && !previousState.IsKeyDown(Keys.NumPad7))
                     {
                         buttonPressed = true;
                         currentText = "7";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D8) || currentState.IsKeyDown(Keys.NumPad8))
                 {
                     if (!previousState.IsKeyDown(Keys.D8) && !previousState.IsKeyDown(Keys.NumPad8))
                     {
                         buttonPressed = true;
                         currentText = "8";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.D9) || currentState.IsKeyDown(Keys.NumPad9))
                 {
                     if (!previousState.IsKeyDown(Keys.D9) && !previousState.IsKeyDown(Keys.NumPad9))
                     {
                         buttonPressed = true;
                         currentText = "9";
                     }
                 }
                 else if (currentState.IsKeyDown(Keys.Back))
                 {
                     if (!previousState.IsKeyDown(Keys.Back))
                     {
                         if (this.text.Length > 0)
                         {
                             buttonPressed = false;
                             this.text = this.text.Remove(this.text.Length - 1, 1);
                         }
                     }
                 }
             }

             if (firstLetter && buttonPressed)
             {
                 this.text = currentText;
                 firstLetter = false;
                 buttonPressed = false;
             }
             else if (buttonPressed)
             {
                 this.text = this.text + currentText;
                 buttonPressed = false;
             }
        }

        #endregion

        #region Draw

         public virtual void Draw(GameScreen screen, GameTime gameTime)
         {
             ScreenManager screenManager = screen.ScreenManager;
             SpriteBatch spriteBatch = screenManager.SpriteBatch;

             spriteBatch.DrawString(font, text, position, color);
         }

        #endregion

        #region Public Methods

         /// <summary>
         /// Queries how wide the entry is, used for centering on the screen.
         /// </summary>
         public int GetHeight()
         {
             return (int)font.MeasureString(start).Y;
         }

         /// <summary>
         /// Queries how wide the entry is, used for centering on the screen.
         /// </summary>
         public int GetWidth()
         {
             return (int)font.MeasureString(start).X;
         }

        #endregion
    }
}
