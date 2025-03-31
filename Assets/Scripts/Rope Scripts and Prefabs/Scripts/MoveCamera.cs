using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is part 4 (final)//
//allow player to look back and forth using cursor//
//Add this script to the camera holder empty object (create one cuh!)//
//in the camera position box, drag the empty object Called "CameraPos" from Player Object//
public class MoveCamera : MonoBehaviour
{
    public Transform cameraposition;
    private void Update()
    {
        transform.position = cameraposition.position;
    }
}
