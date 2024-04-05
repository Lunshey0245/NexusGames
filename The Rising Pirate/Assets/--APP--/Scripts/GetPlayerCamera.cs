using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerCamera : MonoBehaviour
{
    [SerializeField] Transform CameraPos;
    private void Awake()
    {
        GameObject lerpCamera = GameObject.Find("DemoCamera");
        lerpCamera.GetComponent<LerpCam>()._targetPos = CameraPos;
        lerpCamera.GetComponent<LerpCam>()._targetLookatPos = this.transform;
    }
}
