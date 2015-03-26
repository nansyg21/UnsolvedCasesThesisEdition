#region File Description
#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace UnsolvedCases
{
    public class MemoryGameScreen : GameScreen
    {
        #region Struct

        struct memoryImage
        {
            private Texture2D texture;
            private Texture2D coverTexture;
            private Texture2D currentTexture;
            private Rectangle rectangle;
            private Boolean correct;
            private int clicked;
            private string question;
            private string answer;

            public Texture2D Texture
            {
                get { return this.texture; }
                set { this.texture = value; }
            }

            public Texture2D CoverTexture
            {
                get { return this.coverTexture; }
            }

            public Texture2D Current
            {
                get { return this.currentTexture; }
                set { this.currentTexture = value; }
            }

            public Rectangle ImageRectangle
            {
                get { return this.rectangle; }
            }

            public Boolean Correct
            {
                get { return this.correct; }
                set { this.correct = value; }
            }

            public int Clicked
            {
                get { return this.clicked; }
                set { this.clicked = value; }
            }

            public string Question
            {
                get { return this.question; }
            }

            public string Answer
            {
                get { return this.answer; }
            }

            public memoryImage(Texture2D texture, Texture2D coverTexture, Rectangle rectangle, string question, string answer)
            {
                this.texture = texture;
                this.rectangle = rectangle;
                correct = false;
                this.coverTexture = coverTexture;
                currentTexture = coverTexture;
                clicked = 0;
                this.question = question;
                this.answer = answer;
            }
        }

        #endregion

        #region Fields

        List<memoryImage> imagesList = new List<memoryImage>();

        memoryImage[] images;

        ContentManager content;

        Texture2D background;
        Rectangle backgroundRectangle;

        MouseState previousState;
        MouseState currentState;

        int prevHit = -1;
        int currHit;
        int correct = 0;
        int times_to_sleep = 200;
        int times_sleeped = 0;
        int width;
        int height;
        int clicks = 30;
        int sleepMode = 0;
        KeyboardState state;
        SpriteBatch spriteBatch;

        #endregion

        #region Constants

        const int xGap = 120;
        const int yGap = 60;
        const int imageGap = 10;

        #endregion

        #region Initialization

        public MemoryGameScreen() 
            : base()
        {
        }

        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            spriteBatch = ScreenManager.SpriteBatch;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            width = ScreenManager.GraphicsDevice.Viewport.Width / 5;
            height = ScreenManager.GraphicsDevice.Viewport.Height / 5;

            background = content.Load<Texture2D>(@"Textures\Riddles\Puzzle\puzzle_background");
            backgroundRectangle = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
            
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image1"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(xGap, yGap, width, height), "Question1", "Answer1"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image1"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width + xGap + imageGap, yGap, width, height), "Question1", "Answer1"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image2"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width * 2 + xGap + imageGap * 2, yGap, width, height), "Question2", "Answer2"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image2"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width * 3 + xGap + imageGap * 3, yGap, width, height), "Question2", "Answer2"));

            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image3"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(xGap, imagesList[0].ImageRectangle.Height + yGap + imageGap, width, height), "Question3", "Answer3"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image3"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width + xGap + imageGap, imagesList[0].ImageRectangle.Height + yGap + imageGap, width, height), "Question3", "Answer3"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image4"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width * 2 + xGap + imageGap * 2, imagesList[0].ImageRectangle.Height + yGap + imageGap, width, height), "Question4", "Answer4"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image4"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width * 3 + xGap + imageGap * 3, imagesList[0].ImageRectangle.Height + yGap + imageGap, width, height), "Question4", "Answer4"));

            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image5"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(xGap, imagesList[0].ImageRectangle.Height * 2 + yGap + imageGap * 2, width, height), "Question5", "Answer5"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image5"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width + xGap + imageGap, imagesList[0].ImageRectangle.Height * 2 + yGap + imageGap * 2, width, height), "Question5", "Answer5"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image6"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width * 2 + xGap + imageGap * 2, imagesList[0].ImageRectangle.Height * 2 + yGap + imageGap * 2, width, height), "Question6", "Answer6"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image6"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width * 3 + xGap + imageGap * 3, imagesList[0].ImageRectangle.Height * 2 + yGap + imageGap * 2, width, height), "Question6", "Answer6"));

            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image7"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(xGap, imagesList[0].ImageRectangle.Height * 3 + yGap + imageGap * 3, width, height), "Question7", "Answer7"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image7"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width + xGap + imageGap, imagesList[0].ImageRectangle.Height * 3 + yGap + imageGap * 3, width, height), "Question7", "Answer7"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image8"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width * 2 + xGap + imageGap * 2, imagesList[0].ImageRectangle.Height * 3 + yGap + imageGap * 3, width, height), "Question8", "Answer8"));
            imagesList.Add(new memoryImage(content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Image8"),
                content.Load<Texture2D>(@"Textures\Riddles\MemoryGame\Cover"),
                new Rectangle(imagesList[0].ImageRectangle.Width * 3 + xGap + imageGap * 3, imagesList[0].ImageRectangle.Height * 3 + yGap + imageGap * 3, width, height), "Question8", "Answer8"));
            
            images = imagesList.ToArray();

            ShuffleList(images);

            for (int i = 0; i < imagesList.Count; i++)
            {
                images[i].Current = images[i].Texture;
            }

            previousState = Mouse.GetState();
        }

        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        private void ShuffleList(memoryImage[] listToShuffle)
        {
            Random random = new Random();

            for (int i = 0; i < 16; i++)
            {
                int j = random.Next(0, 15);
                Texture2D tmp;

                tmp = listToShuffle[i].Texture;
                listToShuffle[i].Texture = listToShuffle[j].Texture;
                listToShuffle[j].Texture = tmp;
            }
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            currentState = Mouse.GetState();
            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());

            if (sleepMode == 0)
            {
                if (times_sleeped < times_to_sleep)
                    times_sleeped++;
                else
                {
                    times_sleeped = 0;
                    sleepMode = 1;
                    times_to_sleep = 20;

                    for (int i = 0; i < 16; i++)
                    {
                        images[i].Current = images[i].CoverTexture;
                    }
                }
            }

            if (sleepMode == 1)
            {
                for (int i = 0; i < 16; i++)
                {
                    if (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released &&
                                   images[i].ImageRectangle.Contains(currentState.X, currentState.Y) && !images[i].Correct)
                    {
                        if (prevHit == -1)
                        {
                            images[i].Current = images[i].Texture;
                            prevHit = i;
                            clicks--;
                            images[i].Clicked++;
                        }
                        else
                        {
                            if (images[i].Texture.Equals(images[prevHit].Texture) && prevHit != i)
                            {
                                images[i].Current = images[i].Texture;
                                images[i].Correct = true;
                                images[prevHit].Correct = true;
                                prevHit = -1;
                                correct++;
                                clicks--;
                                images[i].Clicked++;

                                //ScreenManager.AddScreen(new QuestionScreen(images[i].Question, images[i].Answer));

                                if (correct == 8)
                                {
                                    ScreenManager.AddScreen(new MessageBoxScreen("YOU WIN!!!", new MultiplayerScreen()));
                                }
                            }
                            else if (!images[i].Texture.Equals(images[prevHit].Texture))
                            {
                                images[i].Current = images[i].Texture;
                                currHit = i;
                                sleepMode = 2;

                                if (images[i].Clicked == 3 || clicks == 0 || images[prevHit].Clicked == 3)
                                {
                                    ScreenManager.AddScreen(new MessageBoxScreen("YOU LOSE!!!", new MultiplayerScreen()));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (times_sleeped < times_to_sleep)
                    times_sleeped++;
                else
                {
                    sleepMode = 1;
                    images[currHit].Current = images[currHit].CoverTexture;
                    images[prevHit].Current = images[prevHit].CoverTexture;  
                    times_sleeped = 0;
                    prevHit = -1;
                }
            }

            previousState = currentState;
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, backgroundRectangle, Color.White);

            spriteBatch.Draw(images[0].Current, images[0].ImageRectangle, Color.White);
            spriteBatch.Draw(images[1].Current, images[1].ImageRectangle, Color.White);
            spriteBatch.Draw(images[2].Current, images[2].ImageRectangle, Color.White);
            spriteBatch.Draw(images[3].Current, images[3].ImageRectangle, Color.White);
            spriteBatch.Draw(images[4].Current, images[4].ImageRectangle, Color.White);
            spriteBatch.Draw(images[5].Current, images[5].ImageRectangle, Color.White);
            spriteBatch.Draw(images[6].Current, images[6].ImageRectangle, Color.White);
            spriteBatch.Draw(images[7].Current, images[7].ImageRectangle, Color.White);
            spriteBatch.Draw(images[8].Current, images[8].ImageRectangle, Color.White);
            spriteBatch.Draw(images[9].Current, images[9].ImageRectangle, Color.White);
            spriteBatch.Draw(images[10].Current, images[10].ImageRectangle, Color.White);
            spriteBatch.Draw(images[11].Current, images[11].ImageRectangle, Color.White);
            spriteBatch.Draw(images[12].Current, images[12].ImageRectangle, Color.White);
            spriteBatch.Draw(images[13].Current, images[13].ImageRectangle, Color.White);
            spriteBatch.Draw(images[14].Current, images[14].ImageRectangle, Color.White);
            spriteBatch.Draw(images[15].Current, images[15].ImageRectangle, Color.White);

            spriteBatch.DrawString(Fonts.LoginFont, "Clicks: " + clicks.ToString(), Vector2.Zero, Color.Red);

            spriteBatch.End();
        }

        #endregion
    }
}
