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

    public GameObject loadingPanel;

    // 룸 생성 시에만 값 찾아옴
    public TMP_InputField roomName;
    public TMP_Dropdown maxPlayersDropdown;
    private byte maxPlayers = 10;
    public TMP_Dropdown playTimeDropdown;
    private int playTime;
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
        PhotonNetwork.CreateRoom(string.Empty, new RoomOptions() { MaxPlayers = 10 });
        Debug.Log(message + "\nCreate Room");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left");
        loadingPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room : { PhotonNetwork.CurrentRoom.Name }");
        Time.timeScale = 0f;
        loadingPanel.SetActive(true);
        connectMessage.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        Debug.Log(PhotonNetwork.IsMasterClient);
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
                    RoomButton newButton = roomPool.Get();
                    newButton.Set(
                        room.Name,
                        room.CustomProperties.ContainsKey("PlayTime") ? room.CustomProperties["PlayTime"].ToString() : null,
                        "고정 단일맵",   // 아직 맵 1개라 고정 단일맵으로 출력
                        $"{room.PlayerCount} / {room.MaxPlayers}");
                    roomDict.Add(room.Name, newButton);
                }
                else
                {
                    roomDict.TryGetValue(room.Name, out roomButton);
                    roomButton.UpdateParticipants($"{room.PlayerCount} / {room.MaxPlayers}");
                }
            }
        }
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
        maxPlayers = byte.Parse(maxPlayersDropdown.options[maxPlayersDropdown.value].text);
        playTime = int.Parse(playTimeDropdown.options[playTimeDropdown.value].text);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "PlayTime", playTime } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "PlayTime" };
        //roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Map", map } };
        //roomOptions.CustomRoomPropertiesForLobby = new string[] { "Map" };
        PhotonNetwork.CreateRoom(roomName.text, roomOptions: roomOptions);
        Debug.Log("Create Room");
    }

    public void JoinRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);

    public void Unjoin() => PhotonNetwork.LeaveRoom();

    public void EnterGame()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("PlayScene");
    }
}