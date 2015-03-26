#region File Description
#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#endregion

namespace UnsolvedCases
{
    public class SelectSuspectScreen : GameScreen
    {
        #region Fields

        Texture2D background;
        Texture2D glass;

        List<Rectangle> suspectsRecs;
        List<Texture2D> suspectsSprites;

        //int count = -1;

        Rectangle rec;

        Button backButton;

        KeyboardState state;

        #endregion

        #region Initialization

        public SelectSuspectScreen()
        {
            suspectsRecs = new List<Rectangle>();
            suspectsSprites = new List<Texture2D>();
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            background = content.Load<Texture2D>(@"Textures\Riddles\SelectSuspect\choose_a_suspect_background");
            glass = content.Load<Texture2D>(@"Textures\Riddles\SelectSuspect\choose_a_suspect_glass");
           
            backButton = new Button(content, "Button");
            backButton.buttonPosition = new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height - 40);
            backButton.Text = "BACK";

            suspectsSprites.Add(content.Load<Texture2D>(@"Characters\2nd Guard\2nd_guard_front"));
            suspectsSprites.Add(content.Load<Texture2D>(@"Characters\Bulger\bulger_front"));
            suspectsSprites.Add(content.Load<Texture2D>(@"Characters\Former Museum Director\former_museum_director_front"));
            suspectsSprites.Add(content.Load<Texture2D>(@"Characters\Merlino\merlino_front"));
            suspectsSprites.Add(content.Load<Texture2D>(@"Characters\Museum Director\museum_director_front"));
            suspectsSprites.Add(content.Load<Texture2D>(@"Characters\Reissfelder\reissfelder_front"));
            suspectsSprites.Add(content.Load<Texture2D>(@"Characters\Turner\turner_front"));
            suspectsSprites.Add(content.Load<Texture2D>(@"Characters\Youngworth\youngworth_front"));

            for (int i = 0; i < 8; i++)
            {
                suspectsRecs.Add(new Rectangle(((i * (ScreenManager.GraphicsDevice.Viewport.Width / 11) + 
                    (ScreenManager.GraphicsDevice.Viewport.Width / 8))), ScreenManager.GraphicsDevice.Viewport.Height/3, 
                    ScreenManager.GraphicsDevice.Viewport.Width / 11, 45*(ScreenManager.GraphicsDevice.Viewport.Height / 100)));
            }

        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            
            MouseState mouse = Mouse.GetState();

            for (int i = 0; i < 8;i++ )
            {
                rec = suspectsRecs[i];
                if (rec.Contains(mouse.X, mouse.Y))
                {
                    if (rec.Y < ((ScreenManager.GraphicsDevice.Viewport.Height / 3) + 20))
                    {
                        //count++;
                        //Rectangle rec = suspectsRecs[count];
                        rec.Y += 1;
                        suspectsRecs[i] = rec;
                    }
                }
                else
                {
                    //count++;
                    //Rectangle rec = suspectsRecs[count];
                    rec.Y = ScreenManager.GraphicsDevice.Viewport.Height / 3;
                    suspectsRecs[i] = rec;
                }
            }

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());

            backButton.PressedButtonIllusion(mouse);
        }

        public override void HandleInput()
        {
            if (backButton.ButtonPressed)
            {
                AudioManager.PlayCue("Continue");
                LoadingScreen.Load(ScreenManager, false, null, new MainMenuScreen());
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, 
                ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(glass, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, 
                ScreenManager.GraphicsDevice.Viewport.Height), Color.White);

            for (int i = 0; i < 8; i++)
            {
                spriteBatch.Draw(suspectsSprites[i], suspectsRecs[i], Color.White);
            }

            backButton.Draw(this, gameTime);

            spriteBatch.End();
        }
       
        #endregion
    }
}
