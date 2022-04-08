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
        static char[,] room;
        static int roomInnerRow;
        static int roomInnerColumn;

        static void Main(string[] args)
        {
            GenerateRoom();

            Console.WriteLine("Press any key to start cleaning...");
            Console.ReadKey();

            DrawRoom();

            FindRobotPos();

            MoveRobotToTopLeft();

            CleanRoom();

            Console.ReadKey();
        }

        private static void GenerateRoom()
        {
            Random rnd = new Random();
            int rowRand = rnd.Next(5, 9);
            int colRand = rnd.Next(5, 9);
            room = new char[rowRand, colRand];

            roomInnerRow = room.GetLength(0) - 2;
            roomInnerColumn = room.GetLength(1) - 2;

            for (int row = 0; row < room.GetLength(0); row++)
            {
                for (int column = 0; column < room.GetLength(1); column++)
                {
                    if (row == 0 || column == 0 || row == room.GetLength(0) - 1 || column == room.GetLength(1) - 1)
                    {
                        room[row, column] = (char)RoomObject.Obstacle;
                    }
                    else
                    {
                        room[row, column] = (char)RoomObject.Dirty;
                    }
                }
            }

            int robotRowRand = rnd.Next(1, roomInnerRow);
            int robotColRand = rnd.Next(1, roomInnerColumn);
            room[robotRowRand, robotColRand] = (char)RoomObject.Robot;

            Console.WriteLine($"room row: {rowRand} | room column: {colRand} | robot pos: [{robotRowRand}, {robotColRand}]");
            Console.ReadKey();

            DrawRoom();
        }

        private static void CleanRoom()
        {
            // start the cleaning
            while ((robot.Row < roomInnerRow && robot.Column < roomInnerColumn) || (robot.Row > 0 && robot.Column > 0))
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
                        DrawRoom();
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
                        DrawRoom();
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
                DrawRoom();
            }
        }

        private static void MoveRobotToTopLeft()
        {
            // move the robot to the left wall
            while (!((room[robot.Row, robot.Column - 1]).Equals((char)RoomObject.Obstacle)))
            {
                // current position -> "1" | next post -> "R"
                room[robot.Row, robot.Column] = (char)RoomObject.Clean;
                room[robot.Row, robot.Column - 1] = (char)RoomObject.Robot;
                robot.Column--;
                Console.WriteLine("--- move left ---");
                DrawRoom();
            }

            // move the robot to the top wall
            while (!((room[robot.Row - 1, robot.Column]).Equals((char)RoomObject.Obstacle)))
            {
                // current position -> "1" | next post -> "R"
                room[robot.Row, robot.Column] = (char)RoomObject.Clean;
                room[robot.Row - 1, robot.Column] = (char)RoomObject.Robot;
                robot.Row--;
                Console.WriteLine("--- move up ---");
                DrawRoom();
            }
        }

        private static void DrawRoom()
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

        private static void FindRobotPos()
        {
            // saving robot coordinates to array
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
        }
    }
}
