using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력에 따른 플레이어 공격 컴포넌트
public class PlayerInputAttack : MeleeAttack
{
    PlayerInputMovement playerMovement;         // 플레이어 이동 컴포넌트 참조
    PlayerInputAction playerAction;             // 플레이어 액션 컴포넌트 참조
    PlayerSoundFx playerSoundFx;                // 플레이어 사운드 효과 컴포넌트 참조

    [SerializeField] protected Vector2 attackOffset;    // 공격 위치 오프셋
    [SerializeField] Damager attack3Damager;    // 3타 공격 정의 컴포넌트 참조
    private Vector2 attackPosition;             // 공격 위치
    private bool isAttacking;                   // 공격 애니메이션 중인지 여부

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

    // 공격 실행 재정의
    protected override void AttackProcess()
    {
        IsAttack = true;
        animator.SetTrigger(AnimatorStringToHash.Attack);
    }

    // 공격 처리 재정의
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

    // 3타 시 위치 조정 이벤트 1
    public void Attack3MoveFowardEvent()
    {
        if (playerMovement.IsRight)
            transform.position += new Vector3(0.6f, 0, 0);
        else
            transform.position -= new Vector3(0.6f, 0, 0);
    }

    // 3타 시 위치 조정 이벤트 2
    public void Attack3MoveBackwardEvent()
    {
        if (playerMovement.IsRight)
            transform.position -= new Vector3(0.6f, 0, 0);
        else
            transform.position += new Vector3(0.6f, 0, 0);
    }

    // 공격 
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
