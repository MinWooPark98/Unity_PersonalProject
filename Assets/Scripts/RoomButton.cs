using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI playTime;
    [SerializeField] private TextMeshProUGUI map;
    [SerializeField] private TextMeshProUGUI participants;

    private IObjectPool<RoomButton> pool;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(JoinRoom);
    }

    public void SetPool(IObjectPool<RoomButton> pool) => this.pool = pool;

    public void Set(string roomName, string playTime, string map, string participants)
    {
        this.roomName.text = roomName;
        this.playTime.text = playTime;
        this.map.text = map;
        UpdateParticipants(participants);
    }

    public void UpdateParticipants(string participants)
    {
        this.participants.text = participants;
    }

    private void JoinRoom() => PhotonNetwork.JoinRoom(roomName.text);
}
