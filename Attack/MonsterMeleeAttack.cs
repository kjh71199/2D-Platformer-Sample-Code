using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ���� ������Ʈ
public class MonsterMeleeAttack : MeleeAttack
{
    protected DirectionMovement movement;   // ���� �̵� ������Ʈ ����
    protected Damageable damageable;        // �ǰ� ������Ʈ ����
    protected WaitForSeconds attackDelay;   // ���� �� ������
    [SerializeField] protected DetectEnemy detectEnemy; // �� Ž�� ���� ������Ʈ
    [SerializeField] protected float attackDelayTime;   // ���� ���� ���ð�

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

    // ���� ����
    protected override void AttackProcess()
    {
        if (!damageable.IsAlive) return;

        if (detectEnemy.DetectEnemyCollider(movement.MoveDirection) != null)
        {
            animator.SetTrigger(AnimatorStringToHash.Attack);
        }
    }

    // ���� ��� �ð� �ڷ�ƾ
    protected virtual IEnumerator AttackCoroutine()
    {
        while (true)
        {
            AttackProcess();

            yield return attackDelay;
        }
    }

}
