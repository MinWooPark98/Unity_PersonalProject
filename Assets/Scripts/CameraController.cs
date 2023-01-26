using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 추후 선형 보간 필요하면 추가
    [SerializeField] private Vector3 offset;
    public Transform player;

    void FixedUpdate()
    {
        transform.position = player.position + offset;
        transform.LookAt(player);
    }
}
