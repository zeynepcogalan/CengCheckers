using System;
using System.Threading;

namespace CENGCheckers
{
    class Program
    {
        struct piece // structure type for checkers pieces
        {
            public int row;
            public int coloumn;
            public int score; // contains the total move that piece can go.
        }
        static char[,] initiateGameboard() //creates the game board array
        {
            //our usable array is 8x8 but we take two block array at the both begining and
            // end of array to avoid the probability that computer can try get out of gameboard
            char[,] gameboard = new char[12, 12];
            for (int i = 0; i < gameboard.GetLength(0); i++)
            {
                for (int j = 0; j < gameboard.GetLength(1); j++)
                {
                    if (i < 2 || j < 2 || i > gameboard.GetLength(0) - 3 || j > gameboard.GetLength(1) - 3)
                        gameboard[i, j] = ' ';

                    else if (i < 5 && j < 5)
                        gameboard[i, j] = 'o';

                    else if (i > 6 && j > 6)
                        gameboard[i, j] = 'x';
                    else
                        gameboard[i, j] = '.';
                }
            }


            return gameboard;
        }

        static void printGameboard(char[,] gameboard) //prints the gameboard
        {
            Console.WriteLine("   1 2 3 4 5 6 7 8 ");
            Console.WriteLine(" +-----------------+");
            for (int i = 2; i < gameboard.GetLength(0) - 2; i++)
            {
                Console.Write(i - 1 + "| ");
                for (int j = 2; j < gameboard.GetLength(1) - 2; j++)
                {
                    Console.Write(gameboard[i, j] + " ");
                }
                Console.Write("|\n");
            }
            Console.WriteLine(" +-----------------+");

            Console.SetCursorPosition(45, 4);
            Console.Write("Press 'Z' button to select the piece you want to move.");
            Console.SetCursorPosition(45, 5);
            Console.Write("Press 'X' button to select the place you want your piece to move.");
            Console.SetCursorPosition(45, 6);
            Console.Write("Press 'C' button to give the turn other player.");

        }

        // it looks the places that player pieces should be placed in order to win the game
        // and if all the places are full with the player pieces ('X') then player wins that game.
        static bool playerWon(char[,] gameboard) //controls whether player won.
        {
            bool flag = true;
            for (int i = 2; i < 5; i++)
            {
                for (int j = 2; j < 5; j++)
                {
                    if (gameboard[i, j] != 'x')
                        flag = false;
                }
            }
            return flag;
        }
        
        // it looks the places that computer pieces should be placed in order to win the game
        // and if all the places are full with the computer pieces ('O') then computer wins the game.
        static bool computerWon(char[,] gameboard) //controls whether computer won.
        {
            bool flag = true;
            for (int i = 7; i < gameboard.GetLength(0) - 2; i++)
            {
                for (int j = 7; j < gameboard.GetLength(1) - 2; j++)
                {
                    if (gameboard[i, j] != 'o')
                        flag = false;
                }
            }
            return flag;
        }

