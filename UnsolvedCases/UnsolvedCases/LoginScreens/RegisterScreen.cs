#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MySql.Data.MySqlClient;

#endregion

namespace UnsolvedCases
{
    /// <summary>
    /// This is the Login Screen
    /// The player must first login to his account and then move to the other options
    /// </summary>
    public class RegisterScreen : GameScreen
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

        Vector2 confirmPosition;

        Vector2 emailPosition;

        Textfield usernameTextfield;

        Textfield passwordTextfield;

        Textfield confirmTextfield;

        Textfield emailTextfield;

        Button loginButton;

        Button registerButton;

        string connection;
        MySqlConnection dataConnection;

        string username;
        string password;
        string confirm;
        string email;

        KeyboardState currentState;
        KeyboardState previousState;

        Texture2D line;
        Rectangle lineRectangle;
        float angle;
        float dist;
        Vector2 point1;
        Vector2 point2;

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mouse;

        Rectangle usernameRec;
        Rectangle passwordRec;
        Rectangle confirmpassRec;
        Rectangle emailRec;

        #endregion

        #region Initialization

        public RegisterScreen()
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

        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            spriteBatch = ScreenManager.SpriteBatch;

            //Set Mouse Cursor
            /*ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);*/

            background = content.Load<Texture2D>(@"Textures\Login\Background2");
            backgroundRectangle = new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            logo = content.Load<Texture2D>(@"Textures\GameLogo2");
            float imageSizeX = (float)logo.Width * ScreenManager.ScaleFactorX;
            float imageSizeY = (float)logo.Height * ScreenManager.ScaleFactorY;
            logoRectangle = new Rectangle((int)ScreenManager.MiddleScreenX - (int)((logo.Width * ScreenManager.ScaleFactorX) / 2), 0, (int)imageSizeX, (int)imageSizeY);

            border = content.Load<Texture2D>(@"Textures\Login\RegisterBorder");
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

            confirmPosition = new Vector2(titlePosition.X, passwordPosition.Y + 40);

            emailPosition = new Vector2(titlePosition.X, confirmPosition.Y + 40);

            usernameTextfield = new Textfield(content, Fonts.LoginFont, "[ CLICK HERE TO ENTER YOUR USERNAME ]", Fonts.LoginColor);
            usernameTextfield.Position = new Vector2(usernamePosition.X + (int)Fonts.MainFont.MeasureString("USERNAME").X + 10, usernamePosition.Y + 5);

            passwordTextfield = new Textfield(content, Fonts.LoginFont, "[ CLICK HERE TO ENTER YOUR PASSWORD ]", Fonts.LoginColor);
            passwordTextfield.Position = new Vector2(passwordPosition.X + (int)Fonts.MainFont.MeasureString("PASSWORD").X + 10, passwordPosition.Y + 5);

            confirmTextfield = new Textfield(content, Fonts.LoginFont, "[ CLICK HERE TO CONFIRM YOUR PASSWORD ]", Fonts.LoginColor);
            confirmTextfield.Position = new Vector2(confirmPosition.X + (int)Fonts.MainFont.MeasureString("CONFIRM PASSWORD").X + 10, confirmPosition.Y + 5);

            emailTextfield = new Textfield(content, Fonts.LoginFont, "[ CLICK HERE TO ENTER YOUR E-MAIL ]", Fonts.LoginColor);
            emailTextfield.Position = new Vector2(emailPosition.X + (int)Fonts.MainFont.MeasureString("E-MAIL").X + 10, emailPosition.Y + 5);

            loginButton = new Button(content, "Button");
            loginButton.Position = new Vector2(ScreenManager.MiddleScreenX + (borderSizeX / 2) - 200, confirmPosition.Y);
            loginButton.Text = "LOGIN";

            registerButton = new Button(content, "Button");
            registerButton.Position = new Vector2(ScreenManager.MiddleScreenX + (borderSizeX / 2) - 200, emailPosition.Y);
            registerButton.Text = "REGISTER";

            usernameRec = new Rectangle((int)usernameTextfield.Position.X, (int)usernameTextfield.Position.Y, usernameTextfield.GetWidth(), usernameTextfield.GetHeight());
            passwordRec = new Rectangle((int)passwordTextfield.Position.X, (int)passwordTextfield.Position.Y, passwordTextfield.GetWidth(), passwordTextfield.GetHeight());
            confirmpassRec = new Rectangle((int)confirmTextfield.Position.X, (int)confirmTextfield.Position.Y, confirmTextfield.GetWidth(), confirmTextfield.GetHeight());
            emailRec = new Rectangle((int)emailTextfield.Position.X, (int)emailTextfield.Position.Y, emailTextfield.GetWidth(), emailTextfield.GetHeight());

            base.LoadContent();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            mouse = Mouse.GetState();

