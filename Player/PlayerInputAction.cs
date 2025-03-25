using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력에 따른 플레이어 액션 컴포넌트
public class PlayerInputAction : MonoBehaviour
{
    PlayerInputMovement playerMovement;     // 플레이어 이동 컴포넌트 참조
    PlayerInputAttack playerAttack;         // 플레이어 공격 컴포넌트 참조
    PlayerSoundFx playerSoundFx;            // 플레이어 사운드 효과 컴포넌트 참조
    Damageable damageable;                  // 플레이어 피격 컴포넌트 참조
    Collider2D groundCollider;              // 아래점프 가능한 플랫폼 참조
    Rigidbody2D rigidbody2d;
    Animator animator;
    [SerializeField] UIManager uiManager;               // UI 매니저 참조
    [SerializeField] InventorySystem inventorySystem;   // 인벤토리 시스템 참조
    [SerializeField] InventoryUI inventoryUI;           // 인벤토리 UI 컴포넌트 참조

    [SerializeField] private bool canAction = true;     // 키 입력으로 액션이 가능한 상태인지
    [SerializeField] private bool canClimb = true;      // 사다리에 매달릴 수 있는 상태인지

    [SerializeField] private float invincibleTime;      // 액션 시 무적 시간
    private WaitForFixedUpdate waitForFixedUpdate;      // 액션 코루틴은 FixedUpdate와 같이 실행

    [Header("Jump")]
    [SerializeField] private bool isAirJumpItemAcquired = false;    // 공중 점프 아이템 획득 여부
    [SerializeField] private bool canAirJump = false;               // 공중 점프가 가능한 상태인지
    [SerializeField] private bool isLanding;                        // 착지 상태인지
    [SerializeField] private float jumpForce;                       // 점프 힘
    [SerializeField] private float jumpTime;                        // 점프 시간
    [SerializeField] private GameObject airJumpEffectPrefab;        // 공중 점프 이펙트
    [SerializeField] private float airJumpEffectDestroyTime;        // 공중 점프 이펙트 지속시간
    [SerializeField] private GameObject landingEffectPrefab;        // 착지 이펙트 
    [SerializeField] private float landingEffectDestroyTime;        // 착지 이펙트 지속시간
    private bool jumpRequested;             // 점프 트리거
    private IEnumerator jumpCoroutine;      // 사다리에 매달린 후 점프 시 일정 시간동안 매달릴 수 없게 하는 코루틴
    private bool isLandingEffect = false;   // 착지 이펙트 표시 여부

    [Header("WallJump")]
    [SerializeField] private bool isWallJumpItemAcquired = false;   // 벽 점프 아이템 획득 여부
    [SerializeField] private bool isWallJump;                       // 벽 점프 상태인지
    [SerializeField] private bool onJumpWall;                       // 벽 점프 가능한 벽에 붙어 있는지
    [SerializeField] private Vector2 wallJumpDirection;             // 벽 점프 시 점프 방향
    [SerializeField] private float directionFixTime;                // 벽 점프 시 방향 고정 시간
    private bool wallJumpRequested;         // 벽 점프 트리거
    private IEnumerator wallJumpCoroutine;  // 벽 점프 시 일정 시간 방향전환 불가 코루틴

    [Header("Slide")]
    [SerializeField] private bool isSlide;                          // 슬라이딩 중인지
    [SerializeField] private float slideSpeed;                      // 슬라이딩 속도
    [SerializeField] private float slideTime;                       // 슬라이딩 시간
    [SerializeField] GameObject slideEffectPrefab;                  // 슬라이딩 이펙트
    [SerializeField] float slideEffectDestroyTime;                  // 슬라이딩 이펙트 지속 시간
    [SerializeField] GameObject ghostObject;                        // 슬라이딩 / 대쉬 시 잔상 효과
    private bool slideRequested;            // 슬라이딩 트리거
    private IEnumerator slideCoroutine;     // 슬라이딩 실행 코루틴

