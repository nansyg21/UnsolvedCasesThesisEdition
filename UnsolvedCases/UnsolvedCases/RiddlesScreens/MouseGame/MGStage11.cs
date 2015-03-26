﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using UnsolvedCases.Toolbox;


namespace UnsolvedCases
{
    public class MGStage11 : GameScreen
    {
        PostIt postit;
        float elapsed;
        float delay = 500f;

        public struct FootPrint
        {
            //A footprint is consisted of 4 parts
            //  upperleft    upperright
            //  bottomleft   bottomright
            Texture2D f1;
            Texture2D f2;
            Texture2D f3;
            Texture2D f4;

            Rectangle f1Rec;
            Rectangle f2Rec;
            Rectangle f3Rec;
            Rectangle f4Rec;
            Boolean isClicked1; //once the image is clicked it turns visible
            Boolean isClicked2;
            Boolean isClicked3;
            Boolean isClicked4;

            


            //constructor: 4 textures , (x, y) starting dimensions   , (width, height) of the 4 parts together
            public FootPrint(Texture2D _f1, Texture2D _f2, Texture2D _f3, Texture2D _f4,
                         int x, int y, int width, int height)
            {
                f1 = _f1;
                f2 = _f2;
                f3 = _f3;
                f4 = _f4;

                f1Rec = new Rectangle(x, y, width / 2, height / 2);
                f2Rec = new Rectangle(f1Rec.X + f1Rec.Width, f1Rec.Y, f1Rec.Width, f1Rec.Height);
                f3Rec = new Rectangle(f1Rec.X, f1Rec.Y + f1Rec.Height, f1Rec.Width, f1Rec.Height);
                f4Rec = new Rectangle(f3Rec.X + f3Rec.Width, f3Rec.Y, f3Rec.Width, f3Rec.Height);

                isClicked1 = false; //once the image is clicked it turns visible
                isClicked2 = false;
                isClicked3 = false;
                isClicked4 = false;

            }
            # region Getters & Setters
            public Texture2D F1
            {
                get { return this.f1; }
                set { this.f1 = value; }
            }
            public Texture2D F2
            {
                get { return this.f2; }
                set { this.f2 = value; }
            }
            public Texture2D F3
            {
                get { return this.f3; }
                set { this.f3 = value; }
            }
            public Texture2D F4
            {
                get { return this.f4; }
                set { this.f4 = value; }
            }

            public Rectangle F1REC
            {
                get { return this.f1Rec; }
                set { this.f1Rec = value; }
            }
            public Rectangle F2REC
            {
                get { return this.f2Rec; }
                set { this.f2Rec = value; }
            }
            public Rectangle F3REC
            {
                get { return this.f3Rec; }
                set { this.f3Rec = value; }
            }
            public Rectangle F4REC
            {
                get { return this.f4Rec; }
                set { this.f4Rec = value; }
            }

            public bool ISCLICKED1
            {
                get { return this.isClicked1; }
                set { this.isClicked1 = value; }
            }
            public bool ISCLICKED2
            {
                get { return this.isClicked2; }
                set { this.isClicked2 = value; }
            }
            public bool ISCLICKED3
            {
                get { return this.isClicked3; }
                set { this.isClicked3 = value; }
            }
            public bool ISCLICKED4
            {
                get { return this.isClicked4; }
                set { this.isClicked4 = value; }
            }

            #endregion

        }


        #region Fields
        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;
        int width, height;
        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;

        public FootPrint[] Prints = new FootPrint[4]; //full Prints
        bool GeneralImageIsVisible = false;


        #endregion


        public MGStage11()
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


            Oldmstate = Mouse.GetState();
            GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");
            GeneralImageRec = new Rectangle(width / 2, 0, 80, 80);


            Prints[0] = new FootPrint(content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint1"),
                                 content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint2"),
                                content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint3"),
                                 content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint4"),
                                 width / 4, height / 4, 100, 100);

            Prints[1] = new FootPrint(content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint1"),
                                 content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint2"),
                                content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint3"),
                                content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint4"),
                                width / 2, height / 2, 100, 100);

