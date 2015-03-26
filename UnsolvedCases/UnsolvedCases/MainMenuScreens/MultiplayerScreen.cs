#region File Description
#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

#endregion

namespace UnsolvedCases
{
    public class MultiplayerScreen : GameScreen
    {
        #region Fields

        Texture2D background;

        Texture2D logo;
        Rectangle logoRectangle;

        Texture2D singleplayer;
        Rectangle singleplayerRectangle;

        Texture2D multiplayer;
        Rectangle multiplayerRectangle;

        Texture2D settings;
        Rectangle settingsRectangle;

        Texture2D credits;
        Rectangle creditsRectangle;

        Texture2D help;
        Rectangle helpRectangle;

        Texture2D quit;
        Rectangle quitRectangle;

        Rectangle selectedRectangle;                    //shows which is the menu selection

        KeyboardState currentState;
        KeyboardState previousState;

        Button backButton;

        #endregion

        #region Initialization

        public MultiplayerScreen()
        {
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            background = content.Load<Texture2D>(@"Textures\MainMenu\Background");

            logo = content.Load<Texture2D>(@"Textures\GameLogo");
            logoRectangle = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 150, 0,
                ScreenManager.GraphicsDevice.Viewport.Width / 5, ScreenManager.GraphicsDevice.Viewport.Height / 5);

            singleplayer = content.Load<Texture2D>(@"Textures\MainMenu\Singleplayer");
            singleplayerRectangle = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 15, 
                ScreenManager.GraphicsDevice.Viewport.Height / 3,
                singleplayer.Width, 2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));
            selectedRectangle = singleplayerRectangle;

            multiplayer = content.Load<Texture2D>(@"Textures\MainMenu\Multiplayer");
            multiplayerRectangle = new Rectangle(singleplayerRectangle.X, singleplayerRectangle.Y + singleplayer.Height, 
                multiplayer.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            settings = content.Load<Texture2D>(@"Textures\MainMenu\Settings");
            settingsRectangle = new Rectangle(singleplayerRectangle.X, multiplayerRectangle.Y + multiplayer.Height, settings.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            credits = content.Load<Texture2D>(@"Textures\MainMenu\Credits");
            creditsRectangle = new Rectangle(singleplayerRectangle.X, settingsRectangle.Y + settings.Height, credits.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            help = content.Load<Texture2D>(@"Textures\MainMenu\Help");
            helpRectangle = new Rectangle(singleplayerRectangle.X, creditsRectangle.Y + credits.Height, help.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            quit = content.Load<Texture2D>(@"Textures\MainMenu\Quit");
            quitRectangle = new Rectangle(singleplayerRectangle.X, helpRectangle.Y + help.Height, quit.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            backButton = new Button(content, "Button");
            backButton.buttonPosition = new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height - 40);
            backButton.Text = "BACK";
        }

        #endregion

        #region Update

        /// <summary>
        /// Method to change the selected menu entry
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        public Rectangle KeybordSelection(KeyboardState current, KeyboardState previous)
        {
            if (selectedRectangle == singleplayerRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = multiplayerRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = quitRectangle;
                }
            }
            else if (selectedRectangle == multiplayerRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = settingsRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = singleplayerRectangle;
                }
            }
            else if (selectedRectangle == settingsRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = creditsRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = multiplayerRectangle;
                }
            }
            else if (selectedRectangle == creditsRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = helpRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = settingsRectangle;
                }
            }
            else if (selectedRectangle == helpRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = quitRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = creditsRectangle;
                }
            }
            else if (selectedRectangle == quitRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = singleplayerRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = helpRectangle;
                }
            }

            return selectedRectangle;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            MouseState mouse = Mouse.GetState();

            //calculate the time that passed in the game

            currentState = Keyboard.GetState();
            KeybordSelection(currentState, previousState);
            previousState = currentState;

            backButton.PressedButtonIllusion(mouse);
        }

        public override void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (selectedRectangle == singleplayerRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new MemoryGameScreen());
                }
                else if (selectedRectangle == multiplayerRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new MGStage1());
                }
                else if (selectedRectangle == settingsRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new PuzzleGameScreen());
                }
                else if (selectedRectangle == helpRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new ClickMeScreen());
                }
                else if (selectedRectangle == creditsRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new SuspectEscapeScreen());
                }
                else if (selectedRectangle == quitRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new SelectSuspectScreen());
                }
            }

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
            //deal with the scale with the menu entries when selected
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f;

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, 
                ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(logo, logoRectangle, Color.White);
            backButton.Draw(this, gameTime);
            //check if the menu entry is selected if so make it scale else just draw it in its place
            if (selectedRectangle == singleplayerRectangle)
            {
                Rectangle safeArea = singleplayerRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(singleplayer, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(singleplayer, singleplayerRectangle, Color.White);
            }

            if (selectedRectangle == multiplayerRectangle)
            {
                Rectangle safeArea = multiplayerRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(multiplayer, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(multiplayer, multiplayerRectangle, Color.White);
            }

            if (selectedRectangle == settingsRectangle)
            {
                Rectangle safeArea = settingsRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(settings, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(settings, settingsRectangle, Color.White);
            }

            if (selectedRectangle == creditsRectangle)
            {
                Rectangle safeArea = creditsRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(credits, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(credits, creditsRectangle, Color.White);
            }

            if (selectedRectangle == helpRectangle)
            {
                Rectangle safeArea = helpRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(help, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(help, helpRectangle, Color.White);
            }

            if (selectedRectangle == quitRectangle)
            {
                Rectangle safeArea = quitRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(quit, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(quit, quitRectangle, Color.White);
            }

            spriteBatch.End();
        }

        #endregion
    }
}
