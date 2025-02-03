using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDebugSystem : MonoBehaviour
{
    private GridSystem gridSystem;

    public GridDebugSystem(GridSystem gridSystem)
    {
        this.gridSystem = gridSystem; 
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int z = 0; z < gridSystem.GetGridWidth(); z++)
        {
            for (int y = 0; y < gridSystem.GetGridHeight(); y++)
            {
                GridPosition gridPosition = new GridPosition(z, y);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, gridSystem.GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(gridSystem.GetGridObject(gridPosition));
            }
        }
    }
}
