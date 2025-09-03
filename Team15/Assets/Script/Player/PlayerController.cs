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

    [Header("Interact Settings")]
    [SerializeField] float interactRadius = 1f;
    [SerializeField] LayerMask interactMask;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private bool ismoving = false;
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
        if (DialogueUI.Instance && DialogueUI.Instance.IsOpen)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        // �뽬 ó�� (�뽬�� Update���� ����)
        if (isDashing)
        {
            dashTime += Time.deltaTime;
            float movedDistance = Vector2.Distance(rb.position, dashStartPos);
            playerCondition.state = PlayerState.Dash;
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
        // ���콺 ��ġ�� ���� ��������Ʈ ������
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        float mouseX = mouseWorldPos.x;
        float playerX = transform.position.x;

        if (mouseX < playerX)
        {
            // ����: x���� -1��
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // ������: x���� +1��
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void FixedUpdate()
    {
        if (DialogueUI.Instance && DialogueUI.Instance.IsOpen) return;

        if (!isDashing)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

            // ���� ����
            if (jumpPressed && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                jumpTimeCounter = jumpHoldTime;
                jumpPressed = false;
                playerCondition.state = PlayerState.Jump;
            }

            // ���� ����
            if (isJumping && jumpHeld)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.AddForce(Vector2.up * jumpHoldForce, ForceMode2D.Force);
                    jumpTimeCounter -= Time.fixedDeltaTime;
                }
                else
                {
                    isJumping = false;

                    if(!ismoving)
                        playerCondition.state = PlayerState.Idle;
                }
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (DialogueUI.Instance && DialogueUI.Instance.IsOpen) return;

        moveInput = context.ReadValue<Vector2>();

        if (moveInput.x != 0 && isGrounded && !isDashing)
        {
            playerCondition.state = PlayerState.Move;
            ismoving = true;
        }
        else if (isGrounded && !isDashing)
        {
            playerCondition.state = PlayerState.Idle;
            ismoving = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (DialogueUI.Instance && DialogueUI.Instance.IsOpen) return;

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
        if (DialogueUI.Instance && DialogueUI.Instance.IsOpen) return;

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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (DialogueUI.Instance && DialogueUI.Instance.IsOpen)
        {
            DialogueUI.Instance.AdvanceOrClose();
            return;
        }

        var target = GetNearestInteractable();
        if (target != null)
        {
            var player = GetComponent<Player>();
            target.Interact(player);
        }
    }

    IInteractable GetNearestInteractable()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactMask);
        IInteractable best = null;
        float bestDist = float.MaxValue;

        foreach (var h in hits)
        {
            if (!h) continue;
            var cand = h.GetComponent<IInteractable>() ?? h.GetComponentInParent<IInteractable>();
            if (cand == null) continue;

            float d = ((Vector2)h.transform.position - (Vector2)transform.position).sqrMagnitude;
            if (d < bestDist) { bestDist = d; best = cand; }
        }
        return best;
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
