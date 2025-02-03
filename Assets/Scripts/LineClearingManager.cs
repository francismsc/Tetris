using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class LineClearingManager : MonoBehaviour
{
    public static event Action<int> OnLinesCleared;

    private void Start()
    {
        BlockParent.OnDestroyed += CheckForLines;
    }

    public void OnDestroy()
    {
        BlockParent.OnDestroyed -= CheckForLines;
    }


    void CheckForLines()
    {

        List<int> completeLines = new List<int>();

        int gridSizeW = LevelGrid.Instance.GetGridWidth();
        int gridSizeH = LevelGrid.Instance.GetGridHeight();
        for (int y = 0; y < gridSizeH; y++)
        {
            bool isLineComplete = true;
            for (int z = 0; z < gridSizeW; z++)
            {

                if (!LevelGrid.Instance.HasBlockOnGridPosition(new GridPosition(z, y)))
                {

                    isLineComplete = false;
                    break;
                }
            }

            if (isLineComplete)
            {

                completeLines.Add(y);
            }

        }
        if (completeLines.Count > 0)
        {
            OnLinesCleared?.Invoke(completeLines.Count);
            ClearLines(completeLines);
        }

    }

    void ClearLines(List<int> completeLinesY)
    {

        foreach (int y in completeLinesY)
        {
            for (int z = 0; z < LevelGrid.Instance.GetGridWidth(); z++)
            {
                // Get the grid position
                GridPosition gridPosition = new GridPosition(z, y);

                // Get the list of blocks at this position
                List<Block> blocks = LevelGrid.Instance.GetBlockListAtGridPosition(gridPosition);

                // Destroy each block and remove from grid
                foreach (Block block in blocks)
                {
                    Destroy(block.gameObject); // Destroy the block GameObject in Unity
                }

                // Clear the grid position
                LevelGrid.Instance.ClearGridPosition(gridPosition);
            }
        }

        // Optionally, shift blocks down after clearing lines
        ShiftBlocksDown(completeLinesY);
    }

    void ShiftBlocksDown(List<int> clearedLines) 
    {

        int gridWidth = LevelGrid.Instance.GetGridWidth();
        int gridHeight = LevelGrid.Instance.GetGridHeight();
        int[] rowShiftst = new int[gridHeight];
        int shiftAmount = 0;

        // Calculate the number of shifts for each row
        for (int y = 0; y < gridHeight; y++)
        {
            if (clearedLines.Contains(y))
            {
                shiftAmount++;
            }
            rowShiftst[y] = shiftAmount;
        }

        // Iterate from top to bottom to shift blocks
        for (int y = 0; y < gridHeight; y++)
        {
            if (rowShiftst[y] > 0)
            {

                int newY = y - rowShiftst[y];
                if (newY >= 0) // Ensure the new position is within bounds
                {
                    for (int z = 0; z < gridWidth; z++)
                    {

                        if (LevelGrid.Instance.HasBlockOnGridPosition(new GridPosition(z, y)) == true)
                        {
                            // Debug.Log("Before: " + x + "," + y);
                            // Debug.Log("After: " + x + "," + newY);
                            LevelGrid.Instance.MoveBlockOnGrid(new GridPosition(z, y), new GridPosition(z, newY));
                        }
                    }
                }
            }
        }
    }

}

