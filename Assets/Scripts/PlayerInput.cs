#define V_PC

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviourPun
{
    public bool isMoving { get; private set; }
    public float moveV { get; private set; }
    public float moveH { get; private set; }

    void Update()
    {
        if (!photonView.IsMine)
            return;
        // 없애고 조이스틱으로 모바일 버전만 남길 예정
        moveV = Input.GetAxisRaw("Vertical");
        moveH = Input.GetAxisRaw("Horizontal");
        isMoving = moveH != 0f || moveV != 0f;
    }
}
