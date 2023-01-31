using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHealth : Health
{
    protected override void OnDie()
    {
        gameObject.SetActive(false);
    }
}
