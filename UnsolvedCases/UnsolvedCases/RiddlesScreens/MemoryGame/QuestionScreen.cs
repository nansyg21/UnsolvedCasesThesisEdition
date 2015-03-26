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
    public class QuestionScreen : GameScreen
    {
        #region Fields

        string question;
        string answer;

        Texture2D questionTexture;

        Textfield answerTextfield;

        #endregion

        #region Initialization

        public QuestionScreen(string question, string answer)
        {
            this.question = question;
            this.answer = answer;
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            questionTexture = content.Load<Texture2D>(@"Textures\Message");

            answerTextfield = new Textfield(content, Fonts.MessageFont, "[ CLICK HERE TO ENTER YOUR ANSWER ]", Fonts.LoginColor);
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(0.2f);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = Fonts.MessageFont.MeasureString(question);
            Vector2 answerSize = Fonts.MessageFont.MeasureString(answerTextfield.Text);
            Vector2 textPosition = (viewportSize - textSize) / 2;
            answerTextfield.Position = (viewportSize - answerSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad, (int)textPosition.Y - vPad, 
                (int)textSize.X + hPad * 2, (int)textSize.Y + vPad * 2);

            // Fade the popup alpha during transitions.
            Color color = Color.White;

            spriteBatch.Begin();

            // Draw the background rectangle.
            spriteBatch.Draw(questionTexture, backgroundRectangle, color);
            // Draw the message box text.
            spriteBatch.DrawString(Fonts.MessageFont, question, textPosition, color);
            answerTextfield.Draw(this, gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
