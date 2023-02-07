using Photon.Pun;
using Photon.Realtime;
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
    public int characterIndex { get; private set; } = 0;
    public Animator[] characterAnimators;
    public Camera characterCamera;

    private string gameVersion = "1";
    public Button participateButton;
    public TextMeshProUGUI connectMessage;

    public GameObject lobbyPanel;
    public GameObject createPanel;
    public GameObject loadingPanel;

    // 룸 생성 시에만 값 찾아옴
    public TMP_InputField roomName;
    public TMP_Dropdown maxPlayersDropdown;
    private byte maxPlayers = 6;
    public TMP_Dropdown playTimeDropdown;
    private int playTime = 180;
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
        loadingPanel.SetActive(false);

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        participateButton.interactable = false;

        roomPool = new ObjectPool<RoomButton>(CreateRoomButton, OnGetRoomButton, OnReleaseRoomButton, OnDestroyRoomButton);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ShowPrevCharacter();
        if (Input.GetKeyDown(KeyCode.D))
            ShowNextCharacter();

        //if (PhotonNetwork.InRoom && PhotonNetwork.currenRooms.players == GameManager.instance.participants)
            //EnterGame();
        if (Input.GetKeyDown(KeyCode.F))
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
        characterAnimators[characterIndex].SetTrigger($"Action{UnityEngine.Random.Range(1, 4)}");
    }

    public void JoinRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRandomRoom();
        else
            PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        var maxPlayers = byte.Parse(maxPlayersDropdown.options[maxPlayersDropdown.value].text);
        var playTime = int.Parse(playTimeDropdown.options[playTimeDropdown.value].text);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = maxPlayers;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "PlayTime", playTime } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "PlayTime" };
        //roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Map", map } };
        //roomOptions.CustomRoomPropertiesForLobby = new string[] { "Map" };
        PhotonNetwork.CreateRoom(roomName.text, roomOptions: roomOptions);
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
        PlayDataManager.instance.character = (Characters)characterIndex;
        PlayDataManager.instance.playTime = playTime;
    }

    public void EnterGame()
    {
        photonView.RPC("SaveSelectedData", RpcTarget.All);
        PhotonNetwork.IsMessageQueueRunning = false;
        if (PhotonNetwork.IsMasterClient)
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

    public override void OnJoinedLobby()
    {
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
        PhotonNetwork.CreateRoom("TEST", new RoomOptions() { MaxPlayers = maxPlayers, IsVisible = true });
        Debug.Log(message + "\nCreate Room");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room : { PhotonNetwork.CurrentRoom.Name }");
        loadingPanel.SetActive(true);
        maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        playTime = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("PlayTime") ? (int)PhotonNetwork.CurrentRoom.CustomProperties["PlayTime"] : playTime;
        connectMessage.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Join fail");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left");
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
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomButton roomButton = null;
        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                roomDict.TryGetValue(room.Name, out roomButton);
                roomPool.Release(roomButton);
                roomDict.Remove(room.Name);
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