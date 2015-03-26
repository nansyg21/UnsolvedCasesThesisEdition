#region File Description
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UnsolvedCases;


public class PuzzlePiece
{
    #region Fields

    Texture2D pieceTexture;
    Texture2D decorativeCircleTex;
    Rectangle decorativeCircleRec;
    public Vector2 position;
    public Vector2 originalPosition;

    float rotation = 0.0f;
    public int width;
    public int height;
    public bool isCritical; //critical pieces are drawn with RED color or with some effects

    public bool IsSelected = false;

    #endregion

    public PuzzlePiece()  //needed for critical piece
    {
    }

    public PuzzlePiece(Texture2D theTexture, Vector2 thePosition, int theWidth, int theHeight)
    {
        position = thePosition;
        originalPosition = thePosition;
        pieceTexture = theTexture;
        width = theWidth;
        height = theHeight;
        isCritical = false;
        decorativeCircleTex = ScreenManager.MainGame.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\decorative-circle");
        
    }


    #region Draw

    public void Draw(SpriteBatch batch)
    {
        Color color = Color.White;
        float layerDepth = 1;

        if (IsSelected)
        {
            layerDepth = 0;
            color = Color.Yellow;// Color on click
        }

        if (!isCritical)
            batch.Draw(pieceTexture, position, null, color, rotation, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
        else
        {
            batch.Draw(pieceTexture, position, null, Color.Gold, rotation, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
            decorativeCircleRec = new Rectangle((int)position.X-width/3, (int)position.Y-height/5, 100, 100);   //draw circle
            batch.Draw(decorativeCircleTex, decorativeCircleRec, Color.White);
        }
    }

    #endregion

    #region Public Methods

    public Rectangle OriginalCollisionRectangle
    {
        get { return new Rectangle((int)originalPosition.X, (int)originalPosition.Y, width, height); }
    }

    public Rectangle CollisionRectangle
    {
        get { return new Rectangle((int)position.X, (int)position.Y, width, height); }
    }

    #endregion
}

