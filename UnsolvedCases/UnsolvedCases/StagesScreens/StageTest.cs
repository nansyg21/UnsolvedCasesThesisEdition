#region File Description
#endregion

#region Using Statements

using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace UnsolvedCases
{
    public class StageTest : GameScreen
    {
        #region Fields

        ContentManager content;
        Texture2D background;                   //background image
        Vector2 FontPos;
        Game thisGame;
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Rectangle backgroundRec;
        StageFloor stagefloor;                  //stage
        Victor victor2;                         //character
        KeyboardState state;

        #endregion

        #region Initialization

        public StageTest()
        {
            thisGame = ScreenManager.MainGame;
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            thisGame.IsMouseVisible = true;
        }

        // Load graphics content for the game.
        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            //load textures and create their rectangles
            background = content.Load<Texture2D>("Textures/Stages/1st_floor");
            backgroundRec = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, 
                ScreenManager.GraphicsDevice.Viewport.Height);
            FontPos = new Vector2(0f, 0f);
            graphics = ScreenManager.GraphicsDevice;

            spriteBatch = ScreenManager.SpriteBatch;
            font = Fonts.MainFont;
            thisGame.Services.AddService(typeof(SpriteBatch), spriteBatch);

            stagefloor = new StageFloor(thisGame);
            victor2 = new Victor(thisGame, "Characters/Victor/victor_down_xsmall_1", new Vector2(200, 200));

            thisGame.Components.Add(stagefloor);
            thisGame.Components.Add(victor2);

            /*XNA2DCollisionDetection.CollisionDetection2D.AdditionalRenderTargetForCollision = new 
                RenderTarget2D(graphics, 1366, 768, false, SurfaceFormat.Color, DepthFormat.Depth24);*/
        }

        public override void UnloadContent()
        {
            //content.Unload();
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            //Check the collision
            /*if (!XNA2DCollisionDetection.CollisionDetection2D.SpriteIsOnValidArea(
                    stagefloor.ShadowTexture, victor2.Texture, victor2.Position, victor2.TextureReferencePoint, victor2.RectanglePoints,
                    victor2.RotationAngle, spriteBatch, Color.White))
            {
                victor2.Speed = 0;
            }*/

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
            {
                stagefloor.Dispose();
                victor2.Dispose();
                LoadingScreen.Load(ScreenManager, true, new MainMenuScreen());
            }

            base.Update(gameTime, otherScreenHasFocus, false);
        }
       
        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //deal with the scale with the menu entries when selected
            spriteBatch.Begin();
            spriteBatch.End();
        }

        #endregion
    }
}