#define V_PC

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool isMoving { get; private set; }
    public float moveV { get; private set; }
    public float moveH { get; private set; }

    void Update()
    {
        // ���ְ� ���̽�ƽ���� ����� ������ ���� ����
        moveV = Input.GetAxisRaw("Vertical");
        moveH = Input.GetAxisRaw("Horizontal");
        isMoving = moveH != 0f || moveV != 0f;
    }
}
