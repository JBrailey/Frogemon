/*
    GridController.cs
    Authors: Shane Adams, Jessica Brailey,
    Date: 27/7/2016
    Project: Academy of Interactive Entertainment - Practice Productions - Assignment
*/



using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour
{

    const int GRID_WIDTH = 15; // The numberof cells wide the grid is
    const int GRID_HEIGHT = 30; // The number of cells High the grid is
    public Vector3[,] grid;
    int pikachuX; // Pikachus current X position
    int pikachuY; // Pikachus current Y position
    int pikaNewX; // New x position
    int pikaNewY; // New Y position
    bool[,] objGrid = new bool[GRID_WIDTH, GRID_HEIGHT]; //2D bool array of where obstacles are

    // Use this for initialization
    void Start()
    {
        //Create 2D Array of Vector3 & Set them
        grid = new Vector3[GRID_WIDTH, GRID_HEIGHT];

        for (int x = 0; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                grid[x, y] = new Vector3(x + 1f, y+1f, 0f);
                objGrid[x, y] = false;
            }
        }

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

        //  Handle repositioning based on the key pressed
        switch (direction.ToLower())
        {
            case "up":
                pikaNewY++;
                break;
            case "down":
                pikaNewY--;
                break;
            case "left":
                pikaNewX--;
                break;
            case "right":
                pikaNewX++;
                break;
        }
        // If Pikachu can move coordinates are update, otherwise they remain unchanged
        if (CheckIfCanMove(pikaNewX, pikaNewY))
        {
            pikachuX = pikaNewX;
            pikachuY = pikaNewY;
        }
        // Return Pikachu's New Position using it's new grid coords
        return grid[pikachuX, pikachuY];
    }

    // Update is called once per frame
    void Update()
    {

    }
}