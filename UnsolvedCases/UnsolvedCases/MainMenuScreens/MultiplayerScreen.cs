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

        Texture2D MemoryGame;
        Rectangle MemoryGameRectangle;

        Texture2D MouseGame;
        Rectangle MouseGameRectangle;

        Texture2D Platform;
        Rectangle PlatformRectangle;

        Texture2D Puzzle;
        Rectangle PuzzleRectangle;

        Texture2D SuspectEscape;
        Rectangle SuspectEscapeRectangle;

        Texture2D BubbleMath;
        Rectangle BubbleMathRectangle;

        Texture2D ClickMe;
        Rectangle ClickMeRectangle;

        Texture2D quit;
        Rectangle quitRectangle;

        Rectangle selectedRectangle;                    //shows which is the menu selection

        KeyboardState currentState;
        KeyboardState previousState;

       

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

            MemoryGame = content.Load<Texture2D>(@"Textures\MiniGames\memorygame");
            MemoryGameRectangle = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 15, 
                ScreenManager.GraphicsDevice.Viewport.Height / 4,
                MemoryGame.Width, 2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));
            selectedRectangle = MemoryGameRectangle;

            MouseGame = content.Load<Texture2D>(@"Textures\MiniGames\mousegame");
            MouseGameRectangle = new Rectangle(MemoryGameRectangle.X, MemoryGameRectangle.Y + MemoryGame.Height, 
                MouseGame.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            Platform = content.Load<Texture2D>(@"Textures\MiniGames\platform");
            PlatformRectangle = new Rectangle(MemoryGameRectangle.X, MouseGameRectangle.Y + MouseGame.Height, Platform.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            Puzzle = content.Load<Texture2D>(@"Textures\MiniGames\puzzle");
            PuzzleRectangle = new Rectangle(MemoryGameRectangle.X, PlatformRectangle.Y + Platform.Height, Puzzle.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            SuspectEscape = content.Load<Texture2D>(@"Textures\MiniGames\suspectescape");
            SuspectEscapeRectangle = new Rectangle(MemoryGameRectangle.X, PuzzleRectangle.Y + Puzzle.Height, SuspectEscape.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            BubbleMath = content.Load<Texture2D>(@"Textures\MiniGames\bubblemath");
            BubbleMathRectangle = new Rectangle(MemoryGameRectangle.X+MemoryGameRectangle.Width+100, MouseGameRectangle.Y, BubbleMath.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            ClickMe = content.Load<Texture2D>(@"Textures\MiniGames\clickme");
            ClickMeRectangle = new Rectangle(MemoryGameRectangle.X + MemoryGameRectangle.Width + 100, PlatformRectangle.Y, ClickMe.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

            quit = content.Load<Texture2D>(@"Textures\MainMenu\Quit");
            quitRectangle = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width/2-quit.Width/2, SuspectEscapeRectangle.Y + SuspectEscape.Height, quit.Width,
                2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));

           
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
            if (selectedRectangle == MemoryGameRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = MouseGameRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = quitRectangle;
                }
            }
            else if (selectedRectangle == MouseGameRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = PlatformRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = MemoryGameRectangle;
                }
            }
            else if (selectedRectangle == PlatformRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = PuzzleRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = MouseGameRectangle;
                }
            }
            else if (selectedRectangle == PuzzleRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = SuspectEscapeRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = PlatformRectangle;
                }
            }
            else if (selectedRectangle == SuspectEscapeRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = BubbleMathRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = PuzzleRectangle;
                }
            }
            else if (selectedRectangle == BubbleMathRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = ClickMeRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = SuspectEscapeRectangle;
                }
            }
            else if (selectedRectangle == ClickMeRectangle)
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
                        selectedRectangle = BubbleMathRectangle;
                }
            }
            else if (selectedRectangle == quitRectangle)
            {
                if (current.IsKeyDown(Keys.Down))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Down))
                        selectedRectangle = MemoryGameRectangle;
                }
                else if (current.IsKeyDown(Keys.Up))
                {
                    AudioManager.PlayCue("MenuMove");

                    if (!previous.IsKeyDown(Keys.Up))
                        selectedRectangle = SuspectEscapeRectangle;
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

         
        }

        public override void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (selectedRectangle == MemoryGameRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new MemoryGameScreen());
                }
                else if (selectedRectangle == MouseGameRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new MGStage1());
                }
                else if (selectedRectangle == PlatformRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new PlatformerLevel());
                }
                else if (selectedRectangle == PuzzleRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new PuzzleGameScreen());
                }
                else if (selectedRectangle == SuspectEscapeRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new SuspectEscapeScreen());
                }
                else if (selectedRectangle == BubbleMathRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new BubbleMathScreen(true));
                }
                else if (selectedRectangle == ClickMeRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new ClickMeScreen());
                }
                else if (selectedRectangle == quitRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new MainMenuScreen());
                }
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
           // backButton.Draw(this, gameTime);
            //check if the menu entry is selected if so make it scale else just draw it in its place
            if (selectedRectangle == MemoryGameRectangle)
            {
                Rectangle safeArea = MemoryGameRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(MemoryGame, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(MemoryGame, MemoryGameRectangle, Color.White);
            }

            if (selectedRectangle == MouseGameRectangle)
            {
                Rectangle safeArea = MouseGameRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(MouseGame, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(MouseGame, MouseGameRectangle, Color.White);
            }

            if (selectedRectangle == PlatformRectangle)
            {
                Rectangle safeArea = PlatformRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(Platform, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(Platform, PlatformRectangle, Color.White);
            }

            if (selectedRectangle == PuzzleRectangle)
            {
                Rectangle safeArea = PuzzleRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(Puzzle, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(Puzzle, PuzzleRectangle, Color.White);
            }

            if (selectedRectangle == SuspectEscapeRectangle)
            {
                Rectangle safeArea = SuspectEscapeRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(SuspectEscape, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(SuspectEscape, SuspectEscapeRectangle, Color.White);
            }

            if (selectedRectangle == BubbleMathRectangle)
            {
                Rectangle safeArea = BubbleMathRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(BubbleMath, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(BubbleMath, BubbleMathRectangle, Color.White);
            }
            if (selectedRectangle == ClickMeRectangle)
            {
                Rectangle safeArea = ClickMeRectangle;
                Vector2 position = new Vector2(safeArea.X, safeArea.Y);
                spriteBatch.Draw(ClickMe, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(ClickMe, ClickMeRectangle, Color.White);
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
