using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviourPunCallbacks
{
    private static PlaySceneManager m_instance;
    public static PlaySceneManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<PlaySceneManager>();
            return m_instance;
        }
    }

    public float playTimeLimit = 30f;
    private float playTimer;
    public float GameProgress { get => playTimer / playTimeLimit; }
    public GameObject[] playerPrefabs;   // 임시 - 온라인으로 바꾸면 캐릭터들 prefab 전부 가진 후에 플레이어들이 선택한 캐릭터 생성
    public GameObject frontArea;
    public GameObject rangeArea;
    public SpriteRenderer throwArea;
    public LineRenderer throwLine;
    public Vector2 mapSize;
    public ShowDamageLancher showDamageLauncher;

    private void Awake()
    {
        playTimer = 0f;
        PhotonNetwork.Instantiate(playerPrefabs[0].name, Vector3.zero, Quaternion.identity);
        Debug.Log(PhotonNetwork.IsMasterClient);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
            playTimer += Time.deltaTime;

        // if () 내가 죽으면 leave button활성화, 누르면 LeaveRoom();
    }

    //public override void OnLeftRoom()
    //{
    //    //SceneManager.LoadScene("Lobby");
    //}
}
