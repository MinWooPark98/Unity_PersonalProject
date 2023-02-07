using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameUi : FollowingObjectUi
{
    private TextMeshProUGUI nameUi;

    private void Awake()
    {
        nameUi = GetComponent<TextMeshProUGUI>();
    }

    public void Set(string name) => nameUi.text = name;
}
