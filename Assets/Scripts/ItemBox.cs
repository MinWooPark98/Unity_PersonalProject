using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : Health
{
    public GameObject item;

    private void OnDisable()
    {
        if (item != null)
            item.SetActive(true);
    }

    protected override void OnDie()
    {
        gameObject.SetActive(false);
    }
}
