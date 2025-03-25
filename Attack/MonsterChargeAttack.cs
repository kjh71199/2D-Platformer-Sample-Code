using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ���� ������Ʈ
public class MonsterChargeAttack : MonsterMeleeAttack
{
    private Collider2D target;      // ������ Ÿ��
    private Vector2 targetPosition; // Ÿ���� ��ġ
    private bool isCharge;          // ���� ���� ������
    private bool chargeDelay;       // ���� ���� ���� ���ð�
    private bool isGuarded;         // ���� ������ ��������
    [SerializeField] private float chargeSpeed;     // ���� ���� �ӵ�
    [SerializeField] private LayerMask enviroLayer; // ���� Ž�� ���̾�

    public Collider2D Target { get => target; set => target = value; }
    public bool IsCharge { get => isCharge; set => isCharge = value; }
    public bool IsGuarded { get => isGuarded; set => isGuarded = value; }

    // ���� ����
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

    // ���� ���� �ڷ�ƾ
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

    // ���� ó��
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