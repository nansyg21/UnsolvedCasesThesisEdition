using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnsolvedCases.Toolbox
{
    public sealed class PostIt
    {
        #region Fields
        ContentManager content;
        Vector2 postitPosition;
        Texture2D postitImage;
        Boolean shown; //Visible or not
        SpriteBatch spriteBatch;
        ScreenManager screenManager;
        int changedWidth, changedHeight;
        string text;

        #endregion

        #region Properties
        public Vector2 Position
        {
            get { return postitPosition; }
            set { postitPosition = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
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

        public Boolean Visible
        {
            get { return shown; }
            set { shown = value; }
        }
        #endregion

        #region Initialization

        /// <summary>
        /// This is for showing the post it with guidelines in each level
        /// </summary>
        /// <param name="content">Content manager</param>
        /** 
        * postitImage -> The yellow postit image
        * changedWidth -> In order to set our prefered width of the post it - initialized with the images original width
        * changedHeight -> In order to set our prefered height of the post it - initialized with the images original height
        * text -> In order to set our prefered width of the post it - initialized with the images original width
         */
        public PostIt(ContentManager content)
        {
            this.content = content;
            postitImage = content.Load<Texture2D>(@"Textures\Stages\StageIcons\post-it");
            changedWidth = postitImage.Width;
            changedHeight = postitImage.Height;
            this.text = "";
        }


        #endregion


        #region Draw

        public void Draw(GameScreen screen, GameTime gameTime)
        {
            screenManager = screen.ScreenManager;
            spriteBatch = screenManager.SpriteBatch;

            Vector2 helpPosition = new Vector2(0f, 0f);
            helpPosition.X += this.Position.X;
            helpPosition.Y += this.Position.Y;

            //Draw the yellow image
            spriteBatch.Draw(postitImage, new Rectangle((int)postitPosition.X, (int)postitPosition.Y,
                                                                changedWidth, changedHeight), Color.White);
            //Draw the string in the image boundaries
            spriteBatch.DrawString(Fonts.MainFont, parseText(text), new Vector2(this.Position.X+(changedWidth/5),this.Position.Y+(changedHeight/5)), Fonts.MainColor);
           
        }

        #endregion

        /** This method is used in order to draw the string in the boundaries of the background sprite
         * If the string is longer then it wraps in multiple lines
         * */
        private String parseText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (Fonts.MainFont.MeasureString(line + word).Length() > (changedWidth-(changedWidth/3)))
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }

    }
}
