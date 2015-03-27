#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace UnsolvedCases
{
    /// <summary>
    /// The Game object for the Unsolved Cases Game.
    /// </summary>
    public class UnsolvedCasesGame : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new UnsolvedCasesGame object.
        /// </summary>
        public UnsolvedCasesGame()
        {
            // Initialize the graphics system.

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1360;
            graphics.PreferredBackBufferHeight = 768;
           // graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
          //  graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            // Configure the content manager.
            Content.RootDirectory = "Content";

            // Add a gamer-services component, which is required for the storage APIs.
            Components.Add(new GamerServicesComponent(this));

            // Add the audio manager.
            AudioManager.Initialize(this, @"Content\Audio\RpgAudio.xgs",
                @"Content\Audio\Wave Bank.xwb", @"Content\Audio\Sound Bank.xsb");

            // Add the screen manager.
            screenManager = new ScreenManager(this, graphics);
            //screenManager.SpriteScale = Matrix.CreateScale((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 1366, (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 768, 1);
            screenManager.MiddleScreenX = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
            screenManager.MiddleScreenY = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
            screenManager.ScaleFactorX = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 1366;
            screenManager.ScaleFactorY = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 768;
            Components.Add(screenManager);
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            InputManager.Initialize();

            base.Initialize();

            //LoadingScreen.Load(screenManager, true, new VideoManager("Intro", new LoginScreen()));
            //LoadingScreen.Load(screenManager, true, new MemoryGameScreen());
            //LoadingScreen.Load(screenManager, true, new ClickMeScreen());
            //LoadingScreen.Load(screenManager, true, new StageTest());
            //LoadingScreen.Load(screenManager, true, new SingleplayerScreen());
            //LoadingScreen.Load(screenManager, true,new SuspectEscapeScreen());
            //LoadingScreen.Load(screenManager,true, new ComicDisplay(12, "Intro", new StageTest()));
            //LoadingScreen.Load(screenManager, true, new SingleplayerScreen());
            //LoadingScreen.Load(screenManager, true, new LoginScreen()); 
            //LoadingScreen.Load(screenManager, true, new PuzzleGameScreen()); 

            LoadingScreen.Load(screenManager, true, new MainMenuScreen());
            //LoadingScreen.Load(screenManager, true, new SelectSuspectScreen());
            //LoadingScreen.Load(screenManager, true, new MGStage8());
            //LoadingScreen.Load(screenManager, true, new PasswordLockScreen());
            //LoadingScreen.Load(screenManager, true, new PlatformerLevel());
            //LoadingScreen.Load(screenManager, true, new BubbleMathScreen(true));
        }

        #endregion

        #region LoadContent and UnloadContent

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Fonts.LoadContent(Content);

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Fonts.UnloadContent();

            base.UnloadContent();
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            base.Draw(gameTime);
        }

        #endregion

        #region OnExiting

        protected override void OnExiting(Object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            // Stop the processes
            foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
            {
                if (myProc.ProcessName == "sakasa")
                {
                    myProc.Kill();
                }
            }
        }

        #endregion

        #region EntryPoint

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            using (UnsolvedCasesGame game = new UnsolvedCasesGame())
            {
                game.Run();
            }
        }

        #endregion
    }
}
