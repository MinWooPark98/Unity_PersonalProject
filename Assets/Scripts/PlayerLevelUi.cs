using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevelUi : FollowingObjectUi
{
    public TextMeshProUGUI levelText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetLevel(int level)
    {
        gameObject.SetActive(true);
        levelText.text = level.ToString();
    }
}
