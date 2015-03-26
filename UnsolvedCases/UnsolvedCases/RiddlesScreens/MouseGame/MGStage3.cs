#region File Description
/**
 * In this stage the general image is broken in 4 parts
 * Every part has many copies, one of those copies is the genuine
 * The player must find all the corrent copies, complete the image and go to the next stage
 * All the copies are shown in 4 rows screen
 * 
 * When game starts, 4 parts are randomly chosen and appears for 4 seconds
 * 
 **/
#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace UnsolvedCases
{
    public class MGStage3 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;

        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;
        Random rand;

        bool GeneralImageIsVisible = false;     //after the user completes the task the general image appears

        //bool finished = false;                              //If the general image is clicked many times, the MessageBoxScreen is 
        //appeared many times so we need to call MessageBoxScreen only once       
        int width, height;
        static int collumns = 4;      //number of rows
        static int rows = 4;  //4 parts
        int CopiesRecWidth = 200, CopiesRecHeight = 200;   //dimensions of copies
        Rectangle[] CopiesRecList = new Rectangle[rows * collumns];   // array for all the copies rectangles;
        Texture2D[] CopiesTexList = new Texture2D[rows * collumns];   // array for all the copies rectangles;

        Texture2D squareTex;              //sprite around chosen copies
        Rectangle ProperRectangle;  //where the chosen copies are drawn
        int ChosenCopy1;      //Chosen copies
        int ChosenCopy2;
        int ChosenCopy3;
        int ChosenCopy4;
        bool isChosenCopy1 = false;
        bool isChosenCopy2 = false;
        bool isChosenCopy3 = false;
        bool isChosenCopy4 = false;

        int genuinePart1;
        int genuinePart2;
        int genuinePart3;
        int genuinePart4;

        bool showingGeneralImage = true;    //when this stage starts, the 4 genuine parts are appeared on screen for a short time
        //Time for showing General Image
        int remainingTime = 4;     //player has a few seconds to see the GeneralImage
        int previousTaskTime;

        bool finished = false;

        KeyboardState state;

        #endregion

        #region Initialization

        public MGStage3()
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
            rand = new Random();
            GeneralImageRec = new Rectangle(0, 0, width, height);
            GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");

            //part 1
            CopiesTexList[0] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_11");
            CopiesTexList[1] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_12");
            CopiesTexList[2] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_13");
            CopiesTexList[3] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_14");
            //part2
            CopiesTexList[4] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_21");
            CopiesTexList[5] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_22");
            CopiesTexList[6] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_23");
            CopiesTexList[7] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_24");
            //part3
            CopiesTexList[8] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_31");
            CopiesTexList[9] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_32");
            CopiesTexList[10] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_33");
            CopiesTexList[11] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_34");
            //part3
            CopiesTexList[12] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_41");
            CopiesTexList[13] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_42");
            CopiesTexList[14] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_43");
            CopiesTexList[15] = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage11\general_image_44");
            CalculateCopiesRectangles(0);
            GenerateGenuineParts();

            int rightSpace = width - (CopiesRecList[3].X + CopiesRecList[3].Width); //space from the right between width and last copy
            squareTex = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage10\square");
            ProperRectangle = new Rectangle(CopiesRecList[3].X + CopiesRecList[3].Width + rightSpace / 2 - CopiesRecWidth / 2
                , height / 2 - CopiesRecHeight / 2, CopiesRecWidth, CopiesRecHeight);


            previousTaskTime = DateTime.Now.Second;
            Oldmstate = Mouse.GetState();
        }

        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        public void CalculateCopiesRectangles(int current)//current: Represents the current row to add
        {
            //part 1
            CopiesRecList[0] = new Rectangle(0, 0, CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[1] = new Rectangle(CopiesRecList[0].X + CopiesRecList[0].Width + CopiesRecWidth / 5,
                CopiesRecList[0].Y, CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[2] = new Rectangle(CopiesRecList[1].X + CopiesRecList[1].Width + CopiesRecWidth / 5,
               CopiesRecList[1].Y, CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[3] = new Rectangle(CopiesRecList[2].X + CopiesRecList[2].Width + CopiesRecWidth / 5,
               CopiesRecList[2].Y, CopiesRecWidth, CopiesRecHeight);

            //part 2
            CopiesRecList[4] = new Rectangle(CopiesRecList[0].X,
                                 CopiesRecList[0].Y + CopiesRecList[0].Height,
                                 CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[5] = new Rectangle(CopiesRecList[4].X + CopiesRecList[4].Width + CopiesRecWidth / 5,
              CopiesRecList[4].Y, CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[6] = new Rectangle(CopiesRecList[5].X + CopiesRecList[5].Width + CopiesRecWidth / 5,
               CopiesRecList[5].Y, CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[7] = new Rectangle(CopiesRecList[6].X + CopiesRecList[6].Width + CopiesRecWidth / 5,
               CopiesRecList[6].Y, CopiesRecWidth, CopiesRecHeight);

            //part3
            CopiesRecList[8] = new Rectangle(CopiesRecList[4].X,
                                CopiesRecList[4].Y + CopiesRecList[4].Height,
                                CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[9] = new Rectangle(CopiesRecList[8].X + CopiesRecList[8].Width + CopiesRecWidth / 5,
             CopiesRecList[8].Y, CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[10] = new Rectangle(CopiesRecList[9].X + CopiesRecList[9].Width + CopiesRecWidth / 5,
               CopiesRecList[9].Y, CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[11] = new Rectangle(CopiesRecList[10].X + CopiesRecList[10].Width + CopiesRecWidth / 5,
               CopiesRecList[10].Y, CopiesRecWidth, CopiesRecHeight);

            //part 4
            CopiesRecList[12] = new Rectangle(CopiesRecList[8].X,
                                CopiesRecList[8].Y + CopiesRecList[8].Height,
                                CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[13] = new Rectangle(CopiesRecList[12].X + CopiesRecList[12].Width + CopiesRecWidth / 5,
                CopiesRecList[12].Y, CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[14] = new Rectangle(CopiesRecList[13].X + CopiesRecList[13].Width + CopiesRecWidth / 5,
               CopiesRecList[13].Y, CopiesRecWidth, CopiesRecHeight);
            CopiesRecList[15] = new Rectangle(CopiesRecList[14].X + CopiesRecList[14].Width + CopiesRecWidth / 5,
               CopiesRecList[14].Y, CopiesRecWidth, CopiesRecHeight);

        }

        public void FindClickedCopy()   //ChosenCopy Texture gets the proper value when the player clicks on a rectangle
        {
            int i = 0;
            foreach (Rectangle rec in CopiesRecList)
            {
                if (rec.Contains(mstate.X, mstate.Y))
                {
                    if (i >= 0 && i < 4)
                    {
                        ChosenCopy1 = i;
                        isChosenCopy1 = true;
                        break;
                    }
                    if (i >= 4 && i < 8)
                    {
                        ChosenCopy2 = i;
                        isChosenCopy2 = true;
                        break;
                    }
                    if (i >= 8 && i < 12)
                    {
                        ChosenCopy3 = i;
                        isChosenCopy3 = true;
                        break;
                    }
                    if (i >= 12 && i < 16)
                    {
                        ChosenCopy4 = i;
                        isChosenCopy4 = true;
                        break;
                    }
                }
                i++;
            }

        }

        public void GenerateGenuineParts()  //Randomly pick some of the parts to be the genuine parts
        {
            genuinePart1 = rand.Next(0, 4);
            genuinePart2 = rand.Next(4, 8);
            genuinePart3 = rand.Next(8, 12);
            genuinePart4 = rand.Next(12, 16);

            Console.WriteLine("Genuine Parts are: " + genuinePart1 + " " + genuinePart2 + " " + genuinePart3 + " " + genuinePart4);
        }
        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            mstate = Mouse.GetState();

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());

            if (showingGeneralImage)
            {

                if (DateTime.Now.Second != previousTaskTime)
                    remainingTime--;

                if (remainingTime <= 0)      //time up
                    showingGeneralImage = false;
                previousTaskTime = DateTime.Now.Second;
            }
            else
            {
                if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released)
                    FindClickedCopy();

                if ((isChosenCopy1 && ChosenCopy1 == genuinePart1) && (isChosenCopy2 && ChosenCopy2 == genuinePart2)
                    && (isChosenCopy3 && ChosenCopy3 == genuinePart3) && (isChosenCopy4 && ChosenCopy4 == genuinePart4) && !finished)
                {
                    finished = true;            //player won
                    ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage4()));
                }
            }
            Oldmstate = mstate;
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            if (showingGeneralImage)
            {
                spriteBatch.Draw(CopiesTexList[genuinePart1], GeneralImageRec, Color.White);
                spriteBatch.Draw(CopiesTexList[genuinePart2], GeneralImageRec, Color.White);
                spriteBatch.Draw(CopiesTexList[genuinePart3], GeneralImageRec, Color.White);
                spriteBatch.Draw(CopiesTexList[genuinePart4], GeneralImageRec, Color.White);
            }
            else
            {
                spriteBatch.DrawString(Fonts.MessageFont, "Mouse: " + mstate.X + ", " + mstate.Y, Vector2.Zero, Color.White);
                spriteBatch.DrawString(Fonts.MainFont, "FIND THE CORRENT PIECES OF THE 'GENERAL IMAGE'", new Vector2(0,
                    ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);

                //Draw copies on screen
                int k = 0;
                foreach (Rectangle rec in CopiesRecList)//Draw all intersections
                {
                    //road to the right
                    spriteBatch.Draw(CopiesTexList[k], CopiesRecList[k], Color.White);

                    k++;
                }
                spriteBatch.Draw(squareTex, ProperRectangle, Color.White);

                //Draw copies on square
                if (isChosenCopy1)
                    spriteBatch.Draw(CopiesTexList[ChosenCopy1], ProperRectangle, Color.White);
                if (isChosenCopy2)
                    spriteBatch.Draw(CopiesTexList[ChosenCopy2], ProperRectangle, Color.White);
                if (isChosenCopy3)
                    spriteBatch.Draw(CopiesTexList[ChosenCopy3], ProperRectangle, Color.White);
                if (isChosenCopy4)
                    spriteBatch.Draw(CopiesTexList[ChosenCopy4], ProperRectangle, Color.White);

            }

            spriteBatch.End();
        }

        #endregion
    }
}
