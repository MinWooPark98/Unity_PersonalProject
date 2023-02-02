using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
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
    public GameObject playerPrefab;   // �ӽ� - �¶������� �ٲٸ� ĳ���͵� prefab ���� ���� �Ŀ� �÷��̾���� ������ ĳ���� ����
    public LinkedList<GameObject> players = new LinkedList<GameObject>();
    public GameObject frontArea;
    public GameObject rangeArea;
    public SpriteRenderer throwArea;
    public LineRenderer throwLine;
    public Vector2 mapSize;
    public ShowDamageLancher showDamageLauncher;

    private void Awake()
    {
        playTimer = 0f;
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        players.AddLast(player);
    }

    private void Update()
    {
        playTimer += Time.deltaTime;
    }
}
