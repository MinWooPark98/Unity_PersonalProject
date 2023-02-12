using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
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
    public Characters character { get; private set; } = Characters.Bear;
    public Animator[] characterAnimators;
    public LobbyCharacterCamera characterCamera;

    private string gameVersion = "1";
    public Button participateButton;
    public TextMeshProUGUI connectMessage;

    public GameObject characterPanel;
    public GameObject characterDescPanel;
    public GameObject attackInfoPanel;
    public GameObject lobbyPanel;
    public GameObject createPanel;
    public GameObject loadingPanel;
    public Button startButton;

    private bool leftRoom = false;

    // 룸 생성 시에만 값 찾아옴
    public TMP_InputField roomName;
    public TMP_Dropdown maxPlayersDropdown;
    public TMP_Dropdown playTimeDropdown;
    //public Dropdown map;

    public GameObject rooms;
    public RoomButton roomPrefab;
    public Dictionary<string, RoomButton> roomDict = new Dictionary<string, RoomButton>();
    private IObjectPool<RoomButton> roomPool;

    private RoomButton CreateRoomButton()
    {
        RoomButton room = Instantiate(roomPrefab);
        room.transform.parent = rooms.transform;
        room.SetPool(roomPool);
        return room;
    }
    private void OnGetRoomButton(RoomButton room) => room.gameObject.SetActive(true);
    private void OnReleaseRoomButton(RoomButton room) => room.gameObject.SetActive(false);
    private void OnDestroyRoomButton(RoomButton room) => Destroy(room.gameObject);

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        participateButton.interactable = false;

        roomPool = new ObjectPool<RoomButton>(CreateRoomButton, OnGetRoomButton, OnReleaseRoomButton, OnDestroyRoomButton);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    public void ActivateCharacterPanel() => characterPanel.SetActive(true);
    public void ActivateCharacterDescPanel() => characterDescPanel.SetActive(true);
    public void ActivateAttackInfoPanel() => attackInfoPanel.SetActive(true);
    public void ActivateCreatePanel() => createPanel.SetActive(true);

    public void Cancel()
    {
        if (loadingPanel.activeSelf)
            LeaveRoom();
        else if (createPanel.activeSelf)
            createPanel.SetActive(false);
        else if (lobbyPanel.activeSelf)
            LeaveLobby();
        else if (attackInfoPanel.activeSelf)
            attackInfoPanel.SetActive(false);
        else if (characterDescPanel.activeSelf)
            characterDescPanel.SetActive(false);
        else if (characterPanel.activeSelf)
        {
            characterPanel.SetActive(false);
            characterCamera.ShowCharacter((int)character);
        }
    }

    public void Home()
    {
        characterPanel.SetActive(false);
        characterDescPanel.SetActive(false);
        attackInfoPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        createPanel.SetActive(false);
        loadingPanel.SetActive(false);
        characterCamera.ShowCharacter((int)character);
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    public void ShowIndexCharacter(int index)
    {
        if (index < 0 || index > (int)Characters.Count - 1)
            return;
        characterCamera.ShowCharacter(index);
        characterAnimators[index].SetTrigger($"Action{UnityEngine.Random.Range(1, 4)}");
    }

    public void SetCharacter(Characters character) => this.character = character;

    public void JoinRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRandomRoom();
        else
            PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateCustomRoom()
    {
        var maxPlayers = byte.Parse(maxPlayersDropdown.options[maxPlayersDropdown.value].text);
        var playTime = int.Parse(playTimeDropdown.options[playTimeDropdown.value].text);
        CreateRoom(maxPlayers, playTime, false);
    }

    public void CreateRandomRoom()
    {
        CreateRoom(6, 180, true);
    }

    private void CreateRoom(byte maxPlayers, int playTime, bool isRandomRoom)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = maxPlayers;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "PlayTime", playTime } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "PlayTime" };
        //roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Map", map } };
        //roomOptions.CustomRoomPropertiesForLobby = new string[] { "Map" };
        PhotonNetwork.CreateRoom(isRandomRoom ? string.Empty : roomName.text, roomOptions: roomOptions);
        Debug.Log("Create Room");
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }
    public void JoinRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    [PunRPC]
    public void SaveSelectedData()
    {
        PlayDataManager.instance.character = character;
        PlayDataManager.instance.playTime = 
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("PlayTime") ? (int)PhotonNetwork.CurrentRoom.CustomProperties["PlayTime"] : 180f;
    }

    public void EnterGame()
    {
        photonView.RPC("SaveSelectedData", RpcTarget.All);
        PhotonNetwork.IsMessageQueueRunning = false;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("PlayScene");
        }
    }

    public override void OnConnectedToMaster()
    {
        participateButton.interactable = true;
        Debug.Log("Connect");
        if (leftRoom)
            JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        participateButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Disconnect");
    }

    public override void OnJoinedLobby()
    {
        leftRoom = false;
        lobbyPanel.SetActive(true);
        Debug.Log("Joined Lobby");
    }

    public override void OnLeftLobby()
    {
        lobbyPanel.SetActive(false);
        Debug.Log("Left Lobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRandomRoom();
        Debug.Log(message + "\nCreate Room");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("CreateFailed");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room : { PhotonNetwork.CurrentRoom.Name }");
        loadingPanel.SetActive(true);
        connectMessage.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    
        if (PhotonNetwork.IsMasterClient)
            startButton.gameObject.SetActive(true);
        else
            startButton.gameObject.SetActive(false);

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            EnterGame();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Join fail");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("LeftRoom");
        leftRoom = true;
        createPanel.SetActive(false);
        loadingPanel.SetActive(false);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("PlayerLeft");
        connectMessage.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        connectMessage.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            EnterGame();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomButton roomButton = null;
        foreach (var room in roomList)
        {
            if (room.RemovedFromList || !room.IsOpen)
            {
                roomDict.TryGetValue(room.Name, out roomButton);
                if (roomButton != null)
                {
                    roomPool.Release(roomButton);
                    roomDict.Remove(room.Name);
                }
            }

            else
            {
                if (!roomDict.ContainsKey(room.Name))
                {
                    roomButton = roomPool.Get();
                    Debug.Log(room.Name);
                    roomButton.Set(
                        room.Name,
                        room.CustomProperties.ContainsKey("PlayTime") ? room.CustomProperties["PlayTime"].ToString() : null,
                        "고정 단일맵",   // 아직 맵 1개라 고정 단일맵으로 출력
                        $"{room.PlayerCount} / {room.MaxPlayers}");
                    roomDict.Add(room.Name, roomButton);
                }
                else
                {
                    roomDict.TryGetValue(room.Name, out roomButton);
                    roomButton.UpdateParticipants($"{room.PlayerCount} / {room.MaxPlayers}");
                }
            }
        }
    }
}