        static char[,] playerPlay(char[,] gameboard)// the function that allows player to play.
        {
            // cursor starts last square of the board in order to be moved.
            int row = 9;
            int coloumn = 9;

            int[] selectedPiece = { 0, 0 };// it means we did not select a piece.

            int move = 2; // not moving -->0  jump-->1  step and jump-->2
            bool flag = true;
            do
            {
                int cursorColoumn = coloumn * 2 - 1; // converts the array coloumn to cursor coloumn (the coloumn we see in the screen)
                Console.SetCursorPosition(cursorColoumn, row);
                ConsoleKeyInfo _Key = Console.ReadKey(true); //takes the operation
                switch (_Key.Key)
                {
                    case ConsoleKey.UpArrow: //moves curseor one square up
                        if (row > 2)
                            row--;
                        else if (row == 2)
                            row = 9;

                        break;

                    case ConsoleKey.LeftArrow: //moves curseor one square left
                        if (coloumn > 2)
                            coloumn--;
                        else if (coloumn == 2)
                            coloumn = 9;
                        break;

                    case ConsoleKey.DownArrow: // moves curseor one square down
                        if (row < 9)
                            row++;
                        else if (row == 9)
                            row = 2;
                        break;

                    case ConsoleKey.RightArrow: // moves curseor one square right
                        if (coloumn < 9)
                            coloumn++;
                        else if (coloumn == 9)
                            coloumn = 2;
                        break;


                    case ConsoleKey.Z: // selects the piece we choose to move.
                        bool canMove = false;
                        for (int i = -2; i < 3; i++)
                        {
                            if (gameboard[row + i, coloumn] == '.' || gameboard[row, coloumn + i] == '.')
                                canMove = true;
                        }
                        // this if controls the piece player select is whether his own piece , player select a piece before , the piece can move.
                        if (gameboard[row, coloumn] == 'x' && (selectedPiece[0] == 0 && selectedPiece[1] == 0) && canMove)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write(gameboard[row, coloumn]);
                            selectedPiece[0] = row;
                            selectedPiece[1] = coloumn;
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                        break;

                    case ConsoleKey.X: //allows player to choose the square he/she wants to move his/her piece.
                        if (gameboard[row, coloumn] == '.')
                        {
                            //if it is step and player did not move that piece in the same round.
                            if ((Math.Abs(selectedPiece[0] - row) + Math.Abs(selectedPiece[1] - coloumn)) == 1 && move == 2)
                            {
                                Console.SetCursorPosition(cursorColoumn, row);
                                gameboard[row, coloumn] = gameboard[selectedPiece[0], selectedPiece[1]];
                                Console.Write(gameboard[row, coloumn]);

                                Console.SetCursorPosition(selectedPiece[1] * 2 - 1, selectedPiece[0]);
                                gameboard[selectedPiece[0], selectedPiece[1]] = '.';
                                Console.Write(gameboard[selectedPiece[0], selectedPiece[1]]);

                                move = 0;
                            }
                            // if the move is a jump and player choose a piece before.
                            else if ((Math.Abs(selectedPiece[0] - row) == 2 && selectedPiece[1] == coloumn) || (Math.Abs(selectedPiece[1] - coloumn) == 2 && selectedPiece[0] == row))
                            {
                                // if player did not make a step at the same round.
                                if (move != 0 && gameboard[(selectedPiece[0] + row) / 2, (selectedPiece[1] + coloumn) / 2] != '.')
                                {
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.SetCursorPosition(cursorColoumn, row);
                                    gameboard[row, coloumn] = gameboard[selectedPiece[0], selectedPiece[1]];
                                    Console.Write(gameboard[row, coloumn]);

                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.SetCursorPosition(selectedPiece[1] * 2 - 1, selectedPiece[0]);
                                    gameboard[selectedPiece[0], selectedPiece[1]] = '.';
                                    Console.Write(gameboard[selectedPiece[0], selectedPiece[1]]);

                                    move = 1;

                                    selectedPiece[0] = row;
                                    selectedPiece[1] = coloumn;
                                }

                            }
                        }

                        break;

                    case ConsoleKey.C:// passes the turn to other player.
                        if (move != 2) // if player played one move he can pass his turn but otherwise he can not.
                        {
                            Console.SetCursorPosition(selectedPiece[1] * 2 - 1, selectedPiece[0]);
                            Console.Write(gameboard[selectedPiece[0], selectedPiece[1]]);
                            flag = false;
                        }
                        break;
                }

            } while (flag);// stays in the loop untill player press 'C' button.

            return gameboard;
        }

        // the function that allows computer to play.
        static char[,] computerPlay(char[,] gameboard)
        {
            Random random = new Random();// allows us to use random function.
            // creates an array as piece structure type that will
            // contain row coloumn and possible best move values of computer pieces
            piece[] computerPieces = new piece[9];

            int k = 0;
            for (int i = 2; i < gameboard.GetLength(0) - 2; i++)
            {
                for (int j = 2; j < gameboard.GetLength(1) - 2; j++)
                {
                    if (gameboard[i, j] == 'o')
                    {
                        computerPieces[k].row = i;
                        computerPieces[k].coloumn = j;
                        k++;
                    }
                }
            }
            // finds the max value among the scores of computer pieces.
            int max = 0;
            for (int i = 0; i < 9; i++)
            {
                computerPieces[i].score = movePiece(computerPieces[i].row, computerPieces[i].coloumn, gameboard);
                if (computerPieces[i].score > max)
                    max = computerPieces[i].score;
            }

            // if there are more then one piece that has max score 
            // we choose rondomly which one to play and plays that move.
            while (true)
            {
                int randPiece = random.Next(0, computerPieces.Length);
                if (computerPieces[randPiece].score == max)
                {
                    movePiece(computerPieces[randPiece].row, computerPieces[randPiece].coloumn, gameboard, true);
                    break;
                }
            }

            return gameboard;
        }

