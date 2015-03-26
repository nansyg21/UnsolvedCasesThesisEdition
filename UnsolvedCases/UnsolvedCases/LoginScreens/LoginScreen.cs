#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MySql.Data.MySqlClient;
using UnsolvedCases.Tools;

#endregion

namespace UnsolvedCases
{
    /// <summary>
    /// This is the Login Screen
    /// The player must first login to his account and then move to the other options
    /// </summary>
    public class LoginScreen: GameScreen
    {
        #region Fields

        Texture2D background;
        Rectangle backgroundRectangle;

        Texture2D logo;
        Rectangle logoRectangle;

        Texture2D border;
        Rectangle borderRectangle;

        Vector2 titlePosition;

        Vector2 usernamePosition;
        
        Vector2 passwordPosition;

        Textfield usernameTextfield;

        Textfield passwordTextfield;

        Button loginButton;

        Button registerButton;
        
        string connection;
        MySqlConnection dataConnection;

        KeyboardState currentState;
        KeyboardState previousState;

        string username;
        string password;

        Texture2D line;
        Rectangle lineRectangle;
        float angle;
        float dist;
        Vector2 point1;
        Vector2 point2;

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mouse;

        Rectangle usernameRectangle;
        Rectangle passwordRectangle;

        #endregion

        #region Initialization

        public LoginScreen()
        {
            try
            {
                connection = "datasource=83.212.105.45;port=3306;username=root;password=Na$ia#1988";
                dataConnection = new MySqlConnection(connection);
            }
            catch (Exception ex)
            {
                ScreenManager.AddScreen(new MessageBoxScreen(ex.Message));
            }

            ScreenManager.MainGame.IsMouseVisible = false;
        }

        // Load graphics content for the game.
        public override void LoadContent()
        {
            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            content = ScreenManager.Game.Content;

            background = content.Load<Texture2D>(@"Textures\Login\Background2");
            backgroundRectangle = new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            
            logo = content.Load<Texture2D>(@"Textures\GameLogo2");
            float imageSizeX = (float)logo.Width * ScreenManager.ScaleFactorX;
            float imageSizeY = (float)logo.Height * ScreenManager.ScaleFactorY;
            logoRectangle = new Rectangle((int)ScreenManager.MiddleScreenX - (int)((logo.Width * ScreenManager.ScaleFactorX) / 2), 0, (int)imageSizeX, (int)imageSizeY);

            border = content.Load<Texture2D>(@"Textures\Login\LoginBorder");
            float borderSizeX = (float)border.Width * ScreenManager.ScaleFactorX;
            float borderSizeY = (float)border.Height * ScreenManager.ScaleFactorY;
            borderRectangle = new Rectangle((int)ScreenManager.MiddleScreenX - (int)((border.Width * ScreenManager.ScaleFactorX) / 2), (int)ScreenManager.MiddleScreenY - (int)((border.Height * ScreenManager.ScaleFactorY) / 2),
                (int)borderSizeX, (int)borderSizeY);

            titlePosition = new Vector2(borderRectangle.X + 40, borderRectangle.Y + 20);

            line = content.Load<Texture2D>(@"Textures\Line");
            point1 = new Vector2(titlePosition.X, titlePosition.Y + 55);
            point2 = new Vector2(titlePosition.X + 500, titlePosition.Y + 55);
            angle = (float)System.Math.Atan2(point1.Y - point2.Y, point1.X - point2.X);
            dist = Vector2.Distance(point1, point2);
            lineRectangle = new Rectangle((int)point2.X, (int)point2.Y, (int)dist, 4);

            usernamePosition = new Vector2(titlePosition.X, lineRectangle.Y + 30);

            passwordPosition = new Vector2(titlePosition.X, usernamePosition.Y + 40);

            usernameTextfield = new Textfield(content, Fonts.LoginFont, "[ CLICK HERE TO ENTER YOUR USERNAME ]", Fonts.LoginColor);
            usernameTextfield.Position = new Vector2(usernamePosition.X + 150, usernamePosition.Y + 5);

            passwordTextfield = new Textfield(content, Fonts.LoginFont, "[ CLICK HERE TO ENTER YOUR  PASSWORD ]", Fonts.LoginColor);
            passwordTextfield.Position = new Vector2(usernameTextfield.Position.X, passwordPosition.Y + 5);

            loginButton = new Button(content, "Button");
            loginButton.Position = new Vector2(ScreenManager.MiddleScreenX + (borderSizeX / 2) - 200, usernamePosition.Y); 
            loginButton.Text = "LOGIN";

            registerButton = new Button(content, "Button");
            
            registerButton.Text = "REGISTER";
            registerButton.Position = new Vector2((ScreenManager.MiddleScreenX + (borderSizeX / 2)) - 200, passwordPosition.Y);

            spriteBatch = ScreenManager.SpriteBatch;

            usernameRectangle = new Rectangle((int)usernameTextfield.Position.X, (int)usernameTextfield.Position.Y, usernameTextfield.GetWidth(), usernameTextfield.GetHeight());
            passwordRectangle = new Rectangle((int)passwordTextfield.Position.X, (int)passwordTextfield.Position.Y, passwordTextfield.GetWidth(), passwordTextfield.GetHeight());

            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            // start the menu music
            //AudioManager.PushMusic("MainTheme");

            base.LoadContent();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            mouse = Mouse.GetState();
            
            if ((mouse.LeftButton == ButtonState.Pressed) && (usernameRectangle.Contains(mouse.X, mouse.Y)))
            {
                usernameTextfield.HandleLeftMouseClick();
                passwordTextfield.Clicked = false;
            }
            else if ((mouse.LeftButton == ButtonState.Pressed) && (passwordRectangle.Contains(mouse.X, mouse.Y)))
            {
                passwordTextfield.HandleLeftMouseClick();
                usernameTextfield.Clicked = false;
            }
            else if ((mouse.LeftButton == ButtonState.Pressed) && (!usernameRectangle.Contains(mouse.X, mouse.Y)) && 
                (!passwordRectangle.Contains(mouse.X, mouse.Y)))
            {
                usernameTextfield.Clicked = false;
                passwordTextfield.Clicked = false;
            }

            registerButton.PressedButtonIllusion(mouse);
            loginButton.PressedButtonIllusion(mouse);

            currentState = Keyboard.GetState();
            usernameTextfield.Update(this, gameTime, previousState, currentState);
            passwordTextfield.Update(this, gameTime, previousState, currentState);
            previousState = currentState;
        }

