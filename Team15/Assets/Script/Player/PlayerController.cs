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

    [Header("Jump (Accessory)")]
    public int extraAirJumps = 0; // 추가 공중 점프 가능 횟수
    int airJumpsUsed = 0;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float dashMaxDistance = 5f; // 최대 대쉬 거리

    [Header("Interact Settings")]
    [SerializeField] float interactRadius = 1f;
    [SerializeField] LayerMask interactMask;

    [Header("Weapon Swap (Dungeon)")]
    [SerializeField] Vector2 swapDropOffset = new Vector2(0.4f, 0.2f);
    [SerializeField] float swapDropImpulse = 3.5f;
    [SerializeField] float swapDropTorque = 5f;

    [Header("Pickup Settings")]
    [SerializeField] LayerMask pickupMask = ~0;
    [SerializeField] float pickMaxDist = 1.25f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private bool ismoving = false;
    private bool isGrounded;
    private bool jumpPressed;
    private bool isJumping;
    private float jumpTimeCounter;
    private bool jumpHeld;

    public bool isDashing = false;
    private float dashTime = 0f;
    private float lastDashTime = -999f;
    private Vector2 dashDirection;
    private float dashTargetDistance = 0f;
    private Vector2 dashStartPos;

    private PlayerCondition playerCondition;

    public Transform weaponHolder;
    [SerializeField] private float handRadius = 0.5f; // 플레이어 중심에서 핸드까지의 거리
    private Transform currentWeapon;
    private bool isWeaponThrown = false;
    private float weaponThrowTime = 0f;
    private float weaponThrowSpeed = 30f;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCondition = GetComponent<PlayerCondition>();
    }

    void Update()
    {
        // 대화 UI가 열려있으면 이동만 멈춤
        if (DialogueUI.Instance && DialogueUI.Instance.IsOpen)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        // 대쉬 처리 (정해진 거리만큼)
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

        // 애니메이션(스프라이트) flip 처리
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.flipX = (flip == -1);

        if (weaponHolder != null)
        {
            Vector2 dir = (mouseWorldPos - transform.position);
            float angleRad = Mathf.Atan2(dir.y, dir.x);
            float angleDeg = angleRad * Mathf.Rad2Deg;

            Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * handRadius;
            weaponHolder.position = (Vector2)transform.position + offset;

            // flip에 따라 무기 localScale.x도 뒤집기
            weaponHolder.localScale = new Vector3(flip, 1f, 1f);

            if (flip == -1)
                weaponHolder.rotation = Quaternion.Euler(180f, 0f, -angleDeg);
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

        if (context.performed && !isDashing)
        {
            if (isGrounded)
            {
                jumpPressed = true;
                jumpHeld = true;
                airJumpsUsed = 0;
            }
            else if (airJumpsUsed < extraAirJumps)
            {
                jumpPressed = true;
                jumpHeld = true;
                airJumpsUsed = 0;
            }
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
            AudioManager.Instance.PlaySFX("DashSFX");
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
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && weaponHolder.childCount > 0)
        {
            var weaponObj = weaponHolder.GetComponentInChildren<WeaponObject>();
            Debug.Log(weaponObj);
            if (weaponObj == null) return;

            // 공격 방향: 마우스 방향
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector2 attackDir = (mouseWorldPos - transform.position).normalized;

            // 공격 시작 위치: 플레이어 위치
            weaponObj.TakeAttack(transform.position, attackDir);
        }
    }

    void DropCurrentWeaponToGround(Vector2 preferredDir)
    {
        if (weaponHolder == null || weaponHolder.childCount == 0) return;

        var old = weaponHolder.GetChild(0);
        old.SetParent(null);

        // 위치 살짝 옆/위
        old.position = (Vector2)transform.position + swapDropOffset;

        // 픽업 가능 켜기
        var oldEq = old.GetComponent<EquipableObject>();
        if (oldEq)
        {
            oldEq.isEquipable = true;
            if (oldEq.objectUI) oldEq.objectUI.SetActive(true);
        }

        var rb2d = old.GetComponent<Rigidbody2D>();
        if (rb2d)
        {
            rb2d.simulated = true;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;

            Vector2 dir = preferredDir.sqrMagnitude > 0.0001f
                ? preferredDir.normalized
                : Vector2.right * Mathf.Sign(transform.localScale.x);

            rb2d.AddForce((dir * 0.6f + Vector2.up * 0.8f) * swapDropImpulse, ForceMode2D.Impulse);
            rb2d.AddTorque(swapDropTorque, ForceMode2D.Impulse);
        }
    }

    public void OnPickWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        var hits = Physics2D.OverlapCircleAll(transform.position, pickMaxDist, pickupMask);

        EquipableObject nearest = null;
        float minSqr = float.MaxValue;

        foreach (var h in hits)
        {
            if (!h) continue;
            var eq = h.GetComponentInParent<EquipableObject>();
            if (eq == null) continue;

            if (weaponHolder && eq.transform.IsChildOf(weaponHolder)) continue;

            if (!eq.isEquipable) continue;

            float sqr = ((Vector2)eq.transform.position - (Vector2)transform.position).sqrMagnitude;
            if (sqr < minSqr)
            {
                minSqr = sqr;
                nearest = eq;
            }
        }

        if (nearest == null)
        {
            // [ADDED] 디버그: 마스크/레이어/Collider 누락 시 확인
            Debug.Log("[OnPickWeapon] 주울 무기를 찾지 못함. pickupMask/레이어/Collider2D 확인 필요");
            return;
        }

        var gs = GameState.Instance;
        bool inDungeon = gs != null && gs.inDungeon;

        if (weaponHolder.childCount > 0)
        {
            if (inDungeon)
            {
                Vector2 preferDir = (nearest.transform.position - transform.position);
                DropCurrentWeaponToGround(preferDir);
            }
            else
            {
                Destroy(weaponHolder.GetChild(0).gameObject);
            }
        }

        AudioManager.Instance.PlaySFX("GetWeaponSFX");

        if (nearest.objectUI) nearest.objectUI.SetActive(false);
        nearest.isEquipable = false;
        nearest.transform.SetParent(weaponHolder.transform);
        nearest.transform.localPosition = Vector3.zero;

        float scaleSign = Mathf.Sign(transform.localScale.x);
        nearest.transform.localScale = new Vector3(
            scaleSign * Mathf.Abs(nearest.transform.localScale.x),
            nearest.transform.localScale.y,
            nearest.transform.localScale.z
        );
        nearest.transform.localRotation = scaleSign < 0
            ? Quaternion.Euler(0f, 180f, 0f)
            : Quaternion.identity;

        var rb2d = nearest.GetComponent<Rigidbody2D>();
        if (rb2d) rb2d.simulated = false;

        isWeaponThrown = false;
        currentWeapon = nearest.transform;
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

            AudioManager.Instance.PlaySFX("ThrowSFX");
            rb2d.AddForce(throwDir * weaponThrowSpeed, ForceMode2D.Impulse);
            rb2d.AddTorque(5f, ForceMode2D.Impulse);

            isWeaponThrown = true;
            weaponThrowTime = 0f;

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

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Time.timeScale = 0f;
            UIManager.Instance.Show<PauseUI>();
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
            airJumpsUsed = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void AddExtraAirJumps(int count) { extraAirJumps += count; if (extraAirJumps < 0) extraAirJumps = 0; }
    public void AddMoveSpeed(float delta) { moveSpeed += delta; if (moveSpeed < 0f) moveSpeed = 0f; }
    public void MultiplyDashCooldown(float m) { dashCooldown *= m; if (dashCooldown < 0.01f) dashCooldown = 0.01f; }
}
