using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 근접 공격 컴포넌트
public class MonsterMeleeAttack : MeleeAttack
{
    protected DirectionMovement movement;   // 방향 이동 컴포넌트 참조
    protected Damageable damageable;        // 피격 컴포넌트 참조
    protected WaitForSeconds attackDelay;   // 공격 후 딜레이
    [SerializeField] protected DetectEnemy detectEnemy; // 적 탐지 설정 컴포넌트
    [SerializeField] protected float attackDelayTime;   // 공격 사이 대기시간

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<DirectionMovement>();
        damageable = GetComponent<Damageable>();
    }

    protected virtual void Start()
    {
        attackDelay = new WaitForSeconds(attackDelayTime);
        StartCoroutine(AttackCoroutine());
    }

    // 공격 실행
    protected override void AttackProcess()
    {
        if (!damageable.IsAlive) return;

        if (detectEnemy.DetectEnemyCollider(movement.MoveDirection) != null)
        {
            animator.SetTrigger(AnimatorStringToHash.Attack);
        }
    }

    // 공격 대기 시간 코루틴
    protected virtual IEnumerator AttackCoroutine()
    {
        while (true)
        {
            AttackProcess();

            yield return attackDelay;
        }
    }

}
