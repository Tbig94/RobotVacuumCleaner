using System;
using System.Threading;

namespace RobotVacuumCleaner
{
    internal class Program
    {
        static Robot robot = new Robot();

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

            MoveRobotToTopLeft(room);

            Cleaning(room);

            Console.ReadKey();
        }

        private static void Cleaning(char[,] room)
        {
            // start the cleaning
            while ((robot.Row < roomInWallVer && robot.Column < roomInWallHor) || (robot.Row > 0 && robot.Column > 0))
            {
                // if the robot goes to right direction
                if (direction == Directions.Right)
                {
                    // while the next coordinate (robotPost[1] + 1) is not "X"
                    while (!room[robot.Row, robot.Column + 1].Equals((char)RoomObject.Obstacle))
                    {
                        // current position -> "1" | next post -> "R"
                        room[robot.Row, robot.Column] = (char)RoomObject.Clean;
                        room[robot.Row, robot.Column + 1] = (char)RoomObject.Robot;
                        robot.Column++;
                        Console.WriteLine("--- move right ---");
                        DrawRoom(room);
                    }

                    MoveRobotDown();
                }
                // if the robot goes to left direction
                else if (direction == Directions.Left)
                {
                    // while the next coordinate (robotPost[1] - 1) is not "X"
                    while (!room[robot.Row, robot.Column - 1].Equals((char)RoomObject.Obstacle))
                    {
                        // current position -> "1" | next post -> "R"
                        room[robot.Row, robot.Column] = (char)RoomObject.Clean;
                        room[robot.Row, robot.Column - 1] = (char)RoomObject.Robot;
                        robot.Column--;
                        Console.WriteLine("--- move left ---");
                        DrawRoom(room);
                    }

                    MoveRobotDown();
                }
            }
        }

        private static void MoveRobotDown()
        {
            // if room[robotPost[0] + 1, robotPost[1] != "X" | robotPost[0] -> + 1 | direction -> left
            if (!room[robot.Row + 1, robot.Column].Equals((char)RoomObject.Obstacle))
            {
                room[robot.Row, robot.Column] = (char)RoomObject.Clean;
                room[robot.Row + 1, robot.Column] = (char)RoomObject.Robot;
                robot.Row++;

                if (direction == Directions.Left)
                {
                    direction = Directions.Right;
                }
                else
                {
                    direction = Directions.Left;
                }
                Console.WriteLine("--- move down ---");
                DrawRoom(room);
            }
        }

        private static void MoveRobotToTopLeft(char[,] room)
        {
            // move the robot to the left wall
            while (!((room[robot.Row, robot.Column - 1]).Equals((char)RoomObject.Obstacle)))
            {
                // current position -> "1" | next post -> "R"
                room[robot.Row, robot.Column] = (char)RoomObject.Clean;
                room[robot.Row, robot.Column - 1] = (char)RoomObject.Robot;
                robot.Column--;
                Console.WriteLine("--- move left ---");
                DrawRoom(room);
            }

            // move the robot to the top wall
            while (!((room[robot.Row - 1, robot.Column]).Equals((char)RoomObject.Obstacle)))
            {
                // current position -> "1" | next post -> "R"
                room[robot.Row, robot.Column] = (char)RoomObject.Clean;
                room[robot.Row - 1, robot.Column] = (char)RoomObject.Robot;
                robot.Row--;
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
                        robot.Row = row;
                        robot.Column = column;
                    }
                }
            }
            Console.WriteLine($"Robot position: [{robot.Row}, {robot.Column}]");
            return robotPos;
        }
    }
}