        public override void HandleInput()
        {
            if (loginButton.ButtonPressed)
            {
                AudioManager.PlayCue("Continue");
                username = usernameTextfield.Text.ToString();
                password = passwordTextfield.Text.ToString();

                try
                {
                    dataConnection.Open();
                    MySqlCommand select = new MySqlCommand("SELECT * FROM unsolved_cases_online.users WHERE username='"
                        + username + "' AND password='" + password + "';", dataConnection);

                    MySqlDataReader reader;
                    reader = select.ExecuteReader();
                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                    }

                    reader.Close();
                    dataConnection.Close();

                    if (count == 1)
                    {
                        LoadingScreen.Load(ScreenManager, true, null, new MainMenuScreen());
                    }
                    else
                    {
                        ScreenManager.AddScreen(new MessageBoxScreen("THE USERNAME OR PASSWORD IS INCORRECT."));
                    }
                }
                catch (Exception e)
                {
                    ScreenManager.AddScreen(new MessageBoxScreen(e.Message));
                }
            }

            if (registerButton.ButtonPressed)
            {
                AudioManager.PlayCue("Continue");
                LoadingScreen.Load(ScreenManager, true, null, new RegisterScreen());
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, ScreenManager.SpriteScale);
            spriteBatch.Begin();

            spriteBatch.Draw(background, backgroundRectangle, Color.White);
            spriteBatch.Draw(logo, logoRectangle, Color.White);
            spriteBatch.Draw(border, borderRectangle, Color.White);
            spriteBatch.DrawString(Fonts.HelpFont, "LOGIN", titlePosition, Fonts.MainColor);
            spriteBatch.Draw(line, lineRectangle, null, Fonts.MainColor, angle, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.DrawString(Fonts.MainFont, "USERNAME", usernamePosition, Fonts.MainColor);
            spriteBatch.DrawString(Fonts.MainFont, "PASSWORD", passwordPosition, Fonts.MainColor);
            usernameTextfield.Draw(this, gameTime);
            passwordTextfield.Draw(this, gameTime);
            registerButton.Draw(this, gameTime);
            loginButton.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}

