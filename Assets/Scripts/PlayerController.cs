using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static readonly int HashMove = Animator.StringToHash("isMoving");
    public static readonly int HashAttack = Animator.StringToHash("isAttacking");

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Attack attack;
    public Transform attackPivot;
    private bool isAttacking = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        //attack.EndAttack = SetAttacking;
    }

    void Update()
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

    public void Attack(Vector3 dir, float ratio)   // 임시 삭제 예정
    {
        SetAttacking(true);
        transform.rotation = Quaternion.LookRotation(dir);
        attack.Execute(attackPivot, dir, ratio);
    }

    private void SetAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
        animator.SetBool(HashAttack, isAttacking);
    }
}
