using ConnectFour.GameObjects;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour.Scenes
{
    class MainScene : GameState
    {

        public enum State
        {
            RedsTurn,
            YellowsTurn,
            GameOver
        }

         public bool CheckWin { get; set; } = false;
         State currentState { get; set; }

        const int HEIGHT = 6;
        const int WIDTH = 7;
        BoardSection[,] boardGrid = new BoardSection[WIDTH, HEIGHT];
        List<BoardSection> gridSection = new List<BoardSection>();
        GameObjectList gameBoard = new GameObjectList();
        List<Checker> checkerPool = new List<Checker>();
        int sectionIndex;
        public bool activateOne;
        int length;
        Rectangle[] dropZone = new Rectangle[7];
        Point screenSize;
        public Checker currentChecker;
        float startTime, timeBetweenTurns = .5f;
        Color winningColor;
        TextGameObject headsUp = new TextGameObject("headsUp", 1f, Color.Black, TextGameObject.Alignment.Center);
        TextGameObject restartPrompt = new TextGameObject("restartPrompt", 1f, Color.Black, TextGameObject.Alignment.Center);

        public MainScene(Point screenSize)
        {

            
            this.screenSize = screenSize;
            currentState = State.RedsTurn;
            startTime = timeBetweenTurns;

            headsUp.LocalPosition = new Vector2(screenSize.X / 2, 15);
            gameObjects.AddChild(headsUp);

            restartPrompt.LocalPosition = new Vector2(screenSize.X/2, 100);
            gameObjects.AddChild(restartPrompt);

            //Initialize the grid positions and add gameboard and available checkers

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                { 
                    boardGrid[x, y] = new BoardSection();

                    length = boardGrid[x, y].Width;

                    boardGrid[x, y].LocalPosition = new Vector2(x * length, y * length);
                    gameBoard.AddChild(boardGrid[x, y]);

                    gameObjects.AddChild(gameBoard);

                    gridSection.Add(boardGrid[x, y]);

                    checkerPool.Add(new Checker());
                    gameObjects.AddChild(checkerPool[sectionIndex]);
                    sectionIndex++;

                }
            }


            

            Reset();

            //position the game board
            gameBoard.LocalPosition = new Vector2(screenSize.X/2, screenSize.Y - (screenSize.Y * .42f)) - new Vector2(length * WIDTH, length * HEIGHT)/2;

        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    //If the player clicks on a given area above the gameboard the drop the checker in that area
                    if (dropZone[x].Contains(inputHelper.MousePositionWorld) && currentChecker != null && !currentChecker.DropChecker && currentState != State.GameOver)
                    {
                            currentChecker.LocalPosition = new Vector2(dropZone[x].X, dropZone[y].Y + 90);

                        if (inputHelper.MouseLeftButtonPressed() && y >= 0)
                        {
                            if (y == 0 && boardGrid[x, y].Contains != null)
                                return;
                            else if (y == 5 && boardGrid[x, y].Contains == null)
                            {
                                currentChecker.DropChecker = true;
                                boardGrid[x, y].Contains = currentChecker;
                            }
                            else if (boardGrid[x, y].Contains != null)
                            {
                                currentChecker.DropChecker = true;
                                boardGrid[x, y - 1].Contains = currentChecker;
                            } 
                        } 
                    }
                }
            }


            if (inputHelper.KeyPressed(Keys.Space) && currentState == State.GameOver)
            {
                Reset();
            }


        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Set the rectangles to click on the area to drop the checker
            Rectangle getZone(int position)
            {
                Rectangle zone = new Rectangle((int)gameBoard.LocalPosition.X + (position * length), 0, length, screenSize.Y);
                return zone;
            }

            for (int i = 0; i < 7; i++)
                dropZone[i] = getZone(i);


            if (activateOne)
            {
                foreach (var obj in checkerPool)
                {
                    if (!obj.Active)
                    {
                        obj.Active = true;
                        currentChecker = obj;

                        break;
                    }
                }
                activateOne = false;
            }

            if (currentChecker != null && currentState != State.GameOver)
            {
                if (currentState == State.RedsTurn)
                {
                    currentChecker.Color = Color.Red;
                    headsUp.Text = "Red's Turn";
                }
                else if (currentState == State.YellowsTurn)
                {
                    currentChecker.Color = Color.Yellow;
                    headsUp.Text = "Yellow's Turn";
                }

            }

            //If all the checkers are used and there's no winner then declare a tie
            if (!gridSection.Exists(m => m.Contains == null))
            {
                headsUp.Text = "Tie!";

                //Check who dropped the last checker so the other person can go first to start the next game
                if (checkerPool[41].Color == Color.Red)
                    winningColor = Color.Red;
                else
                    winningColor = Color.Yellow;

                currentState = State.GameOver;
            }

            if (currentState == State.GameOver)
                restartPrompt.Text = "Press Space to play again";

            if (CheckWin)
            {
                for (int x = 0; x < WIDTH; ++x)
                {
                    for (int y = 0; y < HEIGHT; ++y)
                    {
                        //Check horizontal match
                        if (x < 4 && MatchCheck(boardGrid[x, y], boardGrid[x + 1, y], boardGrid[x + 2, y], boardGrid[x + 3, y]) || //Checks horizontal match
                            y % 6 < 3 && MatchCheck(boardGrid[x, y], boardGrid[x, y + 1], boardGrid[x, y + 2], boardGrid[x, y + 3]) || //Checks vertical match
                            x < 4 && y % 6 < 3 && MatchCheck(boardGrid[x, y], boardGrid[x + 1, y + 1], boardGrid[x + 2, y + 2], boardGrid[x + 3, y + 3]) || //Checks diagnal from left-right top to bottom
                            x < 4 && y % 6 > 2 && MatchCheck(boardGrid[x, y], boardGrid[x + 1, y - 1], boardGrid[x + 2, y - 2], boardGrid[x + 3, y - 3])) //Checks diangal from left-right bottom to top
                        {
                            //If any of those detect a match then declare the winner
                            if (winningColor == Color.Red)
                                headsUp.Text = "Red is the winner!";
                            else if (winningColor == Color.Yellow)
                                headsUp.Text = "Yellow is the winner!";

                            currentState = State.GameOver;
                            break;
                        }

                    }
                }
                    

                currentChecker = null;

                timeBetweenTurns -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timeBetweenTurns <= 0)
                {
                    if (currentState == State.RedsTurn)
                        currentState = State.YellowsTurn;
                    else if (currentState == State.YellowsTurn)
                        currentState = State.RedsTurn;
                    

                    CheckWin = false;

                    activateOne = true;

                    timeBetweenTurns = startTime;
                }
            }
        }


        bool MatchCheck(BoardSection bs1, BoardSection bs2, BoardSection bs3, BoardSection bs4)
        {
           
            if ((bs1.Contains?.Color == bs2.Contains?.Color && bs2.Contains?.Color == bs3.Contains?.Color && bs3.Contains?.Color == bs4.Contains?.Color) &&
                (bs1.Contains?.Color == Color.Red || bs1.Contains?.Color == Color.Yellow))
            {
                winningColor = bs1.Contains.Color;
                return true;
            }
               

            return false;
        }

        public override void Reset()
        {
            base.Reset();

            foreach (BoardSection obj in gridSection)
                obj.Reset();

            foreach (Checker obj in checkerPool)
                obj.Reset();

            sectionIndex = 0;
            activateOne = true;

            

            if (winningColor == Color.Red)
                currentState = State.YellowsTurn;
            else if (winningColor == Color.Yellow)
                currentState = State.RedsTurn;

            restartPrompt.Text = "";

            winningColor = Color.White;
        }


    }

}
