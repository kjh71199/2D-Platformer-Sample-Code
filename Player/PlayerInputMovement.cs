using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// 입력에 따른 플레이어 이동 컴포넌트
public class PlayerInputMovement : DirectionMovement
{
    PlayerInputAction playerAction;
    PlayerInputAttack playerAttack;
    DamageablePlayer damageable;

    private float gravityScale;
    private Vector2 groundNormal;
    [SerializeField] private bool onSlope;                  // 경사로 위에 있음
    [SerializeField] private bool isGrounded;               // 지면 위에 있음
    [SerializeField] Transform groundCheckTransform;        // 지면 검사 위치
    [SerializeField] private float groundCheckRadius;       // 지면 검사 반지름
    [SerializeField] private LayerMask groundCheckLayer;    // 지면 검사 레이어
    [SerializeField] private float maxFallingSpeed;         // 최대 하강 속도

    [SerializeField] private bool onLadder;                 // 사다리와 충돌 중인지
    [SerializeField] private bool onGroundLadder;           // 발 밑에 사다리가 있는지
    [SerializeField] Transform ladderCheckTransform;        // 사다리 검사 위치
    [SerializeField] private float ladderCheckRadius;       // 사다리 검사 반지름
    [SerializeField] private LayerMask ladderCheckLayer;    // 사다리 검사 레이어

    private Vector2 verticalInput;              // 수직 입력
    [SerializeField] private bool isClimb;      // 사다리에 매달린 상태인지
    [SerializeField] private float ladderSpeed; // 사다리 이동 속도

    public Rigidbody2D Rigidbody2d { get => rigidbody2d; set => rigidbody2d = value; }
    public Vector2 GroundNormal { get => groundNormal; set => groundNormal = value; }
    public bool OnSlope { get => onSlope; set => onSlope = value; }
    public bool IsGrounded { get => isGrounded; private set => isGrounded = value; }
    public float GravityScale { get => gravityScale; private set => gravityScale = value; }
    public bool OnLadder { get => onLadder; set => onLadder = value; }
    public bool OnGroundLadder { get => onGroundLadder; set => onGroundLadder = value; }
    public Transform LadderCheckTransform { get => ladderCheckTransform; set => ladderCheckTransform = value; }
    public float LadderCheckRadius { get => ladderCheckRadius; set => ladderCheckRadius = value; }
    public LayerMask LadderCheckLayer { get => ladderCheckLayer; set => ladderCheckLayer = value; }
    public bool IsClimb { get => isClimb; set => isClimb = value; }

    private void OnEnable()
    {
        InventoryUI.onPauseDelegate += PlayerNotMove;
        InventoryUI.onResumeDelegate += PlayerMove;
    }

    private void OnDisable()
    {
        InventoryUI.onPauseDelegate -= PlayerNotMove;
        InventoryUI.onResumeDelegate -= PlayerMove;
    }

    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        base.Awake();
        GravityScale = rigidbody2d.gravityScale;
        playerAction = GetComponent<PlayerInputAction>();
        playerAttack = GetComponent<PlayerInputAttack>();
        damageable = GetComponent<DamageablePlayer>();

