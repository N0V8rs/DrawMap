using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayableTextRPG
{
    internal class Program
    {
        //Player
        static ConsoleKeyInfo playerMovement;
        static int playerPosX; // Position of Player X
        static int playerPosY; // Position of Player Y
        static int maxPlayerHP; // Player Health can't go pass
        static int playerHP; // Player Health
        static int playerDamage; // Player Damage
        //Enemy
        static int enemyPosX; // Position X
        static int enemyPosY; // Position Y
        static int maxEnemyHP; // Health can't go pass
        static int enemyHP; // Health of the enemy
        static int enemyDamage; // Enemy Damage
        static bool enemyAlive; // Check to see if the enemy is alive

        // Map
        static string path; // Path for the Map
        static string RPGMap = @"TextFile1.txt"; // Text File Name of the Map
        static string[] floorMap; // Map Draw
        static char[,] mapLayout; // Map Gentrated
        static int Diamonds; // Pickups
        static bool youWin; // You Win Check
        static bool gameOver; // Game Over Check
        static bool FloorComplete; // Floor Complete Check
        static int mapX; // Map Layout X
        static int mapY; // Map Layout Y
        static int maxX; // Map Maximun
        static int maxY; // Map Maximun

        static void Main(string[] args)
        {
            OnStart();
            Console.WriteLine("playing ");
            Console.WriteLine("\n");
            Console.WriteLine("Get as much Diamonds around the map");
            Console.WriteLine("Watch out for the enemies in the map trying to kill you");
            Console.WriteLine("\n");
            Console.WriteLine("To attack back either run into them or press space near them");
            Console.WriteLine("Best of luck to you and watch out for the camo... enemies");
            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);
            Console.Clear();

            while (gameOver != true)
            {

                DrawMap();
                DisplayHUD();
                DisplayLegend();
                PlayerInput();
                EnemyMovement();

            }
            Console.Clear();
            if (youWin == true)
            {
                Console.WriteLine("YOU WIN!");
                Console.WriteLine("There's always more Diamonds out there to raid");
                Console.WriteLine("\n");
                Console.WriteLine($"You collected : {Diamonds} Diamonds!");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("You Died");
                Console.WriteLine("Try better");
                Console.ReadKey(true);
            }
        }

        static void DisplayHUD()
        {
            Console.SetCursorPosition(0, mapY + 1);
            Console.WriteLine($"Health: {playerHP}/{maxPlayerHP} | Diamonds Raided: {Diamonds} | Enemy Health: {enemyHP}/{maxEnemyHP}");
        }

        static void DisplayLegend()
        {
            Console.SetCursorPosition(0, mapY + 2);
            Console.WriteLine("Player = !" + "\n" + "Enemy 1 = E" + "\n" + "Walls = #" + "\n" + "Floor = -" + "\n" + "Diamonds = &" + "\n" + "SpikeTrap = ^  Door: %");
        }

        static void OnStart()
        {
            maxPlayerHP = 6;
            maxEnemyHP = 3;
            playerHP = maxPlayerHP;
            enemyHP = maxEnemyHP;

            playerDamage = 1;
            enemyDamage = 1;
            EnemyAlive();

            Diamonds = 0;

            gameOver = false;
            FloorComplete = false;

            path = RPGMap;
            floorMap = File.ReadAllLines(path);
            mapLayout = new char[floorMap.Length, floorMap[0].Length];
            CreateMap();
            mapX = mapLayout.GetLength(1);
            mapY = mapLayout.GetLength(0);
            maxX = mapX - 1;
            maxY = mapY - 1;
        }
        static void CreateMap()
        {
            for (int i = 0; i < floorMap.Length; i++)
            {
                for (int j = 0; j < floorMap[i].Length; j++)
                {
                    mapLayout[i, j] = floorMap[i][j];
                }
            }
        }
        static void DrawMap()
        {
            Console.Clear();

            for (int k = 0; k < mapY; k++)
            {
                for (int l = 0; l < mapX; l++)
                {
                    char tile = mapLayout[k, l];

                    if (tile == '=' && FloorComplete == false)
                    {
                        playerPosX = l;
                        playerPosY = k -1;
                        FloorComplete = true;
                        mapLayout[k, l] = '#';
                    }

                    if (tile == 'B' && FloorComplete == false)
                    {
                        if (tile == 'B')
                        {
                            enemyPosX = l;
                            enemyPosY = k;
                        }
                    }
                    Console.Write(tile);
                }
                Console.WriteLine();
            }
            PlayerPosition();
            EnemyPosition();
            Console.SetCursorPosition(0, 0);
        }
        static void PlayerPosition()
        {
            Console.SetCursorPosition(playerPosX, playerPosY);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("+");
            Console.ResetColor();
        }

        static void PlayerInput()
        {
            bool moved = false; // Checks to see if the player moves

            int movementX;
            int movementY;

            int newPlayerPositionX = playerPosX;
            int newPlayerPositionY = playerPosY;

            moved = false;

            playerMovement = Console.ReadKey(true);

            if (moved == false)
            {
                // Attack Key for the player
                if (playerMovement.Key == ConsoleKey.Spacebar)
                {
                    AttackEnemy();
                    moved = true;
                    return;
                }
            }

            // Up movement for the player
            if (playerMovement.Key == ConsoleKey.UpArrow || playerMovement.Key == ConsoleKey.W)
            {
                movementY = Math.Max(playerPosY - 1, 0);

                if (mapLayout[movementY, playerPosX] != '#')
                {
                    moved = true;
                    playerPosY = movementY;
                    if (playerPosY <= 0)
                    {
                        playerPosY = 0;
                    }

                    if (movementY == enemyPosY && playerPosX == enemyPosX)
                    {
                        enemyHP -= 1;
                        if (enemyHP <= 0)
                        {
                            enemyPosX = 0;
                            enemyPosY = 0;
                            enemyAlive = false;
                        }
                    }

                    if (mapLayout[playerPosY, playerPosX] == '^')
                    {
                        playerHP -= 1;
                        if (playerHP <= 0)
                        {
                            gameOver = true;
                        }
                    }

                    if (mapLayout[playerPosY, playerPosX] == 'B')
                    {
                        movementY = playerPosY;
                        playerPosY = movementY;
                        return;
                    }
                }
            }

            // Down movement for the player
            if (playerMovement.Key == ConsoleKey.DownArrow || playerMovement.Key == ConsoleKey.S)
            {
                movementY = Math.Min(playerPosY + 1, maxY);

                if (mapLayout[movementY, playerPosX] != '#')
                {
                    moved = true;
                    playerPosY = movementY;
                    if (playerPosY >= maxY)
                    {
                        playerPosY = maxY;
                    }

                    if (movementY == enemyPosY && playerPosX == enemyPosX)
                    {
                        enemyHP -= 1;
                        if (enemyHP <= 0)
                        {
                            enemyPosX = 0;
                            enemyPosY = 0;
                            enemyAlive = false;
                        }
                    }

                    if (mapLayout[movementY, playerPosX] == '^')
                    {
                        playerHP -= 1;
                        if (playerHP <= 0)
                        {
                            gameOver = true;
                        }
                    }

                    if (mapLayout[playerPosY, playerPosX] == 'B')
                    {
                        movementY = playerPosY;
                        playerPosY = movementY;
                        return;
                    }
                }
            }

            // Left movement for the player
            if (playerMovement.Key == ConsoleKey.LeftArrow || playerMovement.Key == ConsoleKey.A)
            {
                movementX = Math.Max(playerPosX - 1, 0);

                if (mapLayout[playerPosY, movementX] != '#')
                {
                    moved = true;
                    playerPosX = movementX;
                    if (playerPosX <= 0)
                    {
                        playerPosX = 0;
                    }

                    if (movementX == enemyPosX && playerPosY == enemyPosY)
                    {
                        enemyHP -= 1;
                        if (enemyHP <= 0)
                        {
                            enemyPosX = 0;
                            enemyPosY = 0;
                            enemyAlive = false;
                        }
                    }

                    if (mapLayout[playerPosY, playerPosX] == '^')
                    {
                        playerHP -= 1;
                        if (playerHP <= 0)
                        {
                            gameOver = true;
                        }
                    }

                    if (mapLayout[playerPosY, playerPosX] == 'B')
                    {
                        movementX = playerPosX;
                        playerPosX = movementX;
                        return;
                    }
                }
            }

            // Right movement for the player
            if (playerMovement.Key == ConsoleKey.RightArrow || playerMovement.Key == ConsoleKey.D)
            {
                movementX = Math.Min(playerPosX + 1, maxX);

                if (mapLayout[playerPosY, movementX] != '#')
                {
                    moved = true;
                    playerPosX = movementX;
                    if (playerPosX >= maxX)
                    {
                        playerPosX = maxX;
                    }

                    if (movementX == enemyPosX && playerPosY == enemyPosY)
                    {
                        enemyHP -= 1;
                        if (enemyHP <= 0)
                        {
                            enemyPosX = 0;
                            enemyPosY = 0;
                            enemyAlive = false;
                        }
                    }

                    if (mapLayout[playerPosY, playerPosX] == '^')
                    {
                        playerHP -= 1;
                        if (playerHP <= 0)
                        {
                            gameOver = true;
                        }
                    }

                    if (mapLayout[playerPosY, playerPosX] == 'B')
                    {
                        movementX = playerPosX;
                        playerPosX = movementX;
                        return;
                    }
                }
            }

            // Exit of the level
            if (mapLayout[playerPosY, playerPosX] == 'X')
            {
                youWin = true;
                gameOver = true;
            }

            // Collectible Diamonds
            if (mapLayout[playerPosY, playerPosX] == '@')
            {
                Diamonds += 1;
                mapLayout[playerPosY, playerPosX] = '-';
            }

            // Exit game using a key
            if (playerMovement.Key == ConsoleKey.Escape)
            {
                Environment.Exit(1);
            }
        }

            static void EnemyMovement()
            {
                int enemyMovementX = enemyPosX;
                int enemyMovementY = enemyPosY;
                int newEnemyPositionX = enemyPosX;
                int newEnemyPositionY = enemyPosY;

                // random roll to move
                Random randomRoll = new Random();

                // enemy will have 1 of 4 options to move
                int rollResult = randomRoll.Next(1, 5);

                while ((enemyMovementX == playerPosX && enemyMovementY == playerPosY) ||
                (enemyMovementX == newEnemyPositionX && enemyMovementY == newEnemyPositionY))
                {

                    rollResult = randomRoll.Next(1, 5); // Retry if the position is the same as the player
                    if (rollResult == 1)
                    {
                       enemyMovementY = enemyPosY + 1;
                       if (enemyMovementY >= maxY)
                       {
                           enemyMovementY = maxY;
                       }
                    }
                    else if (rollResult == 2)
                    {
                        enemyMovementY = enemyPosY - 1;
                        if (enemyMovementY <= 0)
                        {
                            enemyMovementY = 0;
                        }
                    }
                    else if (rollResult == 3)
                    {
                        enemyMovementX = enemyPosX - 1;
                        if (enemyMovementX <= 0)
                        {
                            enemyMovementX = 0;
                        }
                    }
                    else // The 4 move the enemy can move
                    {
                        enemyMovementX = enemyPosX + 1;
                        if (enemyMovementX >= maxX)
                        {
                            enemyMovementX = maxX;
                        }
                    }
                }
                // Check for collisions and update the enemy position
                if (mapLayout[enemyMovementY, enemyMovementX] != '#')
                {
                    mapLayout[enemyPosY, enemyPosX] = '-';
                    enemyPosX = enemyMovementX;
                    enemyPosY = enemyMovementY;
                }
                // Checks to see if the enemy is in the attack position
                if (enemyMovementX == newEnemyPositionX && enemyMovementY == newEnemyPositionY)
                {
                    playerHP -= 1;
                    if (playerHP <= 0)
                    {
                        gameOver = true;
                    }
                }
            }
        static void AttackEnemy()
        {

            if (Math.Abs(playerPosX - enemyPosX) <= 1 && Math.Abs(playerPosY - enemyPosY) <= 1)
            {
                enemyHP -= 1;
                if (enemyHP <= 0)
                {
                    enemyPosX = 0;
                    enemyPosY = 0;
                    enemyAlive = false;
                }
            }
        }
        static void EnemyPosition()
        {
            Console.SetCursorPosition(enemyPosX, enemyPosY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("B");
            Console.ResetColor();
        }

        // Checking for enemies 
        static void EnemyAlive()
        {
            enemyAlive = true;
        }
    }
}



