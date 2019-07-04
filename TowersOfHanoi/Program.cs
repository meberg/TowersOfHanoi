using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;


namespace TowersOfHanoi
{
    class Program
    {
        static string[] from;
        static string[] spare;
        static string[] to;
        static int topFromValue;
        static int topToValue;
        static int numberOfMoves;

        static List<string[]> pillarList = new List<string[]>();

        static int towerHeight;


        static void Main(string[] args)
        {

            pillarList.Add(from);
            pillarList.Add(spare);
            pillarList.Add(to);

            StartGame();
        }


        static void StartGame()
        {
            CreateDiscs();
            while (!GameOver())
            {
            DrawPillars();
            TryMoveDisc();
            }
            DrawPillars();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Congratulations, you finished the game in {numberOfMoves} moves!");
            Console.ResetColor();
            Console.WriteLine($"The least number of moves possible was {Math.Pow(2, towerHeight)-1} moves.");
        }


        private static bool GameOver()
        {
            string[] toPillar = pillarList[2];
            if (toPillar[toPillar.Length - 1] != " ")
            {
                return true;
            }
            return false;
        }


        private static void CreateDiscs()
        {

            int[] validInput = { 2, 3, 4, 5, 6, 7 };
            var numberOfDiscs = 0;

            while (!validInput.Contains(numberOfDiscs))
            {
                Console.Write("Number of discs (2-7, or exit to quit): ");

                string userInput = Console.ReadLine().ToLower();
                if (userInput == "exit")
                {
                    System.Environment.Exit(1);
                }
                int.TryParse(userInput, out numberOfDiscs);

                if (!validInput.Contains(numberOfDiscs))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input.");
                    Console.ResetColor();
                }
            }
            Console.Clear();
            towerHeight = numberOfDiscs;

            from = new string[numberOfDiscs];
            spare = new string[numberOfDiscs];
            to = new string[numberOfDiscs];

            pillarList[0] = from;
            pillarList[1] = spare;
            pillarList[2] = to;

            for (int i = 0; i < numberOfDiscs; i++)
            {
                from[i] = $"{numberOfDiscs - i}";
                spare[i] = " ";
                to[i] = " ";
            }
        }


        static public void TryMoveDisc()
        {
            int moveFrom = 0;
            int moveTo = 0;

            List<int> approvedInputs = new List<int> { 1, 2, 3 };

            bool allowedMove = false;

            while (!allowedMove)
            {
                while (!approvedInputs.Contains(moveFrom) || !approvedInputs.Contains(moveTo) || !allowedMove)
                {
                    moveFrom = 0;
                    Console.Write("Move from (exit to exit): ");

                    string userInput = Console.ReadLine().ToLower(); 
                    if (userInput == "exit") // Exit game if user types in "exit".
                    {
                        System.Environment.Exit(1);
                    }

                    while (!int.TryParse(userInput, out moveFrom) || !approvedInputs.Contains(moveFrom))
                    {
                        Console.Write("That was not a valid input, try again. Move from: ");
                    }
                    moveTo = 0;
                    Console.Write("Move to: ");
                    while (!int.TryParse(Console.ReadLine(), out moveTo) || !approvedInputs.Contains(moveTo))
                    {
                        Console.Write("That was not a valid input, try again. Move to: ");
                    }
                    allowedMove = true;
                }
                Console.Clear();
                MoveDisc(moveFrom, moveTo);
                numberOfMoves++;
            }
        }


        static void MoveDisc(int from, int to)
        {
            string[] fromArray = pillarList[from - 1];
            string[] toArray = pillarList[to - 1];

            if (toArray[toArray.Length - 1] != " " || from == to) // Check that recieving stack is not full or that "from" is not equal to "to" and if so, set top values to 0.
            {
                topToValue = 0;
                topFromValue = 0;
            }
            else
            {
                if (fromArray[0] == " ") // Check that giving stack is not empty and if it is, set top values to 0.
                {
                    topFromValue = 0;
                    topToValue = 0;
                }
                else
                {
                    foreach (var item in fromArray) // Get the last number in the from-array.
                    {
                        if (item != " ")
                        {
                            topFromValue = int.Parse(item);
                        }
                    }
                    foreach (var item in toArray) // Get the last number in the to-array.
                    {
                        if (item != " ")
                        {
                            topToValue = int.Parse(item);
                        }
                    }
                    if (toArray[0] == " ") // If recieving stack is empty, set topValue to 100.
                    {
                        topToValue = 100;
                    }
                }
            }

            if (topFromValue < topToValue) // If the disc in the from-pillar is smaller than the one in the to-pillar, perform move.
            {
                int fromIndex = Array.IndexOf(fromArray, topFromValue.ToString());
                int toIndex;
                try
                {
                    toIndex = Array.IndexOf(toArray, topToValue.ToString());
                    if (topToValue == 100)
                    {
                        toIndex = 0;
                    }
                    else
                    {
                        toIndex = toIndex + 1;
                    }
                }
                catch
                {
                    toIndex = 0;
                }

                fromArray[fromIndex] = " ";
                toArray[toIndex] = topFromValue.ToString();
                pillarList[from - 1] = fromArray;
                pillarList[to - 1] = toArray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("That was not a valid move. Try again.");
                Console.ResetColor();
            }
        }


        static void DrawPillars()
        {
            string[] fromArr = pillarList[0];
            string[] spareArr = pillarList[1];
            string[] toArr = pillarList[2];

            for (int i = 0; i < towerHeight; i++)
            {
                try
                {
                    Console.Write($"|{fromArr[towerHeight - i - 1]}|   ");
                }
                catch
                {
                    Console.Write($"| |   ");
                }
                try
                {
                    Console.Write($"|{spareArr[towerHeight - i - 1]}|   ");
                }
                catch
                {
                    Console.Write($"| |   ");
                }
                try
                {
                    Console.WriteLine($"|{toArr[towerHeight - i - 1]}|   ");
                }
                catch
                {
                    Console.WriteLine($"| |   ");
                }
            }
            Console.WriteLine("_______________");
            Console.WriteLine("From  Spare  To");
            Console.WriteLine(" 1      2     3");
        }
    }
}
