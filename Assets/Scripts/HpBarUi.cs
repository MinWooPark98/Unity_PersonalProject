using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUi : FollowingObjectUi
{
    public Health subHealth;
    public Slider real;
    public Image realFill;
    public Slider effect;
    public TextMeshProUGUI currHpText;

    private void Start()
    {
        SetColor();
    }

    protected override void Update()
    {
        base.Update();
        if (subject == null)
            return;
        real.value = subHealth.hpRatio;
        effect.value = Mathf.Lerp(effect.value, subHealth.hpRatio, Time.deltaTime);
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

    public void SetCurrHpText(int currHp) => currHpText.text = currHp.ToString();
}