        DontDestroyOnLoad(gameObject);
    }

    protected override void Update()
    {
        if (!damageable.IsAlive) return;

        PlayerStateCheck();
        GravityControll();
        Climb();

        if (playerAction.PlayerNotInputMoveActionFlags() || damageable.IsDamage) return;

        if (playerAction.PlayerNotMoveActionFlags() || playerAttack.IsAttacking)
        {
            rigidbody2d.velocity = Vector3.zero;
            return;
        }

        Move();
        Crouch();
    }

    // 플레이어 이동
    protected override void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        else
            moveDirection = Vector2.zero;

        Flip();

        rigidbody2d.velocity = new Vector2(MoveDirection.x * MoveSpeed, rigidbody2d.velocity.y);

        animator.SetFloat(AnimatorStringToHash.Move, Mathf.Abs(MoveDirection.x));
    }

    // 플레이어 앉기
    protected void Crouch()
    {
        if (Input.GetKey(KeyCode.DownArrow) && IsGrounded)
        {
            animator.SetBool(AnimatorStringToHash.IsCrouch, true);
            rigidbody2d.velocity = Vector2.zero;
            return;
        }

        animator.SetBool(AnimatorStringToHash.IsCrouch, false);
    }

    // 플레이어 사다리 타기
    protected void Climb()
    {
        if (playerAction.IsGuard) return;

        if (Input.GetKey(KeyCode.UpArrow) && OnLadder && !isClimb && playerAction.CanClimb)
        {
            IsClimb = true;
            animator.SetBool(AnimatorStringToHash.IsClimb, true);
        }

        if (Input.GetKey(KeyCode.DownArrow) && OnGroundLadder && !onLadder && !isClimb && playerAction.CanClimb)
        {
            rigidbody2d.velocity = Vector2.zero;
            transform.position += new Vector3(0f, -1f, 0f);
            IsGrounded = false;
            IsClimb = true;
            animator.SetBool(AnimatorStringToHash.IsClimb, true);
        }

        if (IsClimb)
        {
            playerAction.CanAirJump = true;

            Collider2D ladderCollider = Physics2D.OverlapCircle(LadderCheckTransform.position, LadderCheckRadius, LadderCheckLayer);
            if (ladderCollider != null)
            {
                transform.position = new Vector2(ladderCollider.transform.position.x, transform.position.y);
            }

            if (!OnLadder && !OnGroundLadder)
            {
                IsClimb = false;
                animator.SetBool(AnimatorStringToHash.IsClimb, false);
                animator.SetTrigger(AnimatorStringToHash.Falling);
            }

            if (Mathf.Abs(verticalInput.y) > 0f)
            {
                rigidbody2d.velocity = new Vector2(0f, ladderSpeed * verticalInput.y);

                if (verticalInput.y < 0f && IsGrounded)
                {
                    rigidbody2d.velocity = Vector2.zero;
                    transform.position += new Vector3(0f, -0.1f, 0f);
                    IsClimb = false;
                    animator.SetBool(AnimatorStringToHash.IsClimb, false);
                }

                if (verticalInput.y > 0f && !OnLadder)
                {
                    transform.position += new Vector3(0f, 0.6f, 0f);
                    IsClimb = false;
                    animator.SetBool(AnimatorStringToHash.IsClimb, false);
                }
            }
            else
            {
                rigidbody2d.velocity = Vector2.zero;
            }
        }
    }

    // 플레이어의 현재 불 변수 체크
    private void PlayerStateCheck()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundCheckLayer);
        animator.SetBool(AnimatorStringToHash.IsGround, IsGrounded);

        OnGroundLadder = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, ladderCheckLayer);

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            verticalInput.y = Input.GetAxisRaw("Vertical");
        else
            verticalInput.y = 0f;

        animator.SetFloat(AnimatorStringToHash.VerticalInput, verticalInput.y);

        if (IsGrounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(groundCheckTransform.position, Vector2.down, 0.1f, groundCheckLayer);
            if (hit.collider != null)
            {
                GroundNormal = hit.normal;
                float angle = Vector2.Angle(GroundNormal, Vector2.up);

                if (angle > 10f)
                    OnSlope = true;
                else
                    OnSlope = false;
            }
        }
        else
        {
            OnSlope = false;
        }
    }

    // 플레이어 불 변수 값에 따라 중력값 조절
    private void GravityControll()
    {
        if (isClimb || (OnSlope && moveDirection.x == 0 && !playerAction.IsSlide))
        {
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.gravityScale = 0f;
            return;
        }
        else if (playerAction.IsDash || (OnSlope && playerAttack.IsAttack))
        {
            rigidbody2d.gravityScale = 0f;
            return;
        }
        else if (playerAction.OnJumpWall)
        {
            rigidbody2d.gravityScale = 0.5f;
            return;
        }

        if (rigidbody2d.velocity.y < maxFallingSpeed)
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, maxFallingSpeed);

        rigidbody2d.gravityScale = GravityScale;
    }

    public void PlayerNotMove()
    {
        rigidbody2d.velocity = Vector2.zero;
        CanMove = false;
    }

    public void PlayerMove()
    {
        CanMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(LadderCheckTransform.position, LadderCheckRadius);
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);

    }
}
