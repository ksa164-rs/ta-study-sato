using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController controller;

    [SerializeField] float walkSpeed = 4.2f;
    [SerializeField] float runSpeed = 6.8f;

    [SerializeField] float jumpForce = 6f;
    [SerializeField] float gravity = -30f;

    Vector3 velocity;
    Vector3 horizontalVelocity;

    bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        move = move.normalized;

        if (isGrounded)
        {
            horizontalVelocity = move * speed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = horizontalVelocity + velocity;

        controller.Move(finalMove * Time.deltaTime);
    }
}