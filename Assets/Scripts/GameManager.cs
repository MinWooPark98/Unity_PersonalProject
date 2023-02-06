using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Characters
{
    Bear,
    Veteran,
    Soldier,
    Count,
}

public class GameManager : MonoBehaviourPunCallbacks
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

    private float playTime;
    private float playTimer;
    public float GameProgress { get => playTimer / playTime; }
    public GameObject[] playerPrefabs;   // 임시 - 온라인으로 바꾸면 캐릭터들 prefab 전부 가진 후에 플레이어들이 선택한 캐릭터 생성
    public GameObject frontArea;
    public GameObject rangeArea;
    public SpriteRenderer throwArea;
    public LineRenderer throwLine;
    public Vector2 mapSize;
    public ShowDamageLancher showDamageLauncher;

    private void Awake()
    {
        playTime = PlayDataManager.instance.playTime;
        playTimer = 0f;
    }

    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefabs[(int)PlayDataManager.instance.character].name, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        playTimer += Time.deltaTime;
        // if () 내가 죽으면 leave button활성화, 누르면 LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
