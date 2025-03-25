using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 근접 공격 컴포넌트
public abstract class MeleeAttack : MonoBehaviour
{
    protected Animator animator;

    [SerializeField] protected Collider2D[] hitColliders;   // 공격을 맞은 콜라이더 배열
    [SerializeField] protected LayerMask hitLayer;          // 공격 처리 레이어
    [SerializeField] protected bool isAttack;               // 공격 했는지 여부

    [SerializeField] protected Damager damager;             // 공격 정의 컴포넌트
    [SerializeField] protected Transform attackTransform;   // 공격 위치

    [SerializeField] protected Vector2 attackSize;          // 공격 판정 크기

    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public Transform AttackTransform { get => attackTransform; set => attackTransform = value; }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 공격 실행
    protected abstract void AttackProcess();

    // 공격 처리
    public virtual void PerformAttack()
    {
        hitColliders = Physics2D.OverlapBoxAll(AttackTransform.position, attackSize, 0f, hitLayer); 

        foreach (Collider2D collider in hitColliders)
        {
            Damageable damagerble = collider.GetComponent<Damageable>();
            
            if (damagerble != null)
            {
                damagerble.HitPosition = collider.ClosestPoint(AttackTransform.position);
                damagerble.HitProcess(damager);
            }
        }

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(AttackTransform.position, attackSize);
    }
}
