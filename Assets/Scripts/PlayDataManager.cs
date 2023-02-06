using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDataManager : MonoBehaviour
{
    private static PlayDataManager m_instance;
    public static PlayDataManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<PlayDataManager>();
            return m_instance;
        }
    }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public string playerName;
    public Characters character;
    public float playTime;
    public int rank;
}
