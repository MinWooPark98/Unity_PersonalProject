using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjectUi : MonoBehaviour
{
    public Transform subject;
    public float distanceFromSubjectPos = 1f;

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position = subject.position + Camera.main.transform.up * distanceFromSubjectPos;
    }
}
