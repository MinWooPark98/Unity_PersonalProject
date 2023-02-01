using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowDamage : MonoBehaviour
{
    private TextMeshPro text;
    private Transform mainCam;

    private float duration;
    private float timer = 0f;
    private float refloatY;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        mainCam = Camera.main.transform;
    }

    public void Set(string str, Color color, float fontSize, float duration, float refloatY)
    {
        text.text = str;
        text.color = color;
        text.fontSize = fontSize;
        this.duration = duration;
        this.refloatY = refloatY;
    }

    void Update()
    {
        timer += Time.deltaTime;
        transform.forward = mainCam.forward;
        if (timer >= duration)
        {
            Destroy(gameObject);
            return;
        }
        transform.Translate(refloatY * Time.deltaTime * Vector3.up / duration);
    }
}