            if ((mouse.LeftButton == ButtonState.Pressed) && (usernameRec.Contains(mouse.X, mouse.Y)))
            {
                usernameTextfield.HandleLeftMouseClick();
                passwordTextfield.Clicked = false;
                confirmTextfield.Clicked = false;
                emailTextfield.Clicked = false;
            }
            else if ((mouse.LeftButton == ButtonState.Pressed) && (passwordRec.Contains(mouse.X, mouse.Y)))
            {
                passwordTextfield.HandleLeftMouseClick();
                usernameTextfield.Clicked = false;
                confirmTextfield.Clicked = false;
                emailTextfield.Clicked = false;
            }
            else if ((mouse.LeftButton == ButtonState.Pressed) && (confirmpassRec.Contains(mouse.X, mouse.Y)))
            {
                confirmTextfield.HandleLeftMouseClick();
                usernameTextfield.Clicked = false;
                passwordTextfield.Clicked = false;
                emailTextfield.Clicked = false;
            }
            else if ((mouse.LeftButton == ButtonState.Pressed) && (emailRec.Contains(mouse.X, mouse.Y)))
            {
                emailTextfield.HandleLeftMouseClick();
                usernameTextfield.Clicked = false;
                passwordTextfield.Clicked = false;
                confirmTextfield.Clicked = false;
            }
            else if ((mouse.LeftButton == ButtonState.Pressed) && (!usernameRec.Contains(mouse.X, mouse.Y)) && 
                (!passwordRec.Contains(mouse.X, mouse.Y)) && (!confirmpassRec.Contains(mouse.X, mouse.Y)) && 
                (!emailRec.Contains(mouse.X, mouse.Y)))
            {
                usernameTextfield.Clicked = false;
                passwordTextfield.Clicked = false;
                confirmTextfield.Clicked = false;
                emailTextfield.Clicked = false;
            }

            registerButton.PressedButtonIllusion(mouse);
            loginButton.PressedButtonIllusion(mouse);

            currentState = Keyboard.GetState();
            usernameTextfield.Update(this, gameTime, previousState, currentState);
            passwordTextfield.Update(this, gameTime, previousState, currentState);
            confirmTextfield.Update(this, gameTime, previousState, currentState);
            emailTextfield.Update(this, gameTime, previousState, currentState);
            previousState = currentState;
        }

        public override void HandleInput()
        {
            if (registerButton.ButtonPressed)
            {
                AudioManager.PlayCue("Continue");
                username = usernameTextfield.Text.ToString();
                password = passwordTextfield.Text.ToString();
                confirm = confirmTextfield.Text.ToString();
                email = emailTextfield.Text.ToString();

                try
                {
                    dataConnection.Open();
                    MySqlCommand select = new MySqlCommand("SELECT * FROM unsolved_cases_online.users WHERE username='" +
                        username + "' AND password='" + password + "' AND `email`='" + email + "';", dataConnection);

                    MySqlDataReader reader;
                    reader = select.ExecuteReader();
                    int count = 0;

                    while (reader.Read())
                    {
                        count++;
                    }

                    reader.Close();

                    if (count == 0 && password.Equals(confirm))
                    {
                        MySqlCommand insert = new MySqlCommand
                            ("INSERT INTO unsolved_cases_online.users (`username`, `password`, `email`) VALUES('"
                            + username + "','" + password + "','" + email + "');", dataConnection);
                        MySqlDataReader read;
                        read = insert.ExecuteReader();
                        LoadingScreen.Load(ScreenManager, true, null, new LoginScreen());
                    }
                    else
                    {
                        ScreenManager.AddScreen(new MessageBoxScreen("The user already exists."));
                    }

                    dataConnection.Close();
                }
                catch (Exception e)
                {
                    ScreenManager.AddScreen(new MessageBoxScreen(e.Message));
                }
            }

            if (loginButton.ButtonPressed)
            {
                AudioManager.PlayCue("Continue");
                LoadingScreen.Load(ScreenManager, true, null, new LoginScreen());
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, backgroundRectangle, Color.White);
            spriteBatch.Draw(logo, logoRectangle, Color.White);
            spriteBatch.Draw(border, borderRectangle, Color.White);
            spriteBatch.DrawString(Fonts.HelpFont, "REGISTER", titlePosition, Fonts.MainColor);
            spriteBatch.Draw(line, lineRectangle, null, Fonts.MainColor, angle, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.DrawString(Fonts.MainFont, "USERNAME", usernamePosition, Fonts.MainColor);
            spriteBatch.DrawString(Fonts.MainFont, "PASSWORD", passwordPosition, Fonts.MainColor);
            spriteBatch.DrawString(Fonts.MainFont, "CONFIRM PASSWORD", confirmPosition, Fonts.MainColor);
            spriteBatch.DrawString(Fonts.MainFont, "E-MAIL", emailPosition, Fonts.MainColor);
            usernameTextfield.Draw(this, gameTime);
            passwordTextfield.Draw(this, gameTime);
            confirmTextfield.Draw(this, gameTime);
            emailTextfield.Draw(this, gameTime);
            registerButton.Draw(this, gameTime);
            loginButton.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}

