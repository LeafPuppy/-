using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Variable Jump Settings")]
    public float jumpHoldTime = 0.2f;
    public float jumpHoldForce = 5f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float dashMaxDistance = 5f; // �ִ� �뽬 �Ÿ�

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool jumpPressed;
    private bool isJumping;
    private float jumpTimeCounter;
    private bool jumpHeld;

    private bool isDashing = false;
    private float dashTime = 0f;
    private float lastDashTime = -999f;
    private Vector2 dashDirection;
    private float dashTargetDistance = 0f;
    private Vector2 dashStartPos;

    private PlayerCondition playerCondition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCondition = GetComponent<PlayerCondition>();
    }

    void Update()
    {
        // ���� ����
        if (jumpPressed && isGrounded && !isDashing)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            jumpTimeCounter = jumpHoldTime;
            jumpPressed = false;
            playerCondition.state = PlayerState.Jump;
        }

        // ���� ����
        if (isJumping && jumpHeld && !isDashing)
        {
            if (jumpTimeCounter > 0)
            {
                rb.AddForce(Vector2.up * jumpHoldForce, ForceMode2D.Force);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // ���� ��ư�� ���� �� ���� ���� ����
        if (!jumpHeld)
        {
            isJumping = false;
        }

        // �뽬 ó��
        if (isDashing)
        {
            dashTime += Time.deltaTime;
            float movedDistance = Vector2.Distance(rb.position, dashStartPos);
            playerCondition.state = PlayerState.Jump;
            // ��ǥ �Ÿ���ŭ �̵��߰ų�, ���ӽð��� ������ �뽬 ����
            if (movedDistance >= dashTargetDistance || dashTime >= dashDuration)
            {
                isDashing = false;
                rb.velocity = Vector2.zero;
                playerCondition.state = PlayerState.Idle;
            }
            else
            {
                rb.velocity = dashDirection * dashSpeed;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        }
        // �뽬 �߿��� rb.velocity�� Update���� �̹� �����ϹǷ� FixedUpdate������ ����
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput.x != 0 && isGrounded && !isDashing)
        {
            playerCondition.state = PlayerState.Move;
        }
        else if (isGrounded && !isDashing)
        {
            playerCondition.state = PlayerState.Idle;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isDashing)
        {
            jumpPressed = true;
            jumpHeld = true;
        }
        else if (context.canceled)
        {
            jumpHeld = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector2 direction = (mouseWorldPos - transform.position);
            float distance = direction.magnitude;
            direction.Normalize();

            dashDirection = direction;
            dashStartPos = rb.position;
            dashTargetDistance = Mathf.Min(distance, dashMaxDistance);

            isDashing = true;
            dashTime = 0f;
            lastDashTime = Time.time;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
