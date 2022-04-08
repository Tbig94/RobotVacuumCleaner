using System;
using System.Threading;

namespace RobotVacuumCleaner
{
    internal class Program
    {
        enum Directions
        {
            Right,
            Left
        };

        enum RoomObject
        {
            Robot = 'R',
            Obstacle = 'X',
            Dirty = '0',
            Clean = '1'
        }

        static Directions direction = Directions.Right;

        // Init empty room | i = row, j = column
        /*
         *  room must be surrounded by walls ('X')
         *  room must be empty
         *  room can be square or rectangle
         *  there must be ONE robot inside the room ('R')
         */
        static char[,] room = new char[9, 7]
            {
                { 'X', 'X', 'X', 'X', 'X', 'X', 'X' },
                { 'X', '0', '0', '0', '0', '0', 'X' },
                { 'X', '0', '0', '0', 'R', '0', 'X' },
                { 'X', '0', '0', '0', '0', '0', 'X' },
                { 'X', '0', '0', '0', '0', '0', 'X' },
                { 'X', '0', '0', '0', '0', '0', 'X' },
                { 'X', '0', '0', '0', '0', '0', 'X' },
                { 'X', '0', '0', '0', '0', '0', 'X' },
                { 'X', 'X', 'X', 'X', 'X', 'X', 'X' },
            };

        static int roomInWallVer = room.GetLength(0) -2;
        static int roomInWallHor = room.GetLength(1) - 2;

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

        private static void Cleaning(char[,] room, int[] robotPos)
        {
            // start the cleaning
            while ((robotPos[0] < roomInWallVer && robotPos[1] < roomInWallHor) || (robotPos[0] > 0 && robotPos[1] > 0))
            {
                // if the robot goes to right direction
                if (direction == Directions.Right)
                {
                    // while the next coordinate (robotPost[1] + 1) is not "X"
                    while (!room[robotPos[0], robotPos[1] + 1].Equals((char)RoomObject.Obstacle))
                    {
                        // current position -> "1" | next post -> "R"
                        room[robotPos[0], robotPos[1]] = (char)RoomObject.Clean;
                        room[robotPos[0], robotPos[1] + 1] = (char)RoomObject.Robot;
                        robotPos[1]++;
                        Console.WriteLine("--- move right ---");
                        DrawRoom(room);
                    }
                    // if room[robotPost[0] + 1, robotPost[1] != "X" | robotPost[0] -> + 1 | direction -> left
                    if (!room[robotPos[0] + 1, robotPos[1]].Equals((char)RoomObject.Obstacle))
                    {
                        room[robotPos[0], robotPos[1]] = (char)RoomObject.Clean;
                        room[robotPos[0] + 1, robotPos[1]] = (char)RoomObject.Robot;
                        robotPos[0]++;
                        direction = Directions.Left;
                        Console.WriteLine("--- move down ---");
                        DrawRoom(room);
                    }
                }
                // if the robot goes to left direction
                else if (direction == Directions.Left)
                {
                    // while the next coordinate (robotPost[1] - 1) is not "X"
                    while (!room[robotPos[0], robotPos[1] - 1].Equals((char)RoomObject.Obstacle))
                    {
                        // current position -> "1" | next post -> "R"
                        room[robotPos[0], robotPos[1]] = (char)RoomObject.Clean;
                        room[robotPos[0], robotPos[1] - 1] = (char)RoomObject.Robot;
                        robotPos[1]--;
                        Console.WriteLine("--- move left ---");
                        DrawRoom(room);
                    }
                    // if room[robotPost[0] + 1, robotPost[1] != "X" | robotPost[0] -> + 1 | direction -> left
                    if (!room[robotPos[0] + 1, robotPos[1]].Equals((char)RoomObject.Obstacle))
                    {
                        room[robotPos[0], robotPos[1]] = (char)RoomObject.Clean;
                        room[robotPos[0] + 1, robotPos[1]] = (char)RoomObject.Robot;
                        robotPos[0]++;
                        direction = Directions.Right;
                        Console.WriteLine("--- move down ---");
                        DrawRoom(room);
                    }
                }
            }
        }

        private static void MoveRobotToTopLeft(char[,] room, int[] robotPos)
        {
            // move the robot to the left wall
            while (!((room[robotPos[0], robotPos[1] - 1]).Equals((char)RoomObject.Obstacle)))
            {
                // current position -> "1" | next post -> "R"
                room[robotPos[0], robotPos[1]] = (char)RoomObject.Clean;
                room[robotPos[0], robotPos[1] - 1] = (char)RoomObject.Robot;
                robotPos[1]--;
                Console.WriteLine("--- move left ---");
                DrawRoom(room);
            }

            // move the robot to the top wall
            while (!((room[robotPos[0] - 1, robotPos[1]]).Equals((char)RoomObject.Obstacle)))
            {
                // current position -> "1" | next post -> "R"
                room[robotPos[0], robotPos[1]] = (char)RoomObject.Clean;
                room[robotPos[0] - 1, robotPos[1]] = (char)RoomObject.Robot;
                robotPos[0]--;
                Console.WriteLine("--- move up ---");
                DrawRoom(room);
            }
        }

        private static void DrawRoom(char[,] room)
        {
            Thread.Sleep(100);
            Console.Clear();
            for (int row = 0; row < room.GetLength(0); row++)
            {
                for (int column = 0; column < room.GetLength(1); column++)
                {
                    Console.Write(room[row, column] + "   ");
                }
                Console.WriteLine("\n");
            }
        }

        private static int[] FindRobotPos(char[,] room)
        {
            // saving robot coordinates to array
            int[] robotPos = new int[2];
            for (int row = 0; row < room.GetLength(0); row++)
            {
                for (int column = 0; column < room.GetLength(1); column++)
                {
                    if (room[row, column] == (char)RoomObject.Robot)
                    {
                        robotPos[0] = row;
                        robotPos[1] = column;
                    }
                }
            }
            Console.WriteLine($"Robot position: [{robotPos[0]}, {robotPos[1]}]");
            return robotPos;
        }
    }
}
