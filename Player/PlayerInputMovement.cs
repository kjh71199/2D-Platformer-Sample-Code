using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// �Է¿� ���� �÷��̾� �̵� ������Ʈ
public class PlayerInputMovement : DirectionMovement
{
    PlayerInputAction playerAction;
    PlayerInputAttack playerAttack;
    DamageablePlayer damageable;

    private float gravityScale;
    private Vector2 groundNormal;
    [SerializeField] private bool onSlope;                  // ���� ���� ����
    [SerializeField] private bool isGrounded;               // ���� ���� ����
    [SerializeField] Transform groundCheckTransform;        // ���� �˻� ��ġ
    [SerializeField] private float groundCheckRadius;       // ���� �˻� ������
    [SerializeField] private LayerMask groundCheckLayer;    // ���� �˻� ���̾�
    [SerializeField] private float maxFallingSpeed;         // �ִ� �ϰ� �ӵ�

    [SerializeField] private bool onLadder;                 // ��ٸ��� �浹 ������
    [SerializeField] private bool onGroundLadder;           // �� �ؿ� ��ٸ��� �ִ���
    [SerializeField] Transform ladderCheckTransform;        // ��ٸ� �˻� ��ġ
    [SerializeField] private float ladderCheckRadius;       // ��ٸ� �˻� ������
    [SerializeField] private LayerMask ladderCheckLayer;    // ��ٸ� �˻� ���̾�

    private Vector2 verticalInput;              // ���� �Է�
    [SerializeField] private bool isClimb;      // ��ٸ��� �Ŵ޸� ��������
    [SerializeField] private float ladderSpeed; // ��ٸ� �̵� �ӵ�

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

    // �÷��̾� �̵�
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

    // �÷��̾� �ɱ�
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

    // �÷��̾� ��ٸ� Ÿ��
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

    // �÷��̾��� ���� �� ���� üũ
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

    // �÷��̾� �� ���� ���� ���� �߷°� ����
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
