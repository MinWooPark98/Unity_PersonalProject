using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : MonoBehaviourPun
{
    private bool isHiding = false;


    private void Update()
    {
        if (isHiding)
        {
            // ismine이면서 isHiding인 놈이랑 거리가 가까우면 보이게 바뀌도록
            // 멀어지면 다시 안보이게 바뀌도록미ㅏㅇ루ㅡ미ㅏㅈㄷ릐ㅏㅈㄷ
        }
    }
    public void Hide()
    {
        // 나 빼고 다 투명하게
    }
}
