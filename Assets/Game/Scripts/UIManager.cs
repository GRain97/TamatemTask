using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Tooltip("Rolling dice addressable prefab")]
    [SerializeField] AssetReferenceGameObject DiceVisuals;
    [Tooltip("Dice script that is fetched from the instantiated prefab")]
    [SerializeField] DiceRoller DiceScript;
    [SerializeField] bool LoadedDiceVisuals;
    [Tooltip("parent transform the for the rolling dice UI prefab")]
    [SerializeField] Transform DiceBG;

    //interactable buttons as named
    [SerializeField] Button DiceRollButton;
    [SerializeField] Button ChipResetButton;


    void Start()
    {
        DiceRollButton.onClick.AddListener(RollDice);
        ChipResetButton.onClick.AddListener(GameManager.Instance.ResetPosition);
    }

    //UI details when rolling dice. Instantiating the dice prefab if it hasnt been requested before, disabling the roll dice button until we recieve a response from the chip that it has finished moving.
    public void RollDice()
    {
        DiceRollButton.interactable = false;
        ChipResetButton.interactable = false;
        GameManager.Instance.GetCurrentPlayer().CallOnFinishedMoving += ReactivateDiceRollButton;
        if (!LoadedDiceVisuals)
        {
            DiceVisuals.InstantiateAsync(DiceBG.transform).Completed += FinishedLoadingDiceVisuals;
        }
        else
        {
            if (DiceScript)
            {
                DiceScript.RollDice();
            }
        }
    }

    private void FinishedLoadingDiceVisuals(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            handle.Result.TryGetComponent(out DiceScript);
            LoadedDiceVisuals = true;
            DiceScript.Init();
            DiceScript.RollDice();
        }
        else
        {
            Debug.LogError("Failed loading dice visuals");
        }
    }

    //response on player finished moving
    public void ReactivateDiceRollButton()
    {
        DiceRollButton.interactable = true;
        ChipResetButton.interactable = true;
        GameManager.Instance.GetCurrentPlayer().CallOnFinishedMoving -= ReactivateDiceRollButton;
    }

}
