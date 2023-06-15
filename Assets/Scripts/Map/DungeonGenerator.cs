using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int dungeonWidth;
    public int dungeonHeight;

    //Matrix for dungeon
    public int[,,] dungeonMap { protected set; get; }


    void Awake()
    {
        Generate();
    }

    public void Generate()
    {
        Debug.Log("Generating...");
        dungeonMap = new int[dungeonWidth * 9 + 1, dungeonHeight * 5 + 1, 6];

        //Close dungeon
        CierraDungeon();
        //Generated Rooms
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
                GenerateRooms(x, y);
        }

        //Connected rooms
        ConnectRooms();

        //Generate all components
        //Fixed platforms
        GenerateFixedPlatforms(dungeonWidth, dungeonHeight);

        //Obstaculos
        GenerateObstacles();

        //Enemigos
        GenerateEnemies();

        //PowerUps
        GeneratePowerUps();
        Debug.Log("Generated.");
    }

    void CierraDungeon()
    {
        for(int x = 0; x < dungeonWidth * 9 + 1; x++)
        {
            dungeonMap[x, 0, 0] = 1;
            dungeonMap[x, dungeonHeight * 5, 0] = 1;
        }
        for (int y = 0; y < dungeonHeight * 5 + 1; y++)
        {
            dungeonMap[0, y, 0] = 1;
            dungeonMap[dungeonWidth * 9, y, 0] = 1;
        }
    }
    void GenerateRooms(int posX, int posY)
    {

        List<int> roomTypes = new List<int> { 2, 3, 4 }; //Type rooms list
        int randomIndex = Random.Range(0, roomTypes.Count); //Select 1 tipe
        for (int x = posX * 9 + 1; x < (posX + 1) * 9; x++)
        {
            for (int y = posY * 5 + 1; y < (posY + 1) * 5; y++)
            {
               
                dungeonMap[x, y, 0] = roomTypes[randomIndex]; //Add room type
            }
        }

        //Add floor and left
        for (int x = posX * 9; x < (posX + 1) * 9; x++)
        {
            dungeonMap[x, posY * 5, 0] = 1;
        }
        for (int y = posY * 5; y < (posY + 1) * 5; y++)
        {
            dungeonMap[posX * 9, y, 0] = 1;
        }
    }

    void ConnectRooms()
    {
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                int connections = Random.Range(1, 4); //Number of connections for 1 room(1-3)

                // Connect adjacent rooms
                while (connections > 0)
                {

                    int direction = Random.Range(0, 4); //0: top, 1: right, 2: bottom, 3: left

                    int adjX = x;
                    int adjY = y;

                    //Calculate adjacent room coordinates based on the selected direction
                    switch (direction)
                    {
                        case 0: //Top
                            adjY--;
                            break;
                        case 1: //Right
                            adjX++;
                            break;
                        case 2: //Bottom
                            adjY++;
                            break;
                        case 3: //Left
                            adjX--;
                            break;
                    }

                    if (adjX >= 0 && adjX < dungeonWidth && adjY >= 0 && adjY < dungeonHeight)
                    {


                        if (direction == 0 || direction == 2) //Vertical connection
                        {
                            //Remove walls on the top side
                            bool connect = false;
                            for(int a = 1; a < 9; a++)
                            {
                                if(dungeonMap[x * 9 + a, y * 5, 0] == 0)
                                {
                                    connect = true;
                                    
                                }
                            }
                            if (connect == false)
                            {
                                int door = Random.Range(1, 8);
                                dungeonMap[x * 9 + door, y * 5, 0] = 0;
                                dungeonMap[x * 9 + door+1, y * 5, 0] = 0;
                                GenerateMovingPlatforms(x * 9 + door, y * 5, x);
                            }
                            

                        }
                        else if (direction == 1 || direction == 3) //Horizontal connection
                        {

                            //Remove walls on the left side
                            bool connect = false;
                            for (int a = 1; a < 5; a++)
                            {
                                if (dungeonMap[adjX * 9, adjY * 5 + a, 0] == 0)
                                {
                                    connect = true;
                                }
                            }
                            if (connect == false)
                            {
                                int door = Random.Range(1, 4);
                                dungeonMap[adjX * 9, adjY * 5 + door, 0] = 0;
                                dungeonMap[adjX * 9, adjY * 5 + door+1, 0] = 0;

                            }
                        }
                        connections--;
                    }
                }            
            }
        }
        CierraDungeon();//Close area dungeon
    }

    void GenerateFixedPlatforms(int dWidth, int dHeight) //Generate Fixed Platform
    {

        //Walk the dungeon
        for (int x = 0; x < dWidth; x++)
        {
            for (int y = 0; y < dHeight; y++)
            {
                //Review if in the room don't exist any Movile platform
                bool found = false;
                for (int a = x * 9 + 1; a < x * 9 + 9 && found == false; a++)
                {
                    for (int b = y * 5 + 1; b < y * 5 + 4 && found == false; b++)
                    {
                        if (dungeonMap[a, b, 1] == 3)
                        {
                            found = true;
                        }
                    }
                }

                //If we can add platforms
                int numPlatforms = Random.Range(0, 3); //Cuantity of platform
                if (numPlatforms >= 1 && found == false)
                {
                    while (numPlatforms > 0)
                    {
                        try
                        {
                            int platformX = Random.Range(x * 9 + 1, x * 9 + 9); //X coordinate of the fixed platform
                            int platformY = Random.Range(y * 5 + 2, y * 5 + 4); //Y coordinate of the fixed platform
                            dungeonMap[platformX, platformY, 1] = 2; //Set the fixed platform
                            dungeonMap[platformX, platformY, 5] = 1;
                        }
                        catch
                        {

                        }
                        numPlatforms--;
                    }                    
                }
            }
        }

    }

    void GenerateMovingPlatforms(int posX, int posY, int x) //Generated Moving Platform
    {
        int platformX = posX; //X coordinate of the moving platform
        int platformY = posY - 1; //Y coordinate of the moving platform
        try
        {
            dungeonMap[platformX, platformY, 1] = 3; //We always set one of the points below the room connection
            int numMovementPoints = Random.Range(1, 3); // Movement points

            for (int i = 0; i < numMovementPoints; i++)
            {
                int pointX = Random.Range(x * 9 + 1, x * 9 + 8); // X coordinate of the movement point
                int pointY = Random.Range(platformY - 2, platformY - 4); // Y coordinate of the movement point

                dungeonMap[pointX, pointY, 1] = 3; //Set the movement point
                dungeonMap[pointX, pointY, 5] = 1; //Block the possition
            }
        }
        catch
        {

        }

    }

    void GenerateObstacles()
    {
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                int numObstacles = Random.Range(0, 3); //Number obstacles
                for (int i = 0; i < numObstacles; i++)
                {
                    int obstacleX = Random.Range(x * 9 + 1, x * 9 + 9); //X Possition
                    int obstacleY = Random.Range(y * 5 + 1, y * 5 + 5); //Y Possition

                    //Obstacle in top or bottom
                    int obstacleType = obstacleY < (y * 5 + 4) ? Random.Range(1, 5) : 5;
                    if(obstacleType == 5)
                    {
                        //Other object in the possition
                        if(dungeonMap[obstacleX, obstacleY, 5] != 1)
                        {
                            dungeonMap[obstacleX, obstacleY, 2] = obstacleType; //Set the obstacle
                            dungeonMap[obstacleX, obstacleY, 5] = 1; //Block the possition
                        }
                        else
                        {
                            i--;//If we cannot set the obstacle
                        }
                    }
                    else
                    {
                        //Other object in the possition
                        if (dungeonMap[obstacleX, y * 5 + 1, 5] != 1)
                        {
                            dungeonMap[obstacleX, y * 5 + 1, 2] = obstacleType; //Set the obstacle
                            dungeonMap[obstacleX, y * 5 + 1, 5] = 1; //Block the possition
                        }
                        else
                        {
                            i--;//If we cannot set the obstacle
                        }
                    }
                }
            }
        }
    }

    void GenerateEnemies()
    {
        for (int x = 1; x < dungeonWidth; x++)
        {
            for (int y = 1; y < dungeonHeight; y++)
            {
                int numEnemies = Random.Range(0, 6); //Number of enemys
                for (int i = 0; i < numEnemies; i++)
                {
                    int enemyX = Random.Range(x * 9 + 1, x * 9 + 9); //X Possition
                    int enemyY = Random.Range(y * 5 + 1, y * 5 + 5); //Y Possition

                    //Enemy position top o bottom
                    int enemyType = enemyY < (y * 5 + 4) ? Random.Range(1, 5) : Random.Range(5, 7);
                    if (enemyType >= 5)
                    {
                        //Other object in the possition
                        if (dungeonMap[enemyX, y * 5 + 4, 5] != 1)
                        {
                            dungeonMap[enemyX, y * 5 + 4, 3] = enemyType; //Set the obstacle
                            dungeonMap[enemyX, y * 5 + 4, 5] = 1; //Block the possition
                        }
                        else
                        {
                            i--;//If we cannot set the obstacle
                        }
                    }
                    else
                    {
                        //Other object in the possition
                        if (dungeonMap[enemyX, y * 5 + 1, 5] != 1)
                        {
                            dungeonMap[enemyX, y * 5 + 1, 3] = enemyType; //Set the obstacle
                            dungeonMap[enemyX, y * 5 + 1, 5] = 1; //Block the possition
                        }
                        else
                        {
                            i--;//If we cannot set the obstacle
                        }
                    }
                }
            }
        }
    }

    void GeneratePowerUps()
    {
        for (int x = 1; x < dungeonWidth; x++)
        {
            for (int y = 1; y < dungeonHeight; y++)
            {
                if (Random.value <= 0.5f) //Power up only 50% appear
                {
                    int powerUpX = Random.Range(x * 9 + 1, x * 9 + 9); //X Possition
                    int powerUpY = Random.Range(y * 5 + 1, y * 5 + 5); //Y Possition
                    if (dungeonMap[powerUpX, powerUpY, 5] != 1)
                    {
                        int powerUpType = Random.Range(0, 3); //Select powerUp
                        dungeonMap[powerUpX, powerUpY, 4] = powerUpType; //Set the Power up
                        dungeonMap[powerUpX, powerUpY, 5] = 1; //Block the possition
                    }
                    else
                    {
                        y--;//If we cannot set the obstacle
                    }
                }
            }
        }
    }

    //Change Matrix
    public void SetTileValue(int x, int y, int z, int value)
    {
        dungeonMap[x, y, z] = value;
    }

    //GetDungeonMap
    public int[,,] GetDungeonMap()
    {
        return dungeonMap;
    }
}