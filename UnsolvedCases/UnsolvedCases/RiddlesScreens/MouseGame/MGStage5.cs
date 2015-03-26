#region File Description
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
    public class MGStage5 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;

        KeyboardState state;

        int width, height;
        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;

        Texture2D rope1Tex;                             //WARNING: IMAGE DIMENSIONS MUST BE EQUAL TO RECTANGLE DIMENSIONS
        Rectangle rope1Rec;                             //OTHERWISE: outOfBoundsException and incorrect colision

        Texture2D rope2Tex;                             //WARNING: IMAGE DIMENSIONS MUST BE EQUAL TO RECTANGLE DIMENSIONS
        Rectangle rope2Rec;                             //OTHERWISE: outOfBoundsException and incorrect colision

        Texture2D rope3Tex;                             //WARNING: IMAGE DIMENSIONS MUST BE EQUAL TO RECTANGLE DIMENSIONS
        Rectangle rope3Rec;                             //OTHERWISE: outOfBoundsException and incorrect colision

        int generalImageIsOnRope;                       //tells us in wich rope is the general image hidden

        bool finished = false;                          //If the general image is clicked many times, the MessageBoxScreen is 
        //appeared many times so we need to call MessageBoxScreen only once

        PostIt postit;  

        #endregion

        #region Initialization

        public MGStage5()
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

            GeneralImageRec = new Rectangle(Oldmstate.X - 80 / 2, Oldmstate.Y - 80, 80, 80);

            rope1Tex = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage5\rope1");
            rope1Rec = new Rectangle(50, -440, 200, 500);                           //width & height of the .png image!

            rope2Tex = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage5\rope2");
            rope2Rec = new Rectangle(250, -440, 200, 500);                          //width & height of the .png image!

            rope3Tex = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage5\rope3");
            rope3Rec = new Rectangle(450, -440, 200, 500);                          //width & height of the .png image!

            SetGeneralImageRandomlyOnRope();

            //Create postit it visible during the whole game
            postit = new PostIt(content);
            postit.Text = "LETS PULL SOME STRINGS...";
            postit.Width = postit.Width / 2;
            postit.Height = postit.Height / 2;

            postit.Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - postit.Width, 10);
        }

        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        #endregion

        #region Public Methods

        private void SetGeneralImageRandomlyOnRope()
        {
            //General Image goes randomly on one of the ladders
            Random r = new Random();
            int tmp = r.Next(1, 3);

            if (tmp <= 1)
            {
                generalImageIsOnRope = 1;
                GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");
                GeneralImageRec = new Rectangle(rope1Rec.X + 80, rope1Rec.Y + 80, 80, 80);
            }
            else if (tmp <= 2)
            {
                generalImageIsOnRope = 2;
                GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");
                GeneralImageRec = new Rectangle(rope2Rec.X + 80, rope2Rec.Y + 80, 80, 80);
            }
            else if (tmp <= 3)
            {
                generalImageIsOnRope = 3;
                GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");
                GeneralImageRec = new Rectangle(rope3Rec.X + 80, rope3Rec.Y + 80, 80, 80);
            }

            Console.WriteLine("General Image is on: " + generalImageIsOnRope + " tmp: " + tmp);
        }

        private void MoveGeneralImageAlongWithRope()
        {
            if (generalImageIsOnRope == 1)
            {
                GeneralImageRec.X = rope1Rec.X + rope1Rec.Width / 2 - GeneralImageRec.Width / 2;
                GeneralImageRec.Y = rope1Rec.Y - GeneralImageRec.Height / 2;
            }
            else if (generalImageIsOnRope == 2)
            {
                GeneralImageRec.X = rope2Rec.X + rope2Rec.Width / 2 - GeneralImageRec.Width / 2;
                GeneralImageRec.Y = rope2Rec.Y - GeneralImageRec.Height / 2;
            }
            else if (generalImageIsOnRope == 3)
            {
                GeneralImageRec.X = rope3Rec.X + rope3Rec.Width / 2 - GeneralImageRec.Width / 2;
                GeneralImageRec.Y = rope3Rec.Y - GeneralImageRec.Height / 2;
            }
        }

        private Color[,] TextureTo2DArray(Texture2D texture)//get colors from texture and convert: -> to 1d array -> to 2d array
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            Color[,] colors2D = new Color[texture.Width, texture.Height];

            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }

        private bool CollisionMouseWithRope(Texture2D ladder, Rectangle ladderRec)
        {
            Point p = new Point(Mouse.GetState().X, Mouse.GetState().Y);                    //mouse (x,y)

            Color[,] ladderColours2d = TextureTo2DArray(ladder);

            for (int i = 0; i < ladderRec.Width; i++)
            //Works Only if rectangle dimensions are the same with the image file dimension
            {
                for (int j = 0; j < ladderRec.Height; j++)
                {
                    //mouse is on any color 
                    if (i + ladderRec.X == p.X && j + ladderRec.Y == p.Y && ladderColours2d[i, j].A != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            mstate = Mouse.GetState();

            //Drag rope1
            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Pressed
                && rope1Rec.Contains(mstate.X, mstate.Y) && CollisionMouseWithRope(rope1Tex, rope1Rec))
            {
                Console.WriteLine("Collisionnnn");
                rope1Rec.Y += mstate.Y - Oldmstate.Y;
            }

            //Drag rope2
            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Pressed
                && rope2Rec.Contains(mstate.X, mstate.Y) && CollisionMouseWithRope(rope2Tex, rope2Rec))
            {
                Console.WriteLine("Collisionnnn2");
                rope2Rec.Y += mstate.Y - Oldmstate.Y;
            }

            //Drag rope3
            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Pressed
                && rope3Rec.Contains(mstate.X, mstate.Y) && CollisionMouseWithRope(rope3Tex, rope3Rec))
            {
                Console.WriteLine("Collisionnnn3");
                rope3Rec.Y += mstate.Y - Oldmstate.Y;
            }

            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released
                && GeneralImageRec.Contains(mstate.X, mstate.Y) && !finished)
            {
                finished = true;
                ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage6()));
            }

            MoveGeneralImageAlongWithRope();

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());

            Oldmstate = mstate;
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(rope1Tex, rope1Rec, Color.White);
            spriteBatch.Draw(rope2Tex, rope2Rec, Color.White);
            spriteBatch.Draw(rope3Tex, rope3Rec, Color.White);
            spriteBatch.DrawString(Fonts.MessageFont, "Mouse: " + mstate.X + ", " + mstate.Y, Vector2.Zero, Color.White);
           // spriteBatch.DrawString(Fonts.MainFont, "CLICK ON THE 'GENERAL IMAGE'", new Vector2(0,
           //     ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);
            spriteBatch.Draw(GeneralImageTexture, GeneralImageRec, Color.White);

            postit.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
