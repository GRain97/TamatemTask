using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    private void Start()
    {
        //this is to fix the camera with the size of the ludo board. it renders at 100 ppu and we want a bit of extra bleed space so itsnt too small
        Camera.main.orthographicSize = ((Camera.main.pixelHeight*0.8f) / 100) * 0.5f;
    }
}
