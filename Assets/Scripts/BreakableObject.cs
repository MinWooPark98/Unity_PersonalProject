using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject before;
    public GameObject After;

    private void Start()
    {
        before.SetActive(true);
        After.SetActive(false);
    }

    public void Break()
    {
        before.SetActive(false);
        After.SetActive(true);
    }
}
