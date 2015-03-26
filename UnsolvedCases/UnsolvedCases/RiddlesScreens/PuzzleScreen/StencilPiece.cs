#region File Description
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class StencilPiece
{
    public Texture2D texture;
    public Vector2 position;
    public int width;
    public int height;

    public StencilPiece(Texture2D theTexture, Vector2 thePosition, int theWidth, int theHeight)
    {
        position = thePosition;
        texture = theTexture;
        width = theWidth;
        height = theHeight;
    }

    public void Draw(SpriteBatch batch)
    {
        batch.Draw(texture, Vector2.Zero, Color.White);
    }
}