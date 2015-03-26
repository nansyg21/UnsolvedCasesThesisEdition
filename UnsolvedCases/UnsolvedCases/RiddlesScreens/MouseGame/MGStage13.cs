#region File Description
/**
 * Its simple, We Kill the programmers
 **/
#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnsolvedCases.Toolbox;

#endregion

namespace UnsolvedCases
{
    public class MGStage13 : GameScreen
    {
        class Worker
        {
            public Texture2D texture;
            public Rectangle rec;
            public int speedX;
            public int speedY;
            public Color clr;
            public bool enabled;
            public int tag;     //1=programmer, 2=graphics designer, 3 sound designer, 4=economical analysis
            public Worker(Texture2D tex, Rectangle r, int tg)
            {
                texture = tex;
                rec = r;
                speedX = 1;
                speedY = 2;
                clr = Color.White;
                enabled = true;
                tag = tg;
            }

            public void UpdateCoordinates()
            {
                rec.X += speedX;
                rec.Y += speedY;
            }

        }

        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;

        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;

        KeyboardState state;
        int width, height;
        int currentTarget = 1;  //defines the target to click and ends the game
        string currentTargetStr = "PROGRAMMERS";
        List<Worker> masterWorkers; //list contains first: 3 programmers(index:0,1,2)  2 graphics designers(index:3,4) 1 sound d/er:(index:5)  1 economical analyser:(index:6) 
 
        int workersWidth, workersHeight;
        int totalworkers = 7;
        bool finished = false;  //so the message box screen won't appear many times 

        PostIt postit; 
        #endregion

        #region Initialization

        public MGStage13()
        {
        }

        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))     //MOUSE IS INVISIBLE
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);
            width = ScreenManager.GraphicsDevice.Viewport.Width;
            height = ScreenManager.GraphicsDevice.Viewport.Height;

            workersWidth = width/7;
            workersHeight = 200;


            //load workers sprites
            masterWorkers = new List<Worker>();
           

            masterWorkers.Add(new Worker(content.Load<Texture2D>(@"Characters\Credits\credits_nasia"),
                  new Rectangle(0, 0, workersWidth, workersHeight), 1));
                      
            masterWorkers.Add(new Worker(content.Load<Texture2D>(@"Characters\Credits\credits_giorgos"),
                        new Rectangle(masterWorkers[masterWorkers.Count - 1].rec.Right, masterWorkers[masterWorkers.Count - 1].rec.Top, workersWidth, workersHeight),1));

            masterWorkers.Add(new Worker(content.Load<Texture2D>(@"Characters\Credits\credits_panos"),
                       new Rectangle(masterWorkers[masterWorkers.Count - 1].rec.Right, masterWorkers[masterWorkers.Count - 1].rec.Top, workersWidth, workersHeight), 1));

            masterWorkers.Add(new Worker(content.Load<Texture2D>(@"Characters\Credits\credits_leuteris"),
                        new Rectangle(masterWorkers[masterWorkers.Count - 1].rec.Right, masterWorkers[masterWorkers.Count - 1].rec.Top, workersWidth, workersHeight),2));

            masterWorkers.Add(new Worker(content.Load<Texture2D>(@"Characters\Credits\credits_giannis"),
                         new Rectangle(masterWorkers[masterWorkers.Count - 1].rec.Right, masterWorkers[masterWorkers.Count - 1].rec.Top, workersWidth, workersHeight), 2));

            masterWorkers.Add(new Worker(content.Load<Texture2D>(@"Characters\Credits\credits_ilias"),
                     new Rectangle(masterWorkers[masterWorkers.Count - 1].rec.Right, masterWorkers[masterWorkers.Count - 1].rec.Top, workersWidth, workersHeight), 3));

            masterWorkers.Add(new Worker(content.Load<Texture2D>(@"Characters\Credits\credits_dimitra"),
               new Rectangle(masterWorkers[masterWorkers.Count - 1].rec.Right, masterWorkers[masterWorkers.Count - 1].rec.Top, workersWidth, workersHeight), 4));

           
            GeneralImageRec = new Rectangle(width / 2 - 50, height / 2 - 50, 100, 100);
            GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");
     
            Oldmstate = Mouse.GetState();

            //Create postit it visible during the whole game
            postit = new PostIt(content);
            postit.Text = "";
            postit.Width = postit.Width / 2;
            postit.Height = postit.Height / 2;

            postit.Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - postit.Width, 10);
        }


        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        #endregion

        #region Update

        public void CheckState()//If all current targets are down we go to the next targets.. ex after programmers we go to graphics designers.. 
        {
            if (currentTarget == 1)//3 programmers disabled -> next target is 2 graphics designers
            {
                if (!masterWorkers[0].enabled && !masterWorkers[1].enabled && !masterWorkers[2].enabled)
                {
                    currentTarget = 2;
                    currentTargetStr = "GRAGHICS DESIGNERS";
                }
            }
            else if (currentTarget == 2)//2 graphics designers disabled -> next target is 1 sound designers
            {
                if (!masterWorkers[3].enabled && !masterWorkers[4].enabled)
                {
                    currentTarget = 3;
                    currentTargetStr = "SOUND DESIGNER";
                }
            }
            else if (currentTarget == 3)//1 sound designer disabled -> next target is 1 economical analyzer
            {
                if (!masterWorkers[5].enabled)
                {
                    currentTarget = 4;
                    currentTargetStr = "ECONOMICAL ANALYZER";
                }
            }
            else if (currentTarget == 4)//1 economical analyzer disabled -> done
            {
                if (!masterWorkers[6].enabled)
                    currentTarget = 5;
            }
            else if (currentTarget == 5)//1 sound designer disabled -> next target is economical analyzer
            {
                finished = true;
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            mstate = Mouse.GetState();

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());

        
            for (int i = 0; i < totalworkers; i++)
            {
                masterWorkers[i].UpdateCoordinates();
                if (((masterWorkers[i].rec.X + masterWorkers[i].rec.Width) > width) || (masterWorkers[i].rec.X < 0))
                {
                    masterWorkers[i].speedX *= -1;
                }

                if (((masterWorkers[i].rec.Y + masterWorkers[i].rec.Height) > height) || (masterWorkers[i].rec.Y < 0))
                {
                    masterWorkers[i].speedY *= -1;
                }
          
                //Check click on rectangle
                if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released
              && masterWorkers[i].rec.Contains(mstate.X, mstate.Y)  && !finished)
                {
                    if (masterWorkers[i].tag == currentTarget)  //correct click
                    {
                        AudioManager.PlayCue("FireballHit");
                        masterWorkers[i].clr = Color.Black;
                        masterWorkers[i].enabled = false;
                        masterWorkers[i].speedY = 0;
                        masterWorkers[i].speedX = 0;

                        CheckState();                           // if true we go to the next targets
                    }
                    else                                        //wrong click
                    {
                        AudioManager.PlayCue("StaffHit");
                    }

                    
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

            for (int i = 0; i < totalworkers; i++)
            {
                spriteBatch.Draw(masterWorkers[i].texture, masterWorkers[i].rec, masterWorkers[i].clr);
            }

            postit.Text = "KILL THE "+currentTargetStr;
            postit.Draw(this, gameTime);
          //  spriteBatch.DrawString(Fonts.MainFont, "KILL THE " + currentTargetStr, new Vector2(0,
          //         ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);

            spriteBatch.End();
        }

        #endregion
    }
}
