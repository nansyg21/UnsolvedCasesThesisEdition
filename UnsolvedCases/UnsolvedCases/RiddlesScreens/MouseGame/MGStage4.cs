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
    public class MGStage4 : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;

        int width, height;

        Texture2D GeneralImageTexture;
        Rectangle GeneralImageRec;

        Texture2D corner1;              //upper left
        Rectangle corner1Rec;
        bool drawTheCorner = false;

        Color[] colorDataGeneralImage;
        bool finished = false;              //If the general image is clicked many times, the MessageBoxScreen 
        //is appeared many times so we need to call MessageBoxScreen only once

        RenderTarget2D colTex1;             //corner1
        int tileWidth1;
        int tileHeight1;

        KeyboardState state;

        PostIt postit;

        #endregion

        #region Initialization

        public MGStage4()
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
            GeneralImageRec = new Rectangle(Oldmstate.X - 80 / 2, Oldmstate.Y - 80, 80, 80);

            corner1 = content.Load<Texture2D>(@"Textures\Riddles\MouseGame\MGStage4\corner1");
            corner1Rec = new Rectangle(width / 18, height / 18, 50, 50);

            colorDataGeneralImage = new Color[GeneralImageTexture.Width * GeneralImageTexture.Height];      //Get colours of general image
            GeneralImageTexture.GetData<Color>(colorDataGeneralImage);

            colTex1 = new RenderTarget2D(ScreenManager.GraphicsDevice, corner1Rec.Width, corner1Rec.Height, false, SurfaceFormat.Color,
                DepthFormat.Depth24);
            tileWidth1 = corner1Rec.Width;
            tileHeight1 = corner1Rec.Height;

            //Create postit it visible during the whole game
            postit = new PostIt(content);
            postit.Text = "JUST AROUND THE CORNER...";
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

        public bool CheckCollisionWithCorner(RenderTarget2D colTex, Texture2D corner, Rectangle cornerRec, int tileWidth, int tileHeight)
        {
            int TileX = (int)Math.Floor((float)(mstate.X / tileWidth));
            int TileY = (int)Math.Floor((float)(mstate.Y / tileHeight));

            float aXPosition = ((TileX * tileWidth) + (tileWidth / 2));
            float aYPosition = ((TileY * tileHeight) + (tileHeight / 2));

            Texture2D CollisionCheck = CreateCollisionTexture(aXPosition, aYPosition, colTex, corner, tileWidth, tileHeight);
            int pixels = tileWidth * tileHeight;
            Color[] myColors = new Color[pixels];

            CollisionCheck.GetData(0, new Rectangle(0, 0, tileWidth, tileHeight), myColors, 0, pixels);

            foreach (Color aColor in myColors)
            {
                if (aColor == Color.Black && cornerRec.Intersects(GeneralImageRec))
                {
                    Console.WriteLine("Collisionnnn");
                    return true;
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

            //if general image doesn't collides with corner,the general image moves 
            if (!CheckCollisionWithCorner(colTex1, corner1, corner1Rec, tileWidth1, tileHeight1))
            {
                drawTheCorner = false;
                GeneralImageRec.X = mstate.X - GeneralImageRec.Width / 2;                   //general image is above the mouse 
                GeneralImageRec.Y = mstate.Y - GeneralImageRec.Height;                      //and is moving with the mouse  
            }
            else
                //if general image collides with corner,the corner appears and general image is trapped
                drawTheCorner = true;

            if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released
                && GeneralImageRec.Contains(mstate.X, mstate.Y) && !finished)
            {
                finished = true;
                ScreenManager.AddScreen(new MessageBoxScreen("VERY NICE!!!\n GET READY FOR NEXT STAGE", new MGStage5()));
            }

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

            spriteBatch.DrawString(Fonts.MessageFont, "Mouse: " + mstate.X + ", " + mstate.Y, Vector2.Zero, Color.White);
          //  spriteBatch.DrawString(Fonts.MainFont, "CLICK ON THE 'GENERAL IMAGE'", new Vector2(0,
          //      ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White);

            if (drawTheCorner)
                spriteBatch.Draw(corner1, corner1Rec, null, Color.White);                       //upper left corner

            spriteBatch.Draw(GeneralImageTexture, GeneralImageRec, Color.White);
            postit.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
