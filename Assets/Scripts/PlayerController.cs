using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static readonly int HashMove = Animator.StringToHash("isMoving");
    public static readonly int HashEndAttack = Animator.StringToHash("endAttack");

    public VirtualJoystick moveStick;
    public VirtualJoystick basicAttackStick;
    public VirtualJoystick skillAttackStick;
    public Slider skillAvailability;

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

    public Transform attackPivot;
    private bool isAttacking = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        basicController.attack.EndAttack = EndAttack;
        basicController.attack.DoAttack = BasicAttackAnimPlay;
        //skillController.attack.EndAttack = EndAttack;
        //skillController.attack.DoAttack = SkillAttackAnimPlay;

        skillController.Attackable = TakeSkillInput;
    }

    void Update()
    {
        skillAvailability.value = skillController.gaugeRatio;
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
        basicController.ExecuteAttack(attackPivot, dir, ratio);
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
        skillController.ExecuteAttack(attackPivot, dir, ratio);
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
}
