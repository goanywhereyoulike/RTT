using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerKeyBoardGamePad : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 15;
    [SerializeField] private Rigidbody rb;
    private Vector3 input;
    private Vector3 rawInput;

    // Update is called once per frame
    void Update()
    {
        if (input.magnitude == 0.0f)
            rb.velocity = Vector3.zero;
        GetInput();
        LookDir();
    }

    void GetInput()
    {
        rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = rawInput.normalized;
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
        rb.MovePosition(transform.position + transform.forward * input.magnitude * moveSpeed * Time.deltaTime);
    }
}
