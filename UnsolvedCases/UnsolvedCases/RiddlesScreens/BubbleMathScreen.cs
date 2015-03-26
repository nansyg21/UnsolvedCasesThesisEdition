#region File Description
/**
 * Add, Substract (and other actions) on the  bubbles, so the addition/difference etc is bigger/smaller than the limit
 * There is only one number as limit at a time, but there can be many limits
 *  The limit is shown on the upper right corner of the screen
 * 
 * 
 * Heuristic: insert mathemetical action, bubble numbers
 *            get numbers in bubbles, numbers in limits
 *            Increasing difficulty 
 **/
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using UnsolvedCases.Toolbox; //for PostIt

#endregion

namespace UnsolvedCases
{
    public class BubbleMathScreen : GameScreen
    {
        #region BubblePieceClass

        public class BubblePiece
        {
            public Texture2D texture;
            public Rectangle rectangle;
            public int number;
            public bool pressed;
            public bool enabled;//initially all bubbles are enabled, on limit overpassing , pressed bubbles are disabled

            //for moving effect
            public int XleftLimit;//bubbles move up, down, left and right, each one moves a small distance
            public int YupperLimit;
            public int XrightLimit;
            public int YbottomLimit;
            public int XSpeed;
            public int YSpeed;



            public BubblePiece(Texture2D texture, Rectangle rectangle, int num)
            {
                Random rand = new Random();
                this.texture = texture;
                this.rectangle = rectangle;
                number = num;
                pressed = false;
                enabled = true;

                XleftLimit = rectangle.X - rand.Next(20, 100); //bubble moves max 30 pixels up and down
                XrightLimit = rectangle.X + rectangle.Width + rand.Next(20, 100);
                YupperLimit = rectangle.Y - rand.Next(20, 100);  //bubble moves 30 pixels left and right
                YbottomLimit = rectangle.Y + rectangle.Height + rand.Next(20, 100);
                XSpeed = 1;
                YSpeed = 1;

            }
        }

        #endregion

        #region Fields

        Texture2D background;
        Texture2D bubble;
        Texture2D mouseRightClickTex;
        Rectangle mouseRightClickRec;
        Vector2 infoRightClickVec;
        String  infoAboutMathOperationStr;
        Vector2 infoAboutMathOperationVec;
        int Total ;          //Total addition, substraction... of selected bubbles
        Vector2 TotalVec = new Vector2();   //a vector that follows the mouse coordinates
        int wins =0;            //how many times the user ovecomes the limit, totally and in each round
        int roundwins ;
        int rounds = 0;         
        int bubblesUsed =0;     // less bubbles -> better score  
        int neutralElement;     //Addition: 0  Multiplication: 1

        int numberOfBubbles = 10;   
        List<BubblePiece> BubblePieceList ;
        bool doubleLimitMode = false;
        static int limitsCount = 3;                //how many numbers are there as limits
        double[] limits = new double[limitsCount];   //limits the user tries to overpass (one at a time)
        double[] limits2 = new double[limitsCount];   //limits the user tries to overpass (one at a time)

        Vector2 limitVec1;                  //vectors of limit numbers
        Vector2 limitVec2;
        Vector2 limitVec3;
        Vector2 limit2Vec;                  //in case we have double limit, second limit appears next to first limit

        String action = "MUL";      //actions: "add" inversed: "sub",  "mul" inversed ("div"), (maybe square roots, modulo ..)
        Random rand = new Random();
        BubblePiece movingBubble;
        //float rot = 0f;
        float movingTimeLimit = 5f;
        float movingTime = 0f;

        ContentManager content;
        SpriteBatch spriteBatch;
        KeyboardState state;
        MouseState mouse;
        MouseState oldmouse;
        BubblePiece mouseOverlappingTmp ;
        DateTime startingTime  ;
        int currentTime, timeLimit=35;  //each round lasts 35 seconds
        Vector2 clockVector;            //where to draw remaining time

        PostIt postit;
        Vector2 infoPressEnterVec;
        bool PostitFlag ;         //Drawing postit or playing the game immediately

        #endregion

        #region Initialization

