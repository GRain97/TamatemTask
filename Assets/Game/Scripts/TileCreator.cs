#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileCreator : MonoBehaviour
{
    //this code was only created to quicken the setup of the tiles transforms :D
    [SerializeField] float X_Offset = 0.48f;
    [SerializeField] float Y_Offset = 0.48f;
    [SerializeField] GameObject RefObj;
    [SerializeField] int TileCount;
    [SerializeField] bool MoveInX = false;
    [SerializeField] bool XIsNegative = false;
    [SerializeField] bool MoveInY = false;
    [SerializeField] bool YIsNegative = false;
    [SerializeField] int NumberOfNewTiles = 1;
    [ContextMenu("CreateTile")]
    public void CreateTile()
    {
        for (int i = 0; i < NumberOfNewTiles; i++)
        {
            var temp = Instantiate(RefObj, RefObj.transform.position, Quaternion.identity, RefObj.transform.parent);
            RefObj = temp;
            RefObj.transform.localPosition += (((YIsNegative ? -1 : 1) * (Vector3.up * (MoveInY ? Y_Offset : 0))) + ((XIsNegative ? -1 : 1) * (Vector3.right * (MoveInX ? X_Offset : 0))));
            TileCount++;
            RefObj.name = $"Tile_{TileCount}";
        }
    }
}
#endif