            Prints[2] = new FootPrint(content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint1"),
                                content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint2"),
                               content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint3"),
                               content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint4"),
                                width / 2, height / 4, 100, 100);

            Prints[3] = new FootPrint(content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint1"),
                                content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint2"),
                                content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint3"),
                                content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage3\footprint4"),
                                3 * width / 4, 3 * height / 4, 100, 100);

            //Create postit it visible during the whole game
            postit = new PostIt(content);
            postit.Text = "RINGING ANY BELLS?";
            postit.Width = postit.Width / 2;
            postit.Height = postit.Height / 2;

            postit.Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - postit.Width, 10);
        }

        public bool IsCompleted(FootPrint[] arr) //array with structs
        {
            for (int i = 0; i < arr.Length; i++) //even if we find one "false" we are out 
                if (!arr[i].ISCLICKED1 || !arr[i].ISCLICKED2 || !arr[i].ISCLICKED3 || !arr[i].ISCLICKED4)
                    return false;

            return true;
        }

        public bool ValidatedClickOnGeneralImage()
        {
            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released
                                                 && GeneralImageRec.Contains(mstate.X, mstate.Y) && GeneralImageIsVisible)
                return true;
            return false;
        }
        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }


        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            //Count elapsed time
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            mstate = Mouse.GetState();

            for (int i = 0; i < Prints.Length; i++)//for all the prints images
            {
                //for all the pieces
                if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released)//if user has clicked 
                {
                    if (Prints[i].F1REC.Contains(mstate.X, mstate.Y)) //check all the pieces
                        Prints[i].ISCLICKED1 = true;
                    else if (Prints[i].F2REC.Contains(mstate.X, mstate.Y))
                        Prints[i].ISCLICKED2 = true;
                    else if (Prints[i].F3REC.Contains(mstate.X, mstate.Y))
                        Prints[i].ISCLICKED3 = true;
                    else if (Prints[i].F4REC.Contains(mstate.X, mstate.Y))
                        Prints[i].ISCLICKED4 = true;
                }
            }

            //Play sound when hover above footprint - play again after certain delay
            for (int i = 0; i < Prints.Length; i++)//for all the prints images
            {
                if (elapsed >= delay)
                {
                    if (Prints[i].F1REC.Contains(mstate.X, mstate.Y)) //check all the pieces
                        AudioManager.PlayCue("Money");
                    else if (Prints[i].F2REC.Contains(mstate.X, mstate.Y))
                        AudioManager.PlayCue("Money");
                    else if (Prints[i].F3REC.Contains(mstate.X, mstate.Y))
                        AudioManager.PlayCue("Money");
                    else if (Prints[i].F4REC.Contains(mstate.X, mstate.Y))
                        AudioManager.PlayCue("Money");
                    elapsed = 0;
                }
            
            }

            if (IsCompleted(Prints))
                GeneralImageIsVisible = true; //appear the general image 


            if (ValidatedClickOnGeneralImage())
            {
                ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage12()));
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

            spriteBatch.DrawString(Fonts.MessageFont, "Mouse: " + mstate.X + ", " + mstate.Y, Vector2.Zero, Color.White);
          //  spriteBatch.DrawString(Fonts.MainFont, "CLICK ON THE 'GENERAL IMAGE'", new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);

            for (int i = 0; i < Prints.Length; i++)
            {
                if (Prints[i].ISCLICKED1)
                    spriteBatch.Draw(Prints[i].F1, Prints[i].F1REC, Color.White);

                if (Prints[i].ISCLICKED2)
                    spriteBatch.Draw(Prints[i].F2, Prints[i].F2REC, Color.White);

                if (Prints[i].ISCLICKED3)
                    spriteBatch.Draw(Prints[i].F3, Prints[i].F3REC, Color.White);

                if (Prints[i].ISCLICKED4)
                    spriteBatch.Draw(Prints[i].F4, Prints[i].F4REC, Color.White);
            }

            if (GeneralImageIsVisible)//if we should, draw the general image
                spriteBatch.Draw(GeneralImageTexture, GeneralImageRec, Color.White);

            postit.Draw(this, gameTime);
            spriteBatch.End();
        }

        #endregion
    }
}