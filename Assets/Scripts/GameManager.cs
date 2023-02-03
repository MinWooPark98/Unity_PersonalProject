using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters
{
    Bear,
    Veteran,
    Soldier,
    Count,
}

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance;
        }
    }

    private string playerName;
    private Characters character;
    public byte participants = 10;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    public void SetPlayerName(string name) => playerName = name;
    public string GetPlayerName() => playerName;
}
