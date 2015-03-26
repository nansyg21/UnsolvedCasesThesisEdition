#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace UnsolvedCases
{
    /// <summary>
    /// This is the Login Screen
    /// The player must first login to his account and then move to the other options
    /// </summary>
    public class MainMenuScreen : GameScreen
    {
        #region Fields

        ContentManager content;

        float pauseAlpha;
        float elapsed;                          // for animation frame change
        float delay = 200f;                     //delay between two animation frames

        int frame = 0;                          //choose the animation frame

        Texture2D background;

        Texture2D logo;
        Rectangle logoRectangle;

        Texture2D shadow;
        Rectangle shadowRectangle;

        Texture2D smoke;
        Rectangle smokeDestinationRectangle;
        Rectangle smokeSourceRectangle;

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

        Rectangle selectedRectangle;                //shows which is the menu selection

        KeyboardState currentState;
        KeyboardState previousState;

        SpriteBatch spriteBatch;

        #endregion

        #region Initialization

        public MainMenuScreen() 
            : base()
        {
        }

        // Load graphics content for the game.
        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;

            //load textures and create their rectangles
            background = content.Load<Texture2D>(@"Textures\MainMenu\Background");

            smoke = content.Load<Texture2D>(@"Textures\MainMenu\Smoke");
            smokeDestinationRectangle = new Rectangle(930, 0, 100, ((ScreenManager.GraphicsDevice.Viewport.Height / 2)
                + (ScreenManager.GraphicsDevice.Viewport.Height / 12)));

            shadow = content.Load<Texture2D>(@"Textures\MainMenu\Shadow");
            shadowRectangle = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width / 2, 
                ScreenManager.GraphicsDevice.Viewport.Height);

            singleplayer = content.Load<Texture2D>(@"Textures\MainMenu\Singleplayer");
            singleplayerRectangle = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 15, 
                ScreenManager.GraphicsDevice.Viewport.Height / 3, 
                singleplayer.Width, 2 * (ScreenManager.GraphicsDevice.Viewport.Height / 20));
            selectedRectangle = singleplayerRectangle;

            multiplayer = content.Load<Texture2D>(@"Textures\MainMenu\Multiplayer");
            multiplayerRectangle = new Rectangle(singleplayerRectangle.X, singleplayerRectangle.Y + 
                singleplayer.Height, multiplayer.Width,
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

            logo = content.Load<Texture2D>(@"Textures\GameLogo");
            logoRectangle = new Rectangle((singleplayerRectangle.X + (singleplayer.Width / 2) - (logo.Width / 2)), 0,
                logo.Width, ScreenManager.GraphicsDevice.Viewport.Height / 3); 
        }
       
        public override void UnloadContent()
        {
            //content.Unload();
        }

        #endregion

        #region Update and Draw

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
                        selectedRectangle=multiplayerRectangle;
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

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            
            //calculate the time that passed in the game
            elapsed += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            
            currentState = Keyboard.GetState(); 
            KeybordSelection(currentState, previousState);
            previousState = currentState;
          
            //change the frame of animation (the animation has a total of 6 frames)
            //change a frame after some delay to make it smooth
            //if you reach the 6th frame go to the first one again
            //else just go to the next one
            //do not forget to set elapsed back to zero otherwise this will just happen once
            //update the part of the initial image you want to show
            if (elapsed >= delay)
            {
                if (frame >= 6)
                    frame = 0;
                else
                    frame++;
                elapsed = 0;
            }

            smokeSourceRectangle = new Rectangle(frame * 100, 0, 100, 262);
        }

        public override void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (selectedRectangle == singleplayerRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new SingleplayerScreen());
                }
                else if (selectedRectangle == multiplayerRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());
                }
                else if (selectedRectangle == settingsRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new SettingsScreen());
                }
                else if (selectedRectangle == helpRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new HelpScreen());
                }
                else if (selectedRectangle == creditsRectangle)
                {
                    LoadingScreen.Load(ScreenManager, true, new CreditsScreen());
                }
                else if (selectedRectangle == quitRectangle)
                {
                    ScreenManager.Game.Exit();
                }
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch = ScreenManager.SpriteBatch;
            //deal with the scale with the menu entries when selected
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f;

            spriteBatch.Begin();
            //draw background image
            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, 
                ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            //draw animation
            //DestinationRec is where the rectangle will be drawn
            //SourceRec is the part of the texture that will be drawn
            spriteBatch.Draw(smoke, smokeDestinationRectangle, smokeSourceRectangle, Color.White);
            //draw the white layer back of the menu entries
            spriteBatch.Draw(shadow, shadowRectangle, Color.White);
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
                        
            spriteBatch.Draw(logo, logoRectangle, Color.White);
            
            spriteBatch.End();
        }

        #endregion
    }
}