        // prevents piece agglomeration at the bottom and most left of the gameboard.
        static bool bugControl(int pieceX, int pieceY, int targetX, int targetY, char[,] gameboard)
        {

            if (targetX != pieceX)
            {
                switch (targetX)
                {
                    case 9:
                        if (pieceX != 9 && pieceX != 8)
                        {
                            int countLast = 0;
                            int count = 0;
                            for (int i = 2; i < 10; i++)
                            {
                                if (gameboard[9, i] == 'o')
                                    countLast++;
                                if (gameboard[8, i] == 'o')
                                    count++;
                            }
                            if (count + countLast == 6 || countLast == 3)
                                return false;
                        }
                        else if (pieceX == 8)
                        {
                            int countLast = 0;
                            for (int i = 2; i < 10; i++)
                            {
                                if (gameboard[9, i] == 'o')
                                    countLast++;
                            }
                            if (countLast == 3)
                                return false;
                        }

                        break;

                    case 8:
                        if (pieceX != 8)
                        {
                            int count = 0;
                            for (int i = 2; i < 10; i++)
                            {
                                if (gameboard[8, i] == 'o')
                                    count++;
                                if (gameboard[9, i] == 'o')
                                    count++;
                            }
                            if (count == 6)
                                return false;
                        }


                        break;
                }
            }

            else if (targetY != pieceY)
            {
                switch (targetY)
                {
                    case 9:
                        if (pieceY != 9 && pieceY != 8)
                        {
                            int countLast = 0;
                            int count = 0;
                            for (int i = 2; i < 10; i++)
                            {
                                if (gameboard[i, 9] == 'o')
                                    countLast++;
                                if (gameboard[i, 8] == 'o')
                                    count++;
                            }
                            if (count + countLast == 6 || countLast == 3)
                                return false;
                        }
                        else if (pieceY == 8)
                        {
                            int countLast = 0;
                            for (int i = 2; i < 10; i++)
                            {
                                if (gameboard[i, 9] == 'o')
                                    countLast++;
                            }
                            if (countLast == 3)
                                return false;
                        }

                        break;

                    case 8:
                        if (pieceY != 8)
                        {
                            int count = 0;
                            for (int i = 2; i < 10; i++)
                            {
                                if (gameboard[i, 9] == 'o')
                                    count++;
                                if (gameboard[i, 8] == 'o')
                                    count++;
                            }
                            if (count == 6)
                                return false;
                        }


                        break;
                }

            }

            return true;
        }

        // controls whether the piece can jump right.
        static bool canJumpRight(int pieceX, int pieceY, char[,] gameboard)
        {
            if ((gameboard[pieceX, pieceY + 1] != '.' && gameboard[pieceX, pieceY + 2] == '.') && bugControl(pieceX, pieceY, pieceX, pieceY + 2, gameboard))
                return true;
            return false;
        }

        // controls whether the piece can jump down.
        static bool canJumpDown(int pieceX, int pieceY, char[,] gameboard)
        {
            if ((gameboard[pieceX + 1, pieceY] != '.' && gameboard[pieceX + 2, pieceY] == '.') && bugControl(pieceX, pieceY, pieceX + 2, pieceY, gameboard))
                return true;
            return false;
        }

        // controls whether the piece can step right.
        static bool canStepRight(int pieceX, int pieceY, char[,] gameboard)
        {
            if (gameboard[pieceX, pieceY + 1] == '.' && bugControl(pieceX, pieceY, pieceX, pieceY + 1, gameboard))
                return true;
            return false;
        }

        // controls whether the piece can step down.
        static bool canStepDown(int pieceX, int pieceY, char[,] gameboard)
        {
            if (gameboard[pieceX + 1, pieceY] == '.' && bugControl(pieceX, pieceY, pieceX + 1, pieceY, gameboard))
                return true;
            return false;
        }

