using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockParent : MonoBehaviour
{
    private GridPosition gridPosition;

    public static event Action OnDestroyed;

    public static event Action<BlockParent> OnNewBlockSpawned;

    private bool functionCalledThisFrame = false;


    [SerializeField]
    private Block[] childBlockArray;


    private void Start()
    {
        CollectChildBlocks();
        TickSystem.Instance.OnTickChange += TickSystem_OnTickChanged;
        OnNewBlockSpawned?.Invoke(this);
    }

    private void OnDestroy()
    {
        TickSystem.Instance.OnTickChange -= TickSystem_OnTickChanged;
    }

    private void CollectChildBlocks()
    {
        List<Block> blockList = new List<Block>();

        void CollectBlocks(Transform parent)
        {
            foreach (Transform child in parent)
            {
                Block block = child.GetComponent<Block>();
                if (block != null)
                {
                    blockList.Add(block);
                }
                else
                {
                    CollectBlocks(child);
                }
            }
        }

        CollectBlocks(transform);

        childBlockArray = blockList.ToArray();
    }



    private void Update()
    {
        functionCalledThisFrame = false;

    }

    public bool CallMyFunction()
    {
        if (functionCalledThisFrame)
        {
            Debug.Log("Function already called this frame, skipping.");
            return false;
        }

        // Mark the function as called
        functionCalledThisFrame = true;

        // Function logic here
        return true; 
    }

    public void RotateTransform(Vector3 rotationAmount)
    {
        List<Vector3> newPositions = new List<Vector3>();
        Transform pivot = transform.Find("Pivot");
        Vector3 pivotPosition = pivot.position;

        Quaternion rotation = Quaternion.Euler(rotationAmount);

        if (childBlockArray != null)
        {
            foreach (Block block in childBlockArray)
            {

                // Get the block's world position
                Vector3 blockWorldPosition = LevelGrid.Instance.GetWorldPosition(block.GetGridPosition());

                // Calculate the position relative to the pivot
                Vector3 relativePosition = blockWorldPosition - pivotPosition;

                // Rotate the relative position
                Vector3 rotatedRelativePosition = rotation * relativePosition;

                // Calculate the new world position
                Vector3 newWorldPosition = rotatedRelativePosition + pivotPosition;


                if (!CheckIfPositionIsValid(LevelGrid.Instance.GetGridPosition(newWorldPosition)))
                {
                    Debug.Log(LevelGrid.Instance.GetGridPosition(newWorldPosition).ToString());

                    Debug.Log("InVaLiD");
                    return;
                }

            }
           
        }else
        {
            Debug.Log("Child not Found");
            return;
        }

        pivot.Rotate(90f, 0f, 0f, Space.Self);

    }



    public void MoveSideways(int move)
    {
        foreach (Block block in childBlockArray)
        {
            GridPosition blockGridPosition = block.GetGridPosition();
            GridPosition newBlockGridPosition = new GridPosition(blockGridPosition.z+move,blockGridPosition.y);

            if (!CheckIfPositionIsValid(newBlockGridPosition))
            {
                Debug.Log("Invalid");
                return;
            }

        }

        GridPosition parentBlockGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        MoveParentTo(new GridPosition(parentBlockGridPosition.z + move, parentBlockGridPosition.y));

    }

    private void MoveParentSideways(int move)
    {
        GridPosition parentBlockGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        parentBlockGridPosition.z += move;
        transform.position = LevelGrid.Instance.GetWorldPosition(parentBlockGridPosition);
    }

    public void MoveDown()
    {
        if(!CallMyFunction())
        {
            return;
        }
        functionCalledThisFrame = true;

        foreach (Block block in childBlockArray)
        {
            GridPosition blockGridPosition = block.GetGridPosition();
            GridPosition newBlockGridPosition = new GridPosition(blockGridPosition.z, blockGridPosition.y - 1);
            if (!CheckIfPositionIsValid(newBlockGridPosition))
            {
                DeleteBlockParentComponent();
                return;
            }


        }

        GridPosition parentBlockGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        MoveParentTo(new GridPosition(parentBlockGridPosition.z,parentBlockGridPosition.y-1));
    }

    private void MoveParentDown()
    {
        GridPosition parentBlockGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        parentBlockGridPosition.y -= 1;
        transform.position = LevelGrid.Instance.GetWorldPosition(parentBlockGridPosition);
        
        
    }

    private void TickSystem_OnTickChanged(object sender, EventArgs empty)
    {
        MoveDown();
    }

    private bool CheckIfPositionIsValid(GridPosition gridPosition)
    {

        if(!LevelGrid.Instance.IsValidGridPosition(gridPosition))
        {
            return false;
        }


        if(LevelGrid.Instance.HasBlockOnGridPosition(gridPosition))
        {
            foreach (Block block in LevelGrid.Instance.GetBlockListAtGridPosition(gridPosition))
            {

                if (!(block.transform.parent == this.transform.GetChild(0)))
                {
                    return false;
                }

            }
        }

        return true;
    }

    private void DeleteBlockParentComponent()
    {

        OnDestroyed?.Invoke();
        Destroy(this);

    }

    public void MoveParentTo(GridPosition to)
    {
        
        this.gridPosition = to;
        this.transform.position = LevelGrid.Instance.GetWorldPosition(to);
    }

    public void MoveToTheBottom()
    {
        if (!CallMyFunction())
        {
            return;
        }

        functionCalledThisFrame = true;

        int moveDownY = 0;
        while (true)
        {
            bool canMoveDown = true;

            foreach (Block block in childBlockArray)
            {
                GridPosition blockGridPosition = block.GetGridPosition();
                GridPosition newBlockGridPosition = new GridPosition(blockGridPosition.z, blockGridPosition.y - moveDownY);

                // If any block can't move down further, stop the loop
                if (!CheckIfPositionIsValid(newBlockGridPosition))
                {
                    canMoveDown = false;
                    break;
                }
            }

            if (canMoveDown)
            {
                // Move the parent one step down
                GridPosition parentBlockGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
                moveDownY += 1;
            }
            else
            {
                // When no further movement is possible, finalize the state
                GridPosition parentBlockGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
                MoveParentTo(new GridPosition(this.gridPosition.z, this.gridPosition.y-moveDownY+1));
           
                return;
            }

        }
    }


}
