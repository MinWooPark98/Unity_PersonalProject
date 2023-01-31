using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUi : MonoBehaviour
{
    public Transform subject;
    public Health subHealth;
    public float distanceFromSubjectPos = 2f;
    public Slider real;
    public Slider effect;

    void Update()
    {
        if (subject == null)
            return;
        real.value = subHealth.hpRatio;
        effect.value = Mathf.Lerp(effect.value, subHealth.hpRatio, Time.deltaTime);
        transform.position = subject.position + Camera.main.transform.up * distanceFromSubjectPos;
        if (!subject.gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
