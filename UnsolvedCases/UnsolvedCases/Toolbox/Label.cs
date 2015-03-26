#region File Description
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

#endregion

namespace UnsolvedCases
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    public class Label
    {
        #region Fields
        
        // The text rendered for this entry.
        string text;
        
        // The position at which the entry is drawn. This is set by the MenuScreen
        // each frame in Update.
        Vector2 position;
        Color color = Color.Yellow;
        SpriteFont font;

        #endregion

        #region Properties

        // Gets or sets the text of this menu entry.
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        // Gets or sets the position at which to draw this menu entry.
    
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        public Label(ContentManager content, string text, SpriteFont font)
        {
            this.text = text;
            this.font = font;
           // this.font = content.Load<SpriteFont>(@"Fonts\" + font);
        }
        
        #endregion

        #region Update and Draw
       
        // Updates the menu entry.
        public virtual void Update(GameScreen screen, GameTime gameTime) { }

        /// Draws the menu entry. This can be overridden to customize the appearance.
        public virtual void Draw(GameScreen screen, GameTime gameTime)
        {
            // Draw the selected entry in yellow, otherwise white.
            
            // color = Color.Yellow;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;

            //Vector2 origin = new Vector2(0, font.LineSpacing / 2);
            Vector2 origin = new Vector2(0, 0.5f);
            
            //spriteBatch.DrawString(font, text, position, color, 0,
                                  // origin, scale, SpriteEffects.None, 0);
            

            //Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color);
        }

        public virtual Rectangle GetRectagnle(GameScreen screen)
        {
            Rectangle rec = new Rectangle(0, 0, 0, 0);
            rec.X = (int)this.Position.X;
            rec.Y = (int)this.Position.Y;
            rec.Width = this.GetWidth(screen);
            rec.Height = this.GetHeight(screen);

            return rec;
        }

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(GameScreen screen)
        {
            return font.LineSpacing;
        }

        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public virtual int GetWidth(GameScreen screen)
        {
            return (int)font.MeasureString(Text).X;
        }

        public virtual Color GetColor()
        {
            return this.color;
        }

        public Color Color
        {
            set { color = value; }
        }

        #endregion
    }
} 