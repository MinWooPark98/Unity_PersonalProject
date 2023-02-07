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

    private bool allPlayersLoaded = false;
    private int loadedPlayers;
    private float startTime;
    private float playTime;
    private float playTimer;
    public float GameProgress { get => playTimer / playTime; }
    public GameObject[] playerPrefabs;   // �ӽ� - �¶������� �ٲٸ� ĳ���͵� prefab ���� ���� �Ŀ� �÷��̾���� ������ ĳ���� ����
    public GameObject frontArea;
    public GameObject rangeArea;
    public SpriteRenderer throwArea;
    public LineRenderer throwLine;
    public Vector2 mapSize;
    public ShowDamageLancher showDamageLauncher;

    public GameObject loadingPanel;
    public GameObject startPanel;
    public GameObject resultPanel;

    private void Start()
    {
        playTime = PlayDataManager.instance.playTime;
        playTimer = 0f;
        var player = PhotonNetwork.Instantiate(playerPrefabs[(int)PlayDataManager.instance.character].name, Vector3.zero, Quaternion.identity);
        PhotonNetwork.IsMessageQueueRunning = true;
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(CheckAllLoaded());
    }

    private void Update()
    {
        if (!allPlayersLoaded)
            return;
        playTimer = Time.time - startTime;
    }

    private IEnumerator CheckAllLoaded()
    {
        while (true)
        {
            loadedPlayers = 1;
            photonView.RPC("SendLoaded", RpcTarget.Others);
            yield return new WaitForSeconds(1f);
            if (loadedPlayers == PhotonNetwork.CountOfPlayers)
            {
                photonView.RPC("StartPlay", RpcTarget.All);
                break;
            }
        }
    }

    [PunRPC]
    public void SendLoaded() => photonView.RPC("LoadedCount", RpcTarget.MasterClient);

    [PunRPC]
    private void LoadedCount() => ++loadedPlayers;

    [PunRPC]
    public void StartPlay()
    {
        allPlayersLoaded = true;
        startTime = Time.time;
        loadingPanel.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
