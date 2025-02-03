using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectArray;
    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];

        for (int z = 0; z < width; z++)
        {
            for (int y = 0; y < height; y++)
            {
                GridPosition gridPosition = new GridPosition(z, y);
                gridObjectArray[z,y] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(0, gridPosition.y, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(          
            Mathf.RoundToInt(worldPosition.z / cellSize),
            Mathf.RoundToInt(worldPosition.y / cellSize)
            );
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.z, gridPosition.y];

    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.z >= 0 &&
                gridPosition.y >= 0 &&
                gridPosition.z < width &&
                gridPosition.y < height;
    }

    public int GetGridWidth() => width;
    public int GetGridHeight() => height;





}
