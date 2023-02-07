using Photon.Pun;
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
    public Image realFill;
    public Slider effect;

    private void Start()
    {
        SetColor();
    }

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

    private void SetColor()
    {
        if (subject.GetComponent<PlayerHealth>() != null)
        {
            var photonView = subject.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                realFill.color = Color.green;
                return;
            }
        }
        realFill.color = Color.red;
    }
}
