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
    public sealed class Button
    {
        #region Fields

        ContentManager content;
        string name;
        string text;
        public Vector2 buttonPosition;
        Texture2D buttonImage;
        Rectangle buttonRectangle;
        Boolean buttonPressed;
        SpriteBatch spriteBatch;
        ScreenManager screenManager;
        int changedWidth, changedHeight;

        #endregion

        #region Properties

        public Vector2 Position
        {
            get { return buttonPosition; }
            set { buttonPosition = value; }
        }

        public bool ButtonPressed
        {
            get { return buttonPressed; }
            set { buttonPressed = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Rectangle ButtonRectangle
        {
            get { return buttonRectangle; }
        }

        public int Width
        {
            get { return changedWidth; }
            set { changedWidth = value; }
        }
        public int Height
        {
            get { return changedHeight; }
            set { changedHeight = value; }
        }

        #endregion

        #region Initialization

        public Button(ContentManager content, String name)
        {
            this.name = name;
            this.content = content;
            buttonImage = content.Load<Texture2D>(@"Textures\Buttons\" + name);
            buttonPressed = false;
            changedWidth = buttonImage.Width;
            changedHeight = buttonImage.Height;
            this.text = "";

            changedWidth = buttonImage.Width;
            changedHeight = buttonImage.Height;
        }

        public void PressedButtonIllusion(MouseState m)
        {
            MouseState mouse = m;

            buttonRectangle = new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, changedWidth, changedHeight);

            if (mouse.LeftButton == ButtonState.Pressed && buttonRectangle.Contains(mouse.X, mouse.Y))
            {
                ButtonPressed = true;
            }
            else
            {
                ButtonPressed = false;
            }
        }

        #endregion

        #region Draw

        public void Draw(GameScreen screen, GameTime gameTime)
        {
            screenManager = screen.ScreenManager;
            spriteBatch = screenManager.SpriteBatch;

            Vector2 helpPosition = new Vector2(0f, 0f);
            helpPosition.X +=this.Position.X+changedWidth/2;
            helpPosition.Y += this.Position.Y +changedHeight/2;
        
            if (buttonPressed)
            {
                spriteBatch.Draw(buttonImage, new Rectangle((int)buttonPosition.X,(int)buttonPosition.Y,
                                                                changedWidth,changedHeight), Color.Brown);
            }
            else
            {
                spriteBatch.Draw(buttonImage, new Rectangle((int)buttonPosition.X,(int)buttonPosition.Y,
                                                                changedWidth,changedHeight), Color.White);
            }

            spriteBatch.DrawString(Fonts.MainFont, this.Text,(helpPosition - Fonts.MainFont.MeasureString(this.Text)/2), Fonts.MainColor);

        }

        #endregion
    }    
}