    [Header("AirDash")]
    [SerializeField] private bool isAirDashItemAcquired = false;    // 공중 대쉬 아이템 획득 여부
    [SerializeField] private bool canDash = false;                  // 공중 대쉬가 가능한 상태인지
    [SerializeField] private bool isDash;                           // 공중 대쉬 상태인지
    [SerializeField] private float dashSpeed;                       // 공중 대쉬 속도
    [SerializeField] private float dashTime;                        // 공중 대쉬 시간
    [SerializeField] GameObject airDashEffectPrefab;                // 공중 대쉬 이펙트
    [SerializeField] float airDashEffectDestroyTime;                // 공중 대쉬 이펙트 지속 시간
    private bool dashRequested;             // 공중 대쉬 트리거
    private IEnumerator dashCoroutine;      // 공중 대쉬 실행 코루틴

    [Header("Guard")]
    [SerializeField] private bool isGuard;                          // 가드 상태인지
    [SerializeField] private bool canGuard = true;                  // 가드 가능한 상태인지
    [SerializeField] private float guardTime;                       // 가드 지속 시간
    [SerializeField] private float guardDelayTime;                  // 가드 후 딜레이
    [SerializeField] private GameObject guardEffectPrefab;          // 가드 이펙트
    [SerializeField] private float guardEffectDestroyTime;          // 가드 이펙트 지속 시간
    private bool guardRequested;            // 가드 트리거
    private IEnumerator guardCoroutine;     // 가드 실행 코루틴

    [Header("HP Potion")]
    [SerializeField] private int potionCount;                       // 포션 사용 가능 횟수
    [SerializeField] private int recoveryValue;                     // 포션 회복량
    [SerializeField] private GameObject recoveryEffectPrefab;       // 회복 이펙트
    [SerializeField] private float recoveryEffectDestroyTime;       // 회복 이펙트 지속 시간
    private bool isUseItem;                 // 아이템 사용 상태인지

    [SerializeField] private ItemObject itemObject;                 // 충돌 아이템 참조

    public bool CanAction { get => canAction; set => canAction = value; }
    public bool CanClimb { get => canClimb; set => canClimb = value; }
    public bool IsAirJumpItemAcquired { get => isAirJumpItemAcquired; set => isAirJumpItemAcquired = value; }
    public bool CanAirJump { get => canAirJump; set => canAirJump = value; }
    public bool IsLanding { get => isLanding; set => isLanding = value; }
    public bool IsWallJump { get => isWallJump; set => isWallJump = value; }
    public bool IsWallJumpItemAcquired { get => isWallJumpItemAcquired; set => isWallJumpItemAcquired = value; }
    public bool OnJumpWall { get => onJumpWall; set => onJumpWall = value; }
    public bool IsSlide { get => isSlide; set => isSlide = value; }
    public bool IsAirDashItemAcquired { get => isAirDashItemAcquired; set => isAirDashItemAcquired = value; }
    public bool CanDash { get => canDash; set => canDash = value; }
    public bool IsDash { get => isDash; set => isDash = value; }
    public bool IsGuard { get => isGuard; set => isGuard = value; }
    public ItemObject ItemObject { get => itemObject; set => itemObject = value; }
    public GameObject GuardEffectPrefab { get => guardEffectPrefab; set => guardEffectPrefab = value; }
    public float GuardEffectDestroyTime { get => guardEffectDestroyTime; set => guardEffectDestroyTime = value; }
    public int PotionCount { get => potionCount; set => potionCount = value; }
    public int RecoveryValue { get => recoveryValue; set => recoveryValue = value; }
    public bool IsUseItem { get => isUseItem; set => isUseItem = value; }

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerInputMovement>();
        playerAttack = GetComponent<PlayerInputAttack>();
        playerSoundFx = GetComponent<PlayerSoundFx>();
        damageable = GetComponent<Damageable>();

        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    void Update()
    {
        if (!CanAction) return;
        if (!damageable.IsAlive || playerAttack.IsAttacking) return;

        // 점프 & 공중 점프
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Input.GetKey(KeyCode.DownArrow) && groundCollider != null) return;

