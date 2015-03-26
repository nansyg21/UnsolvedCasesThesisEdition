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
    public class PlatformerLevel : GameScreen
    {
        #region Fields

        Texture2D background;
        ContentManager content;
        private SpriteBatch spriteBatch;
        Game thisGame;

        public static PlatformerLevel CurrentBoard { get; private set; }

        Texture2D platformSprite;
        Texture2D woodenPlatformSprite;
        Rectangle platformRect;
        Character victor;

        List<Platform> platforms;       //Platforms of the stage
        int platformType=1;
        Random rnd;                     //The number to generate random type platforms
        Random binary;
        Random itemRnd;

        Platform helpPlatform;
        Rectangle shadowRect;

        PaintingPlatform paintingPlatform;
        Texture2D painting;
        Texture2D paintingBack;
        Texture2D paintingFront1st;
        Texture2D paintingFront2nd;
        Texture2D paintingFront3rd;
        Rectangle paintingRect;

        Texture2D spikes;
        Rectangle spikesRect;
        Boolean lost = false;

        Boolean starter = false;

        int screenWidth, screenHeight;

        float Ytemp = 800;

        Boolean throwgh=false;

        Vector2 distanceForItem;

        PlatformItems itemToThrowgh;

        int level=0;

        KeyboardState state;

        String paintingInfo;

        enum State
        {
            Walking,
            Jumping
        }

        #endregion

        #region Initialization

        /// <summary>
        /// This is a riddle
        /// There are several platforms appearing on the screen
        /// The player must jump on the platforms to collect the items that appear
        /// If the player falls of the platform to the ground he loses
        /// On the corner there is a platform with a painting built from a special materia (ice, glass, paper)
        /// When the player picks up the correct item (fire, rock, water) he can through it to the painting platform
        /// If he aims corect then tha painting falls
        /// </summary>

        public PlatformerLevel()
        {
          
            platformRect = new Rectangle(10, 10, 300, 60); //rectangle helps to draw platforms
            thisGame = ScreenManager.MainGame;
            rnd = new Random();
            binary = new Random();
            itemRnd = new Random();
            platforms = new List<Platform>();
            PlatformerLevel.CurrentBoard = this; //initialize current board instance
            paintingInfo = "";

        }

        public override void LoadContent()
        {
            ScreenManager.MainGame.IsMouseVisible = true;

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            /*Screens width and height*/
            screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;

            background = content.Load<Texture2D>("Textures/Riddles/Puzzle/puzzle_background");

            spriteBatch = ScreenManager.SpriteBatch;

            /*Initialize character*/
            thisGame.Services.AddService(typeof(SpriteBatch), spriteBatch);
            victor = new Character(thisGame, "Characters/Victor/victor_down_xsmall_1", new Vector2(500, 600));
            victor.WidthRange = screenWidth-victor.RectWidth/4;
            thisGame.Components.Add(victor);

            InitializeLevel();
           
            paintingPlatform = new PaintingPlatform(thisGame, 1);
            platformRect.X = 100;
            platformRect.Y = 100;
            platformRect.Width = screenWidth / 10;
            platformRect.Height = screenHeight / 20;
            paintingPlatform.Rectangle = platformRect;
            thisGame.Components.Add(paintingPlatform);

            paintingBack = content.Load<Texture2D>("Textures/Riddles/Platformer/painting_back");
            paintingFront1st = content.Load<Texture2D>("Textures/Riddles/Platformer/pinakas6");
            paintingFront2nd = content.Load<Texture2D>("Textures/Riddles/Platformer/pinakas7");
            paintingFront3rd = content.Load<Texture2D>("Textures/Riddles/Platformer/pinakas8");

            painting = paintingBack;
            paintingRect = new Rectangle(0, 0, 0, 0);

            shadowRect = new Rectangle((victor.Rectangle.X - (victor.Rectangle.Width/2)),victor.Rectangle.Y-(victor.Rectangle.Height/2),2*victor.Rectangle.Width,2*victor.Rectangle.Height);
            
            spikes = content.Load<Texture2D>("Textures/Riddles/Platformer/spikes");
            spikesRect = new Rectangle(0,(int)screenHeight-(spikes.Height/2),screenWidth,spikes.Height/2);

            UpdateLabelLocations();
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Used to set all the labels location on the screen
        /// </summary>
        protected virtual void UpdateLabelLocations()
        {
            paintingRect.X = paintingPlatform.Rectangle.X+(paintingPlatform.Rectangle.Width / 4);
            paintingRect.Y=paintingPlatform.Rectangle.Y-(paintingPlatform.Rectangle.Height*2);
            paintingRect.Width = paintingPlatform.Rectangle.Width / 2;
            paintingRect.Height = paintingPlatform.Rectangle.Height * 2;

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());

            /*Only if there are platforms on the board - else it breaks down
             *Let the character know the available platforms
             *Check if is time to hide one platform if so remove it from the list and create new one*/
            if (platforms.Count != 0)
            {
                victor.Platforms = platforms;
               
                if ((victor.CurrentPlatform!=null)&&(victor.CurrentPlatform.TimeToHide))
                {
                    
                    if (platforms.Count == 4)
                    {
                        thisGame.Components.Remove(platforms[0].PlatformItem);
                        thisGame.Components.Remove(platforms[0]);
                        platforms.RemoveAt(0);
                    }
                }

                if(victor.CurrentPlatform!=null)
                {
                    CheckCollisionWithPlatformItem();
                }

                if (throwgh)
                {
                    FireItem(new Vector2 (itemToThrowgh.Rectangle.X,itemToThrowgh.Rectangle.Y));
                }

                CheckIfLost();
                if (victor.CurrentPlatform.IsStarterPlatform)
                {
                    starter = true;
                }

                if ((starter)&&(!victor.CurrentPlatform.IsStarterPlatform))
                {
                    paintingInfo = "";
                    paintingRect.Width = paintingPlatform.Rectangle.Width / 2;
                    paintingRect.Height = paintingPlatform.Rectangle.Height * 2;
                    paintingRect.Y = paintingPlatform.Rectangle.Y - (paintingPlatform.Rectangle.Height * 2);
                    painting = paintingBack;
                    starter = false;
                }
               
            }

           
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
            
            
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
           
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(painting, paintingRect, Color.White);
            spriteBatch.Draw(spikes,spikesRect,Color.White);
            spriteBatch.DrawString(Fonts.MainFont, "LOST " + lost, Vector2.Zero, Color.Blue);
            spriteBatch.DrawString(Fonts.MainFont, paintingInfo,new Vector2((screenWidth/2)-(paintingInfo.Length/2),paintingRect.Y),Color.Blue);
            
            spriteBatch.End();
        }

        #endregion

        /*Improve the characters movement
         *Allow diagonical move
         *Avoid flow on the air*/
        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            Vector2 movementToTry = destination - originalPosition;
            Vector2 furthestAvailableLocationSoFar = originalPosition;
            int numberOfStepsToBreakMovementInto = (int)(movementToTry.Length() * 2) + 1;
            Vector2 oneStep = movementToTry / numberOfStepsToBreakMovementInto;

            for (int i = 1; i <= numberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + oneStep * i;
                Rectangle newBoundary =
                    CreateRectangleAtPosition(positionToTry, boundingRectangle.Width, boundingRectangle.Height);

                if(TestIfHasRoom(newBoundary))
                {
                        furthestAvailableLocationSoFar = positionToTry;
                       
                }
                else 
                    {
                    
                            UpdateCurrentPlatform();
        
                            bool isDiagonalMove = movementToTry.X != 0 && movementToTry.Y != 0;
                            if (isDiagonalMove)
                            {
                                int stepsLeft = numberOfStepsToBreakMovementInto - (i - 1);

                                Vector2 remainingHorizontalMovement = oneStep.X * Vector2.UnitX * (2 * stepsLeft / 3);
                                Vector2 finalPositionIfMovingHorizontally = furthestAvailableLocationSoFar + remainingHorizontalMovement;
                                furthestAvailableLocationSoFar =
                                    WhereCanIGetTo(furthestAvailableLocationSoFar, finalPositionIfMovingHorizontally, boundingRectangle);

                                Vector2 remainingVerticalMovement = oneStep.Y * Vector2.UnitY * (2 * stepsLeft / 3);
                                Vector2 finalPositionIfMovingVertically = furthestAvailableLocationSoFar + remainingVerticalMovement;
                                furthestAvailableLocationSoFar =
                                    WhereCanIGetTo(furthestAvailableLocationSoFar, finalPositionIfMovingVertically, boundingRectangle);
                            }
                            victor.Rectangle = new Rectangle((int)furthestAvailableLocationSoFar.X, (int)furthestAvailableLocationSoFar.Y, victor.Rectangle.Width, victor.Rectangle.Height);                     
                        break;
                    }
            }
            
            return furthestAvailableLocationSoFar;

        }

        /*Update the current platform
         *If there is not then the current platform is the platform the char is on
         *Else check if the platform the char is on is different from the previous one - it means the char jumped
         *Then update the current platform remove the previous (except the current) from the screen and create 3 new*/
        private void UpdateCurrentPlatform()
        {
            
            foreach (Platform p in platforms)
            {

                //change current plarform if only: intersection happens, victor is higher than platofrm, (victor's X is between: platform X and X+width)
                if (p.Rectangle.Intersects(victor.Rectangle) && (victor.falling)
                    && (victor.Rectangle.X + victor.Rectangle.Width - 5 > p.Rectangle.X && victor.Rectangle.X < p.Rectangle.X + p.Rectangle.Width - 5))
                    
                   // && (victor.Rectangle.Y+victor.Rectangle.Height)<= p.Rectangle.Y)
                    //&& (victor.Rectangle.X + victor.Rectangle.Width > p.Rectangle.X && victor.Rectangle.X < p.Rectangle.X))
                {
                   
                    if (victor.CurrentPlatform == null)
                        victor.CurrentPlatform = p;
                    else
                    {
                        if (!victor.CurrentPlatform.Equals(p)) 
                        {
                            victor.CurrentPlatform = p;
                            if (!victor.CurrentPlatform.IsStarterPlatform)
                            {
                                victor.NeedPlatforms = true;
                                helpPlatform = p;
                                if (helpPlatform.HasItem)
                                {
                                    itemToThrowgh = new PlatformItems(thisGame, helpPlatform.PlatformItem.Type, helpPlatform);
                                    itemToThrowgh.Rectangle = helpPlatform.PlatformItem.Rectangle;
                                    thisGame.Components.Remove(helpPlatform.PlatformItem);
                                }
                                for (int j = 0; j < platforms.Count; j++)
                                {
                                    thisGame.Components.Remove(platforms[j].PlatformItem);
                                    thisGame.Components.Remove(platforms[j]);
                                }
                                platforms.Clear();
                                platforms.Add(helpPlatform);
                                thisGame.Components.Add(platforms[0]);
                                thisGame.Components.Add(platforms[0].PlatformItem);
                                CreatePlatforms(platforms[0]);

                                break;
                            }
                        }
                    }
                
                  
                }
                 
            }
           
        }

        /*Dummy rectangle to check collision*/
        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }

        /*Create platforms*/
        private void CreatePlatforms(Platform p)
        {
            /*Only if needed*/
            if (victor.NeedPlatforms)
            {
                /*3 platforms at a time*/
                for (int k = 0; k < 3; k++)
                {
                    /*Create the platform object then create a random rectangle within the range of the possible movement of the char
                     * Then check if X cordinate is out of screen then recalculate the X
                     * Then check if Y cordinate if out of screen, or if the difference between the current platfor the char is on and the new is less
                     * than the char's height and the X cordinate shows that the platform is above the current platform recalculate to allow the char jump
                     * between platforms
                     * Loop untill there is room for the platform to be created*/
                    this.platforms.Add(new Platform(thisGame, rnd.Next(1, 4)));
                    while(true)
                    {
                        int rand1 = 0;
                        int rand2 = 0;

                        if (rnd.Next(1, 3) == 1)//left side
                            platformRect.X = rnd.Next(victor.CurrentPlatform.Rectangle.X - 200, victor.CurrentPlatform.Rectangle.X - 130);

                        else                    //right side
                            platformRect.X = rnd.Next(victor.CurrentPlatform.Rectangle.X + victor.CurrentPlatform.Rectangle.Width,
                                victor.CurrentPlatform.Rectangle.X + victor.CurrentPlatform.Rectangle.Width + 50);

                        if (rnd.Next(1, 3) == 1)//up
                            platformRect.Y = rnd.Next(victor.CurrentPlatform.Rectangle.Y - 150, victor.CurrentPlatform.Rectangle.Y - 100);
                        else                    //down
                            platformRect.Y = rnd.Next(victor.CurrentPlatform.Rectangle.Y, victor.CurrentPlatform.Rectangle.Y + 100);


                        while ((platformRect.X < 10) || (platformRect.X > (screenWidth - platformRect.Width - 10)))
                        {
                            if (rnd.Next(1, 3) == 1)//left side
                                platformRect.X = rnd.Next(victor.CurrentPlatform.Rectangle.X - 200, victor.CurrentPlatform.Rectangle.X - 130);

                            else                    //right side
                                platformRect.X = rnd.Next(victor.CurrentPlatform.Rectangle.X + victor.CurrentPlatform.Rectangle.Width,
                                    victor.CurrentPlatform.Rectangle.X + victor.CurrentPlatform.Rectangle.Width + 50);
                            rand1++;
                            if(rand1==10)
                            {
                                platformRect.X = screenWidth/2;
                            }
                        }

                        /*Check the Y of the new platform to be above the end of the screen
                         Then if the plafrom is */
                        while ((platformRect.Y < victor.Texture.Height) || (platformRect.Y > (screenHeight - platformRect.Height - 10)) || platformRect.Intersects(victor.Rectangle))
                       {
                            if (rnd.Next(1, 3) == 1)//up
                                platformRect.Y = rnd.Next(victor.CurrentPlatform.Rectangle.Y - 150, victor.CurrentPlatform.Rectangle.Y - 100);
                            else                    //down
                                platformRect.Y = rnd.Next(victor.CurrentPlatform.Rectangle.Y, victor.CurrentPlatform.Rectangle.Y + 100);

                            rand2++;
                            if(rand2==10)
                            {
                                platformRect.Y = screenHeight / 2;
                            }
                        }

                        if (platformRect.Intersects(victor.Rectangle))
                            platforms.Remove(platforms[k+1]);

                        if ((TestIfHasRoom(platformRect)))
                        {
                            platformRect.Width = screenWidth / 10;
                            platformRect.Height = screenHeight / 20;

                            platforms[k + 1].Rectangle = platformRect;
                            thisGame.Components.Add(platforms[k + 1]);

                            if (itemRnd.Next(0, 2) > 0)
                            {
                                platforms[k + 1].HasItem = true;
                                platforms[k + 1].AddItem();
                                thisGame.Components.Add(platforms[k+1].PlatformItem);
                            }
                            else
                            {
                                platforms[k + 1].HasItem = false;
                            }
                            break;
                        }
               
                    }

                    //platforms[k + 1].Rectangle = platformRect;
                    //thisGame.Components.Add(platforms[k + 1]);
                    //thisGame.Components.Add(new PlatformItems(thisGame,1,platforms[k+1]));
                }
                victor.NeedPlatforms = false;
            }
        }

        /*Disapear platform if however the previous test it still collides with the char*/
        private void MustDisapear()
        {
            int j = 0;

            for (int l = 1; l < platforms.Count; l++)
            {
                if (victor.Rectangle.Intersects(platforms[l].Rectangle))
                {
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!");
                    thisGame.Components.Remove(platforms[l]);
                    platforms.RemoveAt(l);
                    //return true;
                }
            }
            /*    foreach (Platform p in platforms)
                {
                    // if ((victor.Rectangle.Intersects(p.Rectangle)))
                    //     Console.WriteLine("test1");

                    if ((victor.Rectangle.Intersects(p.Rectangle)) && (p != victor.CurrentPlatform))
                    {
                        Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!");
                        thisGame.Components.Remove(p);
                        platforms.RemoveAt(j);
                        //return true;
                    }
                    j++;
                }*/
            victor.Platforms = platforms;
          //  return false;
        }

        /*Test if there is room to create the platform*/
        private Boolean TestIfHasRoom(Rectangle rec)
        {
            foreach (Platform p in platforms)
            {
           
                if ((rec.Intersects(p.Rectangle)) || ((victor.Rectangle.Intersects(p.Rectangle)) && (p != victor.CurrentPlatform)))
                {
                    return false;
                }            
            }
            return true;
        }

        private void CheckCollisionWithPlatformItem()
        {
            if (victor.CurrentPlatform.HasItem)
            {
                if (victor.Rectangle.Intersects(victor.CurrentPlatform.PlatformItem.Rectangle))
                {
                    if ((victor.CurrentPlatform.PlatformItem.Type == paintingPlatform.Type)&&(!throwgh))
                    {
                        throwgh = true;
                        itemToThrowgh = victor.CurrentPlatform.PlatformItem;
                        distanceForItem = new Vector2(itemToThrowgh.Rectangle.X - paintingPlatform.Rectangle.X, itemToThrowgh.Rectangle.Y - paintingPlatform.Rectangle.Y);
                    }
                    else
                    {
                        thisGame.Components.Remove(victor.CurrentPlatform.PlatformItem);
                        thisGame.Components.Remove(itemToThrowgh);
                    }

                }
            }
        }

        /*Check if victor has fallen on the spikes*/
        private void CheckIfLost()
        {
            if (victor.Rectangle.Intersects(spikesRect))
            {
                lost = true;
                InitializeLevel();
            }
        }

        /*Generate first platform - The character must fall and land on this platform in order to be able to move and jump
        *So the X cordinate of character must be within the dx distance the platform sprite covers*/
       
        private void InitializeLevel()
        {
            if (platforms.Count>1)
            {
                for (int k = 0; k < platforms.Count; k++)
                {
                    thisGame.Components.Remove(platforms[k].PlatformItem);
                    thisGame.Components.Remove(platforms[k]);
                }
                platforms.Clear();
            }

            victor.Position = new Vector2(500, 600);

            platforms.Add(new Platform(thisGame, 0));
            platformRect.X = 500;
            platformRect.Y = 730;
            platformRect.Width = screenWidth / 10;
            platformRect.Height = screenHeight / 20;
            platforms[0].Rectangle = platformRect;
            thisGame.Components.Add(platforms[0]);
            victor.CurrentPlatform = platforms[0];
            //  CreatePlatforms(platforms[0]);

            platforms.Add(new Platform(thisGame, 3));
            platformRect.X = 250;
            platformRect.Y = 550;
            platformRect.Width = screenWidth / 10;
            platformRect.Height = screenHeight / 20;
            platforms[1].Rectangle = platformRect;
            thisGame.Components.Add(platforms[1]);
            victor.CurrentPlatform = platforms[1];
            //   CreatePlatforms(platforms[1]);

            platforms.Add(new Platform(thisGame, 3));
            platformRect.X = 650;
            platformRect.Y = 700;
            platformRect.Width = screenWidth / 10;
            platformRect.Height = screenHeight / 20;
            platforms[2].Rectangle = platformRect;
            thisGame.Components.Add(platforms[2]);
            victor.CurrentPlatform = platforms[2];
            //    CreatePlatforms(platforms[2]);

            platforms.Add(new Platform(thisGame, 3));
            platformRect.X = 300;
            platformRect.Y = 680;
            platformRect.Width = screenWidth / 10;
            platformRect.Height = screenHeight / 20;
            platforms[3].Rectangle = platformRect;
            thisGame.Components.Add(platforms[3]);
            victor.CurrentPlatform = platforms[3];
            // CreatePlatforms(platforms[3]);
        }

        private void FireItem(Vector2 originalPosition)
        {
            Vector2 step = -distanceForItem / ((distanceForItem.Length() * 2) + 1);
            Vector2 positionToTry;

            positionToTry.X = originalPosition.X - ((originalPosition.X - paintingPlatform.Rectangle.X) / 10);
            positionToTry.Y = originalPosition.Y - ((originalPosition.Y - paintingPlatform.Rectangle.Y) / 10);
               
            itemToThrowgh.Rectangle=new Rectangle((int)positionToTry.X,(int)positionToTry.Y,itemToThrowgh.Rectangle.Width,itemToThrowgh.Rectangle.Height);
            if (itemToThrowgh.Rectangle.Intersects(paintingPlatform.Rectangle))
            {
                throwgh = false;
                level++;
                if(level==1)
                {
                    painting = paintingFront1st;
                    paintingRect.Y=screenHeight/2-(paintingRect.Height*2);
                    paintingRect.Width=paintingRect.Width*4;
                    paintingRect.Height = paintingRect.Height * 4;
                    paintingInfo = "INFORMATION ABOUT PAINTING NUMBER # \n-PAINTER: XXXXX \n-YEAR: XXXX \n-TITLE: XXXX \n\n\n JUMP ON A PLATFORM TO CONTINUE...";
                    thisGame.Components.Remove(paintingPlatform);
                    paintingPlatform = new PaintingPlatform(thisGame, level+1);
                    platformRect.X = 100;
                    platformRect.Y = 100;
                    platformRect.Width = screenWidth / 10;
                    platformRect.Height = screenHeight / 20;
                    paintingPlatform.Rectangle = platformRect;
                    thisGame.Components.Add(paintingPlatform);

                }
                else if (level == 2)
                {
                    painting = paintingFront2nd;
                    paintingRect.Y = screenHeight / 2 - (paintingRect.Height * 2);
                    paintingRect.Width = paintingRect.Width * 4;
                    paintingRect.Height = paintingRect.Height * 4;
                    paintingInfo = "INFORMATION ABOUT PAINTING NUMBER # \n-PAINTER: XXXXX \n-YEAR: XXXX \n-TITLE: XXXX \n\n\n JUMP ON A PLATFORM TO CONTINUE...";
                    thisGame.Components.Remove(paintingPlatform);
                    paintingPlatform = new PaintingPlatform(thisGame, level + 1);
                    platformRect.X = 100;
                    platformRect.Y = 100;
                    platformRect.Width = screenWidth / 10;
                    platformRect.Height = screenHeight / 20;
                    paintingPlatform.Rectangle = platformRect;
                    thisGame.Components.Add(paintingPlatform);
                }
                else if (level == 3)
                {
                    painting = paintingFront3rd;
                    paintingRect.Y = screenHeight / 2 - (paintingRect.Height * 2);
                    paintingRect.Width = paintingRect.Width * 4;
                    paintingRect.Height = paintingRect.Height * 4;
                    paintingInfo = "INFORMATION ABOUT PAINTING NUMBER # \n-PAINTER: XXXXX \n-YEAR: XXXX \n-TITLE: XXXX \n\n\n JUMP ON A PLATFORM TO CONTINUE...";
                    thisGame.Components.Remove(paintingPlatform);
                    paintingPlatform = new PaintingPlatform(thisGame, 1);
                    platformRect.X = 100;
                    platformRect.Y = 100;
                    platformRect.Width = screenWidth / 10;
                    platformRect.Height = screenHeight / 20;
                    paintingPlatform.Rectangle = platformRect;
                    thisGame.Components.Add(paintingPlatform);
                }
                thisGame.Components.Remove(victor.CurrentPlatform.PlatformItem);
                thisGame.Components.Remove(itemToThrowgh);
                InitializeLevel();

           }   
        }
    }

   
}









