using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class PlayerChip : MonoBehaviour
{
    public Action CallOnFinishedMoving;
    public string PlayerName = "Bobbob";

    [SerializeField] private int TileNumber;
    [SerializeField] private int StepsToTake;
    [SerializeField] private LayerMask PlayerLayerMask;
    [SerializeField] private bool ReadyToMove;
    [SerializeField] private GameObject ReadyVisual;

    private void Awake()
    {
        ReadyVisual.SetActive(false);
        //tile number starts from -1 because the starting tile is not part of the normal path it walks on which starts at 0
        TileNumber = -1;
    }

    //enable appropriate chip to be clicked and wait for player
    public void EnableInteraction(int stepsToTake)
    {
        StepsToTake = stepsToTake;
        ReadyToMove = true;
        ReadyVisual.SetActive(true);
    }

    //wait for player to click on chip once its interactable
    private void Update()
    {
        if (ReadyToMove)
        {
            Vector3 ray = Vector3.zero;
            RaycastHit2D hit;
#if Unity_IOS || Unity_Android
            if(Input.touchCount > 0)
            {
                Touch touch;
                touch = Input.GetTouch(0);
                ray = Camera.main.ScreenToWorldPoint(touch.position);
            }
#else
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            }
#endif
            hit = Physics2D.Raycast(ray, ray - Camera.main.transform.position, Mathf.Infinity, PlayerLayerMask);
            if (hit)
            {
                ReadyToMove = false;
                ReadyVisual.SetActive(false);
                Moving();
            }

        }
    }

    //animate chip moving every 300ms
    public async void Moving()
    {
        for (int i = 0; i < StepsToTake; i++)
        {
            int targetTileNumber = TileNumber + 1;
            if (targetTileNumber >= GameManager.Instance.NumberOfTiles)
            {
                targetTileNumber -= GameManager.Instance.NumberOfTiles;
            }

            MoveTo(GameManager.Instance.LudoBoardTilePositions[targetTileNumber]);
            await Task.Delay(200);
            TileNumber = targetTileNumber;
        }

        await Task.Delay(100);
        CallOnFinishedMoving?.Invoke();
    }

    //set chip position using parent transforms
    public void MoveTo(Transform tileTransform)
    {
        transform.parent = tileTransform;
        transform.localPosition = Vector3.zero;
    }

    public void ResetPosition(Transform tileTransform)
    {
        MoveTo(tileTransform);
        TileNumber = -1;
    }

}
