using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public static readonly int HashMove = Animator.StringToHash("isMoving");
    public static readonly int HashEndAttack = Animator.StringToHash("endAttack");

    private VirtualJoystick moveStick;
    private VirtualJoystick basicAttackStick;
    private VirtualJoystick skillAttackStick;
    private Slider skillAvailability;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private BasicAttackController basicController;
    [SerializeField] private SkillAttackController skillController;

    [SerializeField] private string basicAttackAnimName;
    [SerializeField] private bool basicAttackAnimIsLoop;
    [SerializeField] private string skillAttackAnimName;
    [SerializeField] private bool skillAttackAnimIsLoop;

    public int level { get; private set; } = 0;
    public PlayerLevelUi levelUi;
    public PlayerNameUi nameUi;

    public Transform attackPivot;
    private bool isAttacking = false;

    public Vector3 clonePos;
    public Quaternion cloneRot;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        moveStick = GameObject.FindWithTag("MoveJoyStick").GetComponent<VirtualJoystick>();
        basicAttackStick = GameObject.FindWithTag("AttackJoyStick").GetComponent<VirtualJoystick>();
        skillAttackStick = GameObject.FindWithTag("SkillJoyStick").GetComponent<VirtualJoystick>();
        skillAvailability = GameObject.FindWithTag("SkillAvailability").GetComponent<Slider>();
    }

    private void Start()
    {
        if (!photonView.IsMine)
            return;
        SetNameUiOnServer();
        basicController.attack.EndAttack = EndAttack;
        basicController.attack.DoAttack = BasicAttackAnimPlay;
        skillController.attack.EndAttack = EndAttack;
        skillController.attack.DoAttack = SkillAttackAnimPlay;
        skillController.Attackable = TakeSkillInput;

        //moveStick.OnStickDrag.AddListener((dir) => { Move(dir.normalized) });
        basicAttackStick.OnStickDrag.AddListener(basicController.ShowAttackRange);
        basicAttackStick.OnStickUp.AddListener((x, y) => { basicController.StopShowAttackRange(); });
        basicAttackStick.OnStickUp.AddListener(BasicAttack);
        skillAttackStick.OnStickDrag.AddListener(skillController.ShowAttackRange);
        skillAttackStick.OnStickUp.AddListener((x, y) => { skillController.StopShowAttackRange(); });
        skillAttackStick.OnStickUp.AddListener(SkillAttack);
        skillAttackStick.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;
        skillAvailability.value = skillController.gaugeRatio;
        Move();
    }

    private void Move()
    {
        if (photonView.IsMine)
        {
            if (playerInput.isMoving)
            {
                animator.SetBool(HashMove, true);
                var dir = new Vector3(playerInput.moveH, 0f, playerInput.moveV).normalized;
                rb.velocity = dir * speed;
                if (!isAttacking)
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir),
                        Time.deltaTime * 180f / Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) * rotateSpeed);
            }
            else
            {
                animator.SetBool(HashMove, false);
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, clonePos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Slerp(transform.rotation, cloneRot, Time.deltaTime * 10f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            clonePos = (Vector3)stream.ReceiveNext();
            cloneRot = (Quaternion)stream.ReceiveNext();
        }
    }

    public void TakeSkillInput(bool active)
    {
        skillAttackStick.gameObject.SetActive(active);
        skillAvailability.gameObject.SetActive(!active);
    }

    public void BasicAttack(Vector3 dir, float ratio)
    {
        if (isAttacking || !basicController.attackable)
            return;
        transform.rotation = Quaternion.LookRotation(dir);
        isAttacking = true;
        basicController.ExecuteAttack(attackPivot, dir, level, ratio);
    }

    public void BasicAttackAnimPlay()
    {
        if (basicAttackAnimIsLoop &&
            animator.GetCurrentAnimatorStateInfo(0).IsName(basicAttackAnimName))
            return;
        animator.Play(basicAttackAnimName);
    }

    public void SkillAttack(Vector3 dir, float ratio)
    {
        if (isAttacking || !skillController.attackable)
            return;
        transform.rotation = Quaternion.LookRotation(dir);
        isAttacking = true;
        skillController.ExecuteAttack(attackPivot, dir, level, ratio);
    }

    public void SkillAttackAnimPlay()
    {
        if (skillAttackAnimIsLoop &&
            animator.GetCurrentAnimatorStateInfo(0).IsName(skillAttackAnimName))
            return;
        animator.Play(skillAttackAnimName);
    }

    private void EndAttack()
    {
        isAttacking = false;
        animator.SetTrigger(HashEndAttack);
    }

    public void LevelUpOnServer() => photonView.RPC("LevelUp", RpcTarget.All);

    [PunRPC]
    public void LevelUp()
    {
        levelUi.SetLevel(++level);
        GetComponent<PlayerHealth>().MaxHpUp();
    }

    public void SetNameUiOnServer() => photonView.RPC("SetNameUi", RpcTarget.All, PlayDataManager.instance.playerName);

    [PunRPC]
    public void SetNameUi(string name) => nameUi.Set(name);
}