        public BubbleMathScreen(bool drawpostit)
            : base()
        {
            PostitFlag = drawpostit;//true-> game starts immediately , false-> draw info about game using postit and then game starts
            if (!PostitFlag)
                startingTime = DateTime.Now;
        }

        // Load graphics content for the game.
        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;



            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            background = content.Load<Texture2D>("Textures/Riddles/Puzzle/puzzle_background"); //we need a new background here!!
            bubble = content.Load<Texture2D>("Textures/Riddles/BubbleMathScreen/bubble");
            mouseRightClickTex = content.Load<Texture2D>("Textures/Riddles/BubbleMathScreen/mouse");
            mouseRightClickRec = new Rectangle((9 * ScreenManager.GraphicsDevice.Viewport.Width) / 10,
                                    (8 * ScreenManager.GraphicsDevice.Viewport.Height) / 10, 40, 65);

            infoRightClickVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - Fonts.MainFont.MeasureString("RIGHT CLICK TO RESTART").X,
                mouseRightClickRec.Y + mouseRightClickRec.Height);

            //PostIt
            postit = new PostIt(content);
            postit.Text = "-ADD OR MULTIPLY THE NUMBERS IN BUBBLES\n-REACH THE LIMITS SHOWN IN THE SCREEN\n-YOU NEED 10 WINS\n "+
                                        "-PRESS RIGHT CLICK TO RESTART THE ROUND\n    GOOD LUCK!";
            postit.Width = (int)(postit.Width / 1.5f);
            postit.Height = (int)(postit.Height );
            postit.Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - postit.Width / 2,
                            ScreenManager.GraphicsDevice.Viewport.Height / 2 - postit.Height / 2);

            infoPressEnterVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 -
                            Fonts.MainFont.MeasureString("PRESS ENTER TO START").X / 2, postit.Position.Y+postit.Height);

            //Select randomly an action
            if (rand.Next(3) == 2)//1->MUL 2->ADD
                action = "ADD";
            else
                action = "MUL";

            //Select randomly double limit mode
            if (rand.Next(3) == 2)//1->false 2->true
                doubleLimitMode = true;
            else
                doubleLimitMode = false;

            roundwins = 0;
            if (action.Equals("ADD"))
            {
                neutralElement = 0;
                infoAboutMathOperationStr = "ACTION IS ADDITION (+)";
                infoAboutMathOperationVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 -
                                Fonts.MainFont.MeasureString(infoAboutMathOperationStr).X / 2, 0);
            }
            else if (action.Equals("MUL"))
            {
                neutralElement = 1;
                infoAboutMathOperationStr = "ACTION IS MULTIPLICATION (X)";
                infoAboutMathOperationVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 -
                               Fonts.MainFont.MeasureString(infoAboutMathOperationStr).X / 2, 0);
            }
            Total = neutralElement;


            //Adding limits
            limitVec1 = new Vector2(10 * ScreenManager.GraphicsDevice.Viewport.Width / 11, 1 * ScreenManager.GraphicsDevice.Viewport.Height / 20);
            limitVec2 = new Vector2(limitVec1.X + 50, limitVec1.Y + 50);        //10/11: close to width   -   1/20 : close to height
            limitVec3 = new Vector2(limitVec2.X + 20, limitVec2.Y + 20);


            //Score Stuff   in every round score is set to 0, otherwise we have total score
            //wins = 0;      
            // bubblesUsed = 0;   

            //Adding bubbles
            BubblePieceList = new List<BubblePiece>();
            for (int i = 0; i < numberOfBubbles; i++)
            {                                               //the last arg changes value after that
                BubblePieceList.Add(new BubblePiece(bubble, new Rectangle(rand.Next(ScreenManager.GraphicsDevice.Viewport.Width / 2),
                       rand.Next(ScreenManager.GraphicsDevice.Viewport.Height / 2), 120, 120), 1));

                for (int j = 0; j < i; j++)
                    while (BubblePieceList[i].rectangle.Intersects(BubblePieceList[j].rectangle))
                    {
                        Console.WriteLine("NOT ");
                        BubblePieceList[i].rectangle = new Rectangle(rand.Next(ScreenManager.GraphicsDevice.Viewport.Width / 2),
                       rand.Next(ScreenManager.GraphicsDevice.Viewport.Height / 2), 120, 120);

                    }
            }
            Heuristic();


            ChangeMovingBubble();
            limit2Vec = new Vector2(limitVec1.X + 30, limitVec1.Y);

            clockVector = new Vector2(0, Fonts.MainFont.MeasureString("W").Y);// right down from WINS:..BUBBLES USED...
            // startingTime = DateTime.Now;//start counting time



            oldmouse = Mouse.GetState();
            state = Keyboard.GetState();
            spriteBatch = ScreenManager.SpriteBatch;
        }


        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        #endregion
        public void SwitchLimits()//After the limit is overpassed we go to the next limit
        {
            wins++;
            roundwins++;
            for (int i = 0; i < limits.Length - roundwins; i++)
            {
                limits[i] = limits[i + 1];
                limits2[i] = limits2[i + 1];
            }

            for (int i = limits.Length - roundwins; i < limits.Length; i++)
            {
                limits[i] = 0;
                limits2[i] = 0;
            }

            if (limits[0].Equals(0))//Round is over, start new round
            {
                LoadContent();
            }
                 
        }

        public void Heuristic()//Uses the action("ADD","MUL") and the number of bubbles to calculate limits and
            //numbers in bubbles
            //Rules:
            //The sum/product of the numbers is bubbles > sum/product of all limits: the user can win
            //Every number in limits > any number in bubbles:  using at least 2 bubbles each time
        {
            double limittotal ;
            double bubbletotal ;


            do
            {
                if (action.Equals("ADD"))
                {
                    limittotal = 0;
                    bubbletotal = 0;
                }
                else  // (action.Equals("MUL"))
                {
                    limittotal = 1;
                    bubbletotal = 1;
                }
               

                //Generate limits, from 10 to 15
                for (int i = 0; i < limitsCount; i++)
                {
                    limits[i] = rand.Next(10, 15);
                    //Adding double limits
                    limits2[i] = limits[i] + 4; //only used with double limit
                    Console.WriteLine("Limit:" + i + " number:" + limits[i]);
                    if (action.Equals("ADD"))
                        limittotal += limits[i];
                    else if (action.Equals("MUL"))
                        limittotal *= limits[i];
                }


                //Generate numbers in bubbles, from 2 to 5(half the minimum limit)
                int[] templist = new int[numberOfBubbles];
                for (int i = 0; i < numberOfBubbles; i++)
                {
                    BubblePieceList[i].number = rand.Next(2, 6);
                    if (action.Equals("ADD"))
                        bubbletotal += BubblePieceList[i].number;
                    else if (action.Equals("MUL"))
                        bubbletotal *= BubblePieceList[i].number;

                }
                Console.WriteLine("------ limit total:" + limittotal + " bubble total:" + bubbletotal);
            
              


            } while (bubbletotal <= limittotal);



        }

        public void MoveBubble(BubblePiece bp)//Not used yet
        {
            if (rand.Next(10) < 5)
            {
                bp.rectangle.Y = bp.rectangle.Y + bp.YSpeed;
                if (((bp.rectangle.Y + bp.rectangle.Height) > bp.YbottomLimit) || (bp.rectangle.Y < bp.YupperLimit))
                {
                    bp.YSpeed = bp.YSpeed * -1;
                }

                bp.rectangle.X = bp.rectangle.X + bp.XSpeed;
                if (((bp.rectangle.X + bp.rectangle.Width) > bp.XrightLimit) || (bp.rectangle.X < bp.XleftLimit))
                {
                    bp.XSpeed = bp.XSpeed * -1;
                }
            }


        }

        public void PerformAction(int num)
        {
            if (action.Equals("ADD"))
                Total += num;
            else if (action.Equals("MUL"))
                Total = Total * num;
        }

        public void PerformInversedAction(int num)
        {
            if (action.Equals("ADD"))
                Total -= num;
            else if (action.Equals("MUL"))   
            {
                Total = Total / num;
                if (Total == 0)
                    Total = neutralElement;     //if after division total is 0, set it to 1
            }
             
        }

        public void DeleteEnabledBubbles()
        {
            foreach (BubblePiece bp in BubblePieceList)
                if (bp.pressed && bp.enabled )
                {
                    bubblesUsed++;
                    bp.enabled = false;
                }
        }

        public void LimitOverPassed()
        {
            Total = neutralElement;   //after overcoming limit, set new Total to neutralElement          
            DeleteEnabledBubbles();
            SwitchLimits();
           // doubleLimitMode = false;
        }

        public void ChangeMovingBubble()//Select randomly a bubble
        {
            movingBubble = BubblePieceList[rand.Next(BubblePieceList.Count)];
        }

        public bool MouseIsOnManyBubbles()//Mouse vector contains more than one bubble 
        {
            bool flag=false;//starting with: mouse contains 0 bubbles
            foreach (BubblePiece bp in BubblePieceList)
            {
                if (bp.rectangle.Contains(mouse.X, mouse.Y) && flag) //on second bubble return true
                    return true;
                else if (bp.rectangle.Contains(mouse.X, mouse.Y))//on first bubble update flag
                    flag = true;
                 
            }

            return false;


        }

        #region Update


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);


            mouse = Mouse.GetState();

            if (PostitFlag)//Drawing postit
            {
                if (state.IsKeyDown(Keys.Enter) )
                {
                    PostitFlag = false;
                    startingTime = DateTime.Now;
                }
              
            }
            else        //Playing game
            {
                if (mouse.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
                {
                    foreach (BubblePiece bp in BubblePieceList)
                    {
                        if (bp.rectangle.Contains(mouse.X, mouse.Y) && bp.enabled)//collision and bubble is enabled
                        {
                            if (!bp.pressed) //before it wasn't pressed, now it must be
                            {
                                AudioManager.PlayCue("MenuMove");
                                bp.pressed = true;
                                PerformAction(bp.number);
                            }
                            else
                            {
                                AudioManager.PlayCue("Continue");
                                bp.pressed = false;
                                PerformInversedAction(bp.number);
                            }
                            if (!doubleLimitMode)
                            {
                                if (Total >= limits[0])
                                {
                                    AudioManager.PlayCue("HealCreate");
                                    LimitOverPassed();
                                }
                            }
                            else
                            {
                                if (Total >= limits[0] && Total <= limits2[0])
                                {
                                    AudioManager.PlayCue("HealCreate");
                                    LimitOverPassed();
                                }
                            }

                            break;
                        }

                    }
                }
                else
                {
                    //When the mouse is on 2 bubbles they must swap only 1 time
                    //Dont swap when the mouse contains more than one bubbles
                    for (int i = 0; i < BubblePieceList.Count - 1; i++)
                        if (BubblePieceList[i].rectangle.Contains(mouse.X, mouse.Y) && BubblePieceList[i].enabled
                            && !MouseIsOnManyBubbles())//collision and bubble is enabled
                        {
                            Console.WriteLine(i);
                            mouseOverlappingTmp = BubblePieceList[BubblePieceList.Count - 1];
                            BubblePieceList[BubblePieceList.Count - 1] = BubblePieceList[i];
                            BubblePieceList[i] = mouseOverlappingTmp;



                            Console.WriteLine("swaped");
                            break;
                        }


                }


                TotalVec.X = mouse.X + 40; //total number follows the mouse
                TotalVec.Y = mouse.Y + 40;




                /**       //One bubble moves at a time, randomly chosen  
                     //  MoveBubble(movingBubble);
                       if (movingTime < movingTimeLimit)
                           movingTime += 0.1f;
                       else
                       {
                           movingTime = 0;
                           ChangeMovingBubble();
                       }
                 **/

                //Start new round on right click
                if ((mouse.RightButton == ButtonState.Pressed && oldmouse.RightButton == ButtonState.Released))
                {
                    LoadContent();
                }


                //Update Clock
                if (DateTime.Now.Second >= startingTime.Second)
                    currentTime = timeLimit - (DateTime.Now.Second - startingTime.Second);
                else                                                              //when seconds go from 59 to 0, we add 60
                    currentTime = timeLimit - (DateTime.Now.Second - startingTime.Second + 60);

                //Exit or restart game
                if (wins > 10)
                    ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n YOU WON", new StageTest()));
                else if (currentTime == 0)
                    ScreenManager.AddScreen(new MessageBoxScreen("YOU LOST!!!\n TRY AGAIN", new BubbleMathScreen(false)));

            }
            
            oldmouse = mouse;
            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());
            state = Keyboard.GetState();
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            
            //Draw info about right click
            spriteBatch.Draw(mouseRightClickTex, mouseRightClickRec, Color.White);
            spriteBatch.DrawString(Fonts.MainFont, "RIGHT CLICK TO RESTART", infoRightClickVec, Color.Black);


            if (PostitFlag)//Drawing postit
            {
                postit.Draw(this, gameTime);    //draw postit above everytihng
                spriteBatch.DrawString(Fonts.MainFont, "PRESS ENTER TO START", infoPressEnterVec, Color.Black);
            }

            else         //Playing game
            {
                foreach (BubblePiece bp in BubblePieceList)
                {
                    if (bp.enabled)
                    {
                        if (bp.pressed)
                            spriteBatch.Draw(bp.texture, bp.rectangle, Color.BurlyWood);
                        else
                            spriteBatch.Draw(bp.texture, bp.rectangle, Color.White);

                        //Draw Numbers on bubbles
                        spriteBatch.DrawString(Fonts.MainFont, bp.number + "", new Vector2(bp.rectangle.X + bp.rectangle.Width / 2 -
                                                  Fonts.MainFont.MeasureString(bp.number + "").X / 2, bp.rectangle.Y + bp.rectangle.Height / 2 -
                                                  Fonts.MainFont.MeasureString(bp.number + "").Y / 2), Color.Black);
                    }
                }

                //spriteBatch.DrawString(Fonts.MainFont, Total + "", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, 20), Color.Black);
                spriteBatch.DrawString(Fonts.MainFont, Total + "", TotalVec, Color.Black);


                //Draw limits
                //Double Limit Mode
                if (doubleLimitMode)
                {
                    string tempStr = "BETWEEN " + limits[0] + " - " + limits2[0];
                    Vector2 tempVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width -
                                                 Fonts.MainFont.MeasureString(tempStr).X, limitVec1.Y);

                    //Draw limit 1
                    spriteBatch.DrawString(Fonts.MainFont, tempStr, tempVec, Color.Black);

                    //Draw limit 2
                    tempStr = limits[1] + " - " + limits2[1];
                    tempVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - Fonts.MainFont.MeasureString(tempStr).X,
                                                    tempVec.Y + Fonts.MainFont.MeasureString(tempStr).Y);
                    spriteBatch.DrawString(Fonts.MainFont, tempStr, tempVec, Color.Black);

                    //Draw limit 3
                    tempStr = limits[2] + " - " + limits2[2];
                    tempVec = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - Fonts.MainFont.MeasureString(tempStr).X,
                                                    tempVec.Y + Fonts.MainFont.MeasureString(tempStr).Y);
                    spriteBatch.DrawString(Fonts.MainFont, tempStr, tempVec, Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(Fonts.MainFont, ">" + limits[0] + "", limitVec1, Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(Fonts.MainFont, limits[1] + "", limitVec2, Color.Black);
                    spriteBatch.DrawString(Fonts.MainFont, limits[2] + "", limitVec3, Color.Black);
                }



                spriteBatch.DrawString(Fonts.MainFont, infoAboutMathOperationStr, infoAboutMathOperationVec, Color.Black);

                spriteBatch.DrawString(Fonts.MainFont, "WINS: " + wins + " BUBBLES USED:" + bubblesUsed, Vector2.Zero, Color.Black);

                spriteBatch.DrawString(Fonts.MainFont, "TIME REMAINING: " + currentTime, clockVector, Color.Black);
            }


            spriteBatch.End();
        }

        #endregion
    }
}
