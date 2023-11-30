using Microsoft.SqlServer.Server;
using System;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Security.Cryptography;

namespace DrawMap
{
    internal class Program
    {
        //Player
        static int maxPlayerHP;
        static int playerHP;
        static int playerDamage;
        static int playerPosX;
        static int playerPosY;
        static ConsoleKeyInfo playerMoveSet;

        //Enemies
        static int maxEnemy1HP;
        static int MaxEnemy2HP;
        static int enemy1HP;
        static int enemy2HP;
        static int enemy1Damage;
        static int enemy2Damage;
        static int enemy1PosX;
        static int enemy1PosY;
        static int enemy2PosX;
        static int enemy2PosY;
        static bool enemy1Alive;

        //Map
        static string path;
        static string Map = @"TextFile1.txt";
        static string[] MapArea;
        static char[,] MapLayout;
        static bool gameOver;
        static bool levelComplete;
        static int Diamonds;
        static int mapX;
        static int mapY;
        static int maxX;
        static int maxY;

        static void Main(string[] args)
        {

        }

        static void SartUp()
        {
            //Health
            maxPlayerHP = 10;
            playerHP = maxPlayerHP;
            maxEnemy1HP = 3;
            enemy1HP = maxEnemy1HP;

            //Damage
            playerDamage = 1;
            enemy1Damage = 1;
            enemy2Damage = 1;
            //Game Systems
            gameOver = false;
            levelComplete = false;
            Diamonds = 0;
            //Map Load up
            path = Map;
            MapArea = File.ReadAllLines(path);
            MapLayout = new char[MapArea.Length, MapArea[0].Length];
            // DrawMap method

            maxX = mapX - 1; maxY = mapY - 1;
        }

        static void DrawMap()
        {
            for (int a = 0; a < MapArea.Length; a++)
            {
                for (int b = 0; b < MapArea[a].Length; b++)
                {
                    MapLayout[a,b] = MapArea[a][b];
                }
            }
        }

        static void MapCreated()
        {
            Console.Clear();

            for (int y = 0; y < mapY; y++)
            {
                for (int x = 0; x < mapX; x++)
                {
                    char tile = MapLayout[y,x];

                    if (tile == '+' && levelComplete == false)
                    {
                        playerPosX = x;
                        playerPosY = y;
                        levelComplete = true;
                        MapLayout[y,x] = '#';
                    }
                    //'E' = Enemy 1
                    if (tile == 'E' && levelComplete == false)
                    {
                        if (tile == 'E')
                        {
                            enemy1PosX = x;
                            enemy1PosY = y;
                        }
                    }
                    Console.Write(tile);
                }
                Console.WriteLine();
            }
            //playerpostion
            //enemyposition
            Console.SetCursorPosition(0, 0);
        }

        static void PlayerPos()
        {
            Console.SetCursorPosition(playerPosX, playerPosY);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("!");
            Console.ResetColor();
        }

        static void PlayerMovement()
        {
            bool movement = false;
            int playerMoveX = playerPosX;
            int playerMoveY = playerPosY;
            int newPlayerPostX = 0;
            int newPlayerPostY = 0;
            movement = false;

            playerMoveSet = Console.ReadKey(true);

            if (movement == false)
            {
                if (playerMoveSet.Key == ConsoleKey.Spacebar)
                {
                    //attackEnemy method
                    movement = true; // Doesn't move when player attacks
                    return;
                }
            }

            if (playerMoveSet.Key != ConsoleKey.W || playerMoveSet.Key == ConsoleKey.UpArrow) 
            {
                playerMoveY = Math.Max(playerMoveY -1, 0);

                if (playerMoveY < 0)
                {
                    playerMoveY = 0;
                }
                if (playerMoveY == enemy1PosY && playerMoveX == enemy1PosX)
                {
                    enemy1HP -= 1;
                    if (enemy1HP <= 0)
                    {
                        enemy1PosX = 0;
                        enemy1PosY = 0;
                        enemy1Alive = false;
                    }
                    return;
                }

                if (MapLayout[playerMoveY, playerPosY] == '^')
                {
                    playerHP -= -1;
                    if ((playerHP <= 0))
                    {
                        gameOver = true;
                    }
                }
            }

        }
    }
}


