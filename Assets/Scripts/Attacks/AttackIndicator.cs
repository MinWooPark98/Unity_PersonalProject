using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackIndicator : MonoBehaviourPun
{
    public abstract void DrawRange(Vector3 dir, Attack attack);
    public abstract void Clear();
}
