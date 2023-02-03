using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    private static LobbySceneManager m_instance;
    public static LobbySceneManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<LobbySceneManager>();
            return m_instance;
        }
    }

    public RawImage characterRenderer;
    public int characterIndex { get; private set; } = 0;
    public Animator[] characterAnimators;
    public Camera characterCamera;

    private string gameVersion = "1";
    public Button participateButton;
    public TextMeshProUGUI connectMessage;

    public GameObject loadingPanel;

    private void Start()
    {
        loadingPanel.SetActive(false);

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        participateButton.interactable = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ShowPrevCharacter();
        if (Input.GetKeyDown(KeyCode.D))
            ShowNextCharacter();

        if (PhotonNetwork.InRoom && PhotonNetwork.CountOfPlayersInRooms == GameManager.instance.participants)
            EnterGame();
        if (Input.GetKeyDown(KeyCode.Return))
            EnterGame();
    }

    public void ShowPrevCharacter() => ShowIndexCharacter(characterIndex - 1);
    public void ShowNextCharacter() => ShowIndexCharacter(characterIndex + 1);

    public void ShowIndexCharacter(int index)
    {
        if (index < 0 || index > (int)Characters.Count - 1)
            return;
        characterAnimators[characterIndex].SetTrigger("Idle");
        characterIndex = index;
        characterAnimators[characterIndex].SetTrigger($"Action{Random.Range(1, 4)}");
    }

    public void EnterGame()
    {
        PhotonNetwork.LoadLevel("PlayScene");
    }

    public override void OnConnectedToMaster()
    {
        participateButton.interactable = true;
        Debug.Log("Connect");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        participateButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Disconnect");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(string.Empty, new RoomOptions() { MaxPlayers = GameManager.instance.participants });
        Debug.Log(message + "\nCreate Room");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        Time.timeScale = 0f;
        loadingPanel.SetActive(true);
        connectMessage.text = PhotonNetwork.CountOfPlayers.ToString();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
            PhotonNetwork.ConnectUsingSettings();
    }

    public void Unjoin()
    {
        PhotonNetwork.LeaveRoom();
    }
}
