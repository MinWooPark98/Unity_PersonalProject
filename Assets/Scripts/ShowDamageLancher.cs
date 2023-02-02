using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShowDamageLancher : MonoBehaviour
{
    public ShowDamage prefab;
    public int maxCount = 30;
    private IObjectPool<ShowDamage> showDamagePool;

    private void Start()
    {
        showDamagePool = new ObjectPool<ShowDamage>(CreateShowDamage, OnGetShowDamage, OnReleaseShowDamage, OnDestroyShowDamage, maxSize: maxCount); 
    }

    private ShowDamage CreateShowDamage()
    {
        ShowDamage effect = Instantiate(prefab);
        effect.SetPool(showDamagePool);
        return effect;
    }

    private void OnGetShowDamage(ShowDamage showDamage) => showDamage.gameObject.SetActive(true);

    private void OnReleaseShowDamage(ShowDamage showDamage) => showDamage.gameObject.SetActive(false);

    private void OnDestroyShowDamage(ShowDamage showDamage) => Destroy(showDamage.gameObject);

    public ShowDamage Get() => showDamagePool.Get();
}
