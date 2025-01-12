using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    Camera cam; // 카메라
    public Transform target;    // 플레이어

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec = target.transform.position;
        cam.transform.position = new Vector3(vec.x, vec.y, -1);
    }
}
