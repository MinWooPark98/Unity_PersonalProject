using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HpBarUi : MonoBehaviour
{
    public Transform subject;
    public float distanceFromSubjectPos = 2f;
    public Slider real;
    public Slider effect;

    void Update()
    {
        if (subject == null)
            return;
        transform.position = subject.position + Camera.main.transform.up * distanceFromSubjectPos;
        if (!subject.gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
