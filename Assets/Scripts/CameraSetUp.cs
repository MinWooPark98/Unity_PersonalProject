using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetUp : MonoBehaviourPun
{
    IEnumerator Start()
    {
        yield return null;

        if (photonView.IsMine)
        {
            var brain = Camera.main.GetComponent<CinemachineBrain>();
            var vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
            vcam.Follow = transform;
            vcam.LookAt = transform;
        }
    }
}
