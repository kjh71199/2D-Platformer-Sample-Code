using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Է¿� ���� �÷��̾� ���� ������Ʈ
public class PlayerInputAttack : MeleeAttack
{
    PlayerInputMovement playerMovement;         // �÷��̾� �̵� ������Ʈ ����
    PlayerInputAction playerAction;             // �÷��̾� �׼� ������Ʈ ����
    PlayerSoundFx playerSoundFx;                // �÷��̾� ���� ȿ�� ������Ʈ ����

    [SerializeField] protected Vector2 attackOffset;    // ���� ��ġ ������
    [SerializeField] Damager attack3Damager;    // 3Ÿ ���� ���� ������Ʈ ����
    private Vector2 attackPosition;             // ���� ��ġ
    private bool isAttacking;                   // ���� �ִϸ��̼� ������ ����

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    protected override void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerInputMovement>();
        playerAction = GetComponent<PlayerInputAction>();
        playerSoundFx = GetComponent<PlayerSoundFx>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !IsAttack && !playerAction.IsLanding)
        {
            AttackProcess();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            IsAttacking = true;
        }
        else
            IsAttacking = false;
        
    }

    // ���� ���� ������
    protected override void AttackProcess()
    {
        IsAttack = true;
        animator.SetTrigger(AnimatorStringToHash.Attack);
    }

    // ���� ó�� ������
    public override void PerformAttack()
    {
        float xPosition = 0f;
        bool isHit = false;

        if (playerMovement.IsRight)
            xPosition = AttackTransform.position.x + attackOffset.x;
        else
            xPosition = AttackTransform.position.x - attackOffset.x;

        attackPosition = new Vector2(xPosition, AttackTransform.position.y + attackOffset.y);
        hitColliders = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0f, hitLayer);

        foreach (Collider2D collider in hitColliders)
        {
            Damageable damageable = collider.GetComponent<Damageable>();

            if (damageable != null && damageable.IsAlive)
            {
                isHit = true;
                damageable.HitPosition = collider.ClosestPoint(attackPosition);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
                    damageable.HitProcess(attack3Damager);
                else
                    damageable.HitProcess(damager);
            }
        }

        if (isHit)
            playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.ATTACKHIT);
        else
            playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.ATTACK);
    }

    // 3Ÿ �� ��ġ ���� �̺�Ʈ 1
    public void Attack3MoveFowardEvent()
    {
        if (playerMovement.IsRight)
            transform.position += new Vector3(0.6f, 0, 0);
        else
            transform.position -= new Vector3(0.6f, 0, 0);
    }

    // 3Ÿ �� ��ġ ���� �̺�Ʈ 2
    public void Attack3MoveBackwardEvent()
    {
        if (playerMovement.IsRight)
            transform.position -= new Vector3(0.6f, 0, 0);
        else
            transform.position += new Vector3(0.6f, 0, 0);
    }

    // ���� 
    public void ResetIsAttackEvent()
    {
        IsAttack = false;
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }

}
