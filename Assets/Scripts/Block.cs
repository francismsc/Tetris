using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Block : MonoBehaviour
{

    private GridPosition gridPosition;

    private GridPosition newGridPosition;


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddBlockAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            MoveGridPosition(newGridPosition);
        }

    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public void SetGridPosition(GridPosition newGridPosition)
    {
        gridPosition = newGridPosition;
    }

    public void MoveGridPosition(GridPosition to)
    {
        LevelGrid.Instance.BlockedMovedOnGridPosition(this, this.gridPosition, to);
        this.gridPosition = to;
        this.transform.position = LevelGrid.Instance.GetWorldPosition(to);
        
    }

    public void Delete()
    {
        Destroy(this.gameObject);
    }
}
 