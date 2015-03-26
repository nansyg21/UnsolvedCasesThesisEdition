#region File Description
/**
 * This jigsaw puzzle has 4 pictures that need to be solved at the same time.
 * It contains an image that shows the original position of each puzzle piece.
 * It works only on the current dimensions: 1366x768
 * If the screen dimensions are different, it saves the original dimensions, switches to 1366x768
 * and restores the original dimensions in the end.
 **/
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
    public class PuzzleGameScreen : GameScreen
    {
        #region Fields
        
        Puzzle puzzle;
        ContentManager content;
        KeyboardState state;
        Texture2D background;
    

        int OriginalWidth;                        //the dimensions the screen has before this riddle is loaded
        int OriginalHeight;
        MouseState OldMouseState = new MouseState();
        Vector2 staticInfoStringPosition;                   //position to draw the string
        String StringToDraw = "SOLVE THE PUZZLE OF THE STOLEN PAINTINGS AND GAIN CLUES!"; //string to draw
        Random rand;

        //Time before shuffle
        DateTime startingTime;
        int remainingTime=200;     //player has a few minutes to complete all puzzles
        DateTime remainingTimeAsDate;
        Vector2 remainingTimeVec;       

        int currentScrambleTime, scrambleCountdownLimit = 5;  //after 5 seconds tha puzzle pieces are scrambled
        bool waitingForScramble;
        Vector2 ScrambleTimeVec;                 //where to draw remaining time
        int previousTaskTime;       //save previous second to know when time changes, something like mouse state

        //CRITICAL TASK STUFF
            //Before task starts
        int currentTaskScheduleTime;          //when the critical tast is going to start
        int taskScheduleLimitMin = 10, taskScheduleLimitMax = 20;   //in 30 to 70 seconds a critical task appears
        bool haveScheduledTask = false;
            //After task starts
        public bool criticalIsAPiece = false, criticalIsAPuzzle = false;  //either we have criticalPiece or criticalPuzzle
        bool taskDone = false;
        DateTime taskStartingTime;
        int currentTaskTime ;  //after some seconds the critical task is disabled



        String criticalMessage;
        Vector2 criticalMessageVec;
        float criticalMessageScale;     //gains values from min - max       on critical task, message stretches
        float mincritical = 1f;
        float maxcritical = 1.1f;
        float criticalstep = 0.004f;

        bool raisingValue = true;      //from 0.5 to 3.5 or the opposite


        PostIt postit;
        Vector2 infoPressEnterVec;
        bool PostitFlag=true;         //Drawing postit or playing the game immediately

        #endregion

        #region Initialization

        public PuzzleGameScreen()
        {
            //Save original dimensions
            OriginalWidth = ScreenManager.graphicsManager.PreferredBackBufferWidth;
            OriginalHeight = ScreenManager.graphicsManager.PreferredBackBufferHeight;
            Console.WriteLine("ORIGINAL: " + OriginalWidth + "x" + OriginalHeight);

            //if dimensions arent 1360x768 set them(waiting a few seconds here...), and before the puzzle quits, restore them
            if (!OriginalWidth.Equals(1360) || !OriginalHeight.Equals(768))
            {
                Console.WriteLine("Changing game dimensions..");

                if(!ScreenManager.graphicsManager.IsFullScreen)
                   ScreenManager.GetGraphicsDeviceManager.IsFullScreen = true;         //set fullscreen if it isnt
           
                ScreenManager.GetGraphicsDeviceManager.PreferredBackBufferWidth = 1360;//set width , set height, apply
                ScreenManager.GetGraphicsDeviceManager.PreferredBackBufferHeight = 768;
                ScreenManager.GetGraphicsDeviceManager.ApplyChanges();
                Console.WriteLine("Game dimensions successfully changed..");
            }
        }

        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;
      
            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            background = content.Load<Texture2D>("Textures/Riddles/Puzzle/puzzle_background");
            rand = new Random();


            //TEXT 3D EFFECT VECTOR
            Vector2 tmpVector = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2,
                   ScreenManager.GraphicsDevice.Viewport.Height);
            Vector2 tmpVector2 = Fonts.MainFont.MeasureString(StringToDraw);
            tmpVector2.X = tmpVector2.X / 2;//for the middle of the screen
            staticInfoStringPosition = tmpVector - tmpVector2;

            remainingTimeVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width/2, 
                    staticInfoStringPosition.Y-Fonts.MainFont.MeasureString("1").Y);
          
            puzzle = new Puzzle(this);
            puzzle.PuzzlelizeImage();
           
            ScrambleTimeVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height/2);

            //PostIt
            postit = new PostIt(content);
            postit.Text = "-SOLVE THE PUZZLES OF THE FAMOUS BUT STOLEN PAINTINGS\n-FIND INFORMATION ABOUT THOSE PAINTINGS\n "+
                "-BEWARE OF THE CRITICAL PUZZLES OR PIECES\n-THE TIME IS RUNNING..\n    GOOD LUCK!";
            postit.Width = (int)(postit.Width / 1.5f);
            postit.Height = (int)(postit.Height);
            postit.Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - postit.Width / 2,
                            ScreenManager.GraphicsDevice.Viewport.Height / 2 - postit.Height / 2);


            infoPressEnterVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 -
                           Fonts.MainFont.MeasureString("PRESS ENTER TO START").X / 2, postit.Position.Y + postit.Height);

        }
        public void Restart()
        {

            PostitFlag = true;
            StopCriticalTask();
            puzzle.Disable4ShuffleAreas();
            puzzle.PuzzlelizeImage();
            remainingTime = 240;
            scrambleCountdownLimit = 5;
            haveScheduledTask = false;
            criticalIsAPiece = false;
            criticalIsAPuzzle = false;
            taskDone = false;


        }

        public override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region Update

        public void taskIsCompleted()
        {
            taskDone = true;
        }

        public void StopCriticalTask()
        {
            if (criticalIsAPiece)
            {
                puzzle.DisableCriticalPiece();
                criticalIsAPiece = false;
            }
            if (criticalIsAPuzzle)
            {
                puzzle.DisableCriticalPuzzle();
                criticalIsAPuzzle = false;
            }
        }
        
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());
            MouseState NewMouseState = Mouse.GetState();


            if (PostitFlag) //Drawing postit
            {
                if (state.IsKeyDown(Keys.Enter))
                {
                    PostitFlag = false;
                    startingTime = DateTime.Now;
        
        
                    //Set when task will start 
                    currentTaskScheduleTime = rand.Next(taskScheduleLimitMin, taskScheduleLimitMax);//when critical task will begin 
                    haveScheduledTask = true;
                    waitingForScramble = true;
                    Console.WriteLine("Task in " + currentTaskScheduleTime+" seconds");
                }
            }

            else            //Playing game
            {
                //Show the image at the beggining
                if (waitingForScramble)
                {
                    //Update Clock
                    if (DateTime.Now.Second >= startingTime.Second)
                        currentScrambleTime = scrambleCountdownLimit - (DateTime.Now.Second - startingTime.Second);
                    else                                                              //when seconds go from 59 to 0, we add 60
                        currentScrambleTime = scrambleCountdownLimit - (DateTime.Now.Second - startingTime.Second + 60);

                    if (currentScrambleTime <= 0)
                    {
                        puzzle.ShufflePuzzle();
                        waitingForScramble = false;
                    }

                }
                else        //Playing Puzzle
                {
                    //Update remaining time
                    if (DateTime.Now.Second != previousTaskTime)
                    {
                        remainingTime--;
                        remainingTimeAsDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, remainingTime / 60, remainingTime % 60);
                
                    }
                    if (remainingTime <= 0)    //Player Lost, exit
                    {
                        Restart();
                    }
                  
                    //If a task is scheduled, update time. If time comes prepare the task
                    if (haveScheduledTask)
                    {
                        //Update scheduled task remaining time
                        if (DateTime.Now.Second != previousTaskTime)
                            currentTaskScheduleTime--;


                        if (currentTaskScheduleTime <=1 && currentTaskScheduleTime>=-1)
                        {
                            haveScheduledTask = false;

                          
                            if (rand.Next(0, 2) == 0)   //Select randomly between critical puzzle or piece
                            {
                                puzzle.CriticalPuzzle();
                                criticalIsAPuzzle = true;
                                currentTaskTime = 65;
                                criticalMessage = "FIND AND SOLVE THE CRITICAL PUZZLE AND GAIN BONUS TIME";
                            }
                            else
                            {
                                puzzle.CriticalPiece();
                                criticalIsAPiece = true;
                                currentTaskTime = 10;
                                criticalMessage = "FIND AND SOLVE THE CRITICAL PIECE AND GAIN OR LOSE 20 SECONDS";  Console.WriteLine("CRITICAL PIECE");
                            }
                            taskStartingTime = DateTime.Now;
                            previousTaskTime = taskStartingTime.Second;
                            criticalMessageVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - Fonts.MainFont.MeasureString(criticalMessage).X / 2,
                                            ScreenManager.GraphicsDevice.Viewport.Height / 2);
                            criticalMessageScale = mincritical;

                        }
                    }
                    else if (criticalIsAPuzzle || criticalIsAPiece)     //There is a critical task        
                    {

                        if (DateTime.Now.Second != previousTaskTime)
                            currentTaskTime--;


                        //MESSAGE STRETCHING EFFECT
                        if (raisingValue)
                            criticalMessageScale += criticalstep;
                        else
                            criticalMessageScale -= criticalstep;

                        if (criticalMessageScale <= mincritical)       //from now value increases
                            raisingValue = true;
                        else if (criticalMessageScale >= maxcritical)  //from now value decreases
                            raisingValue = false;

                        if (currentTaskTime <= 0 && !taskDone)      //failed to complete
                        {
                            
                            Console.WriteLine("The player didnt completed the task");
                            StopCriticalTask();
                            remainingTime -= 25;
                            AudioManager.PlayCue("StaffHit");
                            //player loses 25 seconds 
                        }
                        else if (taskDone)                           //has been completed
                        {
                          
                            Console.WriteLine("The player COMPLETED the task");
                            StopCriticalTask();
                            remainingTime += 25;
                            AudioManager.PlayCue("Money");
                            //player gets 25 seconds
                        }

                        criticalMessageVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - Fonts.MainFont.MeasureString(criticalMessage).X / 2,
                           ScreenManager.GraphicsDevice.Viewport.Height / 2);   //streching effect
                    }


                    //User selected a piece, just clicked on a piece
                    if (NewMouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released)
                    {
                        Console.WriteLine("SELECTED A PIECE!");
                        AudioManager.PlayCue("Continue");
                        puzzle.SelectPiece(new Vector2(NewMouseState.X, NewMouseState.Y));
                    }
                    //User released click, just dropped a piece
                    else if (NewMouseState.LeftButton == ButtonState.Released && OldMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Console.WriteLine("DROPPED A PIECE!");
                        //Piece is correct placed
                        if (puzzle.PlacePiece(new Vector2(NewMouseState.X, NewMouseState.Y)))
                        {
                            Console.WriteLine("PIECE CORRECT PLACED!");
                            AudioManager.PlayCue("HealImpact");
                            //Puzzle is solved
                            if (puzzle.IsSolved())
                            {
                                Console.WriteLine("SOLVED!!!");
                                AudioManager.PlayCue("WinTheme");
                                //RESTORING DIMENSIONS TO ORIGINAL
                                if (!OriginalWidth.Equals(1360) || !OriginalHeight.Equals(768))
                                {
                                    Console.WriteLine("Restoring to original game dimensions...");
                                    ScreenManager.GetGraphicsDeviceManager.PreferredBackBufferWidth = OriginalWidth;
                                    ScreenManager.GetGraphicsDeviceManager.PreferredBackBufferHeight = OriginalHeight;
                                    ScreenManager.GetGraphicsDeviceManager.ApplyChanges();
                                    Console.WriteLine("RESTORED: " + OriginalWidth + "x" + OriginalHeight);
                                }
                                //EXITING FROM HERE
                                ScreenManager.AddScreen(new MessageBoxScreen("YOU WIN!!!", new MultiplayerScreen()));
                                ScreenManager.RemoveScreen(this);  
                            }
                            //Puzzle NOT solved yet
                            else
                            {
                                Console.WriteLine("STILL PLAYING");
                            }
                        }
                        //Puzzle placed at wrong place
                        else
                        {
                            Console.WriteLine("PIECE MISSPLACED!");
                        }

                        if (puzzle.selectedPiece != null)
                            puzzle.selectedPiece.IsSelected = false;//Release the puzzle
                        puzzle.selectedPiece = null;
                    }

                    //Moving the piece
                    if ((NewMouseState.X != OldMouseState.X || NewMouseState.Y != OldMouseState.Y)
                        && NewMouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Vector2 positionAdjustment = new Vector2(NewMouseState.X, NewMouseState.Y);

                        puzzle.MovePiece(positionAdjustment);
                    }

                    previousTaskTime = DateTime.Now.Second;
                }

            }

            OldMouseState = NewMouseState;
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            //Draw background image
            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);

            if (PostitFlag)//Drawing postit
            {
                postit.Draw(this, gameTime);    //draw postit above everytihng
                spriteBatch.DrawString(Fonts.MainFont, "PRESS ENTER TO START", infoPressEnterVec, Color.Black);
            }

            else        //Playing game
            {

                puzzle.Draw(spriteBatch);

                 
                if (waitingForScramble)
                    spriteBatch.DrawString(Fonts.MainFont, "" + currentScrambleTime, ScrambleTimeVec, Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
                else
                    spriteBatch.DrawString(Fonts.MainFont, remainingTimeAsDate.ToString("mm:ss"), remainingTimeVec, Color.Black);
               
                if (criticalIsAPuzzle || criticalIsAPiece)//draw message and timer on active critical task
                {
                    spriteBatch.DrawString(Fonts.MainFont, criticalMessage, criticalMessageVec, Color.Tomato, 0f, Vector2.Zero, criticalMessageScale, SpriteEffects.None,0f);
                    spriteBatch.DrawString(Fonts.MainFont, currentTaskTime+"",new Vector2(criticalMessageVec.X,criticalMessageVec.Y+20), Color.Tomato, 0f, Vector2.Zero, criticalMessageScale, SpriteEffects.None, 0f);
                    
                }

               
                //TEXT 3D EFFECT
                spriteBatch.DrawString(Fonts.MainFont, StringToDraw, staticInfoStringPosition, Color.Blue);
                //paint again in other place with other colour
                spriteBatch.DrawString(Fonts.MainFont, StringToDraw, staticInfoStringPosition - Vector2.One, Color.White);

            }

            spriteBatch.End();

            base.Draw(gameTime);
         }

        #endregion
    }
}