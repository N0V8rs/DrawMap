using System;
using System.IO;

namespace DrawMap
{
    internal class Program
    {
        static string path = @"TextFile1.txt"; // Map Name
        static char[,] dungeonMap; // Map Arrays

        static int playerX;
        static int playerY;
        static char player = ((char)248);

        static void Main(string[] args)
        {
            LoadMap();
            DrawMap();
            while (true)
            {
                PlayerCharacter();
            }
        }

        static void LoadMap()
        {
            string[] floorMap = File.ReadAllLines(path);

            // Assuming all lines have the same length
            int mapY = floorMap.Length;
            int mapX = floorMap[0].Length;

            dungeonMap = new char[mapY, mapX];

            for (int y = 0; y < mapY; y++)
            {
                for (int x = 0; x < mapX; x++)
                {
                    playerX = x -35;
                    playerY = y -1;
                    dungeonMap[y, x] = floorMap[y][x];
                }
            }

            SetPlayerPosition();
            Console.SetCursorPosition(playerX, playerY); // Move this line to SetPlayerPosition
        }

        static void DrawMap()
        {
            // Draws the map of the current level
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < dungeonMap.GetLength(0); y++)
            {
                for (int x = 0; x < dungeonMap.GetLength(1); x++)
                {
                    char tile = dungeonMap[y, x];
                    Console.Write(tile);
                }
                Console.WriteLine();
            }
        }

        static void PlayerCharacter()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Player position after movement
            int newPlayerX = playerX;
            int newPlayerY = playerY;

            // Player Controls
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    newPlayerY = playerY - 1;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    newPlayerY = playerY + 1;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    newPlayerX = playerX - 1;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    newPlayerX = playerX + 1;
                    break;
            }

            // Check if the new position is valid (within the map boundaries and not a wall)
            if (IsValidMove(newPlayerX, newPlayerY))
            {
                // Erase the player from the current position
                Console.SetCursorPosition(playerX, playerY);
                Console.Write(dungeonMap[playerY, playerX]);

                // Update player position
                playerX = newPlayerX;
                playerY = newPlayerY;

                // Draw the player at the new position
                SetPlayerPosition();
            }
        }

        static bool IsValidMove(int x, int y)
        {
            // Check if the new position is within the map boundaries
            if (x < 0 || x >= dungeonMap.GetLength(1) || y < 0 || y >= dungeonMap.GetLength(0))
            {
                return false;
            }

            // Check if the new position is not a wall (assuming walls are represented by '#')
            return dungeonMap[y, x] != '#';
        }

        static void SetPlayerPosition()
        {
            Console.SetCursorPosition(playerX, playerY);
            DrawPlayer();
        }

        static void DrawPlayer()
        {
            // Used to draw the player
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(player);
            Console.ResetColor();
        }
    }
}


