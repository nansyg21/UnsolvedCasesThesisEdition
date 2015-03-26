#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#endregion

namespace UnsolvedCases
{
    public class ClickMeScreen : GameScreen
    {
        #region Struct

        struct extraImage
        {
            private Texture2D texture;
            private Rectangle rectangle;

            public Texture2D Texture
            {
                get { return this.texture; }
                set { this.texture = value; }
            }

            public Rectangle ImageRectangle
            {
                get { return this.rectangle; }
                set { this.rectangle = value; }
            }

            public extraImage(Texture2D texture, Rectangle rectangle)
            {
                this.texture = texture;
                this.rectangle = rectangle;
            }
        }

        #endregion

        #region Fields

        Random rand = new Random();

        Texture2D item1;
        Rectangle item1Rectangle;

        Texture2D item2;
        Rectangle item2Rectangle;

        Texture2D item3;
        Rectangle item3Rectangle;

        Texture2D item4;
        Rectangle item4Rectangle;

        Texture2D target;
        Rectangle targetRectangle;

        Texture2D cover1;
        Rectangle cover1Rectangle;

        Texture2D cover2;
        Rectangle cover2Rectangle;

        Texture2D cover3;
        Rectangle cover3Rectangle;

        Texture2D cover4;
        Rectangle cover4Rectangle;

        int lives = 10;
        int appearances = 0;

        float timeRemaining = 0.0f;
        float TimePerSquare = 1.5f;

        float alpha1 = 0.0f;
        float alpha2 = 0.0f;
        float alpha3 = 0.0f;
        float alpha4 = 0.0f;

        int xDimension;
        int yDimension;
        int xExtraDimension;
        int yExtraDimension;

        bool clicked = true;

        SpriteFont font;

        ContentManager content;
        SpriteBatch spriteBatch;

        List<extraImage> extraImagesList;

        extraImage[] extraImages;

        KeyboardState state;

        MouseState mouse;

        #endregion

        #region Initialization

        public ClickMeScreen() 
            : base()
        {
            extraImagesList = new List<extraImage>();
        }

        // Load graphics content for the game.
        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            target = content.Load<Texture2D>(@"Textures\Riddles\ClickMe\Target");

            item1 = content.Load<Texture2D>(@"Textures\Riddles\Puzzle\puzzle_background");
            item1Rectangle = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.GraphicsDevice.Viewport.Height / 2);

            item2 = content.Load<Texture2D>(@"Textures\Riddles\Puzzle\puzzle_background");
            item2Rectangle = new Rectangle(item1Rectangle.Width, 0, ScreenManager.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.GraphicsDevice.Viewport.Height / 2);

            item3 = content.Load<Texture2D>(@"Textures\Riddles\Puzzle\puzzle_background");
            item3Rectangle = new Rectangle(0, item1Rectangle.Height, ScreenManager.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.GraphicsDevice.Viewport.Height / 2);

            item4 = content.Load<Texture2D>(@"Textures\Riddles\Puzzle\puzzle_background");
            item4Rectangle = new Rectangle(item1Rectangle.Width, item1Rectangle.Height, ScreenManager.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.GraphicsDevice.Viewport.Height / 2);

