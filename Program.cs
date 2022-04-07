using System;
using System.Threading;

namespace RobotVacuumCleaner
{
    // R - robot vacuum cleaner
    // X - obstacle
    // 0 - dirty
    // 1 - clean
    internal class Program
    {
        static string direction = "right";

        // Init empty room | i = row, j = column
        static string[,] room = new string[8, 8]
            {
                { "X", "X", "X", "X", "X", "X", "X", "X" },
                { "X", "0", "0", "0", "0", "0", "0", "X" },
                { "X", "0", "0", "0", "0", "0", "0", "X" },
                { "X", "0", "0", "0", "0", "0", "0", "X" },
                { "X", "0", "0", "0", "R", "0", "0", "X" },
                { "X", "0", "0", "0", "0", "0", "0", "X" },
                { "X", "0", "0", "0", "0", "0", "0", "X" },
                { "X", "X", "X", "X", "X", "X", "X", "X" },
            };

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start cleaning...");
            Console.ReadKey();
            
            DrawRoom(room);

            int[] robotPos = FindRobotPos(room);

            MoveRobotToTopLeft(room, robotPos);

            Cleaning(room, robotPos);   

            Console.ReadKey();
        }

        private static void Cleaning(string[,] room, int[] robotPos)
        {
            // start the cleaning
            while ((robotPos[0] < 5 && robotPos[1] < 5) || (robotPos[0] > 0 && robotPos[1] > 0))
            {
                // if the robot goes to right direction
                if (direction == "right")
                {
                    // while the next coordinate (robotPost[1] + 1) is not "X"
                    while (room[robotPos[0], robotPos[1] + 1] != "X")
                    {
                        // current position -> "1" | next post -> "R"
                        room[robotPos[0], robotPos[1]] = "1";
                        room[robotPos[0], robotPos[1] + 1] = "R";
                        robotPos[1] = robotPos[1] + 1;
                        Console.WriteLine("--- move right ---");
                        DrawRoom(room);
                    }
                    // if room[robotPost[0] + 1, robotPost[1] != "X" | robotPost[0] -> + 1 | direction -> left
                    if (room[robotPos[0] + 1, robotPos[1]] != "X")
                    {
                        room[robotPos[0], robotPos[1]] = "1";
                        room[robotPos[0] + 1, robotPos[1]] = "R";
                        robotPos[0] = robotPos[0] + 1;
                        direction = "left";
                        Console.WriteLine("--- move down ---");
                        DrawRoom(room);
                    }
                }
                // if the robot goes to left direction
                else if (direction == "left")
                {
                    // while the next coordinate (robotPost[1] - 1) is not "X"
                    while (room[robotPos[0], robotPos[1] - 1] != "X")
                    {
                        // current position -> "1" | next post -> "R"
                        room[robotPos[0], robotPos[1]] = "1";
                        room[robotPos[0], robotPos[1] - 1] = "R";
                        robotPos[1] = robotPos[1] - 1;
                        Console.WriteLine("--- move left ---");
                        DrawRoom(room);
                    }
                    // if room[robotPost[0] + 1, robotPost[1] != "X" | robotPost[0] -> + 1 | direction -> left
                    if (room[robotPos[0] + 1, robotPos[1]] != "X")
                    {
                        room[robotPos[0], robotPos[1]] = "1";
                        room[robotPos[0] + 1, robotPos[1]] = "R";
                        robotPos[0] = robotPos[0] + 1;
                        direction = "right";
                        Console.WriteLine("--- move down ---");
                        DrawRoom(room);
                    }
                }
            }
        }

        private static void MoveRobotToTopLeft(string[,] room, int[] robotPos)
        {
            // move the robot to the left wall
            while (room[robotPos[0], robotPos[1] - 1] != "X")
            {
                // current position -> "1" | next post -> "R"
                room[robotPos[0], robotPos[1]] = "1";
                room[robotPos[0], robotPos[1] - 1] = "R";
                robotPos[1] -= 1;
                Console.WriteLine("--- move left ---");
                DrawRoom(room);
            }

            // move the robot to the top wall
            while (room[robotPos[0] - 1, robotPos[1]] != "X")
            {
                // current position -> "1" | next post -> "R"
                room[robotPos[0], robotPos[1]] = "1";
                room[robotPos[0] - 1, robotPos[1]] = "R";
                robotPos[0] -= 1;
                Console.WriteLine("--- move up ---");
                DrawRoom(room);
            }
        }

        private static void DrawRoom(string[,] room)
        {
            Thread.Sleep(500);
            Console.Clear();
            for (int i = 0; i < room.GetLength(0); i++)
            {
                for (int j = 0; j < room.GetLength(1); j++)
                {
                    Console.Write(room[i, j] + "   ");
                }
                Console.WriteLine("\n");
            }
        }

        private static int[] FindRobotPos(string[,] room)
        {
            // saving robot coordinates to array
            int[] robotPos = new int[2];
            for (int i = 0; i < room.GetLength(0); i++)
            {
                for (int j = 0; j < room.GetLength(1); j++)
                {
                    if (room[i, j] == "R")
                    {
                        robotPos[0] = i;
                        robotPos[1] = j;
                    }
                }
            }
            Console.WriteLine($"coords: [{robotPos[0]}, {robotPos[1]}]");
            return robotPos;
        }
    }
}
