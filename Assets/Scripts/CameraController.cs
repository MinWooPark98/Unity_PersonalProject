using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ���� ���� ���� �ʿ��ϸ� �߰�
    [SerializeField] private Vector3 offset;
    public Transform player;

    void FixedUpdate()
    {
        transform.position = player.position + offset;
        transform.LookAt(player);
    }
}