            if (playerMovement.IsGrounded && !IsLanding)
            {
                if (IsSlide)
                {
                    StopCoroutine(slideCoroutine);
                    IsSlide = false;
                }
                jumpRequested = true;
            }
            else if (playerMovement.IsClimb)
            {
                playerMovement.IsClimb = false;
                animator.SetBool(AnimatorStringToHash.IsClimb, false);
                jumpCoroutine = JumpCoroutine();
                StartCoroutine(jumpCoroutine);
                jumpRequested = true;
            }
            else if (!playerMovement.IsGrounded && CanAirJump && IsAirJumpItemAcquired && !OnJumpWall)
            {
                if (IsDash) return;

                CanAirJump = false;
                jumpRequested = true;

                float angle = 0f;
                if (playerMovement.IsRight)
                    angle = -15f;
                else
                    angle = 15f;

                GameObject effect = Instantiate(airJumpEffectPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
                Destroy(effect, airJumpEffectDestroyTime);
            }
        }

        // 벽 점프
        if (Input.GetKeyDown(KeyCode.C) && OnJumpWall && IsWallJumpItemAcquired)
        {
            wallJumpRequested = true;

            float angle = 0f;
            if (playerMovement.IsRight)
                angle = 15f;
            else
                angle = -15f;

            GameObject effect = Instantiate(airJumpEffectPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
            Destroy(effect, airJumpEffectDestroyTime);
        }

        // 슬라이딩 & 대쉬
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerMovement.IsClimb)
            {
                playerMovement.IsClimb = false;
                animator.SetBool(AnimatorStringToHash.IsClimb, false);
                animator.SetTrigger(AnimatorStringToHash.Jump);
            }
            else if (playerMovement.IsGrounded && !IsSlide && !IsLanding)
            {
                slideRequested = true;
                GameObject effect = Instantiate(slideEffectPrefab, transform.position, Quaternion.identity);
                if (!playerMovement.IsRight)
                    effect.transform.localScale = new Vector3(-1f, 1f, 1f);

                Destroy(effect, slideEffectDestroyTime);
            }
            else if (!playerMovement.IsGrounded && CanDash && IsAirDashItemAcquired)
            {
                dashRequested = true;
                GameObject effect = Instantiate(airDashEffectPrefab, transform.position + (Vector3)playerMovement.MoveDirection, Quaternion.identity);
                Destroy(effect, airDashEffectDestroyTime);
            }
        }

        // 가드
        if (Input.GetKeyDown(KeyCode.Z) && playerMovement.IsGrounded && canGuard && !IsSlide && !IsLanding)
        {
            guardRequested = true;
        }

        // 점프 관련 속성 설정
        if (playerMovement.IsGrounded || OnJumpWall)
        {
            CanDash = true;
            CanAirJump = true;
            jumpTime = 0;
            animator.SetFloat(AnimatorStringToHash.JumpTime, jumpTime);
        }
        else
        {
            if (rigidbody2d.velocity.y < 0 && !playerMovement.IsClimb)
            {
                jumpTime += Time.deltaTime;
                animator.SetFloat(AnimatorStringToHash.JumpTime, jumpTime);
            }
            else
            {
                jumpTime = 0;
                animator.SetFloat(AnimatorStringToHash.JumpTime, jumpTime);
            }

            if (jumpTime >= 0.75f)
                animator.SetTrigger(AnimatorStringToHash.Landing);
        }

        // 착지 애니메이션
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
        {
            if (!isLandingEffect)
            {
                isLandingEffect = true;
                playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.LANDING);
                GameObject effect = Instantiate(landingEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, landingEffectDestroyTime);
            }

            IsLanding = true;
        }
        else
        {
            isLandingEffect = false;
            IsLanding = false;
        }

        // 아이템 획득
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (ItemObject != null)
            {
                bool invenAdded = inventorySystem.AddItem(ItemObject.index);

                if (invenAdded)
                {
                    Item item = inventorySystem.ItemList.List[ItemObject.index];
                    item.Use();

                    GameObject effect = Instantiate(item.PickupEffect, new Vector2(transform.position.x, transform.position.y + 1f), Quaternion.identity);
                    effect.gameObject.GetComponent<SpriteRenderer>().color = item.PickupColor;
                    Destroy(effect, item.EffectDestroyTime);

                    if (inventoryUI.transform.parent.gameObject.activeSelf == false)
                    {
                        inventoryUI.OpenUI();
                        InventoryUI.onPauseDelegate();

                        if (inventorySystem.HasItemList.Count > 0)
                        {
                            inventoryUI.ItemUIs[ItemObject.index].ItemSelect();
                        }
                    }

                    ItemObject.ItemPickUp();
                    playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.PICKUP);
                }
            }
        }

        // 회복 포션 사용
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (PotionCount > 0 && damageable.Hp < damageable.MaxHp)
            {
                PotionCount--;
                damageable.Hp += RecoveryValue;
                uiManager.UseHpPotion(PotionCount);
                GameObject effect = Instantiate(recoveryEffectPrefab, new Vector2(transform.position.x, transform.position.y + 1.5f), Quaternion.identity);
                Destroy(effect, recoveryEffectDestroyTime);
                animator.SetTrigger(AnimatorStringToHash.UseItem);
                playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.POTION);

                if (playerMovement.IsClimb)
                    animator.ResetTrigger(AnimatorStringToHash.UseItem);
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("UseItem"))
            IsUseItem = true;
        else
            IsUseItem = false;

        // 테스트용 무적 설정
        if (Input.GetKeyDown(KeyCode.P))
        {
            damageable.MaxHp = int.MaxValue;
            damageable.Hp = damageable.MaxHp;
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequested)
        {
            jumpRequested = false;
            animator.SetTrigger(AnimatorStringToHash.Jump);
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
            playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.JUMP);
        }

        if (wallJumpRequested)
        {
            wallJumpRequested = false;
            animator.SetTrigger(AnimatorStringToHash.Jump);
            wallJumpCoroutine = WallJumpCoroutine();
            StartCoroutine(wallJumpCoroutine);
            rigidbody2d.velocity = new Vector2(wallJumpDirection.x * playerMovement.MoveSpeed, jumpForce);
            playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.JUMP);
        }

        if (slideRequested)
        {
            slideRequested = false;
            slideCoroutine = SlideCoroutine();
            StartCoroutine(slideCoroutine);
            playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.SLIDE);
        }

        if (dashRequested)
        {
            dashRequested = false;
            dashCoroutine = DashCoroutine();
            StartCoroutine(dashCoroutine);
            playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.DASH);
        }

        if (guardRequested)
        {
            guardRequested = false;
            guardCoroutine = GuardCoroutine();
            StartCoroutine(guardCoroutine);
        }
    }

    public bool PlayerNotInputMoveActionFlags()
    {
        if (IsWallJump || IsSlide || IsDash)
            return true;

        return false;
    }

    public bool PlayerNotMoveActionFlags()
    {
        if (IsGuard || IsUseItem || IsLanding || IsUseItem)
            return true;

        return false;
    }

    // 벽 점프 시 방향 설정
    private Vector2 GetWallJumpDirection(Collision2D collider)
    {
        if (collider.transform.position.x < transform.position.x)
            return Vector2.right;
        else
            return Vector2.left;
    }

    // 점프 실행 시 잠시 사다리 타기 불가능 코루틴
    private IEnumerator JumpCoroutine()
    {
        float time = 0f;
        
        CanClimb = false;

        while (time < 0.5f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        
        CanClimb = true;
    }

    // 벽 점프 시 잠시 방향 고정 코루틴
    private IEnumerator WallJumpCoroutine()
    {
        float time = 0f;
        IsWallJump = true;

        while (time < directionFixTime)
        {
            playerMovement.MoveDirection = wallJumpDirection;
            playerMovement.Flip();
            time += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        IsWallJump = false;
    }

    // 슬라이딩 실행 코루틴
    private IEnumerator SlideCoroutine()
    {
        float time = 0f;
        float actionSpeed = slideSpeed;
        float yVelocity = 0f;
        Vector2 dashDirection = Vector2.zero;
        Vector2 slopeDirection = Vector2.zero;
        damageable.IsInvincible = true;
        IsSlide = true;
        animator.SetTrigger(AnimatorStringToHash.Slide);

        float ghostDelayTime = 0.1f;

        while (time <= slideTime)
        {
            time += Time.fixedDeltaTime;
            ghostDelayTime -= Time.fixedDeltaTime;

            if (ghostDelayTime <= 0)
            {
                ghostDelayTime = 0.17f;

                GameObject currentGhost = Instantiate(ghostObject, transform.position, Quaternion.identity);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;

                if (!playerMovement.IsRight)
                    currentGhost.transform.localScale = new Vector3(-1f, 1f, 1f);

                Destroy(currentGhost, 1f);
            }

            if (time > invincibleTime)
                damageable.IsInvincible = false;

            if (playerMovement.OnSlope)
            {
                slopeDirection = new Vector2(playerMovement.GroundNormal.y, -playerMovement.GroundNormal.x).normalized;
                dashDirection = playerMovement.IsRight ? slopeDirection : -slopeDirection;
                yVelocity = actionSpeed * dashDirection.y;
            }
            else
            {
                dashDirection = playerMovement.IsRight ? Vector2.right : -Vector2.right;
                yVelocity = rigidbody2d.velocity.y;
            }

            rigidbody2d.velocity = new Vector2(actionSpeed * dashDirection.x, yVelocity);
            actionSpeed *= 0.955f;

            yield return waitForFixedUpdate;
        }

        IsSlide = false;
    }

    // 공중 대쉬 실행 코루틴
    private IEnumerator DashCoroutine()
    {
        float time = 0f;
        float actionSpeed = dashSpeed;
        damageable.IsInvincible = true;
        IsDash = true;
        CanDash = false;
        animator.SetTrigger(AnimatorStringToHash.Dash);

        float ghostDelayTime = 0.1f;

        while (time <= dashTime)
        {
            time += Time.fixedDeltaTime;
            ghostDelayTime -= Time.fixedDeltaTime;

            if (ghostDelayTime <= 0)
            {
                ghostDelayTime = 0.1f;

                GameObject currentGhost = Instantiate(ghostObject, transform.position, Quaternion.identity);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;

                if (!playerMovement.IsRight)
                    currentGhost.transform.localScale = new Vector3(-1f, 1f, 1f);

                Destroy(currentGhost, 1f);
            }

            if (playerMovement.IsClimb)
            {
                StopCoroutine(dashCoroutine);
                damageable.IsInvincible = false;
                IsDash = false;
                CanDash = true;
                CanAirJump = true;
            }

            Vector2 dashDirection = playerMovement.IsRight ? Vector2.right : -Vector2.right;

            rigidbody2d.velocity = new Vector2(actionSpeed * dashDirection.x, 0f);
            actionSpeed *= 0.9f;

            yield return waitForFixedUpdate;
        }

        jumpTime = 0f;
        animator.ResetTrigger(AnimatorStringToHash.Landing);
        IsDash = false;
        damageable.IsInvincible = false;
    }

    // 가드 실행 코루틴
    private IEnumerator GuardCoroutine()
    {
        float time = 0f;
        IsGuard = true;
        canGuard = false;
        animator.SetTrigger(AnimatorStringToHash.Guard);

        while (time < guardTime)
        {
            time += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }
        IsGuard = false;
        time = 0f;

        while (time < guardDelayTime)
        {
            time += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }
        canGuard = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("DownJumpPlatform"))
        {
            groundCollider = collision.collider;
        }

        if (collision.gameObject.tag.Equals("JumpWall"))
        {
            if (!isWallJumpItemAcquired) return;

            ContactPoint2D contact = collision.GetContact(0);
            float angle = Vector2.Angle(contact.normal, Vector2.up);
            
            if (Mathf.Approximately(angle, 90f))
            {
                OnJumpWall = true;
                rigidbody2d.velocity = Vector2.zero;
                wallJumpDirection = GetWallJumpDirection(collision);
                animator.SetBool(AnimatorStringToHash.WallSlide, true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("DownJumpPlatform"))
        {
            groundCollider = null;
        }

        if (collision.gameObject.tag.Equals("JumpWall"))
        {
            if (!isWallJumpItemAcquired) return;

            OnJumpWall = false;
            wallJumpDirection = Vector2.zero;
            animator.SetBool(AnimatorStringToHash.WallSlide, false);
        }
    }
}
