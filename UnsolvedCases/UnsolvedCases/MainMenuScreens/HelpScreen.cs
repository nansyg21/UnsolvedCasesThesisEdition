#region File Description
#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

#endregion

namespace UnsolvedCases
{
    public class HelpScreen : GameScreen
    {
        #region Fields

        Texture2D background;

        Texture2D logo;
        Rectangle logoRectangle;

        Texture2D book;
        Rectangle bookRectangle;

        Vector2 titleRectangle;

        Texture2D image;
        Rectangle imageRectangle;

        Texture2D line;
        float angle;
        float dist;
        Vector2 point1;
        Vector2 point2;

        Button backButton;

        int changedWidth;
        int changedHeight;

        String mainbuttons;
        String riddleChar;
        String riddleInstructions;

        Texture2D leftarrow;
        Texture2D rightarrow;

        Rectangle leftarrowRect;
        Rectangle rightarrowRect;

        String[] riddleCharTable;
        String[] riddleInstructionsTable;
        Texture2D[] riddleImageTable;

        int screenWidth, screenHeight;

        MouseState mouse;
        MouseState oldmouse;

        int riddleNum;

        #endregion

        #region Initialization

        public HelpScreen()
        {
            riddleCharTable=new String[5];
            riddleInstructionsTable = new String[5];
            riddleImageTable = new Texture2D[5];
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

             /*Screens width and height*/
            screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;

            background = content.Load<Texture2D>(@"Textures\SubMenu\Background");

            logo = content.Load<Texture2D>(@"Textures\GameLogo");
            logoRectangle = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 150, 0,
                ScreenManager.GraphicsDevice.Viewport.Width / 5, ScreenManager.GraphicsDevice.Viewport.Height / 5);

            titleRectangle = new Vector2(0, logoRectangle.Height);

            book = content.Load<Texture2D>(@"Textures\SubMenu\Open_Book");
            bookRectangle = new Rectangle(50, (int)titleRectangle.Y + 50, ScreenManager.GraphicsDevice.Viewport.Width - 50,
                                             ScreenManager.GraphicsDevice.Viewport.Height - 200);

            changedWidth = book.Width;
            changedHeight = book.Height;

            image = content.Load<Texture2D>(@"Textures\TrialImage");
            imageRectangle = new Rectangle(730, 415, 430, 200);

            line = content.Load<Texture2D>(@"Textures\Line");
            point1 = new Vector2(0, titleRectangle.Y + 55);
            point2 = new Vector2(titleRectangle.X + 480, titleRectangle.Y + 55);
            angle = (float)System.Math.Atan2(point1.Y - point2.Y, point1.X - point2.X);
            dist = Vector2.Distance(point1, point2);

            backButton = new Button(content, "Button");
            backButton.buttonPosition = new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height - 40);
            backButton.Text = "BACK";

            mainbuttons = "MAIN BUTTONS\n\nUP: UP ARROW\nDOWN: DOWN ARROW\nRIGHT: RIGHT ARROW\nLEFT: LEFT ARROW\n\n";
          //  riddleChar = "RIDDLE\n\nNAME: MEMORY GAME\nCASE: GARDNER MUSEUM HEIST";
          //  riddleInstructions = "INSTRUCTIONS\n\nYOU MUST FIND THE SAME PICTURES IN THE GIVEN NUMBER OF CLICKS.";

            riddleCharTable[0] = "RIDDLE\n\nNAME: PASSWORD LOCK\nCASE: GARDNER MUSEUM HEIST";
            riddleInstructionsTable[0] = "INSTRUCTIONS\n\nYOU MUST ENTER THE CORRECT PASSWORD TO UNLOCK THE NEXT ROOM.";

            riddleCharTable[1] = "RIDDLE\n\nNAME: PLATFORM GAME\nCASE: GARDNER MUSEUM HEIST";
            riddleInstructionsTable[1] = "INSTRUCTIONS\n\nJUMP ON PLATFORMS COLLECT THE ITEMS AND SHOOT THE PAINTING DOWN.";

            riddleCharTable[2] = "RIDDLE\n\nNAME: PARALLEL PUZZLE\nCASE: GARDNER MUSEUM HEIST";
            riddleInstructionsTable[2] = "INSTRUCTIONS\n\nSOLVE 4 PUZZLES USING MOUSE. FIND THE CRITICAL PIECE IN TIME.";

            riddleCharTable[3] = "RIDDLE\n\nNAME: SUSPECT ESCAPE\nCASE: GARDNER MUSEUM HEIST";
            riddleInstructionsTable[3] = "INSTRUCTIONS\n\nCLICK TO BLOCK THE WAY AND PREVENT THE SUSPECT TO ESCAPE.";

