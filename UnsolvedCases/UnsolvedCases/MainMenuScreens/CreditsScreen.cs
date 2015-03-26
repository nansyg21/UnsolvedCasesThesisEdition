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
    public class CreditsScreen : GameScreen
    {
        #region Fields

        Texture2D background;

        Texture2D logo;
        Rectangle logoRectangle;

        Button backButton;

        List<Texture2D> team_icons;
        List<string> team_names;
        string team_name;
        Texture2D displaySprite;
        Rectangle displayRec;
        Vector2 namePosition;
        float elapsed = 0;
        float delay;
        int frame = 0;
        string subteamTitle;
        Vector2 subteamTitlePos;
        Vector2 screenCenter;
        Vector2 logoCenter;
        Texture2D team;
        bool final = true;
        //string specialthanks;
        Vector2 specialthanksPos;

        #endregion

        #region Initialization

        public CreditsScreen()
        {
            team_icons = new List<Texture2D>();
            team_names = new List<string>();
            displayRec = new Rectangle(0, 0, 0, 0);
            namePosition = new Vector2(0f, 0f);
            delay = 5000f;
            subteamTitlePos = new Vector2(0f, 0f);
            screenCenter = new Vector2(0f, 0f);
            logoCenter = new Vector2(0f, 0f);
            //specialthanks = "";
            specialthanksPos = new Vector2(0f, 0f);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            background = content.Load<Texture2D>(@"Textures\MainMenu\Background");

            screenCenter.X = ScreenManager.GraphicsDevice.Viewport.Width / 2;

            logo = content.Load<Texture2D>(@"Textures\GameLogo");
            logoRectangle = new Rectangle((ScreenManager.GraphicsDevice.Viewport.Width / 2), 0,
                ScreenManager.GraphicsDevice.Viewport.Width / 5, ScreenManager.GraphicsDevice.Viewport.Height / 5);
            logoCenter.X = logoRectangle.Width / 2;

            backButton = new Button(content, "Button");
            backButton.buttonPosition = new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height - 40);
            backButton.Text = "BACK";

            team_icons.Add(content.Load<Texture2D>(@"Characters/Credits/credits_giorgos"));
            team_icons.Add(content.Load<Texture2D>(@"Characters/Credits/credits_panos"));
            team_icons.Add(content.Load<Texture2D>(@"Characters/Credits/credits_nasia"));
            team_icons.Add(content.Load<Texture2D>(@"Characters/Credits/credits_giannis"));
            team_icons.Add(content.Load<Texture2D>(@"Characters/Credits/credits_leuteris"));
            team_icons.Add(content.Load<Texture2D>(@"Characters/Credits/credits_ilias"));
            team_icons.Add(content.Load<Texture2D>(@"Characters/Credits/credits_dimitra"));

            team_names.Add("CHATZIPARASKEVAS \n GIORGOS");
            team_names.Add("KOTSIKORIS \n PANAGIOTIS");
            team_names.Add("MOSCHOU \n ATHANASIA");
            team_names.Add("BANDOS\n GIANNIS");
            team_names.Add("PANELAS\n LEFTERIS");
            team_names.Add("BADIS\n ILIAS");
            team_names.Add("PANAGIOTIDOU\n DIMITRA");

            team = content.Load<Texture2D>(@"Characters/Credits/team");

            displaySprite = team_icons[frame];
            team_name = team_names[frame];
            subteamTitle = "GAME DEVELOPING";
            subteamTitlePos.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - 
                (Fonts.HelpFont.MeasureString(subteamTitle).Length() / 2);
            subteamTitlePos.Y = ScreenManager.GraphicsDevice.Viewport.Height / 3;

            displayRec.Width = displaySprite.Width / 2;
            displayRec.Height = displaySprite.Height / 2;
            displayRec.X = 0 - displayRec.Width;
            displayRec.Y = ScreenManager.GraphicsDevice.Viewport.Height / 2;
            namePosition.X = ScreenManager.GraphicsDevice.Viewport.Width;
            namePosition.Y = ScreenManager.GraphicsDevice.Viewport.Height / 2;

        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (frame < team_icons.Count - 1)
                {
                    frame++;
                    displaySprite = team_icons[frame];
                    team_name = team_names[frame];

                    displayRec.X = 0 - displayRec.Width;
                    namePosition.X = ScreenManager.GraphicsDevice.Viewport.Width;

                    if (frame > 2)
                        subteamTitle = "GRAPHIC DESIGN";
                    if (frame > 4)
                        subteamTitle = "SOUND DESIGN";
                    if (frame > 5)
                        subteamTitle = "ECONOMIC ANALYSIS";

                    subteamTitlePos.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (Fonts.HelpFont.MeasureString(subteamTitle).Length() / 2);

                }
                else
                {
                    if (final)
                    {
                        subteamTitle = "TEAM NAME";
                        displaySprite = team;
                        team_name = "";
                        displayRec.Height = 0;
                        displayRec.Width = 0;
                        // specialthanks = "SPECIAL THANKS TO OUR MENTOR \n ALEXANDER CHATZIGEORGIOU \n FOR HIS SUPPORT THROUGHOUT THIS PROJECT";
                        //specialthanksPos.X = 10;

                        //  specialthanksPos.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (Fonts.MainFont.MeasureString(specialthanks).Length() / 2);
                        //specialthanksPos.Y = 5*(ScreenManager.GraphicsDevice.Viewport.Height / 6);

                        final = false;
                        frame++;
                    }
                }
                elapsed = 0;
            }
            if (frame < team_icons.Count)
            {
                GetInRigthLeft(team_name);
                GetInLeftRight(displaySprite);
            }
            else
            {
                ExpandImg(displaySprite);
            }

            MouseState mouse = Mouse.GetState();

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
            spriteBatch.Draw(logo, screenCenter, null, Color.White, 0f, logoCenter, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(displaySprite, displayRec, Color.White);
            spriteBatch.DrawString(Fonts.MainFont, team_name, namePosition, Fonts.MainColor, 0, new Vector2(0, 0), 1.5f, 
                SpriteEffects.None, 0f);
            spriteBatch.DrawString(Fonts.HelpFont, subteamTitle, subteamTitlePos, Fonts.MainColor);

            if (!final)
            {
                spriteBatch.DrawString(Fonts.MainFont, "SPECIAL THANKS TO OUR MENTOR", 
                    new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 
                        (Fonts.MainFont.MeasureString("SPECIAL THANKS TO OUR MENTOR").Length() / 2), 5 * 
                        (ScreenManager.GraphicsDevice.Viewport.Height / 6)), Fonts.MainColor);
                spriteBatch.DrawString(Fonts.MainFont, "ALEXANDER CHATZIGEORGIOU", 
                    new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 
                        (Fonts.MainFont.MeasureString("ALEXANDER CHATZIGEORGIOU").Length() / 2), 
                        ((5 * (ScreenManager.GraphicsDevice.Viewport.Height / 6)) + 
                        (ScreenManager.GraphicsDevice.Viewport.Height / 18))), Fonts.MainColor);
                spriteBatch.DrawString(Fonts.MainFont, "FOR HIS SUPPORT THOUGHOUT THIS PROJECT", 
                    new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 
                        (Fonts.MainFont.MeasureString("FOR HIS SUPPORT THOUGHOUT THIS PROJECT").Length() / 2), 
                        ((5 * (ScreenManager.GraphicsDevice.Viewport.Height / 6)) + 
                        (2 * (ScreenManager.GraphicsDevice.Viewport.Height / 18)))), Fonts.MainColor);
            }
            backButton.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion

        #region Public Methods

        // The image enters the screen from left
        public void GetInLeftRight(Texture2D currSprite)
        {
            if (displayRec.X < ((ScreenManager.GraphicsDevice.Viewport.Width / 2) - displayRec.Width))
            {
                displayRec.X = displayRec.X + 5;
            }
        }

        // The image enters the screen from right
        public void GetInRigthLeft(string currString)
        {
            if (namePosition.X > ((ScreenManager.GraphicsDevice.Viewport.Width / 2) + 10))
            {
                namePosition.X = namePosition.X - 5;
            }
        }

        public void ExpandImg(Texture2D currSprite)
        {
            if ((displayRec.Width < displaySprite.Width) && (displayRec.Height < displaySprite.Height))
            {
                displayRec.Width = displayRec.Width + 22;
                displayRec.Height = displayRec.Height + 5;
            }

            displayRec.X = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (displayRec.Width / 2);
            displayRec.Y = 2 * (ScreenManager.GraphicsDevice.Viewport.Height / 3) - (displayRec.Height / 2);
        }

        #endregion
    }
}
