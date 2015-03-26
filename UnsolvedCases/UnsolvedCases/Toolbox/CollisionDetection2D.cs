#region File Description
#endregion

 #region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

#endregion

namespace XNA2DCollisionDetection
{
    public static class CollisionDetection2D
    {
        /// <summary>
        /// This should be initialize at the initialize of the main game loop as follows:
        /// <example>
        /// <code>
        /// CollisionDetection2D.AdditionalRenderTargetForCollision=new RenderTarget2D(_graphics.GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight,1,_graphics.GraphicsDevice.DisplayMode.Format);
        /// </code>
        /// </example>
        /// </summary>
        public static RenderTarget2D AdditionalRenderTargetForCollision { get; set; }
        
        /// <summary>
        /// Checks whether a sprite is on a valid position in the scene (non collision)
        /// </summary>
        /// <param name="TextureScene">The texture used for the scene (texture for CD)</param>
        /// <param name="TextureSprite">The sprite's texture</param>
        /// <param name="PosSprite">The sprite's position</param>
        /// <param name="OrigSprite">The sprite texture's reference point</param>
        /// <param name="RectSprite">The sprite's bounding rectangle</param>
        /// <param name="ThetaSprite">The sprite's angle of rotation</param>
        /// <param name="spriteBatch">The current spriteBatch</param>
        /// <param name="OKColor">The color that defines a valid position on the scene's texture</param>
        /// <returns>True if sprite is on a valid area</returns>
        public static bool SpriteIsOnValidArea(Texture2D TextureScene, Texture2D TextureSprite, Vector2 PosSprite, Vector2 OrigSprite, 
                                      List<Vector2> RectSprite, float ThetaSprite, SpriteBatch spriteBatch, Color OKColor)
        {
            Color[] TextureDataSprite = _getSingleSpriteAloneFromScene(spriteBatch, TextureSprite, PosSprite, OrigSprite, 
                ThetaSprite, RectSprite);
            Color[] TextureDataScene =  new Color[TextureScene.Width*TextureScene.Height];
            TextureScene.GetData(TextureDataScene);

            Rectangle RectangleSpriteBounding = _getBoundingRectangleOfRotatedRectangle(RectSprite);
            for (int i = RectangleSpriteBounding.Top; i < RectangleSpriteBounding.Bottom; i++)
                for (int j = RectangleSpriteBounding.Left; j < RectangleSpriteBounding.Right; j++)
                    if (TextureDataScene[i * TextureScene.Width + j] != OKColor && TextureDataSprite[(i - RectangleSpriteBounding.Top) 
                        * RectangleSpriteBounding.Width + (j - RectangleSpriteBounding.Left)] != new Color(0, 0, 0, 0))
                        return false;
            
            return true;
        }

        #region Private members

        private static Rectangle _getBoundingRectangleOfRotatedRectangle(List<Vector2> RectanglePoints)
        {
            Vector2 BoundingRectangleStart = new Vector2()
            {
                X = _getMinimumOf(RectanglePoints[0].X, RectanglePoints[1].X,RectanglePoints[2].X,RectanglePoints[3].X),
                Y = _getMinimumOf(RectanglePoints[0].Y, RectanglePoints[1].Y, RectanglePoints[2].Y, RectanglePoints[3].Y)
            };

            int BoundingRectangleWidth = -(int)BoundingRectangleStart.X + _getMaximumOf(RectanglePoints[0].X, RectanglePoints[1].X, 
                RectanglePoints[2].X, RectanglePoints[3].X);
            int BoundingRectangleHeight = -(int)BoundingRectangleStart.Y + _getMaximumOf(RectanglePoints[0].Y, RectanglePoints[1].Y, 
                RectanglePoints[2].Y, RectanglePoints[3].Y);

            return new Rectangle((int)BoundingRectangleStart.X, (int)BoundingRectangleStart.Y, BoundingRectangleWidth, 
                BoundingRectangleHeight);
        }

        private static int _getMinimumOf(float a1, float a2, float a3, float a4)
        {
            return (int)MathHelper.Min(MathHelper.Min(MathHelper.Min(a1, a2), a3), a4);
        }

        private static int _getMaximumOf(float a1, float a2, float a3, float a4)
        {
            return (int)MathHelper.Max(MathHelper.Max(MathHelper.Max(a1, a2), a3), a4);
        }

        private static Color[] _getSingleSpriteAloneFromScene(SpriteBatch spriteBatch, Texture2D textureToDraw, Vector2 Position,
            Vector2 Origin,float Theta, List<Vector2> RectanglePoints)
        {
            spriteBatch.GraphicsDevice.SetRenderTarget(AdditionalRenderTargetForCollision);
            spriteBatch.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(textureToDraw, Position, null, Color.White, Theta, Origin, 1.0f, SpriteEffects.None, 0);

            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            Texture2D SceneTexture = (Texture2D) AdditionalRenderTargetForCollision;
            Rectangle BoundingRectangle = _getBoundingRectangleOfRotatedRectangle(RectanglePoints);
            int PixelSize = BoundingRectangle.Width * BoundingRectangle.Height;
            Color[] TextureData = new Color[PixelSize];
            SceneTexture.GetData(0, BoundingRectangle, TextureData, 0, PixelSize);

            return TextureData;
        }

        #endregion
    }
}