            riddleCharTable[4] = "RIDDLE\n\nNAME: MOUSE GAME\nCASE: GARDNER MUSEUM HEIST";
            riddleInstructionsTable[4] = "INSTRUCTIONS\n\nUSE YOUR BRAIN AND MOUSE TO SOLVE THE RIDDLE IN EVERY LEVEL.";

            riddleChar = riddleCharTable[0];
            riddleInstructions = riddleInstructionsTable[0];

            riddleNum = 0;

            leftarrow = content.Load<Texture2D>(@"Misc\left_arrow");
            rightarrow = content.Load<Texture2D>(@"Misc\right_arrow");
            leftarrowRect = new Rectangle(10,screenHeight/2-leftarrow.Height/4,leftarrow.Width/2,leftarrow.Height/2);
            rightarrowRect = new Rectangle(screenWidth-(rightarrow.Width/2)-10, screenHeight / 2 - leftarrow.Height / 4, rightarrow.Width/2, rightarrow.Height/2);
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                if (leftarrowRect.Contains(mouse.X, mouse.Y))
                {
                    if (riddleNum == 0)
                        riddleNum = riddleCharTable.Length - 1;
                    else
                        riddleNum--;
                    riddleChar = riddleCharTable[riddleNum];
                    riddleInstructions = riddleInstructionsTable[riddleNum];
                }
                if (rightarrowRect.Contains(mouse.X, mouse.Y))
                {
                    if (riddleNum == riddleCharTable.Length - 1)
                        riddleNum = 0;
                    else
                        riddleNum++;
                    riddleChar = riddleCharTable[riddleNum];
                    riddleInstructions = riddleInstructionsTable[riddleNum];
                }
                
            }
            oldmouse = mouse;
            backButton.PressedButtonIllusion(mouse);
        }

        public override void HandleInput()
        {
            if (backButton.ButtonPressed)
            {
                AudioManager.PlayCue("Continue");
                LoadingScreen.Load(ScreenManager, false, null, new MainMenuScreen());
            }

            
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            
            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, 
                                ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(logo, logoRectangle, Color.White);
            spriteBatch.DrawString(Fonts.HelpFont, "INSTRUCTIONS", titleRectangle, Color.White);
            spriteBatch.Draw(book, bookRectangle, Color.White);

            Vector2 helpPosition = new Vector2(0f, 0f);
            helpPosition.X += bookRectangle.X;
            helpPosition.Y += bookRectangle.Y;

            spriteBatch.DrawString(Fonts.MainFont, parseText(mainbuttons+riddleChar), new Vector2(bookRectangle.X + (changedWidth / 6), bookRectangle.Y + (changedHeight / 19)), Fonts.MainColor);
            spriteBatch.DrawString(Fonts.MainFont, parseText(riddleInstructions), new Vector2(bookRectangle.X + (changedWidth / 2) + (changedWidth / 9), bookRectangle.Y + (changedHeight / 19)), Fonts.MainColor);
           
           // spriteBatch.DrawString(Fonts.MainFont, "MAIN BUTTONS\n\nUP: UP ARROW\nDOWN: DOWN ARROW\nRIGHT: RIGHT ARROW\nLEFT: LEFT ARROW\n\nRIDDLE\n\nRIDDLE GAME: MEMORY GAME\nCASE: GARDNER MUSEUM HEIST", 
           //     new Vector2(bookRectangle.X + 180, bookRectangle.Y + 40), Color.Black);
           // spriteBatch.DrawString(Fonts.MainFont, "INSTRUCTIONS\n\nYOU MUST FIND THE SAME PICTURES IN THE GIVEN NUMBER OF\nCLICKS.",
           //  new Vector2(bookRectangle.X + 665, bookRectangle.Y + 40), Color.Black);
            spriteBatch.Draw(image, imageRectangle, Color.White);
            spriteBatch.Draw(line, new Rectangle((int)point2.X, (int)point2.Y, (int)dist, 4), null, Color.White, angle, 
                Vector2.Zero, SpriteEffects.None, 0);
            backButton.Draw(this, gameTime);

            spriteBatch.Draw(leftarrow,leftarrowRect,Color.White);
            spriteBatch.Draw(rightarrow,rightarrowRect,Color.White);
 
            spriteBatch.End();
        }

        #endregion

        private String parseText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (Fonts.MainFont.MeasureString(line + word).Length() > ((changedWidth/2)-(changedWidth/15)))
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
