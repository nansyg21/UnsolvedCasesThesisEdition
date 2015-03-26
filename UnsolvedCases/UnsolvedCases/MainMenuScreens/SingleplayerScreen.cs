#region File Description
#endregion

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace UnsolvedCases
{
    public class SingleplayerScreen : GameScreen
    {
        #region Fields

        Texture2D background;

        Texture2D logo;
        Rectangle logoRectangle;

        Vector2 titleRectangle;

        Texture2D case1;
        Rectangle case1Rectangle;

        Texture2D case2;
        Rectangle case2Rectangle;

        Texture2D case3;
        Rectangle case3Rectangle;

        Texture2D case4;
        Rectangle case4Rectangle;

        Texture2D case5;
        Rectangle case5Rectangle;

        Texture2D case6;
        Rectangle case6Rectangle;

        Texture2D line;
        float angle;
        float dist;
        Vector2 point1;
        Vector2 point2;

        Button backButton;

        #endregion

        #region Initialization

        public SingleplayerScreen()
        {
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            background = content.Load<Texture2D>(@"Textures\SubMenu\Background");

            logo = content.Load<Texture2D>(@"Textures\GameLogo");
            logoRectangle = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 150, 0,
                ScreenManager.GraphicsDevice.Viewport.Width / 5, ScreenManager.GraphicsDevice.Viewport.Height / 5);
            
            titleRectangle = new Vector2(0, logoRectangle.Height);

            case1 = content.Load<Texture2D>(@"Textures\SubMenu\Empty_Book");
            case1Rectangle = new Rectangle(400, (int)titleRectangle.Y + 50, 265, 265);

            case2 = content.Load<Texture2D>(@"Textures\SubMenu\Unknown_Case");
            case2Rectangle = new Rectangle(600, (int)titleRectangle.Y + 50, 265, 265);

            case3 = content.Load<Texture2D>(@"Textures\SubMenu\Unknown_Case");
            case3Rectangle = new Rectangle(800, (int)titleRectangle.Y + 50, 265, 265);

            case4 = content.Load<Texture2D>(@"Textures\SubMenu\Unknown_Case");
            case4Rectangle = new Rectangle(400, case1Rectangle.Y + 250, 265, 265);

            case5 = content.Load<Texture2D>(@"Textures\SubMenu\Unknown_Case");
            case5Rectangle = new Rectangle(600, case1Rectangle.Y + 250, 265, 265);

            case6 = content.Load<Texture2D>(@"Textures\SubMenu\Unknown_Case");
            case6Rectangle = new Rectangle(800, case1Rectangle.Y + 250, 265, 265);

            line = content.Load<Texture2D>(@"Textures\Line");
            point1 = new Vector2(0, titleRectangle.Y + 55);
            point2 = new Vector2(titleRectangle.X + 230, titleRectangle.Y + 55);
            angle = (float)System.Math.Atan2(point1.Y - point2.Y, point1.X - point2.X);
            dist = Vector2.Distance(point1, point2);

            backButton = new Button(content, "Button");
            backButton.buttonPosition = new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height - 40);
            backButton.Text = "BACK";
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && case1Rectangle.Contains(mouse.X, mouse.Y))
            {
                LoadingScreen.Load(ScreenManager, true, new ComicDisplay(12, "Intro", new StageTest()));
            }

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
            spriteBatch.Draw(logo, logoRectangle, Color.White);
            spriteBatch.DrawString(Fonts.HelpFont, "CASES", titleRectangle, Color.White);
            spriteBatch.Draw(case1, case1Rectangle, Color.White);
            spriteBatch.Draw(case2, case2Rectangle, Color.White);
            spriteBatch.Draw(case3, case3Rectangle, Color.White);
            spriteBatch.Draw(case4, case4Rectangle, Color.White);
            spriteBatch.Draw(case5, case5Rectangle, Color.White);
            spriteBatch.Draw(case6, case6Rectangle, Color.White);
            spriteBatch.DrawString(Fonts.MainFont, "GARDNER\n MUSEUM\n    HEIST", 
                new Vector2(case1Rectangle.X + 30, case1Rectangle.Y + 52), Fonts.MainColor,
                -0.32f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(line, new Rectangle((int)point2.X, (int)point2.Y, (int)dist, 4), null, Color.White, angle, 
                Vector2.Zero, SpriteEffects.None, 0);
            backButton.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
