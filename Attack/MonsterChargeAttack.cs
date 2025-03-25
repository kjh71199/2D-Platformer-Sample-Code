using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 돌진 공격 컴포넌트
public class MonsterChargeAttack : MonsterMeleeAttack
{
    private Collider2D target;      // 돌진할 타겟
    private Vector2 targetPosition; // 타겟의 위치
    private bool isCharge;          // 돌진 공격 중인지
    private bool chargeDelay;       // 돌진 공격 사이 대기시간
    private bool isGuarded;         // 돌진 공격이 막혔는지
    [SerializeField] private float chargeSpeed;     // 돌진 공격 속도
    [SerializeField] private LayerMask enviroLayer; // 지형 탐지 레이어

    public Collider2D Target { get => target; set => target = value; }
    public bool IsCharge { get => isCharge; set => isCharge = value; }
    public bool IsGuarded { get => isGuarded; set => isGuarded = value; }

    // 공격 실행
    protected override void AttackProcess()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, transform.position.y), chargeSpeed * Time.deltaTime);
        PerformAttack();

        RaycastHit2D hit = Physics2D.Raycast(AttackTransform.position, movement.MoveDirection, 1.2f, enviroLayer);
        if (hit.collider != null)
        {
            animator.SetBool(AnimatorStringToHash.Attack, false);
        }

        if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.1f)
        {
            animator.SetBool(AnimatorStringToHash.Attack, false);
        }
    }

    // 돌진 공격 코루틴
    protected override IEnumerator AttackCoroutine()
    {
        while (true)
        {
            if (!damageable.IsAlive) yield return null;

            IsCharge = animator.GetBool(AnimatorStringToHash.Charge);
            IsAttack = animator.GetBool(AnimatorStringToHash.Attack);
            
            if (animator.GetBool(AnimatorStringToHash.IsChargeEnd))
            {
                chargeDelay = true;
                animator.SetBool(AnimatorStringToHash.IsChargeEnd, false);
            }

            if (chargeDelay)
            {
                yield return attackDelay;
                chargeDelay = false;
            }

            Target = detectEnemy.DetectEnemyCollider(movement.MoveDirection);

            if (Target != null && !IsCharge && !chargeDelay)
            {
                if (movement.IsRight)
                    targetPosition = new Vector2(Target.transform.position.x + 3f, Target.transform.position.y);
                else
                    targetPosition = new Vector2(Target.transform.position.x - 3f, Target.transform.position.y);

                animator.SetBool(AnimatorStringToHash.Charge, true);
            }

            if (IsAttack)
            {
                AttackProcess();
            }

            yield return null;
        }
    }

    // 공격 처리
    public override void PerformAttack()
    {
        base.PerformAttack();

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.tag.Equals("Player") && collider.GetComponent<PlayerInputAction>().IsGuard == true)
            {
                IsGuarded = true;
                animator.SetTrigger(AnimatorStringToHash.Hit);
                DamageableHitAnimation damageable = GetComponent<DamageableHitAnimation>();
                damageable.KnockBack(attackTransform.position, new Vector2(3f, 0f), 0.3f);
            }
        }
    }
}