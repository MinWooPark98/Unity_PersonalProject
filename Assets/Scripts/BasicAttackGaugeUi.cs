using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicAttackGaugeUi : MonoBehaviour
{
    public Transform subject;
    public float distanceFromSubjectPos = 1f;
    public BasicAttackController controller;
    public Slider slider;
    public GameObject borders;
    public GameObject borderPrefab;

    void Start()
    {
        for (int i = 0; i < controller.basicControllerBase.maxSave; ++i)
        {
            borders.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = controller.SavedRatio;
        transform.position = subject.position + Camera.main.transform.up * distanceFromSubjectPos;
    }
}
