#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace UnsolvedCases
{
    /// <summary>
    /// This is the Login Screen
    /// The player must first login to his account and then move to the other options
    /// </summary>
    public class ComicDisplay : GameScreen
    {
        #region Fields

        Texture2D background;
        ContentManager content;
        Texture2D[] comic;          //the table that holds the comic frames every time
        int frames;                 //the total amount of frames each comic has
        string comic_name;          //unique name for every comic
        float delay;                //the delay between frames
        //int displayFrame;
        float elapsed; 
        Texture2D displaySprite;    //the sprite to show on screen
        Rectangle displayRec;       //the rectangle of the sprite
        int numOfFramesPassed;      //flag to check if the comic has ended
        float fadeRate = 0.0f;
        Texture2D lightSprite;      //the light behind the image
        Rectangle lightRec;         //the rectangle for the light
       // Boolean zoom = true;
        Random rand;
       // int effectNum = 7;
        GameScreen nextScreen;
        
        #endregion

        #region Initialization

        /// <summary>
        /// Create a comic screen
        /// </summary>
        /// <param name="frames">The total number of frames the comic has</param>
        /// <param name="comic_name">The unique name of the comic</param>
        public ComicDisplay(int frames, string comic_name, GameScreen nextScreen)
        {
            comic = new Texture2D[frames];
            this.frames = frames;
            this.comic_name = comic_name;
            numOfFramesPassed = 0;
            delay = 5000f;
            displayRec = new Rectangle(10, 10, 400, 400);
            lightRec = new Rectangle(10, 10, 10, 10);
            rand = new Random();
            this.nextScreen = nextScreen;
        }

        // Load graphics content for the game.
        public override void LoadContent()
        {
            ScreenManager.MainGame.IsMouseVisible = false;
           
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
           
            background = content.Load<Texture2D>("Textures/Riddles/Puzzle/puzzle_background");
            comic = new Texture2D[frames];

            // Load the frames of the comic book in the table
                for (int i = 0; i < frames; i++)
                {
                    comic[i] = content.Load<Texture2D>("Comics/Case1/"+comic_name+"/frame" + (i+1));
                }
                // Display the first frame with 3/4 width and height and in the middle of the screen
                // First frame in the center of the screen
                displaySprite = comic[0];
                displayRec.Width = 3 * (displaySprite.Width / 4);
                displayRec.Height = 3 * (displaySprite.Height / 4);
                displayRec.X=(ScreenManager.GraphicsDevice.Viewport.Width / 2)-(displayRec.Width/2);
                displayRec.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (displayRec.Height / 2);

                lightSprite = content.Load<Texture2D>("Comics/Misc/light2");
                lightRec.Width = displayRec.Width;
                lightRec.Height = displayRec.Width;

                lightRec.X = displayRec.X - 20;
                lightRec.Y = displayRec.Y - (Math.Abs(displayRec.Width - displayRec.Height) / 2);
         }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Used to set all the labels location on the screen
        /// </summary>
        protected virtual void UpdateLabelLocations()
        {
            
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            // Change the frame to be displayed after 2secs
            if (elapsed >= delay)
            {                
                /*
                 * If there are more frames in the list to be displayed increase the number of frames passed by one
                 * Choose the number of effect to perform 
                 */
                if (numOfFramesPassed < frames - 1)
                {
                    numOfFramesPassed++;
                    displaySprite = comic[numOfFramesPassed];

                    //Update width and height according to the new sprite - also X and Y
                    displayRec.Width = 3 * (displaySprite.Width / 4);
                    displayRec.Height = 3 * (displaySprite.Height / 4);

                    displayRec.X = rand.Next(0, ScreenManager.GraphicsDevice.Viewport.Width - displayRec.Width);
                    displayRec.Y = rand.Next(0, ScreenManager.GraphicsDevice.Viewport.Height - displayRec.Height/2);

                    lightRec.X = displayRec.X - (displayRec.Width / 10);
                    lightRec.Y = displayRec.Y - (Math.Abs(displayRec.Width - displayRec.Height) / 2);

                    lightRec.Width = displayRec.Width + (displayRec.Width/5);
                    lightRec.Height = displayRec.Width;
                   //effectNum = rand.Next(1, 8);
                   //displayRec.Width = 3 * (displaySprite.Width / 4);
                   //displayRec.Height = 3 * (displaySprite.Height / 4);
                    //displayRec.X = 0;
                    //displayRec.Y = ScreenManager.GraphicsDevice.Viewport.Height;

                    // Give the initial values to the features of every effect
           //         switch (effectNum)
           //         {
           //             case 1:
           //                 zoom = true;
           //                 break;
           //             case 2:
                            fadeRate = 1.0f;
           //                 break;
           //             case 3:
           //                 displayRec.X = ScreenManager.GraphicsDevice.Viewport.Width;
           //                 displayRec.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (displayRec.Height / 2);
           //                 break;
           //             case 4:
           //                 displayRec.X = 0 - displayRec.Width;
           //                 displayRec.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (displayRec.Height / 2);
           //                 break;
           //             case 5:
           //                 displayRec.Y = ScreenManager.GraphicsDevice.Viewport.Height;
           //                 break;
           //             case 6:
           //                 displayRec.Y = -displayRec.Height;
           //                 break;
           //             case 7:
           //                 displayRec.Width = 0;
           //                 displayRec.Height = 0;
           //                 break;
           //         }
                }
                else
                {
                    LoadingScreen.Load(ScreenManager, false, null, nextScreen);
                }
                elapsed = 0;
            }

           //Perform the choosen effect
       //     if (effectNum == 1)
       //         ZoomIn(displaySprite);
       //     else if (effectNum == 2)
                FadeIn(displaySprite);
       //     else if (effectNum == 3)
       //         GetInRigthLeft(displaySprite);
       //     else if (effectNum == 4)
       //         GetInLeftRight(displaySprite);
       //     else if (effectNum == 5)
       //         GetInBottomUp(displaySprite);
       //    else if (effectNum == 6)
       //        GetInTopDown(displaySprite);
       //    else
       //        ExpandImg(displaySprite);

            base.Update(gameTime, otherScreenHasFocus, false); 
        }

        public override void HandleInput()
        {
          
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            UpdateLabelLocations();

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.Black);
            spriteBatch.Draw(lightSprite,lightRec,Color.White);
            spriteBatch.Draw(displaySprite, displayRec, Color.White);
            //Must be performed for the fade effect
            spriteBatch.Draw(displaySprite, displayRec, Color.Black * fadeRate);
            spriteBatch.End();
        }

        #endregion
        
        #region Image Effects

        /// <summary>
        /// These are the effects for the sprites
        /// </summary>
        // The image begins with 0 width and height and expands on the screen
        /*public void ExpandImg(Texture2D currSprite)
        {
            if (displayRec.Width < (3 * (displaySprite.Width / 4)))
                displayRec.Width = displayRec.Width + 5;
            if (displayRec.Height < (3 * (displaySprite.Height / 4)))
                displayRec.Height = displayRec.Height + 5;

            displayRec.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (displayRec.Width / 2);
            displayRec.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (displayRec.Height / 2);
        }*/

     /*   public void MoveLight(int oldX, int oldY, int newX, int newY)
        {
            if (oldX < newX)
            {
                GetInLeftRight(lightSprite);
            }
            else
            {
                GetInRigthLeft(lightSprite);
            }

            if (oldY > newY)
            {
                GetInBottomUp(lightSprite);
            }
            else
            {
                GetInTopDown(lightSprite);
            }
        }*/

        // The image enters the screen from above
    /*    public void GetInTopDown(Texture2D currSprite)
        {
          //  displayRec.Width = (3 * (displaySprite.Width / 4));
          //  displayRec.Height = (3 * (displaySprite.Height / 4));
          //  displayRec.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (displayRec.Width / 2);

            if (displayRec.Y < ((ScreenManager.GraphicsDevice.Viewport.Height / 2) - (displayRec.Height / 2)))
            {
                displayRec.Y = displayRec.Y + 5;
            }

        }

        // The image enters the screen from bottom
        public void GetInBottomUp(Texture2D currSprite)
        {
            displayRec.Width = (3 * (displaySprite.Width / 4));
            displayRec.Height = (3 * (displaySprite.Height / 4));
            displayRec.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (displayRec.Width / 2);

            if (displayRec.Y > ((ScreenManager.GraphicsDevice.Viewport.Height / 2) - (displayRec.Height / 2)))
            {
                displayRec.Y = displayRec.Y - 5;
            }

        }

        // The image enters the screen from left
        public void GetInLeftRight(Texture2D currSprite)
        {
            displayRec.Width = (3 * (displaySprite.Width / 4));
            displayRec.Height = (3 * (displaySprite.Height / 4));
            displayRec.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (displayRec.Height / 2);

            if (displayRec.X < ((ScreenManager.GraphicsDevice.Viewport.Width / 2) - (displayRec.Width / 2)))
            {
                displayRec.X = displayRec.X + 5;
            }
        }

        // The image enters the screen from right
        public void GetInRigthLeft(Texture2D currSprite)
        {
            displayRec.Width = (3 * (displaySprite.Width / 4));
            displayRec.Height = (3 * (displaySprite.Height / 4));
            displayRec.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (displayRec.Height / 2);

            if (displayRec.X > ((ScreenManager.GraphicsDevice.Viewport.Width / 2) - (displayRec.Width / 2)))
            {
                displayRec.X = displayRec.X - 5;
            }
        }
        */
        // The image is total black and becomes brighter
        public void FadeIn(Texture2D currSprite)
        {
         //   displayRec.Width = (3 * (displaySprite.Width / 4));
         //   displayRec.Height = (3 * (displaySprite.Height / 4));

           
          

           // displayRec.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (displayRec.Height / 2);
            //displayRec.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (displayRec.Width / 2);
            fadeRate -= 0.005f;
        }

        // The image has its natural size, it expands to take all the window and shrinks again to its initial size
     /*   public void ZoomIn(Texture2D currSprite)
        {
            if (zoom)
            {
                if ((displayRec.Width < ScreenManager.GraphicsDevice.Viewport.Width) && (displayRec.Height < ScreenManager.GraphicsDevice.Viewport.Height))
                {
                    displayRec.Width = displayRec.Width + 5;
                    displayRec.Height = displayRec.Height + 5;
                }
                else
                    zoom = false;
            }
            else
            {
                if ((displayRec.Width > 3 * (displaySprite.Width / 4)) && (displayRec.Height > 3 * (displaySprite.Height / 4)))
                {
                    displayRec.Width = displayRec.Width - 5;
                    displayRec.Height = displayRec.Height - 5;
                }
            }

            displayRec.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (displayRec.Width / 2);
            displayRec.Y = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (displayRec.Height / 2);
        }*/
       
        #endregion
    }
}

