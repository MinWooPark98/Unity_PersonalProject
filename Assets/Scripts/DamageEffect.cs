using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    public ShowDamage prefab;

    public float height = 2f;
    public float duration = 1f;
    public float refloatY = 1f;
    public Color color;
    public float characterSize = 6f;

    public void OnHit(int dmg)
    {
        var effect = PlaySceneManager.instance.showDamageLauncher.Get();
        effect.transform.position = transform.position + new Vector3(Random.Range(0, 0.5f), 1f, Random.Range(0, 0.5f)) * height;
        effect.Set(dmg.ToString(), color, characterSize, duration, refloatY);
    }
}
