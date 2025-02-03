using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField]
    GameObject[] blockPrefabsArray;
    [SerializeField]
    GameObject[] NextPiecesPrefabArray;
    [SerializeField]
    private BlockParent activeBlockParent;

    private GridPosition startGridPosition;

    private float spacebarCooldown = 1f; // Adjust as needed
    private float lastSpacebarPress = 0f;

    public static event Action OnGameOver;
    public static event Action OnPause;

    private GameObject nextPrefab;
    private GameObject nextPrefabShowPiece;

    private int randomNumber;

    [SerializeField] 
    private GameObject touchControls;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one GameController! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        BlockParent.OnDestroyed += SpawnBlockAtTop;
        BlockParent.OnDestroyed += SpawnNextBlock;
        BlockParent.OnDestroyed += CheckForGameOver;
        BlockParent.OnNewBlockSpawned += SetActiveBlockParent;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            touchControls.SetActive(true);
        }
        else
        {
            touchControls.SetActive(false);
        }

        Time.timeScale = 1f;
        SpawnNextBlock();
        SpawnBlockAtTop();
        SpawnNextBlock();
    }

    private void OnDisable()
    {
        BlockParent.OnDestroyed -= SpawnBlockAtTop;
        BlockParent.OnDestroyed -= SpawnNextBlock;
        BlockParent.OnDestroyed -= CheckForGameOver;
        BlockParent.OnNewBlockSpawned -= SetActiveBlockParent;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnBlockAtTop();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            TickSystem.Instance.ChangeTickSpeed(0.5f);
        }

        if (activeBlockParent != null)
        {
            HandlePlayerInput();
        }
    }

    private void HandlePlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSide(1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSide(-1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastSpacebarPress + spacebarCooldown)
        {
            DropDown();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void MoveSide(int number)
    {
        activeBlockParent.MoveSideways(number);
    }

    public void MoveDown()
    {
        activeBlockParent.MoveDown();
    }

    public void Rotate()
    {
        activeBlockParent.RotateTransform(new Vector3(90, 0, 0));
    }

    public void DropDown()
    {
        lastSpacebarPress = Time.time;
        activeBlockParent.MoveToTheBottom();
    }

    private void SetActiveBlockParent(BlockParent newBlockParent)
    {
        activeBlockParent = newBlockParent;
    }

    public void Pause()
    {
        OnPause?.Invoke();
    }

    private void SpawnBlockAtTop()
    {
        startGridPosition = new GridPosition(4, 20);
        lastSpacebarPress = Time.time;
        LevelGrid.Instance.SpawnBlockAtStartGridPosition(nextPrefab, startGridPosition);
    }

    private void SpawnNextBlock()
    {
        randomNumber = GetRandomNumber();
        nextPrefab = blockPrefabsArray[randomNumber];
        foreach (GameObject prefab in NextPiecesPrefabArray)
        {
            if (nextPrefab.name == prefab.name)
            {
                prefab.SetActive(true);
            }
            else
            {
                prefab.SetActive(false);
            }
        }

    }


    private int GetRandomNumber()
    {
        int randomNumb = UnityEngine.Random.Range(0, blockPrefabsArray.Length);
        return randomNumb;
    }
    private void CheckForGameOver()
    {

        for (int y = 20; y < LevelGrid.Instance.GetGridHeight(); y++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetGridWidth(); z++)
            {
                if (LevelGrid.Instance.HasBlockOnGridPosition(new GridPosition(z, y)))
                {
                    OnGameOver?.Invoke();
                }
            }
        }

    }




}





