using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        static int enemy2PosX; // Position X for enemy 2
        static int enemy2PosY; // Positoon Y for enemy 2
        static int maxEnemyHP; // Health can't go pass
        static int maxEnemy2HP; // Health can't go pass for Enemy 2
        static int enemyHP; // Health of the enemy
        static int enemy2HP; // Health of the enemy 2
        static int enemyDamage; // Enemy Damage
        static int enemy2Damage; // Damage of Enemy 2 
        static bool enemyAlive; // Check to see if the enemy is alive
        static bool enemy2Alive; // Check to see if the enemy 2 is alive

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
            Console.WriteLine("Best of luck to you and watch out for the camo.f.... enemies");
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
                MoveEnemy2();

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
            Console.WriteLine($"||Health: {playerHP}/{maxPlayerHP} | Diamonds Raided: {Diamonds} | Enemy Health: {enemyHP}/{maxEnemyHP} | Enemy 2 Health: {enemy2HP}/{maxEnemy2HP}||");
        }

        static void DisplayLegend()
        {
            Console.SetCursorPosition(0, mapY + 3);
            Console.WriteLine("Map Legend");
            Console.SetCursorPosition(0, mapY + 4);
            Console.WriteLine("|| Player = + " + " Enemy 1 = B " + " Enemy 2 = ?" + "\n" + "|| Walls = #" + "\n" + "|| Floor = -" + "\n" + "|| Diamonds = @" + "\n" + "SpikeTrap = ^" + "\n" +  "Door = X");
        }

        static void OnStart()
        {
            maxPlayerHP = 10;
            maxEnemyHP = 3;
            playerHP = maxPlayerHP;
            enemyHP = maxEnemyHP;

            playerDamage = 1;
            enemyDamage = 1;
            EnemyAlive();
            
            maxEnemy2HP = 1;
            enemy2HP = maxEnemy2HP;
            enemy2Damage = 2;
            EnemyAlive(); // Testing this for enemy 2 might have to make a second method 


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
                        playerPosY = k - 1;
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
                    if (tile == '?' && FloorComplete == false)
                    {
                        if (tile == '?')
                        {
                            enemy2PosX = l;
                            enemy2PosY = k;
                        }
                    }
                    if (tile == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    // Set color for floor '-'
                    else if (tile == '-')
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    // Set color for diamonds '@'
                    else if (tile == '@')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (tile == '^')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    // Reset color for other characters
                    else
                    {
                        Console.ResetColor();
                    }
                    Console.Write(tile);
                }
                Console.WriteLine();
            }
            PlayerPosition();
            Enemy1Position();
            Enemy2Position(); // Add this line to update the position of Enemy 2
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
                    AttackEnemy2();
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

                    if (mapLayout[playerPosY, playerPosX] == '?')
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

                    if (mapLayout[playerPosY, playerPosX] == '?')
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

                    if (mapLayout[playerPosY, playerPosX] == '?')
                    {
                        movementX = playerPosY;
                        playerPosY = movementX;
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

                    if (mapLayout[playerPosY, playerPosX] == '?')
                    {
                        movementX = playerPosY;
                        playerPosY = movementX;
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

            // random roll to move
            Random randomRoll = new Random();

            // enemy will have 1 of 4 options to move
            int rollResult = randomRoll.Next(1, 5);

            int newEnemyPositionX = enemyMovementX;
            int newEnemyPositionY = enemyMovementY;

            // Retry movement if the position is the same as the player or the new position
            while ((enemyMovementX == playerPosX && enemyMovementY == playerPosY) || (enemyMovementX == newEnemyPositionX && enemyMovementY == newEnemyPositionY))
            {
                rollResult = randomRoll.Next(1, 5);
                if (rollResult == 1)
                {
                    enemyMovementY = Math.Min(enemyPosY + 1, maxY);
                }
                else if (rollResult == 2)
                {
                    enemyMovementY = Math.Max(enemyPosY - 1, 0);
                }
                else if (rollResult == 3)
                {
                    enemyMovementX = Math.Max(enemyPosX - 1, 0);
                }
                else // The 4 move the enemy can move
                {
                    enemyMovementX = Math.Min(enemyPosX + 1, maxX);
                }
            }

            // Check for collisions and update the enemy position
            if (mapLayout[enemyMovementY, enemyMovementX] != '#' && mapLayout[enemyMovementY, enemyMovementX] != '^' && mapLayout[enemyMovementY, enemyMovementX] != 'X')
            {
                // Reset the old position
                mapLayout[newEnemyPositionY, newEnemyPositionX] = '-';
                // Check if the enemy is still alive before updating the new position
                if (enemyAlive)
                {
                    mapLayout[enemyMovementY, enemyMovementX] = '-'; // Set the enemy symbol
                    enemyPosX = enemyMovementX;
                    enemyPosY = enemyMovementY;
                    // Update the new position
                    newEnemyPositionX = enemyPosX;
                    newEnemyPositionY = enemyPosY;
                }
            }

            // Checks to see if the enemy is in the attack position
            if (Math.Abs(enemyPosX - playerPosX) <= 1 && Math.Abs(enemyPosY - playerPosY) <= 1)
            {
                playerHP -= 1;
                if (playerHP <= 0)
                {
                    gameOver = true;
                }
            }
        }

        static void MoveEnemy2()
        {
            if (enemy2Alive)
            {
                int playerDistanceX = Math.Abs(playerPosX - enemy2PosX);
                int playerDistanceY = Math.Abs(playerPosY - enemy2PosY);
                int enemyMovement2X = enemy2PosX;
                int enemyMovement2Y = enemy2PosY;
                int newEnemyPosition2X = enemyMovement2X;
                int newEnemyPosition2Y = enemyMovement2Y;

                // Checks to see if the player is near
                if (playerDistanceX <= 2 && playerDistanceY <= 2)
                {
                    // Moves towards the player
                    if (playerPosX < enemy2PosX && mapLayout[enemy2PosY, enemy2PosX - 1] != '#')
                    {
                        enemyMovement2X--;
                    }
                    else if (playerPosX > enemy2PosX && mapLayout[enemy2PosY, enemy2PosX + 1] != '#')
                    {
                        enemyMovement2X++;
                    }

                    if (playerPosY < enemy2PosY && mapLayout[enemy2PosY - 1, enemy2PosX] != '#')
                    {
                        enemyMovement2Y--;
                    }
                    else if (playerPosY > enemy2PosY && mapLayout[enemy2PosY + 1, enemy2PosX] != '#')
                    {
                        enemyMovement2Y++;
                    }
                }

                // Makes the enemy not move if the player is on the same position
                if ((enemyMovement2X != playerPosX || enemyMovement2Y != playerPosY) &&
                    mapLayout[enemyMovement2Y, enemyMovement2X] != '#')
                {
                    // Reset the old position
                    mapLayout[newEnemyPosition2Y, newEnemyPosition2X] = '-';
                    // Check if the enemy is still alive before updating the new position
                    if (enemy2Alive)
                    {
                        mapLayout[enemyMovement2Y, enemyMovement2X] = '-';
                        enemy2PosX = enemyMovement2X;
                        enemy2PosY = enemyMovement2Y;
                        newEnemyPosition2X = enemy2PosX;
                        newEnemyPosition2Y = enemy2PosY;
                    }
                }
                if (Math.Abs(enemy2PosX - playerPosX) <= 1 && Math.Abs(enemy2PosY - playerPosY) <= 1)
                {
                    playerHP -= 1;
                    if (playerHP <= 0)
                    {
                        gameOver = true;
                    }
                }
            }
        }
        // Spawns Enemy 1
        static void Enemy1Position()
        {
            if (enemyAlive)
            {
                Console.SetCursorPosition(enemyPosX, enemyPosY);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("B");
                Console.ResetColor();
            }
        }
        // Spawns Enemy 2 
        static void Enemy2Position()
        {
            if (enemy2Alive)
            {
            Console.SetCursorPosition(enemy2PosX, enemy2PosY);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("?");
            Console.ResetColor();
            }
        }
        // Enmey 1 attack
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
        //Enemy 2 attack
        static void AttackEnemy2()
        {
            if (Math.Abs(playerPosX - enemy2PosX) <= 1 && Math.Abs(playerPosY - enemy2PosY) <= 1)
            {
                enemy2HP -= 1;
                if (enemy2HP <= 0)
                {
                    enemy2PosX = 0;
                    enemy2PosY = 0;
                    enemy2Alive = false;
                }
            }
        }

        // Checking for enemies 
        static void EnemyAlive()
        {
            enemyAlive = true;
            enemy2Alive = true;
        }
    }
}



