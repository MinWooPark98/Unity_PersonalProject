using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    private bool isLoaded = false;
    private float startTime;
    private float playTime;
    private float playTimer;
    public float GameProgress { get => playTimer / playTime; }
    public GameObject[] playerPrefabs;   // 임시 - 온라인으로 바꾸면 캐릭터들 prefab 전부 가진 후에 플레이어들이 선택한 캐릭터 생성
    private GameObject myPlayer;
    public GameObject frontArea;
    public GameObject rangeArea;
    public SpriteRenderer throwArea;
    public LineRenderer throwLine;
    public Vector2 mapSize;
    public ShowDamageLancher showDamageLauncher;
    public GameObject grid;
    private bool gridRebooted = false;
    public List<HideOnBush> bushes = new List<HideOnBush>();

    public GameObject loadingPanel;
    public GameObject startPanel;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    private void Start()
    {
        playTime = PlayDataManager.instance.playTime;
        playTimer = 0f;
        myPlayer = PhotonNetwork.Instantiate(playerPrefabs[(int)PlayDataManager.instance.character].name, Vector3.zero, Quaternion.identity);
        PhotonNetwork.IsMessageQueueRunning = true;
        var bushesGO = GameObject.FindGameObjectsWithTag("Bush");
        foreach (var bush in bushesGO)
        {
            bushes.Add(bush.GetComponent<HideOnBush>());
        }
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(CheckAllLoaded());
    }

    private void Update()
    {
        if (!allPlayersLoaded)
            return;
        playTimer = Time.time - startTime;
        if (Input.GetKeyDown(KeyCode.Escape))
            LeaveGame();
    }

    private IEnumerator CheckAllLoaded()
    {
        isLoaded = true;
        loadedPlayers = 1;
        while (true)
        {
            photonView.RPC("SendLoaded", RpcTarget.Others);
            yield return new WaitForSeconds(1f);
            if (loadedPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                photonView.RPC("StartPlay", RpcTarget.All, loadedPlayers);
                break;
            }
        }
    }

    [PunRPC]
    private void SendLoaded()
    {
        if (isLoaded)
            return;
        if (myPlayer != null)
        {
            photonView.RPC("LoadedCount", RpcTarget.MasterClient);
            isLoaded = true;
        }
    }

    [PunRPC]
    private void LoadedCount() => ++loadedPlayers;

    [PunRPC]
    public void StartPlay(int loadedPlayers)
    {
        this.loadedPlayers = loadedPlayers;
        allPlayersLoaded = true;
        startTime = Time.time;
        RebootGrid();
        StartCoroutine(ShowStartPanel());
    }

    private IEnumerator ShowStartPanel()
    {
        loadingPanel.SetActive(false);
        startPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        startPanel.SetActive(false);
    }

    public void GameOver()
    {
        if (!isLoaded)
            return;
        isLoaded = false;
        resultPanel.SetActive(true);
        resultText.text = $"{loadedPlayers}등!";
        photonView.RPC("SubPlayerCount", RpcTarget.Others);
    }

    [PunRPC]
    private void SubPlayerCount()
    {
        if (!isLoaded)
            return;
        --loadedPlayers;
        if (loadedPlayers < 2)
            Win();
    }

    public void Win()
    {
        resultPanel.SetActive(true);
        resultText.text = $"1등!";
    }

    public void LeaveGame()
    {
        photonView.RPC("SubPlayerCount", RpcTarget.Others);
        loadingPanel.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    [PunRPC]
    public void RebootGrid()
    {
        if (gridRebooted)
            return;
        gridRebooted = true;
        grid.SetActive(false);
        grid.SetActive(true);
    }
}
