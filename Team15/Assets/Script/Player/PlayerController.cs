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
    public float dashMaxDistance = 5f; // 최대 대쉬 거리

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

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private float handRadius = 0.5f; // 플레이어 중심에서 핸드까지의 거리
    private Transform currentWeapon;
    private bool isWeaponThrown = false;
    private float weaponThrowTime = 0f;
    private float weaponThrowSpeed = 20f;


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

        // 대쉬 처리 (대쉬는 Update에서 유지)
        if (isDashing)
        {
            dashTime += Time.deltaTime;
            float movedDistance = Vector2.Distance(rb.position, dashStartPos);
            playerCondition.state = AnimationState.Dash;
            if (movedDistance >= dashTargetDistance || dashTime >= dashDuration)
            {
                isDashing = false;
                rb.velocity = Vector2.zero;
                playerCondition.state = AnimationState.Idle;
            }
            else
            {
                rb.velocity = dashDirection * dashSpeed;
            }
        }

        // 마우스 방향에 따라 flip 결정
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        float mouseX = mouseWorldPos.x;
        float playerX = transform.position.x;
        int flip = mouseX < playerX ? -1 : 1;

        // 플레이어 스프라이트 뒤집기
        transform.localScale = new Vector3(flip * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (weaponHolder != null)
        {
            Vector2 dir = (mouseWorldPos - transform.position);
            float angleRad = Mathf.Atan2(dir.y, dir.x);
            float angleDeg = angleRad * Mathf.Rad2Deg;

            // 원 궤도 위치 계산 (마우스 방향 기준, flip과 무관)
            Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * handRadius;
            weaponHolder.position = (Vector2)transform.position + offset;

            // 핸드 회전: flip에 따라 Y축 180도, Z축 각도 반전
            if (flip == -1)
                weaponHolder.rotation = Quaternion.Euler(0f, 180f, -angleDeg);
            else
                weaponHolder.rotation = Quaternion.Euler(0f, 0f, angleDeg);
        }
    }

    void FixedUpdate()
    {
        if (DialogueUI.Instance && DialogueUI.Instance.IsOpen) return;

        if (!isDashing)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

            // 점프 시작
            if (jumpPressed && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                jumpTimeCounter = jumpHoldTime;
                jumpPressed = false;
                playerCondition.state = AnimationState.Jump;
            }

            // 점프 유지
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

                    if (!ismoving)
                        playerCondition.state = AnimationState.Idle;
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
            playerCondition.state = AnimationState.Move;
            ismoving = true;
        }
        else if (isGrounded && !isDashing)
        {
            playerCondition.state = AnimationState.Idle;
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
            direction.Normalize();

            dashDirection = direction;
            dashStartPos = rb.position;
            dashTargetDistance = dashMaxDistance; // 항상 최대 거리만큼 대쉬

            isDashing = true;
            dashTime = 0f;
            lastDashTime = Time.time;
        }
    }


    public void OnPickWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EquipableArea[] equipables = FindObjectsOfType<EquipableArea>();
            if (equipables.Length == 0) return;

            EquipableArea nearest = null;
            float minDist = float.MaxValue;
            Vector3 playerPos = transform.position;

            foreach (var obj in equipables)
            {
                float dist = Vector3.Distance(playerPos, obj.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = obj;
                }
            }

            if (nearest != null)
            {
                if (nearest.isEquipable)
                {
                    nearest.objectUI.SetActive(false);
                    nearest.isEquipable = false;
                    nearest.transform.SetParent(weaponHolder.transform);
                    nearest.transform.localPosition = Vector3.zero;

                    // flip 상태에 따라 무기 회전/스케일 초기화
                    float flip = Mathf.Sign(transform.localScale.x);
                    nearest.transform.localScale = new Vector3(flip * Mathf.Abs(nearest.transform.localScale.x), nearest.transform.localScale.y, nearest.transform.localScale.z);

                    // 무기 회전도 flip에 맞게 초기화
                    nearest.transform.localRotation = flip < 0
                        ? Quaternion.Euler(0f, 180f, 0f)
                        : Quaternion.identity;

                    nearest.GetComponent<Rigidbody2D>().simulated = false;

                    isWeaponThrown = false;
                    currentWeapon = nearest.transform;
                }
            }
        }
    }

    public void OnThrowWeapon(InputAction.CallbackContext context)
    {
        if (context.performed && !isWeaponThrown && weaponHolder.childCount > 0)
        {
            currentWeapon = weaponHolder.GetChild(0);
            var rb2d = currentWeapon.GetComponent<Rigidbody2D>();
            if (rb2d == null) return;

            currentWeapon.SetParent(null);
            rb2d.simulated = true;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;

            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector2 throwDir = (mouseWorldPos - transform.position).normalized;

            rb2d.AddForce(throwDir * weaponThrowSpeed, ForceMode2D.Impulse);
            rb2d.AddTorque(5f, ForceMode2D.Impulse);

            isWeaponThrown = true;
            weaponThrowTime = 0f;

            // 던진 후 currentWeapon 참조 해제 (필요시)
            currentWeapon = null;
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
