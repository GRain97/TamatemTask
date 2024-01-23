using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class GameManager : MonoBehaviour
{
    [Tooltip("Ludo board visual addressable asset")]
    [SerializeField] AssetReferenceGameObject LudoBoardReference;
    [SerializeField] bool LoadedboardVisuals;
    [Tooltip("Player addressable prefab")]
    [SerializeField] AssetReferenceGameObject PlayerChipReference;
    [SerializeField] bool LoadedChipVisuals;

    [Tooltip("Transforms of all normal paths on the board")]
    public List<Transform> LudoBoardTilePositions;
    [Tooltip("Transforms of all chip starting positions on the board")]
    public List<Transform> LudoBoardTileStartingPositions;

    public PlayerChip OnlyPlayer;

    [Tooltip("Number of tiles on the board..usually this wont change but just incase if creating a new game mode or something")]
    public int NumberOfTiles;

    //Gamemanager simple singleton to access certain details in a one way relationship
    public static GameManager Instance;
    private void Awake()
    {
        //assign singleton value and destroy any duplicate objects... this will not happen in this task's case but its an appropriate precaution
        if (Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        NumberOfTiles = LudoBoardTilePositions.Count;
    }

    void Start()
    {
        //instantiate ludo board visual asynchronously
        if (!LoadedboardVisuals)
        {
            AsyncOperationHandle<GameObject> handle = LudoBoardReference.InstantiateAsync(transform);
            handle.Completed += FinishedLoadingBoardVisuals;
        }

        //instantiate player prefab asynchronously
        if (!LoadedChipVisuals)
        {
            AsyncOperationHandle<GameObject> handle = PlayerChipReference.InstantiateAsync(LudoBoardTileStartingPositions[0]);
            handle.Completed += FinishedLoadingChipVisuals;
        }
    }

    //on complete handling for player addressable
    private void FinishedLoadingChipVisuals(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            LoadedChipVisuals = true;
            handle.Result.TryGetComponent(out OnlyPlayer);
        }
        else
        {
            Debug.LogError("Failed to load player chip addressable");
        }
    }

    //on complete handling for board addressable
    private void FinishedLoadingBoardVisuals(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            LoadedboardVisuals = true;
        }
        else
        {
            Debug.LogError("Failed loading ludo board addressable");
        }
    }

    //gamemanager player control funcitonalities to allow any external systems to move players without interacting with them directly
    public void MoveChip(int stepsToTake)
    {
        var currentPlayer = GetCurrentPlayer();
        if (currentPlayer)
        {
            currentPlayer.EnableInteraction(stepsToTake);
        }
    }


    //reset player
    public void ResetPosition()
    {
        var currentPlayer = GetCurrentPlayer();
        if (currentPlayer)
        {
            currentPlayer.ResetPosition(LudoBoardTileStartingPositions[0]);
        }
    }

    //this is just to setup the ability to fetch different players when we have more than one
    public PlayerChip GetCurrentPlayer()
    {
        return OnlyPlayer;
    }
}
