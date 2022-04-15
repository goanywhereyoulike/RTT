using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerKeyBoardGamePad : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float normalSpeed = 5.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [SerializeField] private float turnSpeed = 15;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool canSprint = true;
    public KeyCode sprintHotkey = KeyCode.LeftShift;
    [SerializeField] private bool canDash = true;
    public bool dashTrigger = false;
    public bool BeenDeteced = false;
    public KeyCode dashHotkey = KeyCode.Space;
    public float dashStatrTime;
    public float dashEndTime = -10.0f;
    public float dashSpeed = 150.0f;
    public float dashTime = 0.25f;
    public float dashCoolDown = 5.0f;
    private Vector3 dashDir;
    private Vector3 input;
    private Vector3 rawInput;
    [SerializeField] private Animator playerAnimator;

    public KeyCode attackHotkey = KeyCode.J;
    public bool attackTrigger = false;
    public float attackStatrTime;
    public float attackEndTime = -10.0f;
    public float attackTime = 0.25f;
    public float attackCoolDown = 5.0f;
    public GameObject attackHitbox;
    public int attackDamage = 20;
    public int stealthDamage = 120;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.magnitude == 0.0f)
            rb.velocity = Vector3.zero;
        GetInput();
        LookDir();
        if (BeenDeteced)
            attackHitbox.GetComponent<Damage>().damageAmount = attackDamage;
        else
            attackHitbox.GetComponent<Damage>().damageAmount = stealthDamage;
        attackHitbox.SetActive(attackTrigger);
        if (canSprint)
            Sprint();
        SetAnimation();
    }

    void SetAnimation()
    {
        bool isMoving = rb.velocity.magnitude > 0;
        bool isRun = moveSpeed == sprintSpeed && isMoving;
        playerAnimator.SetBool("IsWalk", isMoving);
        playerAnimator.SetBool("IsRun", isRun); 
        playerAnimator.SetBool("IsRoll", dashTrigger);
        playerAnimator.SetBool("IsSlash", attackTrigger);
        playerAnimator.SetFloat("SlashSpeed", 1.5f / attackTime);
    }

    void GetInput()
    {
        if (Time.time >= attackEndTime)
            attackTrigger = false;
        //Move Direction Input
        if(!dashTrigger && !attackTrigger)
        {
            rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            input = rawInput.normalized;
        }

        //Dash Input
        if (Input.GetKeyDown(dashHotkey) && Time.time > dashEndTime + dashCoolDown)
        {
            dashTrigger = true;
            dashStatrTime = Time.time;
            dashEndTime = Time.time + dashTime;
        }

        if (!dashTrigger && Input.GetKeyDown(attackHotkey) && Time.time > attackEndTime + attackCoolDown)
        {
            attackTrigger = true;
            attackStatrTime = Time.time;
            attackEndTime = Time.time + attackTime;
        }


    }

    void Sprint()
    {
        if (Input.GetKey(sprintHotkey))
            moveSpeed = sprintSpeed;
        else
            moveSpeed = normalSpeed;
    }
    void Dash()
    {
        if (Time.time < dashStatrTime + dashTime)
        {
            rb.MovePosition(transform.position + transform.forward * dashSpeed * Time.deltaTime);
        }
        else
        {
            dashTrigger = false;
        }
    }

    void LookDir()
    {
        if (input != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0.0f,45.0f,0.0f));

            var modifiedInput = matrix.MultiplyPoint3x4(rawInput);

            var relativeV = (transform.position + modifiedInput) - transform.position;
            var rot = Quaternion.LookRotation(relativeV, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (dashTrigger)
        {
            Dash();
        }
        else if (attackTrigger)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.MovePosition(transform.position + transform.forward * input.magnitude * moveSpeed * Time.deltaTime);
        }
    }
}