            cover1 = content.Load<Texture2D>(@"Textures\HideScreen");
            cover1Rectangle = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.GraphicsDevice.Viewport.Height / 2);

            cover2 = content.Load<Texture2D>(@"Textures\HideScreen");
            cover2Rectangle = new Rectangle(cover1Rectangle.Width, 0, ScreenManager.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.GraphicsDevice.Viewport.Height / 2);

            cover3 = content.Load<Texture2D>(@"Textures\HideScreen");
            cover3Rectangle = new Rectangle(0, cover1Rectangle.Height, ScreenManager.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.GraphicsDevice.Viewport.Height / 2);

            cover4 = content.Load<Texture2D>(@"Textures\HideScreen");
            cover4Rectangle = new Rectangle(cover1Rectangle.Width, cover1Rectangle.Height, ScreenManager.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.GraphicsDevice.Viewport.Height / 2);

            font = content.Load<SpriteFont>(@"Fonts\TimesNewRoman");

            spriteBatch = ScreenManager.SpriteBatch;
        }

        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (timeRemaining == 0.0f && lives != 0)
            {
                appearances++;

                if (appearances % 5 == 0)
                {
                    extraImagesList.Add(new extraImage(content.Load<Texture2D>(@"Textures\Riddles\ClickMe\Extra"), 
                        new Rectangle(0, 0, 0, 0)));
                }

                extraImages = extraImagesList.ToArray();

                for (int i = 0; i < extraImagesList.Count; i++)
                {
                    xExtraDimension = rand.Next(15, 150);
                    yExtraDimension = rand.Next(15, 150);

                    extraImages[i].ImageRectangle = new Rectangle(
                        rand.Next(0, ScreenManager.GraphicsDevice.Viewport.Width - xExtraDimension),
                        rand.Next(0, ScreenManager.GraphicsDevice.Viewport.Height - yExtraDimension), xExtraDimension, yExtraDimension);
                }

                xDimension = rand.Next(15, 150);
                yDimension = rand.Next(15, 150);

                targetRectangle = new Rectangle(
                    rand.Next(0, ScreenManager.GraphicsDevice.Viewport.Width - xDimension),
                    rand.Next(0, ScreenManager.GraphicsDevice.Viewport.Height - yDimension), xDimension, yDimension);

                if (TimePerSquare > 0.4f)
                    TimePerSquare -= 0.03f;

                timeRemaining = TimePerSquare;

                if (!clicked)
                    lives--;
            }

            mouse = Mouse.GetState();

            if ((mouse.LeftButton == ButtonState.Pressed) && (targetRectangle.Contains(mouse.X, mouse.Y)))
            {
                timeRemaining = 0.0f;

                clicked = true;

                if (item1Rectangle.Contains(mouse.X, mouse.Y) && alpha1 < 1.0f)
                    alpha1 += 0.2f;
                else if (item2Rectangle.Contains(mouse.X, mouse.Y) && alpha2 < 1.0f)
                    alpha2 += 0.2f;
                else if (item3Rectangle.Contains(mouse.X, mouse.Y) && alpha3 < 1.0f)
                    alpha3 += 0.2f;
                else if (item4Rectangle.Contains(mouse.X, mouse.Y) && alpha4 < 1.0f)
                    alpha4 += 0.2f;
            }
            else
                clicked = false;

            timeRemaining = MathHelper.Max(0, timeRemaining - (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (lives == 0)
                ScreenManager.AddScreen(new MessageBoxScreen("YOU LOSE!!!", new MultiplayerScreen()));

            if (alpha1 == 1.0f &&  alpha2 == 1.0f && alpha3 == 1.0f && alpha4 == 1.0f)
                ScreenManager.AddScreen(new MessageBoxScreen("YOU WIN!!!", new MultiplayerScreen()));

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(item1, item1Rectangle, Color.White);
            spriteBatch.Draw(item2, item2Rectangle, Color.White);
            spriteBatch.Draw(item3, item3Rectangle, Color.White);
            spriteBatch.Draw(item4, item4Rectangle, Color.White);

            spriteBatch.Draw(cover1, cover1Rectangle, Color.Black * alpha1);
            spriteBatch.Draw(cover2, cover2Rectangle, Color.Black * alpha2);
            spriteBatch.Draw(cover3, cover3Rectangle, Color.Black * alpha3);
            spriteBatch.Draw(cover4, cover4Rectangle, Color.Black * alpha4);

            for (int i = 0; i < extraImagesList.Count; i++)
            {
                spriteBatch.Draw(extraImages[i].Texture, extraImages[i].ImageRectangle, Color.White);
            }

            spriteBatch.DrawString(font, "Lives : " + lives.ToString(), new Vector2(0, 0), Color.OrangeRed);
            spriteBatch.Draw(target, targetRectangle, Color.White);

            spriteBatch.End();
        }

        #endregion
    }
}
