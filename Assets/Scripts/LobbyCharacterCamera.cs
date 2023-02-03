using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacterCamera : MonoBehaviour
{
    void Update()
    {
        var targetPos = new Vector3(LobbySceneManager.instance.characterIndex * (-3f), 3.5f, 7f);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 3f);
    }
}
