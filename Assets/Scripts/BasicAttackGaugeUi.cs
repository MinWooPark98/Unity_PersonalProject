using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicAttackGaugeUi : FollowingObjectUi
{
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
    protected override void Update()
    {
        base.Update();
        slider.value = controller.SavedRatio;
    }
}
