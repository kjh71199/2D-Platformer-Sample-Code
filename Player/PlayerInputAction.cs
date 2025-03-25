using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Է¿� ���� �÷��̾� �׼� ������Ʈ
public class PlayerInputAction : MonoBehaviour
{
    PlayerInputMovement playerMovement;     // �÷��̾� �̵� ������Ʈ ����
    PlayerInputAttack playerAttack;         // �÷��̾� ���� ������Ʈ ����
    PlayerSoundFx playerSoundFx;            // �÷��̾� ���� ȿ�� ������Ʈ ����
    Damageable damageable;                  // �÷��̾� �ǰ� ������Ʈ ����
    Collider2D groundCollider;              // �Ʒ����� ������ �÷��� ����
    Rigidbody2D rigidbody2d;
    Animator animator;
    [SerializeField] UIManager uiManager;               // UI �Ŵ��� ����
    [SerializeField] InventorySystem inventorySystem;   // �κ��丮 �ý��� ����
    [SerializeField] InventoryUI inventoryUI;           // �κ��丮 UI ������Ʈ ����

    [SerializeField] private bool canAction = true;     // Ű �Է����� �׼��� ������ ��������
    [SerializeField] private bool canClimb = true;      // ��ٸ��� �Ŵ޸� �� �ִ� ��������

    [SerializeField] private float invincibleTime;      // �׼� �� ���� �ð�
    private WaitForFixedUpdate waitForFixedUpdate;      // �׼� �ڷ�ƾ�� FixedUpdate�� ���� ����

    [Header("Jump")]
    [SerializeField] private bool isAirJumpItemAcquired = false;    // ���� ���� ������ ȹ�� ����
    [SerializeField] private bool canAirJump = false;               // ���� ������ ������ ��������
    [SerializeField] private bool isLanding;                        // ���� ��������
    [SerializeField] private float jumpForce;                       // ���� ��
    [SerializeField] private float jumpTime;                        // ���� �ð�
    [SerializeField] private GameObject airJumpEffectPrefab;        // ���� ���� ����Ʈ
    [SerializeField] private float airJumpEffectDestroyTime;        // ���� ���� ����Ʈ ���ӽð�
    [SerializeField] private GameObject landingEffectPrefab;        // ���� ����Ʈ 
    [SerializeField] private float landingEffectDestroyTime;        // ���� ����Ʈ ���ӽð�
    private bool jumpRequested;             // ���� Ʈ����
    private IEnumerator jumpCoroutine;      // ��ٸ��� �Ŵ޸� �� ���� �� ���� �ð����� �Ŵ޸� �� ���� �ϴ� �ڷ�ƾ
    private bool isLandingEffect = false;   // ���� ����Ʈ ǥ�� ����

    [Header("WallJump")]
    [SerializeField] private bool isWallJumpItemAcquired = false;   // �� ���� ������ ȹ�� ����
    [SerializeField] private bool isWallJump;                       // �� ���� ��������
    [SerializeField] private bool onJumpWall;                       // �� ���� ������ ���� �پ� �ִ���
    [SerializeField] private Vector2 wallJumpDirection;             // �� ���� �� ���� ����
    [SerializeField] private float directionFixTime;                // �� ���� �� ���� ���� �ð�
    private bool wallJumpRequested;         // �� ���� Ʈ����
    private IEnumerator wallJumpCoroutine;  // �� ���� �� ���� �ð� ������ȯ �Ұ� �ڷ�ƾ

    [Header("Slide")]
    [SerializeField] private bool isSlide;                          // �����̵� ������
    [SerializeField] private float slideSpeed;                      // �����̵� �ӵ�
    [SerializeField] private float slideTime;                       // �����̵� �ð�
    [SerializeField] GameObject slideEffectPrefab;                  // �����̵� ����Ʈ
    [SerializeField] float slideEffectDestroyTime;                  // �����̵� ����Ʈ ���� �ð�
    [SerializeField] GameObject ghostObject;                        // �����̵� / �뽬 �� �ܻ� ȿ��
    private bool slideRequested;            // �����̵� Ʈ����
    private IEnumerator slideCoroutine;     // �����̵� ���� �ڷ�ƾ

    [Header("AirDash")]
    [SerializeField] private bool isAirDashItemAcquired = false;    // ���� �뽬 ������ ȹ�� ����
    [SerializeField] private bool canDash = false;                  // ���� �뽬�� ������ ��������
    [SerializeField] private bool isDash;                           // ���� �뽬 ��������
    [SerializeField] private float dashSpeed;                       // ���� �뽬 �ӵ�
    [SerializeField] private float dashTime;                        // ���� �뽬 �ð�
    [SerializeField] GameObject airDashEffectPrefab;                // ���� �뽬 ����Ʈ
    [SerializeField] float airDashEffectDestroyTime;                // ���� �뽬 ����Ʈ ���� �ð�
    private bool dashRequested;             // ���� �뽬 Ʈ����
    private IEnumerator dashCoroutine;      // ���� �뽬 ���� �ڷ�ƾ

    [Header("Guard")]
    [SerializeField] private bool isGuard;                          // ���� ��������
    [SerializeField] private bool canGuard = true;                  // ���� ������ ��������
    [SerializeField] private float guardTime;                       // ���� ���� �ð�
    [SerializeField] private float guardDelayTime;                  // ���� �� ������
    [SerializeField] private GameObject guardEffectPrefab;          // ���� ����Ʈ
    [SerializeField] private float guardEffectDestroyTime;          // ���� ����Ʈ ���� �ð�
    private bool guardRequested;            // ���� Ʈ����
    private IEnumerator guardCoroutine;     // ���� ���� �ڷ�ƾ

    [Header("HP Potion")]
    [SerializeField] private int potionCount;                       // ���� ��� ���� Ƚ��
    [SerializeField] private int recoveryValue;                     // ���� ȸ����
    [SerializeField] private GameObject recoveryEffectPrefab;       // ȸ�� ����Ʈ
    [SerializeField] private float recoveryEffectDestroyTime;       // ȸ�� ����Ʈ ���� �ð�
    private bool isUseItem;                 // ������ ��� ��������

    [SerializeField] private ItemObject itemObject;                 // �浹 ������ ����

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

        // ���� & ���� ����
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

        // �� ����
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

        // �����̵� & �뽬
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

        // ����
        if (Input.GetKeyDown(KeyCode.Z) && playerMovement.IsGrounded && canGuard && !IsSlide && !IsLanding)
        {
            guardRequested = true;
        }

        // ���� ���� �Ӽ� ����
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

        // ���� �ִϸ��̼�
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

        // ������ ȹ��
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

        // ȸ�� ���� ���
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

        // �׽�Ʈ�� ���� ����
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

    // �� ���� �� ���� ����
    private Vector2 GetWallJumpDirection(Collision2D collider)
    {
        if (collider.transform.position.x < transform.position.x)
            return Vector2.right;
        else
            return Vector2.left;
    }

    // ���� ���� �� ��� ��ٸ� Ÿ�� �Ұ��� �ڷ�ƾ
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

    // �� ���� �� ��� ���� ���� �ڷ�ƾ
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

    // �����̵� ���� �ڷ�ƾ
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

    // ���� �뽬 ���� �ڷ�ƾ
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

    // ���� ���� �ڷ�ƾ
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
