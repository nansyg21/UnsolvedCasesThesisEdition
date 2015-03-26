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
    public class SuspectEscapeScreen : GameScreen
    {

        public class Block //Elements of Width First Tree
        {
            //Nullable values
            public int suspectPosition; //from 1 to rows*collumns -1
            public int? direction;      //how we got to this block , previous move
            public Block parent;       //parent block
            public bool isRoot;           //byte default all blocks are NOT roots, root is assigned after creation

            public Block(int _suspectPosition, int? _direction, Block _parent)
            {
                suspectPosition = _suspectPosition;
                direction = _direction;
                parent = _parent;
                isRoot = false;
            }
            public void SetRoot()
            {
                isRoot = true;
            }
        }

        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;

        MouseState mstate;
        MouseState Oldmstate;
        int width, height;

        Texture2D suspectTex;
        Rectangle suspectRec;
        Texture2D intersectionTex;
        Texture2D closedTex;       //image that is drawn on an intersection when it is closed
        Texture2D roadTex;
        Texture2D houseTex1;

        static int collumns = 11;      //number of rows
        static int rows = 10;  //number of collumns

        Rectangle[] IntersectRecList;                      // array for all the intersections;
        bool[] isClosedList;                 // array to hold closed intersections
        bool[,] isClosed2D;                 //false:open, true:closed      by default initialized as false
        List<Rectangle> listOfClosed;   //list with the recs and textures to be drawn, on closed intersections

        float widthMultiplier = 2f;//distance between elements to the right
        float heigthMultiplier = 1f;//distance between elements downwards

        KeyboardState state;

        int IntersectRecWidth = 35, IntersectRecHeight = 35;   //all intersections width & height(arxika dokimastika=50)
        bool playersTurn = true;
        int suspectIsOn;             //initially suspect is in the middle
        int playersMove;            //players move in intersections
        int movesStillLeft = 0;              //how many moves the player still has, usually player has 1
        string strToDraw = "";
        bool finished = false;
        List<int> bonusIntersections;   //on click the user has +1 move
        int bonusTotal = 5;

        //WIDTH FIST SEARCH
        //Heuristic     CONSIDERS CLOSED INTERSECTIONS BUT NOT THE POSSIBLE PLAYERS MOVES   this means that all children have the same map
        List<Block> ChildrenArray;      //We keep only the suspects position
        List<Block> ParentArray;      //We keep only the suspects position
               
        int[] priorityUp = new int[] { 1, 2, 4, 3 };   
        int[] priorityRight = new int[] { 2, 3, 1, 4 };           
        int[] priorityDown = new int[] { 3, 4, 2, 1 };        
        int[] priorityLeft = new int[] { 4, 1, 3, 2 };
         

        int maxDepth = 30;              //maximun depth on search tree
        int maxBlocks = 100000;

        /** Stuff to do
         * animation sthn kinhsh
         * kalytero suspect image
         * kalytero intersection image
         * 
         * 
         * Ο παίχτης θα μπορεί να μετακινήσει ένα εμπόδιο από εκεί που το έχει βάλει προκειμένου να οδηγήσει
         *               τον ύποπτο στην φυλακή. (Εδώ θα μπορούσε να έχει και συγκεκριμένο αριθμό εμποδίων στην διάθεσή του)
         *               
         * Διαφορετικός αριθμός τετραγώνων ανά οδό.
         * 
         * Να αλλάζουν τα κτήρια με τυχαίο τρόπο (αναφορικά με την εμφάνιση τους) και επίσης εκτός από κτήρια να υπάρχουν 
         *                 και πάρκα ή πλατείες πχ από τα οποία δεν θα μπορεί να περάσει ο ύποπτος.
         * 
         **/

        float myrotation = (float)Math.PI / 2;
        #endregion

        #region Initialization

        public SuspectEscapeScreen()
        {
        }

        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;

            //Set Mouse Cursor
            ScreenManager.MainGame.IsMouseVisible = false;
            if (!ScreenManager.MainGame.Components.Contains(ScreenManager.GetCursor))
                ScreenManager.MainGame.Components.Add(ScreenManager.GetCursor);

            width = ScreenManager.GraphicsDevice.Viewport.Width;
            height = ScreenManager.GraphicsDevice.Viewport.Height;
            IntersectRecList = new Rectangle[collumns * rows];
            isClosedList = new bool[collumns * rows];                 // array to hold closed intersections
            isClosed2D = new bool[rows, collumns];
            listOfClosed = new List<Rectangle>();
            ChildrenArray = new List<Block>();
            bonusIntersections = new List<int>();
            AddBonusIntersections();


            for (int i = 0; i < rows; i++)//add elements 
            {
                AddARowOfIntersections(i);
                for (int j = 0; j < collumns; j++)//open intersections
                {
                    isClosed2D[i, j] = false;
                }
            }

            int randPlacedSuspect = ((collumns / 2) * collumns) + rows / 2;
            suspectIsOn = randPlacedSuspect;
            suspectTex = content.Load<Texture2D>(@"Textures\Riddles\SuspectEscape\suspect_escape");
            suspectRec = new Rectangle(IntersectRecList[randPlacedSuspect].X, IntersectRecList[randPlacedSuspect].Y,
                2 * IntersectRecWidth, IntersectRecHeight);
            //oi diastaseis einai prosarmosmenes sthn sygkekrimenh eikona

            closedTex = content.Load<Texture2D>(@"Textures\Riddles\SuspectEscape\closed");
            intersectionTex = content.Load<Texture2D>(@"Textures\Riddles\SuspectEscape\intersection");
            roadTex = content.Load<Texture2D>(@"Textures\Riddles\SuspectEscape\road");
            houseTex1 = content.Load<Texture2D>(@"Textures\Riddles\SuspectEscape\house");

            Oldmstate = Mouse.GetState();
        }

        public override void UnloadContent()
        {
            //content.Unload(); This content.Unload() causes a problem with spriteBatch on Begin() and End()
        }

        #endregion

        #region Public Methods

        public bool PosExists(int pos)//Checks if the position given exists or not
        {
            if (pos >= 0 && collumns * rows > pos)
                return true;
            return false;
        }

        public void FindNeighbors(int i) //Finds if i, j are neighbors
        {
            //Euresi Seiras                   : i /collumns  (ksekiname apo to 0)
            //H Seira Einai pros ta mesa      : ((i / collumns ) %2==0) :         
            // Euresi elaxistou se kathe seira :   (i / collumns )*rows
            // Euresi megistoy se kathe seira  :   (i / collumns )*rows+collumns-1

            Console.WriteLine("Seira: " + (i / rows) + ", Elaxisto: " + ((i / rows) * collumns) + ", Megisto: " +
                ((i / rows) * collumns + rows - 1));

            //i is a Valid Position
            if (PosExists(i))
            {
                int k = 0;
                // if (((i / collumns + 1) % 2 == 1))              //otan h seira einai pio mesa exoume +1 gia ton elegxo pros ta panw    
                //    k = 1;                                  //kai +1 gia ton elegxo pros ta katw


                //Elegxos mprosta kai pisw
                int min_row = (i / rows) * collumns;                                //1o stoixeio grammhs
                int max_row = (i / rows) * collumns + rows - 1;                 //teleutaio stoixeio grammhs
                int min_prevrow = ((i) / rows - 1) * collumns;                      //1o stoixeio grammhs
                int max_prevrow = ((i) / rows - 1) * collumns + rows - 1;       //teleutaio stoixeio grammhs
                int min_nextrow = ((i) / rows + 1) * collumns;                      //1o stoixeio grammhs
                int max_nextrow = ((i) / rows + 1) * collumns + rows - 1;       //teleutaio stoixeio grammhs
                Console.WriteLine("min_prevrow: " + min_prevrow + ", max_prevrow: " + max_prevrow);
                Console.WriteLine("Neightbors are: ");

                //Find 4 neighbors:
                if (i + 1 <= max_row) //next element IN SAME ROW
                {
                    CloseIntersection(i + 1);
                    Console.WriteLine((i + 1));
                }

                if (i - 1 >= min_row) //previous element IN SAME ROW
                {
                    CloseIntersection(i - 1);
                    Console.WriteLine((i - 1));
                }

                CloseIntersection(i + collumns); //Bottom element
                Console.WriteLine((i + collumns));

                CloseIntersection(i - collumns); //Upper Element
                Console.WriteLine((i - collumns));


                #region DiagonalNeighbors
                /**
                if ((i - rows + k) <= max_prevrow)          //an to diagoneio stoixeio einai mikrotero apo to max ths prohgoumenhs grammhs
                {
                    Console.WriteLine((i - rows + k));
                }

                if (i - rows - 1 + k >= min_prevrow)        //an to diagoneio stoixeio -1, einai megalytero apo to min ths 
                                                            //prohgoumenhs grammhs
                {
                    Console.WriteLine((i - rows - 1 + k));
                }
                **/
                #endregion
            }
        }

        public void AddBonusIntersections() //Generates random numbers as bonus intersections
        {
            Random rand = new Random();
            int temp;

            Console.WriteLine("Bonus Blocks");
            for (int i = 0; i < bonusTotal; i++)        //numbers are not repeated
            {
                temp = rand.Next(0, rows * collumns - 1);
                while (bonusIntersections.Contains(temp))
                    temp = rand.Next(0, rows * collumns - 1);

                bonusIntersections.Add(temp);
                Console.WriteLine(i + ": " + bonusIntersections[i]);
            }

        }

        public void AddARowOfIntersections(int current)//current: Represents the current row to add
        {
            //(0,0) : width/2 - (collumns-1/2) * width_of_each_piece

            //first row, first collumn : initialize dimensions
            if (current == 0)
            {
                // Console.WriteLine("Starting dimensions at 0");
                Rectangle tmpRec = new Rectangle(width / 3 - (int)((collumns - 0.5) * IntersectRecWidth), 10,/*height/11 insted of 10*/
                    IntersectRecWidth, IntersectRecHeight);
                IntersectRecList[0] = tmpRec;
            }

            //first collumn but not first row : retrieving dimensions from upper element
            else
            {
                int upperElement = ((current * collumns) - collumns);

                //Console.WriteLine("Got dimensions from index: " + ((current * collumns) - collumns));

                Rectangle tmpRec = new Rectangle(IntersectRecList[upperElement].X,
                                 IntersectRecList[upperElement].Y + IntersectRecList[upperElement].Height + IntersectRecHeight,
                                 IntersectRecWidth, IntersectRecHeight);
                IntersectRecList[(current * collumns)] = tmpRec;
            }
            //add the rest elements of each row
            for (int i = 1; i < collumns; i++)
            {
                int previous = (i + (current * collumns)) - 1;
                //Console.WriteLine("Added index: " + (i + (current * rows)) + ", previous: " + (i + (current * rows) - 1));
                Rectangle tmpRec = new Rectangle(IntersectRecList[previous].X + IntersectRecList[previous].Width + (int)(widthMultiplier * IntersectRecWidth),
                                     IntersectRecList[previous].Y, IntersectRecWidth, IntersectRecHeight);
                IntersectRecList[(i + (current * collumns))] = tmpRec;
            }

            #region AlternativeWay
            /**
            *  (0, *): alternately each row starts 'gap' pixels to the right 
            int gap = 0;                    //kathe deyterh grammi ksekinaei pio mesa
            if (current == 0)               //first row, first collumn : initialize dimensions
            {
                Console.WriteLine("Starting dimensions at 0");
                Rectangle tmpRec = new Rectangle(width / 2 - (int)((collumns - 0.5) * IntersectRecWidth), height / 11, 
                    IntersectRecWidth, IntersectRecHeight);
                IntersectRecList[0] = tmpRec;
            }
            else                                                        //Not the first row, first collumn : get diensions from upper rec
            {
                if ((current * rows) % (rows + collumns) == 0)          //1rst piece of every second row
                    gap = IntersectRecWidth;
                else
                    gap = -IntersectRecWidth;

                int upper = ((current * rows) - rows);

                Console.WriteLine("Got dimensions from index: " + ((current * rows) - rows));

                Rectangle tmpRec = new Rectangle(gap + IntersectRecList[upper].X,
                                 IntersectRecList[upper].Y + IntersectRecList[upper].Height + IntersectRecHeight,
                                 IntersectRecWidth, IntersectRecHeight);
                IntersectRecList[(current * rows)] = tmpRec;
            }

            for (int i = 1; i < rows; i++)
            {
                int previous = (i + (current * rows)) - 1;
                //Console.WriteLine("Added index: " + (i + (current * rows)) + ", previous: " + (i + (current * rows) - 1));
                Rectangle tmpRec = new Rectangle(IntersectRecList[previous].X + IntersectRecList[previous].Width + IntersectRecWidth,
                                     IntersectRecList[previous].Y, IntersectRecWidth, IntersectRecHeight);
                IntersectRecList[(i + (current * rows))] = tmpRec;
            }
             **/

            #endregion
        }

        public void MoveSuspectTo(int pos)                  //AI calls this method to move the suspect (position: starting from 0)
        {
            suspectRec = new Rectangle(IntersectRecList[pos].X, IntersectRecList[pos].Y, 2 * IntersectRecWidth, IntersectRecHeight);
            suspectIsOn = pos;
            //Console.WriteLine("Suspect moved to: " + suspectIsOn);
        }

        public void CloseIntersection(int pos)//Closes the intersection: 1)make it unavailable 2)draw an image on it (starting from 0)
        {
            try
            {
                listOfClosed.Add(IntersectRecList[pos]);
                isClosedList[pos] = true;        //close intersection
                isClosed2D[pos / collumns, pos % collumns] = true;

                //Check for bonus block
                if (bonusIntersections.Contains(pos))
                {
                    Console.WriteLine("BONUS----------------------------");
                    movesStillLeft++;
                    AudioManager.PlayCue("HealPotion");
                }
            }
            catch (IndexOutOfRangeException E) { } //do nothing
        }

        public int FindClickedIntersection()//Uses a loop to find the intersection the user clicked on
        {
            int i = 0;

            foreach (Rectangle rec in IntersectRecList)
            {
                if (rec.Contains(mstate.X, mstate.Y) && suspectIsOn != i && !isClosedList[i])
                    return i;
                else
                    i++;
            }
            return -1;
        }

        public int Heuristic_WidthFirstSearch(int rootSuspectPlace)//Moves the suspect using Width First Search(iteratively)
        //An array holds all the children
        //Steps: Given the root(a state: array and suspect position), find all the children and add then to array
        //       Check all the children for solution
        //          If a solution is found, 
        //              return from parents to root and get the path
        //          else 
        //              for every state on children array, find all the children and repeat
        //  RETURNS: One Move: 1,2,3 or 4
        {
            ChildrenArray = new List<Block>();
            ParentArray = new List<Block>();
            Block rootBlock = new Block(rootSuspectPlace, null, null);
            rootBlock.SetRoot();
            FindChildren(rootBlock);
            ParentArray = ChildrenArray;

            Console.WriteLine("INITIALLY HAS:" + ChildrenArray.Count + " CHILDREN");

            int left_child = 0;   //index (on the array) of the left child
            int count = ChildrenArray.Count;

            int tree_depth = 0;
            while (true)
            {
                //Check for solution
                int t = ChildrenArrayContainsSolution(0);//check only the children on the lowest height so far
                if (t != -1)
                {
                    Console.WriteLine(" COUNT IS: " + count + " PARENTS ARE: " + ParentArray.Count + " DEPTH IS" + tree_depth);
                    Console.WriteLine("FOUND SOLUTION: " + ChildrenArray[t].suspectPosition + " DEPTH:" + tree_depth);
                    Block solution = ChildrenArray[t];
                    ChildrenArray.Clear(); 
                    return FindBackwardsPath(solution);

                }

                //No solution found, Find children for all final blocks
                ParentArray = ChildrenArray;// now children become parents
                ChildrenArray = new List<Block>();
                for (int k = 0; k < count; k++)
                {
                    FindChildren(ParentArray[k]);
                }


               

              //  left_child = count;
                count = ChildrenArray.Count;
               // Console.WriteLine(" COUNT IS: " + count + " PARENTS ARE: " + ParentArray.Count + " DEPTH IS" + tree_depth);

                tree_depth++;

                //AI cant find a move, suspect is trapped
                if (tree_depth > maxDepth || count>maxBlocks)     //NOT SURE FOR COUNT
                    return -2;          //-2 means suspect is trapped

            }
        }

        public int ValidMove(int suspectPosition, int direction)//Given the direction, checks if the move is valid
        //Returns -1: Cant make that move(on fail)   OR   suspects next place(on success)
        //  ROW:    suspectPosition/collumns
        //COLLUMN:  suspectPosition%collumns
        {
            switch (direction)
            {
                case 1:      //up: previous row, same collumn
                    if (!isClosed2D[(suspectPosition - collumns) / collumns, suspectPosition % collumns])
                        return suspectPosition - collumns;
                    else
                        return -1;

                case 2:      //right: same row, next collumn
                    //   Console.WriteLine("up: Now:" + suspectPosition + " next: " + (suspectPosition +1)
                    //  + " row: " + ((suspectPosition + 1) / collumns) + " collumn: " + ((suspectPosition + 1) % collumns));
                    if (!isClosed2D[(suspectPosition + 1) / collumns, (suspectPosition + 1) % collumns])
                        return suspectPosition + 1;
                    else
                        return -1;
                case 3:      //down: next row, same collumn
                    if (!isClosed2D[(suspectPosition + collumns) / collumns, (suspectPosition) % collumns])
                        return suspectPosition + collumns;
                    else
                        return -1;

                case 4:      //left: same row, previous collumn
                    //  Console.WriteLine("up: Now:" + suspectPosition + " next: " + (suspectPosition -1)
                    // + " row: " + (suspectPosition / collumns) + " collumn: " + ((suspectPosition-1) % collumns));
                    if (!isClosed2D[suspectPosition / collumns, (suspectPosition - 1) % collumns])
                        return suspectPosition - 1;
                    else
                        return -1;

                case -2: //-2 means suspect is trapped
                    return -2;

                default:
                    Console.WriteLine("WRONG DIRECTION INSERTED");
                    return -1;
            }
        }


        public bool StateIsSolution(int suspectPosition)//A state is a solution if the suspect is on the edge
        //On array[x,y] Edge means: x==0 || y==0 || x==rows-1 || y= collumns-1
        {
            int x = suspectPosition / collumns;
            int y = suspectPosition % collumns;
            return ((x == 0) || (y == 0) || (x == rows - 1) || (y == collumns - 1));
        }

        public void FindChildren(Block parentBlock)//Given the state(block) finds all the children of the block 
        //and adds them to ChildrenArray, children might be from 1 to 4
        {
 
            #region Priority
            /** an h trexousa kinhsh einai 1 (up), h epomenh kinhsh tha einai:       Priority: up    right   left    down
         *  an h trexousa kinhsh einai 2 (right), h epomenh kinhsh tha einai:    Priority: right down    up      left  
         *  an h trexousa kinhsh einai 3 (down), h epomenh kinhsh tha einai:     Priority: down  right   left    up  
         *  an h trexousa kinhsh einai 1 (left), h epomenh kinhsh tha einai:     Priority: left  up      down    right  
         *  px gia to up: se symferei na synexiseis na phgaineis sthn idia kateuthinsi (up), an den mpoireis sthn xeiroterh tha pas pros ta pisw
                **/
             #endregion

            int next_possible_move;
            int[] priority ;

            if (parentBlock.direction == 2)
                priority = priorityRight;
            else if (parentBlock.direction == 3)
                priority = priorityDown;
            else if(parentBlock.direction == 4)
                priority = priorityLeft;
            else
                priority = priorityUp;




            for (int l = 0; l < 4; l++)
            {
                next_possible_move = ValidMove((int)parentBlock.suspectPosition, priority[l]);
                if (next_possible_move != -1)
                {
                    ChildrenArray.Add(new Block(next_possible_move, (int)priority[l], parentBlock));
                     //Console.WriteLine(parentBlock.suspectPosition + ": " + ChildrenArray[ChildrenArray.Count-1].suspectPosition);
                }
            }
        
        }

        public int ChildrenArrayContainsSolution(int left_child)//Checks all the children from ChildrenArray, if solution is found return the index else -1
        {
            int c = ChildrenArray.Count;
            for (int k = 0; k < c; k++)
                if (StateIsSolution((int)ChildrenArray[k].suspectPosition))
                    return k;
            return -1;
        }

        public int FindBackwardsPath(Block leaf) //Given the leaf, go from parent to parent and find the root. Return the move the root did
        {
            string solution = "" + leaf.direction;
            Block parent = leaf.parent;
            while (!parent.isRoot)
            {
                solution += " " + parent.direction;
                parent = parent.parent;
            }

            Console.WriteLine("SOLUTIONNN IS: " + solution);

            return int.Parse(solution[solution.Length - 1].ToString());
        }


        #endregion

        #region Update

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            mstate = Mouse.GetState();

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                LoadingScreen.Load(ScreenManager, true, new MultiplayerScreen());
            if (!finished)
            {
                if (playersTurn)
                {
                    if (mstate.LeftButton == ButtonState.Pressed && Oldmstate.LeftButton == ButtonState.Released)
                    //first needs to be click
                    {
                        playersMove = FindClickedIntersection();              //then, it needs to be valid click 

                        if (playersMove != -1)                                    //valid click
                        {
                            //FindNeighbors(playersMove);                           //just to see if it works
                            CloseIntersection(playersMove);
                            if (movesStillLeft == 0)
                            {
                                playersTurn = false;
                                movesStillLeft = 0;
                                strToDraw = "";
                            }
                            else
                            {
                                movesStillLeft--;
                                strToDraw = " +1 MOVE";
                            }
                            Console.WriteLine("Clicked on: " + playersMove + " row: " + playersMove / collumns + " collumns: " + playersMove % collumns);
                        }
                    }
                }
                else
                {
                    //AI MAKES MOVE
                    int nextMove = ValidMove(suspectIsOn, Heuristic_WidthFirstSearch(suspectIsOn));
                    if (nextMove == -2)
                    {
                        strToDraw = "CONGRATULATIONS, YOU TRAPPED THE SUSPECT";
                        finished = true;
                        AudioManager.PlayCue("LevelUp");
                    }
                    else if (nextMove != -1)
                    {
                        MoveSuspectTo(nextMove);
                        if (StateIsSolution(suspectIsOn))
                        {
                            strToDraw = "YOU LOST...";
                            finished = true;
                            AudioManager.PlayCue("FireballHit");
                        }
                    }
                    else
                        Console.WriteLine("Something wrong with direction");
                    playersTurn = true;

                }
            }
            else
            {
                finished = false;
                LoadContent();

            }




            //Console.WriteLine("Suspect is on: " + suspectIsOn);
            Oldmstate = mstate;
        }

        #endregion



        #region Draw

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch = ScreenManager.SpriteBatch;


            spriteBatch.Begin();

            //draw a house on map
            //    spriteBatch.Draw(houseTex1, new Rectangle(IntersectRecList[0].X + 3*IntersectRecList[0].Width/4,
            //               IntersectRecList[0].Y + 3*IntersectRecList[0].Height/4,
            //              (int) ((widthMultiplier+1/2f) * IntersectRecWidth), 
            //              (int) ((1+1/2f) * IntersectRecHeight)),
            //               Color.White);

            foreach (Rectangle rec in IntersectRecList)//Draw all intersections
            {
                //road to the right
                spriteBatch.Draw(roadTex, new Rectangle(rec.X + IntersectRecWidth, rec.Y + IntersectRecHeight / 4,
                    (int)(widthMultiplier * IntersectRecWidth), IntersectRecHeight / 2), Color.White);


                //road downwards
                spriteBatch.Draw(roadTex, new Rectangle(rec.X + 3 * IntersectRecWidth / 4, rec.Y + IntersectRecHeight,
                                IntersectRecWidth, IntersectRecHeight / 2), null, Color.White,
                                myrotation, Vector2.Zero, SpriteEffects.None, 0f);
                //diastaurwsh
                spriteBatch.Draw(intersectionTex, rec, Color.White);

                //draw a house on map
                spriteBatch.Draw(houseTex1, new Rectangle(rec.X + 3 * rec.Width / 4,
                               rec.Y + 3 * rec.Height / 4,
                               (int)((widthMultiplier + 1 / 2f) * IntersectRecWidth),
                               (int)((heigthMultiplier + 1 / 2f) * IntersectRecHeight)),
                               Color.White);


                #region DiagonialDrawing
                /**
                 //float myrotation = 0.8899f;    //proekypse katopin peiramatwn
                 //float myrotation2 = -0.8949f;
                //spriteBatch.DrawString(Fonts.MessageFont, "rot is: " + myrotation2, Vector2.One, Color.White);
                 //road to the right
                spriteBatch.Draw(roadTex, new Rectangle(rec.X + IntersectRecWidth, rec.Y + IntersectRecHeight / 4,
                    IntersectRecWidth, IntersectRecHeight / 2), Color.White);
                 
                //road diagonial & up
                spriteBatch.Draw(roadTex, new Rectangle(rec.X, rec.Center.Y, (int)widthMultiplier * IntersectRecWidth,
                    IntersectRecHeight / 2), null, Color.White, myrotation2, Vector2.Zero, SpriteEffects.None, 0f);

                //road diagonial & down
                spriteBatch.Draw(roadTex, new Rectangle(rec.Center.X, rec.Center.Y, (int)widthMultiplier * IntersectRecWidth,
                    IntersectRecHeight / 2), null, Color.White, myrotation, Vector2.Zero, SpriteEffects.None, 0f);             
               **/
                #endregion
            }

            //Draw closedTexture on clicked intersections
            Color clr = Color.White;
            clr.A = 125;                                        //small transparency

            foreach (Rectangle rec in listOfClosed)
            {
                spriteBatch.Draw(closedTex, rec, clr);
            }

            //draw suspect
            spriteBatch.Draw(suspectTex, suspectRec, Color.White);

            //draw message
            spriteBatch.DrawString(Fonts.MainFont, strToDraw, new Vector2(width / 2, height - 100), Color.Black);
            spriteBatch.End();
        }

        #endregion
    }
}

