#region File Description
/**
 * The mouse is on the one side of the screen and the 'general image' on the other
 * A hidden labyrinth lies above the screen and doesn't allow the player to move the mouse freely
 * The player must find the path and reach the general image
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
    public class MGStage12 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;

        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;

        bool GeneralImageIsVisible = false;                 //after the user completes the task the general image appears
        KeyboardState state;
        int width, height;

        Texture2D labyrinthTex;
        Rectangle labyrinthRec;

        Vector2 blockPos, oldblockPos;
        Vector2 blockReferencePoint;
        Texture2D blockTex;
        Rectangle blockRec;
        float RotationAngle = 0f;
        bool collision = false;
        bool finished = false;


        #endregion

        #region Initialization

        public MGStage12()
        {
        }

        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
           // if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))     //MOUSE IS INVISIBLE
            //    ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);
            width = ScreenManager.GraphicsDevice.Viewport.Width;
            height = ScreenManager.GraphicsDevice.Viewport.Height;


            
            GeneralImageRec = new Rectangle(width/2- 50, height/2 -50, 100, 100);
            GeneralImageTexture = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\general_image");
            labyrinthTex = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage12\labyrinth");
            labyrinthRec = new Rectangle(0, 0, labyrinthTex.Width, labyrinthTex.Height);
          

            blockTex = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage12\block");
            blockRec = new Rectangle(width / 2, height / 2, blockTex.Width, blockTex.Height);

            blockPos = new Vector2(2 * width / 5, 4 * height / 5);  //on stage start the red block must be on labyrinth start
            blockReferencePoint = new Vector2(blockTex.Width / 100, blockTex.Height / 100);


            XNA2DCollisionDetection.CollisionDetection2D.AdditionalRenderTargetForCollision = new
                    RenderTarget2D(ScreenManager.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            Oldmstate = Mouse.GetState();
        }


        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        public List<Vector2> GetRectanglePoints()
        {
            List<Vector2> RectanglePoints = new List<Vector2>()
                {
                     Rotations.RotatePoint((blockPos-blockReferencePoint),blockPos, RotationAngle),
                     Rotations.RotatePoint(new Vector2() { X = (blockPos-blockReferencePoint).X + blockTex.Width, Y = 
                         (blockPos-blockReferencePoint).Y }, blockPos, RotationAngle),
                     Rotations.RotatePoint(new Vector2() { X = (blockPos-blockReferencePoint).X + blockTex.Width, Y = 
                         (blockPos-blockReferencePoint).Y + blockTex.Height }, blockPos, RotationAngle),
                     Rotations.RotatePoint(new Vector2() { X = (blockPos-blockReferencePoint).X, Y = 
                         (blockPos-blockReferencePoint).Y + blockTex.Height }, blockPos, RotationAngle)
                };
            return RectanglePoints;
        }

        public bool CheckCollisionWithLabyrinth(RenderTarget2D colTex, Texture2D labTex, Rectangle labRec, int tileWidth, int tileHeight)
        {
            int TileX = (int)Math.Floor((float)(mstate.X / tileWidth));
            int TileY = (int)Math.Floor((float)(mstate.Y / tileHeight));

            float aXPosition = ((TileX * tileWidth) + (tileWidth / 2));
            float aYPosition = ((TileY * tileHeight) + (tileHeight / 2));

            Texture2D CollisionCheck = CreateCollisionTexture(aXPosition, aYPosition, colTex, labTex, tileWidth, tileHeight);
            int pixels = tileWidth * tileHeight;
            Color[] myColors = new Color[pixels];

            CollisionCheck.GetData(0, new Rectangle(0, 0, tileWidth, tileHeight), myColors, 0, pixels);

            foreach (Color aColor in myColors)
            {
                if (aColor == Color.Black && labRec.Contains(mstate.X,mstate.Y))
                {
                    Console.WriteLine("Collisionnnn");
                    return true;
                }
            }

            return false;
        }

        private Texture2D CreateCollisionTexture(float X, float Y, RenderTarget2D colTex, Texture2D txtToDraw, int tileWidth,
           int tileHeight)
        {
            ScreenManager.GraphicsDevice.SetRenderTarget(colTex);
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Red, 0, 0);

            spriteBatch.Begin();

            spriteBatch.Draw(txtToDraw, new Rectangle(0, 0, tileWidth, tileHeight), new Rectangle((int)(X - tileWidth / 2),
                (int)(Y - tileHeight / 2), tileWidth - 1, tileHeight - 1), Color.White);

            spriteBatch.End();

            ScreenManager.GraphicsDevice.SetRenderTarget(null);

            return colTex;
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

            //Move block using mouse or keys
            //X
            if (Oldmstate.X < mstate.X || state.IsKeyDown(Keys.Right))
                blockPos.X += 5;
            else if (Oldmstate.X > mstate.X || state.IsKeyDown(Keys.Left))
                blockPos.X -= 5;
            //Y
            if (Oldmstate.Y < mstate.Y || state.IsKeyDown(Keys.Down))
                blockPos.Y += 5;
            else if (Oldmstate.Y > mstate.Y || state.IsKeyDown(Keys.Up))
                blockPos.Y -= 5;
            
            //If mouse reaches screen limits, restore it ot the middle of the screen
            if (mstate.X > width - 100 || mstate.X<100)
                Mouse.SetPosition(width / 2, mstate.Y);
            if (mstate.Y > height - 100 || mstate.Y < 100)
                Mouse.SetPosition(mstate.X, height/2);
            blockRec.X =(int) blockPos.X;
            blockRec.Y =(int) blockPos.Y;


            //Collision mouse with invisible labyrinth (moving only on white)
            if (!XNA2DCollisionDetection.CollisionDetection2D.SpriteIsOnValidArea(
               labyrinthTex, blockTex, blockReferencePoint, blockPos, GetRectanglePoints(),
                 RotationAngle, spriteBatch, Color.White))
                collision = true;  
            else
                collision = false;

            if (collision)      //can't move on black
                blockPos = oldblockPos;


            //Check Win
            if (blockRec.Intersects(GeneralImageRec) && !finished)
            {
                finished = true;
                ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT AND FINAL STAGE!", new MGStage13()));
            }


            oldblockPos = blockPos;
            Oldmstate = mstate;
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            
            //spriteBatch.Draw(labyrinthTex, labyrinthRec, Color.White);

            spriteBatch.Draw(blockTex, blockRec, Color.White);

            if(collision)
                spriteBatch.DrawString(Fonts.MainFont, "COLLISION", Vector2.Zero, Color.Blue);
            else
                spriteBatch.DrawString(Fonts.MainFont, "NOT COLLISION", Vector2.Zero, Color.Blue);


            spriteBatch.DrawString(Fonts.MainFont, "REACH THE 'GENERAL IMAGE'",
                   new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);

            spriteBatch.Draw(GeneralImageTexture, GeneralImageRec, Color.White);

            spriteBatch.End();
        }

        #endregion
    }
}
