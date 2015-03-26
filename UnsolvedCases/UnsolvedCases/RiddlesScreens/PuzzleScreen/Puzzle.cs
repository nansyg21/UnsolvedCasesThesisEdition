#region File Description
#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace UnsolvedCases
{
    public class Puzzle : GameScreen
    {
        #region Fields
        bool fourRegions=true;//true: 4 shuffling regions, false: one shuffling regions
        Rectangle PuzzleCommonShuffledAreaRec;

        //Stencil Lists
        static List<StencilPiece> puzzleStencilList1 = new List<StencilPiece>(); //PUZZLE1(pinakas 1)
        static List<StencilPiece> puzzleStencilList2 = new List<StencilPiece>(); //PUZZLE2(pinakas 2)
        static List<StencilPiece> puzzleStencilList3 = new List<StencilPiece>(); //PUZZLE3(pinakas 3)
        static List<StencilPiece> puzzleStencilList4 = new List<StencilPiece>(); //PUZZLE4(pinakas 4)

        //Stencil Pieces
        StencilPiece Puzzle1Stencil1;
        StencilPiece Puzzle1Stencil2; StencilPiece Puzzle1Stencil2N;
        StencilPiece Puzzle1Stencil3;
        StencilPiece Puzzle1Stencil4;
        StencilPiece Puzzle1Stencil5; StencilPiece Puzzle1Stencil5N;
        StencilPiece Puzzle1Stencil6;
        StencilPiece Puzzle1Stencil7;
        StencilPiece Puzzle1Stencil8; StencilPiece Puzzle1Stencil8N;
        StencilPiece Puzzle1Stencil9;
        StencilPiece Puzzle1Stencil10;
        StencilPiece Puzzle1Stencil11; StencilPiece Puzzle1Stencil11N;
        StencilPiece Puzzle1Stencil12;
        //ta kommatia 2N, 5N, 8N, 11N prosthetoun mia 3h sthlh gia na xwraei h eikona

        StencilPiece Puzzle2Stencil1;
        StencilPiece Puzzle2Stencil2; StencilPiece Puzzle2Stencil2N;
        StencilPiece Puzzle2Stencil3;
        StencilPiece Puzzle2Stencil4;
        StencilPiece Puzzle2Stencil5; StencilPiece Puzzle2Stencil5N;
        StencilPiece Puzzle2Stencil6;
        StencilPiece Puzzle2Stencil7;
        StencilPiece Puzzle2Stencil8; StencilPiece Puzzle2Stencil8N;
        StencilPiece Puzzle2Stencil9;
        StencilPiece Puzzle2Stencil10;
        StencilPiece Puzzle2Stencil11; StencilPiece Puzzle2Stencil11N;
        StencilPiece Puzzle2Stencil12;
        //ta kommatia 2N, 5N, 8N, 11N prosthetoun mia 3h sthlh gia na xwraei h eikona

        StencilPiece Puzzle3Stencil1;
        StencilPiece Puzzle3Stencil2; StencilPiece Puzzle3Stencil2N;
        StencilPiece Puzzle3Stencil3;
        StencilPiece Puzzle3Stencil4;
        StencilPiece Puzzle3Stencil5; StencilPiece Puzzle3Stencil5N;
        StencilPiece Puzzle3Stencil6;
        StencilPiece Puzzle3Stencil7;
        StencilPiece Puzzle3Stencil8; StencilPiece Puzzle3Stencil8N;
        StencilPiece Puzzle3Stencil9;
        StencilPiece Puzzle3Stencil10;
        StencilPiece Puzzle3Stencil11; StencilPiece Puzzle3Stencil11N;
        StencilPiece Puzzle3Stencil12;
        //ta kommatia 2N, 5N, 8N, 11N prosthetoun mia 3h sthlh gia na xwraei h eikona
        
        StencilPiece Puzzle4Stencil1;
        StencilPiece Puzzle4Stencil2; StencilPiece Puzzle4Stencil2N;
        StencilPiece Puzzle4Stencil3;
        StencilPiece Puzzle4Stencil4;
        StencilPiece Puzzle4Stencil5; StencilPiece Puzzle4Stencil5N;
        StencilPiece Puzzle4Stencil6;
        StencilPiece Puzzle4Stencil7;
        StencilPiece Puzzle4Stencil8; StencilPiece Puzzle4Stencil8N;
        StencilPiece Puzzle4Stencil9;
        StencilPiece Puzzle4Stencil10;
        StencilPiece Puzzle4Stencil11; StencilPiece Puzzle4Stencil11N;
        StencilPiece Puzzle4Stencil12;
        //ta kommatia 2N, 5N, 8N, 11N prosthetoun mia 3h sthlh gia na xwraei h eikona

        static List<Vector2> renderPositions = new List<Vector2>();                 // Swzoume se vector thn thesh (apo thn 1h eikona) 
                                                                                    //pou tha zwgrafistei h 2h, 3h, 4h eikona 
        int i;                                                                      //pointer for renderPositions List

        static List<PuzzlePiece> puzzlePieces1 = new List<PuzzlePiece>();           //Pieces of a puzzle for Puzzle1
        static List<PuzzlePiece> puzzlePieces2 = new List<PuzzlePiece>();           //Pieces of a puzzle for Puzzle2
        static List<PuzzlePiece> puzzlePieces3 = new List<PuzzlePiece>();           //Pieces of a puzzle for Puzzle3
        static List<PuzzlePiece> puzzlePieces4 = new List<PuzzlePiece>();           //Pieces of a puzzle for Puzzle4

        Texture2D PuzzleImage1;                                                     //Image for Puzzle1
        Texture2D PuzzleImage2;                                                     //Image for Puzzle2
        Texture2D PuzzleImage3;                                                     //Image for Puzzle3
        Texture2D PuzzleImage4;                                                     //Image for Puzzle4
        Texture2D PuzzleFixedArea;                                                  //Image for proper places of pieces
        Texture2D PuzzleShuffledArea;                                               //Image for shuffled places of pieces
        Color[] fixedAreasColors = new Color[4];

        Rectangle Puzzle1ShuffledAreaRec;
        Rectangle Puzzle2ShuffledAreaRec;
        Rectangle Puzzle3ShuffledAreaRec;
        Rectangle Puzzle4ShuffledAreaRec;

        GraphicsDevice graphics;
        Game game;
        PuzzleGameScreen puzzlegamescreen;
     
        public PuzzlePiece selectedPiece;
        int width, height;
        //diastaseis gia ta background patterns
        int TotalWidthOfPuzzle1, TotalWidthOfPuzzle2, TotalWidthOfPuzzle3, TotalWidthOfPuzzle4;     
        int TotalHeightOfPuzzle1, TotalHeightOfPuzzle2, TotalHeightOfPuzzle3, TotalHeightOfPuzzle4;
        bool SolvedP1 , SolvedP2 , SolvedP3 , SolvedP4 ;      //flags wether the player solved the current puzzles or not
        int criticalPuzzle;
        PuzzlePiece criticalPiece;
        
        #endregion

        public Puzzle(PuzzleGameScreen _puzzlegamescreen)
        {
            game = ScreenManager.MainGame;
            graphics = game.GraphicsDevice;

            puzzlegamescreen = _puzzlegamescreen;

            //Images for a puzzle Dimensions: Image1, Image2, Image3, Image4: 768x768
            PuzzleImage1 = game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\pinakes1");
            PuzzleImage2 = game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\pinakes2");
            PuzzleImage3 = game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\pinakes3");
            PuzzleImage4 = game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\pinakes4");
            PuzzleFixedArea = game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\puzzle_fix_area");
            PuzzleShuffledArea = game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\puzzle_shuffle_area");

            //All colors are white except from critical puzzle
            fixedAreasColors[0] = Color.White;
            fixedAreasColors[1] = Color.White;
            fixedAreasColors[2] = Color.White;
            fixedAreasColors[3] = Color.White;

        }

        #region Public Methods

        public void PuzzlelizeImage() //Convert the image to a puzzle
        {
            width =  graphics.Viewport.Width;
            height = graphics.Viewport.Height;

            //Load the stencil pieces if they haven't been loaded before
            /**
             *  1   2   2N   3
             *  4   5   5N   6
             *  7   8   8N   9
             *  10  11  11N  12
             *  
             * 2N:  puzzle02
             * 5N:  puzzle05
             * 8N:  puzzle05    idio me to puzzle05 gia thn kalyterh topothethsh twn kommatiwn sthn othonh
             * 11N: puzzle11
             *
             * 
             * puzzle1    puzzle2            puzzle3               puzzle4
             *  (0,0)     (width / 4f, 0)    (2 * width / 4f, 0)   (3 * width / 4f, 0)
         
             * * 
             * */
            if (puzzleStencilList1.Count == 0)
            {
                Console.WriteLine("Loading  stencil 1");

                //Metakinontas to 1o kommati metakineitai oloklhro to puzzle
                Puzzle1Stencil1 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle01"), 
                    new Vector2(0, 0), width / 20, height / 9);
                Puzzle1Stencil2 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle02"), 
                    new Vector2(Puzzle1Stencil1.position.X + Puzzle1Stencil1.width * 0.8f, Puzzle1Stencil1.position.Y), width / 20, 
                    height / 9);
                Puzzle1Stencil2N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle02"), 
                    new Vector2(Puzzle1Stencil2.position.X + Puzzle1Stencil2.width * 0.8f, Puzzle1Stencil2.position.Y), width / 20, 
                    height / 9);
                Puzzle1Stencil3 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle03"), 
                    new Vector2(Puzzle1Stencil2N.position.X + Puzzle1Stencil2N.width * 0.8f, Puzzle1Stencil2N.position.Y), width / 20, 
                    height / 9);

                Puzzle1Stencil4 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle04"), 
                    new Vector2(Puzzle1Stencil1.position.X, Puzzle1Stencil1.position.Y + Puzzle1Stencil1.height * 0.8f), width / 20, 
                    height / 11);
                Puzzle1Stencil5 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle1Stencil4.position.X + Puzzle1Stencil4.width * 0.8f, Puzzle1Stencil2.position.Y + 
                        Puzzle1Stencil2.height * 0.8f), width / 20, height / 9);
                Puzzle1Stencil5N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle1Stencil5.position.X + Puzzle1Stencil5.width * 0.8f, Puzzle1Stencil2.position.Y + 
                        Puzzle1Stencil2N.height * 0.8f), width / 20, height / 9);
                Puzzle1Stencil6 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle06"), 
                    new Vector2(Puzzle1Stencil5N.position.X + Puzzle1Stencil5N.width * 0.8f, Puzzle1Stencil3.position.Y + 
                        Puzzle1Stencil3.height * 0.8f), width / 20, height / 11);

                //h eikones 7,8,9 einai mikroteres ara bazoume mikrotero pollaplasiasth
                Puzzle1Stencil7 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle07"), 
                    new Vector2(Puzzle1Stencil4.position.X, Puzzle1Stencil4.position.Y + Puzzle1Stencil4.height * 0.75f), width / 20, 
                    height / 9);
                Puzzle1Stencil8 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle08"), 
                    new Vector2(Puzzle1Stencil7.position.X + Puzzle1Stencil7.width * 0.62f, Puzzle1Stencil5.position.Y + 
                        Puzzle1Stencil5.height * 0.79f), width / 22, height / 9);
                Puzzle1Stencil8N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle1Stencil8.position.X + Puzzle1Stencil5N.width, Puzzle1Stencil5N.position.Y + 
                        Puzzle1Stencil5N.height * 0.79f), width / 22, height / 9);//eikona 5 apo puzzle
                Puzzle1Stencil9 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle09"), 
                    new Vector2(Puzzle1Stencil8N.position.X + Puzzle1Stencil8N.width * 0.88f, Puzzle1Stencil6.position.Y + 
                        Puzzle1Stencil6.height * 0.75f), width / 20, height / 9);

                Puzzle1Stencil10 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle10"), 
                    new Vector2(Puzzle1Stencil7.position.X, Puzzle1Stencil7.position.Y + Puzzle1Stencil7.height * 0.8f), 
                    width / 20, height / 9);
                Puzzle1Stencil11 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle11"), 
                    new Vector2(Puzzle1Stencil10.position.X + Puzzle1Stencil10.width * 0.63f, Puzzle1Stencil8.position.Y + 
                        Puzzle1Stencil8.height * 0.8f), width / 20, height / 9);
                Puzzle1Stencil11N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle11"), 
                    new Vector2(Puzzle1Stencil11.position.X + Puzzle1Stencil11.width * 0.8f, Puzzle1Stencil8N.position.Y + 
                        Puzzle1Stencil8N.height * 0.8f), width / 20, height / 9);
                Puzzle1Stencil12 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle12"), 
                    new Vector2(Puzzle1Stencil11N.position.X + Puzzle1Stencil11N.width * 0.82f, Puzzle1Stencil9.position.Y + 
                        Puzzle1Stencil9.height), width / 20, height / 11);

                puzzleStencilList1.Add(Puzzle1Stencil1);
                puzzleStencilList1.Add(Puzzle1Stencil2); puzzleStencilList1.Add(Puzzle1Stencil2N);
                puzzleStencilList1.Add(Puzzle1Stencil3);
                puzzleStencilList1.Add(Puzzle1Stencil4);
                puzzleStencilList1.Add(Puzzle1Stencil5); puzzleStencilList1.Add(Puzzle1Stencil5N);
                puzzleStencilList1.Add(Puzzle1Stencil6);
                puzzleStencilList1.Add(Puzzle1Stencil7);
                puzzleStencilList1.Add(Puzzle1Stencil8); puzzleStencilList1.Add(Puzzle1Stencil8N);
                puzzleStencilList1.Add(Puzzle1Stencil9);
                puzzleStencilList1.Add(Puzzle1Stencil10);
                puzzleStencilList1.Add(Puzzle1Stencil11); puzzleStencilList1.Add(Puzzle1Stencil11N);
                puzzleStencilList1.Add(Puzzle1Stencil12);
            }

            if (puzzleStencilList2.Count == 0)
            {
                Console.WriteLine("Loading  stencil 2");

                //Metakinontas to 1o kommati metakineitai oloklhro to puzzle
                Puzzle2Stencil1 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle01"), 
                    new Vector2(width / 4f, 0), width / 20, height / 9);
                Puzzle2Stencil2 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle02"), 
                    new Vector2(Puzzle2Stencil1.position.X + Puzzle2Stencil1.width * 0.8f, Puzzle2Stencil1.position.Y), width / 20, 
                    height / 9);
                Puzzle2Stencil2N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle02"), 
                    new Vector2(Puzzle2Stencil2.position.X + Puzzle2Stencil2.width * 0.8f, Puzzle2Stencil2.position.Y), width / 20, 
                    height / 9);
                Puzzle2Stencil3 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle03"), 
                    new Vector2(Puzzle2Stencil2N.position.X + Puzzle2Stencil2N.width * 0.8f, Puzzle2Stencil2N.position.Y), width / 20, 
                    height / 9);

                Puzzle2Stencil4 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle04"), 
                    new Vector2(Puzzle2Stencil1.position.X, Puzzle2Stencil1.position.Y + Puzzle2Stencil1.height * 0.8f), width / 20, 
                    height / 11);
                Puzzle2Stencil5 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle2Stencil4.position.X + Puzzle2Stencil4.width * 0.8f, Puzzle2Stencil2.position.Y + 
                        Puzzle2Stencil2.height * 0.8f), width / 20, height / 9);
                Puzzle2Stencil5N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle2Stencil5.position.X + Puzzle2Stencil5.width * 0.8f, Puzzle2Stencil2.position.Y + 
                        Puzzle2Stencil2N.height * 0.8f), width / 20, height / 9);
                Puzzle2Stencil6 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle06"), 
                    new Vector2(Puzzle2Stencil5N.position.X + Puzzle2Stencil5N.width * 0.8f, Puzzle2Stencil3.position.Y + 
                        Puzzle2Stencil3.height * 0.8f), width / 20, height / 11);
                //h eikones 7,8,9 einai mikroteres ara bazoume mikrotero pollaplasiasth

                Puzzle2Stencil7 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle07"), 
                    new Vector2(Puzzle2Stencil4.position.X, Puzzle2Stencil4.position.Y + Puzzle2Stencil4.height * 0.75f), width / 20, 
                    height / 9);
                Puzzle2Stencil8 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle08"), 
                    new Vector2(Puzzle2Stencil7.position.X + Puzzle2Stencil7.width * 0.62f, Puzzle2Stencil5.position.Y + 
                        Puzzle2Stencil5.height * 0.79f), width / 22, height / 9);
                Puzzle2Stencil8N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle2Stencil8.position.X + Puzzle2Stencil5N.width, Puzzle2Stencil5N.position.Y + 
                        Puzzle2Stencil5N.height * 0.79f), width / 22, height / 9);                          //eikona 5 apo puzzle
                Puzzle2Stencil9 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle09"), 
                    new Vector2(Puzzle2Stencil8N.position.X + Puzzle2Stencil8N.width * 0.88f, Puzzle2Stencil6.position.Y + 
                        Puzzle2Stencil6.height * 0.75f), width / 20, height / 9);

                Puzzle2Stencil10 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle10"), 
                    new Vector2(Puzzle2Stencil7.position.X, Puzzle2Stencil7.position.Y + Puzzle2Stencil7.height * 0.8f), width / 20, 
                    height / 9);
                Puzzle2Stencil11 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle11"), 
                    new Vector2(Puzzle2Stencil10.position.X + Puzzle2Stencil10.width * 0.63f, Puzzle2Stencil8.position.Y + 
                        Puzzle2Stencil8.height * 0.8f), width / 20, height / 9);
                Puzzle2Stencil11N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle11"), 
                    new Vector2(Puzzle2Stencil11.position.X + Puzzle2Stencil11.width * 0.8f, Puzzle2Stencil8N.position.Y + 
                        Puzzle2Stencil8N.height * 0.8f), width / 20, height / 9);
                Puzzle2Stencil12 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle12"), 
                    new Vector2(Puzzle2Stencil11N.position.X + Puzzle2Stencil11N.width * 0.82f, Puzzle2Stencil9.position.Y + 
                        Puzzle2Stencil9.height), width / 20, height / 11);

                puzzleStencilList2.Add(Puzzle2Stencil1);
                puzzleStencilList2.Add(Puzzle2Stencil2); puzzleStencilList2.Add(Puzzle2Stencil2N);
                puzzleStencilList2.Add(Puzzle2Stencil3);
                puzzleStencilList2.Add(Puzzle2Stencil4);
                puzzleStencilList2.Add(Puzzle2Stencil5); puzzleStencilList2.Add(Puzzle2Stencil5N);
                puzzleStencilList2.Add(Puzzle2Stencil6);
                puzzleStencilList2.Add(Puzzle2Stencil7);
                puzzleStencilList2.Add(Puzzle2Stencil8); puzzleStencilList2.Add(Puzzle2Stencil8N);
                puzzleStencilList2.Add(Puzzle2Stencil9);
                puzzleStencilList2.Add(Puzzle2Stencil10);
                puzzleStencilList2.Add(Puzzle2Stencil11); puzzleStencilList2.Add(Puzzle2Stencil11N);
                puzzleStencilList2.Add(Puzzle2Stencil12);
            }

            if (puzzleStencilList3.Count == 0)
            {
                Console.WriteLine("Loading  stencil 3");

                //Metakinontas to 1o kommati metakineitai oloklhro to puzzle
                Puzzle3Stencil1 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle01"), 
                    new Vector2(2 * width / 4f, 0), width / 20, height / 9);
                Puzzle3Stencil2 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle02"), 
                    new Vector2(Puzzle3Stencil1.position.X + Puzzle3Stencil1.width * 0.8f, Puzzle3Stencil1.position.Y), width / 20, 
                    height / 9);
                Puzzle3Stencil2N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle02"), 
                    new Vector2(Puzzle3Stencil2.position.X + Puzzle3Stencil2.width * 0.8f, Puzzle3Stencil2.position.Y), width / 20, 
                    height / 9);
                Puzzle3Stencil3 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle03"), 
                    new Vector2(Puzzle3Stencil2N.position.X + Puzzle3Stencil2N.width * 0.8f, Puzzle3Stencil2N.position.Y), width / 20, 
                    height / 9);

                Puzzle3Stencil4 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle04"), 
                    new Vector2(Puzzle3Stencil1.position.X, Puzzle3Stencil1.position.Y + Puzzle3Stencil1.height * 0.8f), width / 20, 
                    height / 11);
                Puzzle3Stencil5 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle3Stencil4.position.X + Puzzle3Stencil4.width * 0.8f, Puzzle3Stencil2.position.Y + 
                        Puzzle3Stencil2.height * 0.8f), width / 20, height / 9);
                Puzzle3Stencil5N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle3Stencil5.position.X + Puzzle3Stencil5.width * 0.8f, Puzzle3Stencil2.position.Y + 
                        Puzzle3Stencil2N.height * 0.8f), width / 20, height / 9);
                Puzzle3Stencil6 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle06"), 
                    new Vector2(Puzzle3Stencil5N.position.X + Puzzle3Stencil5N.width * 0.8f, Puzzle3Stencil3.position.Y + 
                        Puzzle3Stencil3.height * 0.8f), width / 20, height / 11);
                //h eikones 7,8,9 einai mikroteres ara bazoume mikrotero pollaplasiasth

                Puzzle3Stencil7 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle07"), 
                    new Vector2(Puzzle3Stencil4.position.X, Puzzle3Stencil4.position.Y + Puzzle3Stencil4.height * 0.75f), width / 20, 
                    height / 9);
                Puzzle3Stencil8 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle08"), 
                    new Vector2(Puzzle3Stencil7.position.X + Puzzle3Stencil7.width * 0.62f, Puzzle3Stencil5.position.Y + 
                        Puzzle3Stencil5.height * 0.79f), width / 22, height / 9);
                Puzzle3Stencil8N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle3Stencil8.position.X + Puzzle3Stencil5N.width, Puzzle3Stencil5N.position.Y + 
                        Puzzle3Stencil5N.height * 0.79f), width / 22, height / 9);//eikona 5 apo puzzle
                Puzzle3Stencil9 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle09"), 
                    new Vector2(Puzzle3Stencil8N.position.X + Puzzle3Stencil8N.width * 0.88f, Puzzle3Stencil6.position.Y + 
                        Puzzle3Stencil6.height * 0.75f), width / 20, height / 9);

                Puzzle3Stencil10 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle10"), 
                    new Vector2(Puzzle3Stencil7.position.X, Puzzle3Stencil7.position.Y + Puzzle3Stencil7.height * 0.8f), width / 20, 
                    height / 9);
                Puzzle3Stencil11 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle11"), 
                    new Vector2(Puzzle3Stencil10.position.X + Puzzle3Stencil10.width * 0.63f, Puzzle3Stencil8.position.Y + 
                        Puzzle3Stencil8.height * 0.8f), width / 20, height / 9);
                Puzzle3Stencil11N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle11"), 
                    new Vector2(Puzzle3Stencil11.position.X + Puzzle3Stencil11.width * 0.8f, Puzzle3Stencil8N.position.Y + 
                        Puzzle3Stencil8N.height * 0.8f), width / 20, height / 9);
                Puzzle3Stencil12 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle12"), 
                    new Vector2(Puzzle3Stencil11N.position.X + Puzzle3Stencil11N.width * 0.82f, Puzzle3Stencil9.position.Y + 
                        Puzzle3Stencil9.height), width / 20, height / 11);

                puzzleStencilList3.Add(Puzzle3Stencil1);
                puzzleStencilList3.Add(Puzzle3Stencil2); puzzleStencilList3.Add(Puzzle3Stencil2N);
                puzzleStencilList3.Add(Puzzle3Stencil3);
                puzzleStencilList3.Add(Puzzle3Stencil4);
                puzzleStencilList3.Add(Puzzle3Stencil5); puzzleStencilList3.Add(Puzzle3Stencil5N);
                puzzleStencilList3.Add(Puzzle3Stencil6);
                puzzleStencilList3.Add(Puzzle3Stencil7);
                puzzleStencilList3.Add(Puzzle3Stencil8); puzzleStencilList3.Add(Puzzle3Stencil8N);
                puzzleStencilList3.Add(Puzzle3Stencil9);
                puzzleStencilList3.Add(Puzzle3Stencil10);
                puzzleStencilList3.Add(Puzzle3Stencil11); puzzleStencilList3.Add(Puzzle3Stencil11N);
                puzzleStencilList3.Add(Puzzle3Stencil12);
            }

            if (puzzleStencilList4.Count == 0)
            {
                Console.WriteLine("Loading  stencil 3");

                //Metakinontas to 1o kommati metakineitai oloklhro to puzzle
                Puzzle4Stencil1 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle01"), 
                    new Vector2(3 * width / 4f, 0), width / 20, height / 9);
                Puzzle4Stencil2 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle02"), 
                    new Vector2(Puzzle4Stencil1.position.X + Puzzle4Stencil1.width * 0.8f, Puzzle4Stencil1.position.Y), width / 20, 
                    height / 9);
                Puzzle4Stencil2N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle02"), 
                    new Vector2(Puzzle4Stencil2.position.X + Puzzle4Stencil2.width * 0.8f, Puzzle4Stencil2.position.Y), width / 20, 
                    height / 9);
                Puzzle4Stencil3 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle03"), 
                    new Vector2(Puzzle4Stencil2N.position.X + Puzzle4Stencil2N.width * 0.8f, Puzzle4Stencil2N.position.Y), width / 20, 
                    height / 9);

                Puzzle4Stencil4 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle04"), 
                    new Vector2(Puzzle4Stencil1.position.X, Puzzle4Stencil1.position.Y + Puzzle4Stencil1.height * 0.8f), width / 20, 
                    height / 11);
                Puzzle4Stencil5 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle4Stencil4.position.X + Puzzle4Stencil4.width * 0.8f, Puzzle4Stencil2.position.Y + 
                        Puzzle4Stencil2.height * 0.8f), width / 20, height / 9);
                Puzzle4Stencil5N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle4Stencil5.position.X + Puzzle4Stencil5.width * 0.8f, Puzzle4Stencil2.position.Y + 
                        Puzzle4Stencil2N.height * 0.8f), width / 20, height / 9);
                Puzzle4Stencil6 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle06"), 
                    new Vector2(Puzzle4Stencil5N.position.X + Puzzle4Stencil5N.width * 0.8f, Puzzle4Stencil3.position.Y + 
                        Puzzle4Stencil3.height * 0.8f), width / 20, height / 11);
                //h eikones 7,8,9 einai mikroteres ara bazoume mikrotero pollaplasiasth

                Puzzle4Stencil7 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle07"), 
                    new Vector2(Puzzle4Stencil4.position.X, Puzzle4Stencil4.position.Y + Puzzle4Stencil4.height * 0.75f), width / 20, 
                    height / 9);
                Puzzle4Stencil8 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle08"), 
                    new Vector2(Puzzle4Stencil7.position.X + Puzzle4Stencil7.width * 0.62f, Puzzle4Stencil5.position.Y + 
                        Puzzle4Stencil5.height * 0.79f), width / 22, height / 9);
                Puzzle4Stencil8N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle05"), 
                    new Vector2(Puzzle4Stencil8.position.X + Puzzle4Stencil5N.width, Puzzle4Stencil5N.position.Y + 
                        Puzzle4Stencil5N.height * 0.79f), width / 22, height / 9);                      //eikona 5 apo puzzle
                Puzzle4Stencil9 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle09"), 
                    new Vector2(Puzzle4Stencil8N.position.X + Puzzle4Stencil8N.width * 0.88f, Puzzle4Stencil6.position.Y + 
                        Puzzle4Stencil6.height * 0.75f), width / 20, height / 9);

                Puzzle4Stencil10 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle10"), 
                    new Vector2(Puzzle4Stencil7.position.X, Puzzle4Stencil7.position.Y + Puzzle4Stencil7.height * 0.8f), width / 20, 
                    height / 9);
                Puzzle4Stencil11 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle11"), 
                    new Vector2(Puzzle4Stencil10.position.X + Puzzle4Stencil10.width * 0.63f, Puzzle4Stencil8.position.Y + 
                        Puzzle3Stencil8.height * 0.8f), width / 20, height / 9);
                Puzzle4Stencil11N = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle11"), 
                    new Vector2(Puzzle4Stencil11.position.X + Puzzle4Stencil11.width * 0.8f, Puzzle4Stencil8N.position.Y + 
                        Puzzle4Stencil8N.height * 0.8f), width / 20, height / 9);
                Puzzle4Stencil12 = new StencilPiece(game.Content.Load<Texture2D>(@"Textures\Riddles\Puzzle\Puzzle1\Puzzle12"), 
                    new Vector2(Puzzle4Stencil11N.position.X + Puzzle4Stencil11N.width * 0.82f, Puzzle4Stencil9.position.Y + 
                        Puzzle4Stencil9.height), width / 20, height / 11);

                puzzleStencilList4.Add(Puzzle4Stencil1);
                puzzleStencilList4.Add(Puzzle4Stencil2); puzzleStencilList4.Add(Puzzle4Stencil2N);
                puzzleStencilList4.Add(Puzzle4Stencil3);
                puzzleStencilList4.Add(Puzzle4Stencil4);
                puzzleStencilList4.Add(Puzzle4Stencil5); puzzleStencilList4.Add(Puzzle4Stencil5N);
                puzzleStencilList4.Add(Puzzle4Stencil6);
                puzzleStencilList4.Add(Puzzle4Stencil7);
                puzzleStencilList4.Add(Puzzle4Stencil8); puzzleStencilList4.Add(Puzzle4Stencil8N);
                puzzleStencilList4.Add(Puzzle4Stencil9);
                puzzleStencilList4.Add(Puzzle4Stencil10);
                puzzleStencilList4.Add(Puzzle4Stencil11); puzzleStencilList4.Add(Puzzle4Stencil11N);
                puzzleStencilList4.Add(Puzzle4Stencil12);
            }

            puzzlePieces1.Clear();
            puzzlePieces2.Clear();
            puzzlePieces3.Clear();
            puzzlePieces4.Clear();
 
            //Diastaseis ths perimetrikhs eikonas gia to puzzle1
            
            TotalWidthOfPuzzle1 = (int)(Puzzle1Stencil12.position.X + Puzzle1Stencil12.width - Puzzle1Stencil1.position.X);
            TotalHeightOfPuzzle1 = (int)(Puzzle1Stencil12.position.Y + Puzzle1Stencil12.height - Puzzle1Stencil1.position.Y);

            //Diastaseis ths perimetrikhs eikonas gia to puzzle2
            TotalWidthOfPuzzle2 = (int)(Puzzle2Stencil12.position.X + Puzzle2Stencil12.width - Puzzle2Stencil1.position.X);
            TotalHeightOfPuzzle2 = (int)(Puzzle1Stencil12.position.Y + Puzzle1Stencil12.height - Puzzle1Stencil1.position.Y);

            //Diastaseis ths perimetrikhs eikonas gia to puzzle3
            TotalWidthOfPuzzle3 = (int)(Puzzle3Stencil12.position.X + Puzzle3Stencil12.width - Puzzle3Stencil1.position.X);
            TotalHeightOfPuzzle3 = (int)(Puzzle3Stencil12.position.Y + Puzzle3Stencil12.height - Puzzle3Stencil1.position.Y);

            //Diastaseis ths perimetrikhs eikonas gia to puzzle4
            TotalWidthOfPuzzle4 = (int)(Puzzle4Stencil12.position.X + Puzzle4Stencil12.width - Puzzle4Stencil1.position.X);
            TotalHeightOfPuzzle4 = (int)(Puzzle4Stencil12.position.Y + Puzzle4Stencil12.height - Puzzle4Stencil1.position.Y);

            GC.Collect();

            SpriteBatch batch = new SpriteBatch(graphics); 

            foreach (StencilPiece stencilPiece in puzzleStencilList1)
            {

                Console.WriteLine("Rendering  stencil 1");
                RenderTarget2D renderTarget = new RenderTarget2D(graphics, width / 3, height / 3, false, SurfaceFormat.Color, 
                    DepthFormat.Depth24Stencil8);

                graphics.SetRenderTarget(renderTarget);
                graphics.Clear(Color.Transparent);

                batch.Begin(SpriteSortMode.Immediate, null, null, StateObjects.StencilMaskBefore, null, 
                    StateObjects.AlphaEffect(graphics));
                batch.Draw(stencilPiece.texture, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.End();

                //SAVE Which part of the original image we are going to draw
                //Using this, the puzzle beggins each time from the upper left corner of the image
                renderPositions.Add(-stencilPiece.position * 2.7f);

                batch.Begin(SpriteSortMode.Immediate, null, null, StateObjects.StencilMaskAfter, null, 
                    StateObjects.AlphaEffect(graphics));
                batch.Draw(PuzzleImage1, -stencilPiece.position * 2.7f, null, Color.White, 0f, Vector2.Zero, 01f, 
                    SpriteEffects.None, 0.0f);
                batch.End();

                graphics.SetRenderTarget(null);

                puzzlePieces1.Add(new PuzzlePiece(renderTarget, stencilPiece.position, stencilPiece.width, stencilPiece.height));
            }

            i = 0;

            foreach (StencilPiece stencilPiece in puzzleStencilList2)
            {

                Console.WriteLine("Rendering  stencil 2");
                RenderTarget2D renderTarget = new RenderTarget2D(graphics, width / 3, height / 3, false, SurfaceFormat.Color, 
                    DepthFormat.Depth24Stencil8);

                graphics.SetRenderTarget(renderTarget);
                graphics.Clear(Color.Transparent);

                batch.Begin(SpriteSortMode.Immediate, null, null, StateObjects.StencilMaskBefore, null, 
                    StateObjects.AlphaEffect(graphics));
                batch.Draw(stencilPiece.texture, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.End();

                batch.Begin(SpriteSortMode.Immediate, null, null, StateObjects.StencilMaskAfter, null, 
                    StateObjects.AlphaEffect(graphics));
                batch.Draw(PuzzleImage2, renderPositions[i], null, Color.White, 0f, Vector2.Zero, 01f, SpriteEffects.None, 0.0f);
                batch.End();

                graphics.SetRenderTarget(null);

                puzzlePieces2.Add(new PuzzlePiece(renderTarget, stencilPiece.position, stencilPiece.width, stencilPiece.height));
                i++;
            }

            i = 0;

            foreach (StencilPiece stencilPiece in puzzleStencilList3)
            {
                Console.WriteLine("Rendering  stencil 3");
                RenderTarget2D renderTarget = new RenderTarget2D(graphics, width / 3, height / 3, false, SurfaceFormat.Color, 
                    DepthFormat.Depth24Stencil8);

                graphics.SetRenderTarget(renderTarget);
                graphics.Clear(Color.Transparent);

                batch.Begin(SpriteSortMode.Immediate, null, null, StateObjects.StencilMaskBefore, null, 
                    StateObjects.AlphaEffect(graphics));
                batch.Draw(stencilPiece.texture, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.End();

                //SAVE Which part of the original image we are going to draw
                //Using this, the puzzle beggins each time from the upper left corner of the image
                renderPositions.Add(Vector2.Zero - stencilPiece.position);

                batch.Begin(SpriteSortMode.Immediate, null, null, StateObjects.StencilMaskAfter, null, 
                    StateObjects.AlphaEffect(graphics));
                batch.Draw(PuzzleImage3, renderPositions[i], null, Color.White, 0f, Vector2.Zero, 01f, SpriteEffects.None, 0.0f);
                batch.End();

                graphics.SetRenderTarget(null);

                puzzlePieces3.Add(new PuzzlePiece(renderTarget, stencilPiece.position, stencilPiece.width, stencilPiece.height));
                i++;
            }

            i = 0;

            foreach (StencilPiece stencilPiece in puzzleStencilList4)
            {

                Console.WriteLine("Rendering  stencil 4");
                RenderTarget2D renderTarget = new RenderTarget2D(graphics, width / 3, height / 3, false, SurfaceFormat.Color, 
                    DepthFormat.Depth24Stencil8);

                graphics.SetRenderTarget(renderTarget);
                graphics.Clear(Color.Transparent);

                batch.Begin(SpriteSortMode.Immediate, null, null, StateObjects.StencilMaskBefore, null, 
                    StateObjects.AlphaEffect(graphics));
                batch.Draw(stencilPiece.texture, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.End();

                //SAVE Which part of the original image we are going to draw
                //Using this, the puzzle beggins each time from the upper left corner of the image
                renderPositions.Add(Vector2.Zero - stencilPiece.position);

                batch.Begin(SpriteSortMode.Immediate, null, null, StateObjects.StencilMaskAfter, null, 
                    StateObjects.AlphaEffect(graphics));
                batch.Draw(PuzzleImage4, renderPositions[i], null, Color.White, 0f, Vector2.Zero, 01f, SpriteEffects.None, 0.0f);
                batch.End();

                graphics.SetRenderTarget(null);

                puzzlePieces4.Add(new PuzzlePiece(renderTarget, stencilPiece.position, stencilPiece.width, stencilPiece.height));
                i++;
            }
        }       

        public void ShufflePuzzle()//Shuffle puzzle on areas or in a single area
        {
            Random random = new Random();

            #region 4ShuffleAreas
            if (fourRegions)
            {
                //Shuffle area 1: akribws katw apo to puzzle 1
                Puzzle1ShuffledAreaRec = new Rectangle((int)puzzleStencilList1[12].position.X,
                    (int)puzzleStencilList1[12].position.Y + puzzleStencilList1[12].height + 50,
                    (int)(puzzleStencilList1[15].position.X + puzzleStencilList1[15].width - puzzleStencilList1[12].position.X), 400);

                //Shuffle area 2: akribws katw apo to puzzle 2
                Puzzle2ShuffledAreaRec = new Rectangle((int)puzzleStencilList2[12].position.X,
                    (int)puzzleStencilList2[12].position.Y + puzzleStencilList2[12].height + 50,
                    (int)(puzzleStencilList2[15].position.X + puzzleStencilList2[15].width - puzzleStencilList2[12].position.X), 400);

                //Shuffle area 3: akribws katw apo to puzzle 3
                Puzzle3ShuffledAreaRec = new Rectangle((int)puzzleStencilList3[12].position.X,
                    (int)puzzleStencilList3[12].position.Y + puzzleStencilList3[12].height + 50,
                    (int)(puzzleStencilList3[15].position.X + puzzleStencilList3[15].width - puzzleStencilList3[12].position.X), 400);

                //Shuffle area 4: akribws katw apo to puzzle 4
                Puzzle4ShuffledAreaRec = new Rectangle((int)puzzleStencilList4[12].position.X,
                    (int)puzzleStencilList4[12].position.Y + puzzleStencilList4[12].height + 50,
                    (int)(puzzleStencilList4[15].position.X + puzzleStencilList4[15].width - puzzleStencilList4[12].position.X), 400);

                foreach (PuzzlePiece piece in puzzlePieces1)                        //Scrumble Puzzle1 according to the shuffle area 1
                {
                    //x,y: +10 gia na mhn akoumpane ta kommatia epanw stis grammes
                    piece.position = new Vector2(random.Next(Puzzle1ShuffledAreaRec.X + 10, Puzzle1ShuffledAreaRec.X +
                        Puzzle1ShuffledAreaRec.Width - piece.width - 10), random.Next(Puzzle1ShuffledAreaRec.Y + 10,
                        Puzzle1ShuffledAreaRec.Y + Puzzle1ShuffledAreaRec.Height - piece.height - 10));
                }

                foreach (PuzzlePiece piece in puzzlePieces2)                        //Scrumble Puzzle2  according to the shuffle area 2
                {
                    //width, height: -10 gia na mhn proeksexoun apo thn ShuffleArea
                    piece.position = new Vector2(random.Next(Puzzle2ShuffledAreaRec.X + 10, Puzzle2ShuffledAreaRec.X +
                        Puzzle2ShuffledAreaRec.Width - piece.width - 10), random.Next(Puzzle2ShuffledAreaRec.Y + 10,
                        Puzzle2ShuffledAreaRec.Y + Puzzle2ShuffledAreaRec.Height - piece.height - 10));
                }

                foreach (PuzzlePiece piece in puzzlePieces3)                        //Scrumble Puzzle3  according to the shuffle area 3
                {
                    piece.position = new Vector2(random.Next(Puzzle3ShuffledAreaRec.X + 10, Puzzle3ShuffledAreaRec.X +
                        Puzzle3ShuffledAreaRec.Width - piece.width - 10), random.Next(Puzzle3ShuffledAreaRec.Y + 10,
                        Puzzle3ShuffledAreaRec.Y + Puzzle3ShuffledAreaRec.Height - piece.height - 10));
                }

                foreach (PuzzlePiece piece in puzzlePieces4)                        //Scrumble Puzzle4  according to the shuffle area 4
                {
                    piece.position = new Vector2(random.Next(Puzzle4ShuffledAreaRec.X + 10, Puzzle4ShuffledAreaRec.X +
                        Puzzle4ShuffledAreaRec.Width - piece.width - 10), random.Next(Puzzle4ShuffledAreaRec.Y + 10,
                        Puzzle4ShuffledAreaRec.Y + Puzzle4ShuffledAreaRec.Height - piece.height - 10));
                }
            }
            #endregion

            #region 1ShuffleArea
            if (!fourRegions)
            {
                //Common Shuffle area : from puzzle2 to puzle 3
                 PuzzleCommonShuffledAreaRec = new Rectangle((int)puzzleStencilList2[12].position.X,
                    (int)puzzleStencilList2[12].position.Y + puzzleStencilList2[12].height + 50,
                    (int)(puzzleStencilList3[15].position.X + puzzleStencilList3[15].width - puzzleStencilList2[12].position.X), 400);


                foreach (PuzzlePiece piece in puzzlePieces1)                        //Scrumble Puzzle1 according to the shuffle area 1
                {
                    //x,y: +10 gia na mhn akoumpane ta kommatia epanw stis grammes
                    piece.position = new Vector2(random.Next(PuzzleCommonShuffledAreaRec.X + 10, PuzzleCommonShuffledAreaRec.X +
                        PuzzleCommonShuffledAreaRec.Width - piece.width - 10), random.Next(PuzzleCommonShuffledAreaRec.Y + 10,
                        PuzzleCommonShuffledAreaRec.Y + PuzzleCommonShuffledAreaRec.Height - piece.height - 10));
                }

                foreach (PuzzlePiece piece in puzzlePieces2)                        //Scrumble Puzzle2  according to the shuffle area 2
                {
                    //width, height: -10 gia na mhn proeksexoun apo thn ShuffleArea
                    piece.position = new Vector2(random.Next(PuzzleCommonShuffledAreaRec.X + 10, PuzzleCommonShuffledAreaRec.X +
                        PuzzleCommonShuffledAreaRec.Width - piece.width - 10), random.Next(PuzzleCommonShuffledAreaRec.Y + 10,
                        PuzzleCommonShuffledAreaRec.Y + PuzzleCommonShuffledAreaRec.Height - piece.height - 10));
                }

                foreach (PuzzlePiece piece in puzzlePieces3)                        //Scrumble Puzzle3  according to the shuffle area 3
                {
                    piece.position = new Vector2(random.Next(PuzzleCommonShuffledAreaRec.X + 10, PuzzleCommonShuffledAreaRec.X +
                        PuzzleCommonShuffledAreaRec.Width - piece.width - 10), random.Next(PuzzleCommonShuffledAreaRec.Y + 10,
                        PuzzleCommonShuffledAreaRec.Y + PuzzleCommonShuffledAreaRec.Height - piece.height - 10));
                }

                foreach (PuzzlePiece piece in puzzlePieces4)                        //Scrumble Puzzle4  according to the shuffle area 4
                {
                    piece.position = new Vector2(random.Next(PuzzleCommonShuffledAreaRec.X + 10, PuzzleCommonShuffledAreaRec.X +
                        PuzzleCommonShuffledAreaRec.Width - piece.width - 10), random.Next(PuzzleCommonShuffledAreaRec.Y + 10,
                        PuzzleCommonShuffledAreaRec.Y + PuzzleCommonShuffledAreaRec.Height - piece.height - 10));
                }
            }

            #endregion

        }

        public void CriticalPiece()  //Changes a puzzle piece to critical
        {
            AudioManager.PlayCue("SpellBlock");//sound effects for warning go here..
            criticalPiece = new PuzzlePiece();
 
            //Generate a random number for a random piece
            Random rnd = new Random();
            int totalpieces = puzzlePieces1.Count + puzzlePieces2.Count + puzzlePieces3.Count + puzzlePieces4.Count; //sum of all pieces
            int randpiece = rnd.Next(0,totalpieces - 1);
            int belongsonpuzzle = randpiece / 16 + 1; 
            Console.WriteLine("Chosen: "+ randpiece+" Belongs On puzzle: "+belongsonpuzzle);


            //Capture the object reference the random generated piece belongs to
                //puzzle -1 : apokleistikh afairesh giati xreiazomaste mono ta prohgoumena puzzle 
            
            int PositionAtPuzzle = randpiece - 16 * (belongsonpuzzle - 1); //the position of the generated puzzle in the list
            Console.WriteLine("It is the " + PositionAtPuzzle + " piece of puzzle " + belongsonpuzzle);

            if (belongsonpuzzle == 1) 
                criticalPiece = puzzlePieces1[PositionAtPuzzle ];
            else if (belongsonpuzzle == 2)
                criticalPiece = puzzlePieces2[PositionAtPuzzle ];
            else if (belongsonpuzzle == 3)
                criticalPiece = puzzlePieces3[PositionAtPuzzle ];
            else if (belongsonpuzzle == 4)
                criticalPiece = puzzlePieces4[PositionAtPuzzle ];

            criticalPiece.isCritical = true;//we have the object reference

        }

        public void CriticalPuzzle()  //Changes a whole puzzle to critical
        {
            //Generate a number for a random puzzle
            Random rnd = new Random();
            criticalPuzzle = rnd.Next(1, 5);
            fixedAreasColors[criticalPuzzle - 1] = Color.Gold;
             Console.WriteLine("Chosen: " + criticalPuzzle + " puzzle");

        }

        public void DisableCriticalPiece()//when critical task ends, we disable critical piece
        {
            criticalPiece.isCritical = false;
        }

        public void DisableCriticalPuzzle()//when critical task ends, we disable critical puzzle
        {
            fixedAreasColors[criticalPuzzle - 1] = Color.White;
            criticalPuzzle = -1;
        }

        public void Disable4ShuffleAreas()
        {
            Puzzle1ShuffledAreaRec = new Rectangle();
            Puzzle2ShuffledAreaRec = new Rectangle();
            Puzzle3ShuffledAreaRec = new Rectangle();
            Puzzle4ShuffledAreaRec = new Rectangle();

        }

        public void SelectPiece(Vector2 currentSelectPosition)//Selection of a piece
        {
            if (selectedPiece != null)
            {
                selectedPiece.IsSelected = false;
            }

            foreach (PuzzlePiece aPiece in puzzlePieces1)                               //Select for Puzzle1
            {
                if (aPiece.CollisionRectangle.Contains((int)currentSelectPosition.X, (int)currentSelectPosition.Y))
                {
                    selectedPiece = aPiece;
                }
            }

            foreach (PuzzlePiece aPiece in puzzlePieces2)                               //Select for Puzzle2
            {
                if (aPiece.CollisionRectangle.Contains((int)currentSelectPosition.X, (int)currentSelectPosition.Y))
                {
                    selectedPiece = aPiece;
                }
            }

            foreach (PuzzlePiece aPiece in puzzlePieces3)                               //Select for Puzzle3
            {
                if (aPiece.CollisionRectangle.Contains((int)currentSelectPosition.X, (int)currentSelectPosition.Y))
                {
                    selectedPiece = aPiece;
                }
            }

            foreach (PuzzlePiece aPiece in puzzlePieces4)                               //Select for Puzzle4
            {
                if (aPiece.CollisionRectangle.Contains((int)currentSelectPosition.X, (int)currentSelectPosition.Y))
                {
                    selectedPiece = aPiece;
                }
            }

            if (selectedPiece != null)
            {
                selectedPiece.IsSelected = true;
            }
        }

        public void MovePiece(Vector2 positionAdjustment)//Move a piece allong with mouse
        {
            if (selectedPiece == null)
            {
                Console.WriteLine("Selected piece is null!");
                return;
            }

            selectedPiece.position += positionAdjustment;

            Vector2 MouseAdjustment;                            //h diafora tou pontikiou apo thn panw-aristerh gonia tou kommatiou
            MouseAdjustment.X = selectedPiece.OriginalCollisionRectangle.Width;
            MouseAdjustment.Y = selectedPiece.OriginalCollisionRectangle.Height;

            selectedPiece.position = positionAdjustment - MouseAdjustment / 2;//Moving from the middle
        }

        public bool PlacePiece(Vector2 currentSelectPosition)//Drop a piece
        {
            if (selectedPiece == null)
            {
                return false;
            }

            if (selectedPiece.OriginalCollisionRectangle.Contains((int)currentSelectPosition.X, (int)currentSelectPosition.Y))
            {
                selectedPiece.position = selectedPiece.originalPosition;

               
                //Check if critical task is over and notify PuzzleGameScreen
                if (selectedPiece.isCritical)
                {
                    selectedPiece.isCritical = false;
                    puzzlegamescreen.taskIsCompleted();
                    AudioManager.PlayCue("QuestComplete");//Task is done
                }

                return true;
            }
            return false;
        }

        public bool IsSolved()//Checking if puzzle is solved
        {
            SolvedP1 = true; SolvedP2 = true; SolvedP3 = true; SolvedP4 = true;

            foreach (PuzzlePiece puzzle in puzzlePieces1)      //Solution for Puzzle1
            {
                if (puzzle.position != puzzle.originalPosition)
                {
                    SolvedP1 = false;
                }
            }

            foreach (PuzzlePiece puzzle in puzzlePieces2)      //Solution for Puzzle2
            {
                if (puzzle.position != puzzle.originalPosition)
                {
                    SolvedP2 = false;
                }
            }

            foreach (PuzzlePiece puzzle in puzzlePieces3)      //Solution for Puzzle3
            {
                if (puzzle.position != puzzle.originalPosition)
                {
                    SolvedP3 = false;
                }
            }

            foreach (PuzzlePiece puzzle in puzzlePieces4)      //Solution for Puzzle4
            {
                if (puzzle.position != puzzle.originalPosition)
                {
                    SolvedP4 = false;
                }
            }
            
            //Check if critical task is over and notify PuzzleGameScreen
            if (puzzlegamescreen.criticalIsAPuzzle)        
            {
                if (criticalPuzzle == 1 && SolvedP1)
                {
                    puzzlegamescreen.taskIsCompleted();
                    AudioManager.PlayCue("QuestComplete");//Task is done
                }
                else if (criticalPuzzle == 2 && SolvedP2)
                {
                    puzzlegamescreen.taskIsCompleted();
                    AudioManager.PlayCue("QuestComplete");//Task is done
                }
                else if (criticalPuzzle == 3 && SolvedP3)
                {
                    puzzlegamescreen.taskIsCompleted();
                    AudioManager.PlayCue("QuestComplete");//Task is done
                }
                else if (criticalPuzzle == 4 && SolvedP4)
                {
                    puzzlegamescreen.taskIsCompleted();
                    AudioManager.PlayCue("QuestComplete");//Task is done
                }
                
            }


            if (SolvedP1 && SolvedP2 && SolvedP3 && SolvedP4)
            {
                Console.WriteLine("SOLVED ALL PUZZLES!");
                return true;
            }

            return false;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch batch)
        {
           
            if (fourRegions)
            {
                //Draw Shuffled areas for puzzle pieces
                batch.Draw(PuzzleShuffledArea, Puzzle1ShuffledAreaRec, fixedAreasColors[0]);                //1
                batch.Draw(PuzzleShuffledArea, Puzzle2ShuffledAreaRec, fixedAreasColors[1]);                //2
                batch.Draw(PuzzleShuffledArea, Puzzle3ShuffledAreaRec, fixedAreasColors[2]);                //3
                batch.Draw(PuzzleShuffledArea, Puzzle4ShuffledAreaRec, fixedAreasColors[3]);                //4
            }
            else
            {
                //Draw Shuffled areas for puzzle pieces
                batch.Draw(PuzzleShuffledArea, PuzzleCommonShuffledAreaRec, Color.White);
               

            }

            //Draw perimeter for puzzle1
            batch.Draw(PuzzleFixedArea, new Rectangle((int)Puzzle1Stencil1.position.X, (int)Puzzle1Stencil1.position.Y, 
                TotalWidthOfPuzzle1, TotalHeightOfPuzzle1), fixedAreasColors[0]);

            //Draw perimeter for puzzle2
            batch.Draw(PuzzleFixedArea, new Rectangle((int)Puzzle2Stencil1.position.X, (int)Puzzle2Stencil1.position.Y, 
                TotalWidthOfPuzzle2, TotalHeightOfPuzzle2), fixedAreasColors[1]);

            //Draw perimeter for puzzle3
            batch.Draw(PuzzleFixedArea, new Rectangle((int)Puzzle3Stencil1.position.X, (int)Puzzle3Stencil1.position.Y,
                TotalWidthOfPuzzle3, TotalHeightOfPuzzle3), fixedAreasColors[2]);

            //Draw perimeter for puzzle4
            batch.Draw(PuzzleFixedArea, new Rectangle((int)Puzzle4Stencil1.position.X, (int)Puzzle4Stencil1.position.Y,
                TotalWidthOfPuzzle4, TotalHeightOfPuzzle4), fixedAreasColors[3]);

            //batch.Draw(PuzzleImage1, new Rectangle(0, 0, width/3, height), Color.White);
            foreach (PuzzlePiece aPiece in puzzlePieces1)                               //Draw Puzzle1
            {
                aPiece.Draw(batch);
            }

            foreach (PuzzlePiece aPiece in puzzlePieces2)                               //Draw Puzzle2
            {
                aPiece.Draw(batch);
            }

            foreach (PuzzlePiece aPiece in puzzlePieces3)                               //Draw Puzzle3
            {
                aPiece.Draw(batch);
            }

            foreach (PuzzlePiece aPiece in puzzlePieces4)                               //Draw Puzzle4
            {
                aPiece.Draw(batch);
            }

            if (selectedPiece != null)
            {
                selectedPiece.Draw(batch);
            }
        }

        #endregion
    }
}

