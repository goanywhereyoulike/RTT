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
    public float dashSpeed = 150.0f;
    public float dashTime = 0.25f;
    private Vector3 dashDir;
    private Vector3 input;
    private Vector3 rawInput;
    [SerializeField] private Animator playerAnimator;
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
        if (canSprint)
            Sprint();
        SetAnimation();
    }

    void SetAnimation()
    {
        bool isMoving = rb.velocity.magnitude > 0;
        bool isRun = moveSpeed == sprintSpeed && isMoving;
        playerAnimator.SetBool("IsWalk", isMoving);
        playerAnimator.SetBool("IsRun", isRun || dashTrigger);
    }

    void GetInput()
    {
        //Move Direction Input
        if(!dashTrigger)
        {
            rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            input = rawInput.normalized;
        }

        //Dash Input
        if (Input.GetKeyDown(dashHotkey))
        {
            dashTrigger = true;
            dashStatrTime = Time.time;
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
            playerAnimator.SetBool("IsRun", true);
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
        else
        {
            rb.MovePosition(transform.position + transform.forward * input.magnitude * moveSpeed * Time.deltaTime);
        }
    }
}
