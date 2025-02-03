using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Block> blockList;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        blockList = new List<Block>();
    }

    public override string ToString()
    {
        string blockString = "";
        foreach (Block block in blockList)
        {
            blockString += block + "\n";
        }

        return gridPosition.ToString() + "\n" + blockString;

    }

    public void AddBlock(Block block)
    {
        blockList.Add(block);
    }

    public void RemoveBlock(Block block)
    {
        blockList.Remove(block);
    }

    public List<Block> GetBlockList()
    {
        return blockList;
    }

    public bool HasAnyBlock()
    {

        if (blockList.Count > 0)
        {
            return true;
        }
        foreach(Block block in blockList)
        {
            Debug.Log(block.ToString());
        }
        return false;
    }

    public void DeleteBlockList()
    {
        blockList.Clear();
    }



}

