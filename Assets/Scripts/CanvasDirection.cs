using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDirection : MonoBehaviour
{
    private Transform mainCamera;

    private void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        transform.forward = mainCamera.forward;
    }
}
