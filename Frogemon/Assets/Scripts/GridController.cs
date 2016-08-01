/*
    GridController.cs
    Authors: Shane Adams, Jessica Brailey, Timothy Ayres
    Date: 27/7/2016
    Project: Academy of Interactive Entertainment - Practice Productions - Assignment
*/



using UnityEngine;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{
    const int GRID_WIDTH = 13; // The numberof cells wide the grid is
    const int GRID_HEIGHT = 17; // The number of cells High the grid is
    public Vector3[,] grid;
    int pikachuX = 6; // Pikachus current X position
    int pikachuY = 0; // Pikachus current Y position
    int pikaNewX; // New x position
    int pikaNewY; // New Y position
    bool[,] objGrid = new bool[GRID_WIDTH, GRID_HEIGHT]; //2D bool array of where obstacles are

    public GameObject pikachuObject; //Pikachu Object
    public GameObject foodObject;
    GameObject pikachu; //Pikachu Instance

    public GameObject levelController; // level controller instance
    public bool autoLevelStart = false; // flag to indicate if the level should start automatically

    public GameObject leftTrainerObject; // the trainer prefab
    public GameObject rightTrainerObject; // the trainer prefab

    public Vector3 ReturnEndPos(Vector3 position)
    {
        //int y = (int)position.y;
        //if (position.x < 5f)
        //{
        //    return grid[GRID_WIDTH - 1, y];
        //}
        //return Vector3.right;

        // just hacking this in for now to get an x and z for the right-hand side
        float x = grid[GRID_WIDTH - 1, 0].x;
        float z = grid[GRID_WIDTH - 1, 0].z;
        return new Vector3(x, position.y, z);
    }

    // Use this for initialization
    void Start()
    {
        //Create 2D Array of Vector3 & Set them
        grid = new Vector3[GRID_WIDTH, GRID_HEIGHT];

        for (int x = 0; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                // had to update this, all sprites have center pivot and needed
                // to adjust for the layout, this change will not break any other
                // objects or code
                grid[x, y] = new Vector3(x * .5f + .75f, y * .5f + .75f, ACTION_Z);
                objGrid[x, y] = false;
            }
        }

        //// LEVEL GEN ////

        // setup the background
        BuildBackground();

        // setup the obstacles
        BuildObstacles();

        // setup the endzone
        BuildEndZone();

        // setup the food
        BuildFood();

        // should the level auto start?
        if (autoLevelStart)
        {
            StartLevel();
        }

        //// LEVEL GEN ////
    }

    // called to start the level
    public void StartLevel()
    {
        // setup the characters
        BuildCharacters();
    }

    //  Return an objects actual grid position
    Vector3 ReturnPosition(int x, int y)
    {
        return grid[x, y];
    }

    //  Check if there is an object in the path
   bool CheckIfCanMove(int x, int y)
    {
        if (objGrid[x, y] == true) // If there is an object in the newcoordinates
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //  Handle Pikachus positioning
    public Vector3 ReturnNewPikaPos(string direction)
    {
        //  Set the grids local value of pikachus position to it's current position
        pikaNewX = pikachuX;
        pikaNewY = pikachuY;
        bool canMove = false;

        //  Handle repositioning based on the key pressed
        switch (direction.ToLower())
        {
            case "up":
                if(pikachuY < GRID_HEIGHT-1)
                {
                    pikaNewY++;
                    canMove = CheckIfCanMove(pikaNewX, pikaNewY);
                }               
                break;
            case "down":
                if (pikachuY > 0)
                {
                    pikaNewY--;
                    canMove = CheckIfCanMove(pikaNewX, pikaNewY);
                }
                break;
            case "left":
                if (pikachuX > 0)
                {
                    pikaNewX--;
                    canMove = CheckIfCanMove(pikaNewX, pikaNewY);
                }
                break;
            case "right":
                if (pikachuX < GRID_WIDTH-1)
                {
                    pikaNewX++;
                    canMove = CheckIfCanMove(pikaNewX, pikaNewY);
                }
                break;
        }
        // If Pikachu can move coordinates are update, otherwise they remain unchanged
        if (canMove)
        {
            pikachuX = pikaNewX;
            pikachuY = pikaNewY;
        }
        // Return Pikachu's New Position using it's new grid coords
        return grid[pikachuX, pikachuY];
    }

    // called when pikachu dies
    public void PikachuDead()
    {
        levelController.GetComponent<LevelController>().GameOver();
    }

    // called when the level is completed
    public void FoodEaten()
    {
        levelController.GetComponent<LevelController>().NextLevel();
    }



    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////
    // begin level generation stuff
    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////

    /*
     Levels roughly follow the spreadsheet on google drive and are laid out as
     follows:

        1 rows at the top for the end, with food in the center
        2 rows after that for water and the bridge
        X number of rows after those for game play area
        1 rows at the bottom for start section

     The spreadsheet shows that there are 2 groups of 3 trainers with 1 group in
     each half of the game play area, roughly centered within that half.
     */

    ////////////////////////////////////////////////////////////////
    // editor data

    // the prefab to use as a grass background
    public GameObject[] grassTiles;

    // the prefab to use as water
    public GameObject waterTile;

    // the prefab to use as the bridge
    public GameObject bridgeTile;

    // an array of obstacle prefabs to be randomly placed
    public GameObject[] obstacleTiles;

    ////////////////////////////////////////////////////////////////
    // internal data

    // the z position of background tiles
    const float BACKGROUND_Z = 0.0f;

    // the z position where the action takes place, this ensures sprites are
    // drawn in the correct order
    const float ACTION_Z = -1.0f;

    // obstacle coverage
    const float OBSTACLE_COVERAGE = .6f;

    // storage for trainer rows
    List<int> m_trainerRows = new List<int>();

    // number of trainers in each group
    const int TRAINERS_PER_GROUP = 3;

    ////////////////////////////////////////////////////////////////
    // code

    // instantiate game objects
    GameObject CreateObject(GameObject a_source, Vector3 a_pos)
    {
        // instantiated game objects are placed using world coordinates.
        // to position the generated objects relative to the parent we
        // need to create a relative position ourselves.
        Vector3 position = new Vector3(this.transform.position.x + a_pos.x, this.transform.position.y + a_pos.y, this.transform.position.z + a_pos.z);

        // instantiate the object
        GameObject go = (GameObject)Instantiate(a_source, position, this.transform.rotation);
        go.transform.parent = this.transform;

        // return the object
        return go;
    }

    // build the background
    void BuildBackground()
    {
        // grass selector
        RandomInt intGenG = new RandomInt(0, grassTiles.Length - 1);
        int grassIdx = intGenG.Value;

        for (int y = 0; y < GRID_HEIGHT; ++y)
        {
            for (int x = 0; x < GRID_WIDTH; ++x)
            {
                // instantiate the tile
                Vector3 tilePos = new Vector3(grid[x, y].x, grid[x, y].y, BACKGROUND_Z);
                CreateObject(grassTiles[grassIdx], tilePos);
            }
        }
    }

    // processes a region of obstacles and determines the best way to make a
    // path through them, if a more advanced path cannot be constructed, a
    // straight path through the center is made
    void ProcessPath(int a_startY, int a_endY, int a_startX, int a_endX)
    {
        // is there 3 or more rows?
        if (a_endY - a_startY >= 2)
        {
            // carve a path that starts randomly and ends at the bridge
            CarvePath(a_startY, a_endY, a_startX, a_endX);
        }
        else // 1 or 2 rows
        {
            // create a path straight through the center
            for (int y = a_startY; y <= a_endY; ++y)
            {
                objGrid[GRID_WIDTH / 2, y] = false;
            }
        }
    }

    // carve a path through a set of obstacles, this function creates a pattern 
    // similar to this:
    //      ****** ***
    //      **     ***
    //      ** *******
    void CarvePath(int a_startY, int a_endY, int a_startX, int a_endX)
    {
        // loop until the y positions meet
        do
        {
            // clear the first tile and move up
            objGrid[a_startX, a_startY] = false;
            ++a_startY;

            // if the y position do not match, remove the last tile and move down
            if (a_startY != a_endY)
            {
                objGrid[a_endX, a_endY] = false;
                --a_endY;
            }
       }
        while (a_startY != a_endY);

        // clear the row to complete the path, both start and end y contain
        // the y position of the row to clear and the x positions must be sorted
        int startX = Mathf.Min(a_startX, a_endX);
        int endX = Mathf.Max(a_startX, a_endX);
        for (; startX <= endX; ++startX) 
        {
            objGrid[startX, a_startY] = false;
        }
    }

    // maps obstacles, instantiates random obstacles and guarantees a path
    // through the obstacles from start to finish
    void BuildObstacles()
    {
        // the range of rows that can contain obstacles
        int begRowIdx = 1;
        int endRowIdx = GRID_HEIGHT - 4;

        // how many rows in the game play area
        int numRows = endRowIdx - begRowIdx + 1;

        // randomly set obstacle locations
        RandomInt intGenX = new RandomInt(0, GRID_WIDTH - 1);
        RandomInt intGenY = new RandomInt(begRowIdx, endRowIdx);

        // map the obstacles
        int numObstacles = Mathf.FloorToInt(numRows * GRID_WIDTH * OBSTACLE_COVERAGE);
        for (int i = 0; i < numObstacles; ++i)
        {
            // find a valid location
            int x, y;
            bool accepted;
            do
            {
                // generate a coordinate
                x = intGenX.Value;
                y = intGenY.Value;

                // make sure its valid
                accepted = !objGrid[x, y];
            }
            while (!accepted);

            // set the state
            objGrid[x, y] = true;
        }

        // map obstacles along both edges
        for (int y = 0; y <= endRowIdx; ++y)
        {
            objGrid[0, y] = true;
            objGrid[GRID_WIDTH - 1, y] = true;
        }

        // find the start of each trainer group
        int halfSize = numRows / 2;
        int groupOneIdx = begRowIdx + (halfSize - TRAINERS_PER_GROUP) / 2;
        int groupTwoIdx = begRowIdx + halfSize + (halfSize - TRAINERS_PER_GROUP) / 2;

        // clear all obstacles from the trainer rows and build the trainer list
        for (int y = 0; y < TRAINERS_PER_GROUP; ++y)
        {
            // add trainer rows
            m_trainerRows.Add(groupOneIdx + y);
            m_trainerRows.Add(groupTwoIdx + y);

            // clear the trainer rows
            for (int x = 0; x < GRID_WIDTH; ++x)
            {
                objGrid[x, groupOneIdx + y] = false;
                objGrid[x, groupTwoIdx + y] = false;
            }
        }

        // clear a path to ensure the player can reach the finish
        //
        // there will be up to three sections of obstacles through which a path
        // must be guaranteed to exist
        //
        // the first is between the second group of trainers and the end zone
        int startY = groupTwoIdx + TRAINERS_PER_GROUP;
        int endY = GRID_HEIGHT - 4;

        // recreate the x range to ensure the guaranteed path never starts or ends
        // directly on an edge
        intGenX = new RandomInt(2, GRID_WIDTH - 3);

        // is there at least one row of obstacles here?
        if (startY <= endY)
        {
            ProcessPath(startY, endY, intGenX.Value, GRID_WIDTH / 2);
        }

        // the second is between the two groups of trainers
        startY = groupOneIdx + TRAINERS_PER_GROUP;
        endY = groupTwoIdx - 1;

        // is there at least one row of obstacles here?
        if (startY <= endY)
        {
            ProcessPath(startY, endY, intGenX.Value, intGenX.Value);
        }

        // the third is between the start and first group of trainers
        startY = 1;
        endY = groupOneIdx - 1;

        // is there at least one row of obstacles here?
        if (startY <= endY)
        {
            ProcessPath(startY, endY, intGenX.Value, intGenX.Value);
        }

        // obstacle selector
        RandomInt intGenO = new RandomInt(0, obstacleTiles.Length - 1);

        for (int y = 0; y < GRID_HEIGHT; ++y)
        {
            for (int x = 0; x < GRID_WIDTH; ++x)
            {
                // is this tile an obstacle?
                if (objGrid[x, y])
                {
                    // instantiate the obstacle
                    CreateObject(obstacleTiles[intGenO.Value], grid[x, y]);
                }
            }
        }

        // ensure pikachu cannot walk to trainer positions
        for (int y1 = groupOneIdx, y2 = groupTwoIdx; y1 <= groupOneIdx + TRAINERS_PER_GROUP; ++y1, ++y2)
        {
            objGrid[0, y1] = true;
            objGrid[GRID_WIDTH - 1, y1] = true;
            objGrid[0, y2] = true;
            objGrid[GRID_WIDTH - 1, y2] = true;
        }
    }

    // constructs the end zone of the level
    void BuildEndZone()
    {
        // the center x position
        int centerX = GRID_WIDTH / 2;

        // map the water into the obstacle grid and instantiate the tiles
        for (int i = 0; i < GRID_WIDTH; ++i)
        {
            // ignore the center tile
            if (i != centerX)
            {
                // map the obstacles
                objGrid[i, GRID_HEIGHT - 2] = true;
                objGrid[i, GRID_HEIGHT - 3] = true;

                // instantiate the tiles
                CreateObject(waterTile, grid[i, GRID_HEIGHT - 2]);
                CreateObject(waterTile, grid[i, GRID_HEIGHT - 3]);
            }
        }

        // instantiate the bridge
        CreateObject(bridgeTile, grid[centerX, GRID_HEIGHT - 2]);
        CreateObject(bridgeTile, grid[centerX, GRID_HEIGHT - 3]);
    }

    // spawn the level food
    void BuildFood()
    {
        CreateObject(foodObject, grid[pikachuX, GRID_HEIGHT - 1]);
    }

    // spawns the level characters
    void BuildCharacters()
    {
        // create and setup pikachu
        pikachu = CreateObject(pikachuObject, grid[pikachuX, pikachuY]);
        pikachu.GetComponent<Pikachu>().gridController = gameObject.GetComponent<GridController>();

        // create and setup trainers

        // if someone else adds trainer instantiation you can use
        // levelController.GetComponent<LevelController>().level
        // to pass the level number to the trainers

        GameObject go = CreateObject(leftTrainerObject, grid[0, m_trainerRows[0]]);
        go.GetComponent<Trainer>().gridController = gameObject.GetComponent<GridController>();
        go.GetComponent<Trainer>().levelController = levelController.GetComponent<LevelController>();
        go.GetComponent<Trainer>().endPosition = grid[GRID_WIDTH - 1, m_trainerRows[0]];

        go = CreateObject(leftTrainerObject, grid[0, m_trainerRows[1]]);
        go.GetComponent<Trainer>().gridController = gameObject.GetComponent<GridController>();
        go.GetComponent<Trainer>().levelController = levelController.GetComponent<LevelController>();
        go.GetComponent<Trainer>().endPosition = grid[GRID_WIDTH - 1, m_trainerRows[1]];

        go = CreateObject(leftTrainerObject, grid[0, m_trainerRows[2]]);
        go.GetComponent<Trainer>().gridController = gameObject.GetComponent<GridController>();
        go.GetComponent<Trainer>().levelController = levelController.GetComponent<LevelController>();
        go.GetComponent<Trainer>().endPosition = grid[GRID_WIDTH - 1, m_trainerRows[2]];

        go = CreateObject(rightTrainerObject, grid[GRID_WIDTH - 1, m_trainerRows[3]]);
        go.GetComponent<Trainer>().gridController = gameObject.GetComponent<GridController>();
        go.GetComponent<Trainer>().levelController = levelController.GetComponent<LevelController>();
        go.GetComponent<Trainer>().endPosition = grid[0, m_trainerRows[3]];

        go = CreateObject(rightTrainerObject, grid[GRID_WIDTH - 1, m_trainerRows[4]]);
        go.GetComponent<Trainer>().gridController = gameObject.GetComponent<GridController>();
        go.GetComponent<Trainer>().levelController = levelController.GetComponent<LevelController>();
        go.GetComponent<Trainer>().endPosition = grid[0, m_trainerRows[4]];

        go = CreateObject(rightTrainerObject, grid[GRID_WIDTH - 1, m_trainerRows[5]]);
        go.GetComponent<Trainer>().gridController = gameObject.GetComponent<GridController>();
        go.GetComponent<Trainer>().levelController = levelController.GetComponent<LevelController>();
        go.GetComponent<Trainer>().endPosition = grid[0, m_trainerRows[5]];
    }

    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////
    // end level generation stuff
    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////
}
