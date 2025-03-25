using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ������Ʈ
public abstract class MeleeAttack : MonoBehaviour
{
    protected Animator animator;

    [SerializeField] protected Collider2D[] hitColliders;   // ������ ���� �ݶ��̴� �迭
    [SerializeField] protected LayerMask hitLayer;          // ���� ó�� ���̾�
    [SerializeField] protected bool isAttack;               // ���� �ߴ��� ����

    [SerializeField] protected Damager damager;             // ���� ���� ������Ʈ
    [SerializeField] protected Transform attackTransform;   // ���� ��ġ

    [SerializeField] protected Vector2 attackSize;          // ���� ���� ũ��

    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public Transform AttackTransform { get => attackTransform; set => attackTransform = value; }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // ���� ����
    protected abstract void AttackProcess();

    // ���� ó��
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