        // if the boolean change value is false it just controls how far the piece can go
        // otherwise the boolean change parameter is true it also makes the best move for real.
        static int movePiece(int pieceX, int pieceY, char[,] gameboard, bool change = false, int score = 0) //controls how far a piece can go.
        {

            if (canJumpDown(pieceX, pieceY, gameboard) && canJumpRight(pieceX, pieceY, gameboard))
            {
                int rightJumpScore = movePiece(pieceX, pieceY + 2, gameboard, false, score + 2);// controls how far can go after right jump
                int downJumpScore = movePiece(pieceX + 2, pieceY, gameboard, false, score + 2);//controls how far can go after down jump.

                if (rightJumpScore > downJumpScore)
                {
                    if (change)
                    {
                        consoleMove(pieceX, pieceY, pieceX, pieceY + 2, gameboard);
                    }
                    return movePiece(pieceX, pieceY + 2, gameboard, change, rightJumpScore);
                }
                else
                {
                    if (change)
                    {
                        consoleMove(pieceX, pieceY, pieceX + 2, pieceY, gameboard);
                    }
                    return movePiece(pieceX + 2, pieceY, gameboard, change, downJumpScore);
                }
            }

            else if (canJumpDown(pieceX, pieceY, gameboard))
            {
                if (change)
                {
                    consoleMove(pieceX, pieceY, pieceX + 2, pieceY, gameboard);
                }
                return movePiece(pieceX + 2, pieceY, gameboard, change, score + 2);
            }


            else if (canJumpRight(pieceX, pieceY, gameboard))
            {
                if (change)
                {
                    consoleMove(pieceX, pieceY, pieceX, pieceY + 2, gameboard);
                }

                return movePiece(pieceX, pieceY + 2, gameboard, change, score + 2);
            }

            else if (score == 0 && (canStepDown(pieceX, pieceY, gameboard) || canStepRight(pieceX, pieceY, gameboard)))
            {
                if (change)
                {
                    if (canStepDown(pieceX, pieceY, gameboard) && canStepRight(pieceX, pieceY, gameboard))
                    {
                        if (pieceX < pieceY)
                            consoleMove(pieceX, pieceY, pieceX + 1, pieceY, gameboard);

                        else
                            consoleMove(pieceX, pieceY, pieceX, pieceY + 1, gameboard);

                    }

                    else if (canStepDown(pieceX, pieceY, gameboard))
                        consoleMove(pieceX, pieceY, pieceX + 1, pieceY, gameboard);

                    else if (canStepRight(pieceX, pieceY, gameboard))
                        consoleMove(pieceX, pieceY, pieceX, pieceY + 1, gameboard);
                }

                return score + 1;
            }


            else
                return score;
        }

        //moves the pieces you want both in array and on the console.
        static void consoleMove(int row, int coloumn, int targetRow, int targetColoumn, char[,] gameboard)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(targetColoumn * 2 - 1, targetRow);
            gameboard[targetRow, targetColoumn] = gameboard[row, coloumn];
            Console.Write(gameboard[targetRow, targetColoumn]);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(coloumn * 2 - 1, row);
            gameboard[row, coloumn] = '.';
            Console.Write(gameboard[row, coloumn]);

            Console.SetCursorPosition(35, 6);
            Thread.Sleep(1500);

            Console.SetCursorPosition(targetColoumn * 2 - 1, targetRow);
            Console.Write(gameboard[targetRow, targetColoumn]);
        }
        
        // print winner at the screen with Color.(player-->Red)(Computer-->DarkGreen)
        static void printWinner(char Winner)
        {
            if (Winner == 'X')
                Console.BackgroundColor = ConsoleColor.Red;
            else
                Console.BackgroundColor = ConsoleColor.DarkGreen;

            Console.SetCursorPosition(26, 10);
            Console.WriteLine("Winner " + Winner);


        }
        
        // starts Playing the game.
        static void play(char[,] gameBoard, int round = 0)
        {
            Console.SetCursorPosition(25, 5);
            Console.Write("Round: " + round);
            Console.SetCursorPosition(25, 6);
            Console.Write("Turn: X");

            playerPlay(gameBoard);
            if (playerWon(gameBoard))
            {
                printWinner('X');
            }

            Console.SetCursorPosition(25, 6);
            Console.Write("Turn: O");
            computerPlay(gameBoard);
            if (computerWon(gameBoard))
            {
                printWinner('O');
            }

            play(gameBoard, round + 1);

        }

        static void Main(string[] args)
        {

            char[,] gameBoard = initiateGameboard();
            printGameboard(gameBoard);

            play(gameBoard);


            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\n\n\n");
        }
    }
}