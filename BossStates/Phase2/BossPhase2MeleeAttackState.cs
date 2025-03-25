using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase2MeleeAttackState : BossPhase2State
{
    [SerializeField] private float attackTime;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private Vector2 attackSize;
    [SerializeField] private Vector2 attackOffset;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private Damager damager;

    private float time;
    private bool isAttack;
    private Vector2 attackPosition;
    private Collider2D[] hitColliders;
    private WaitForSeconds waitForSeconds;

    protected override void Awake()
    {
        base.Awake();
        waitForSeconds = new WaitForSeconds(fsmInfo.MeleeAttackCooldown);
    }

    public override void EnterState(BossPhase2FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        time = 0f;
        isAttack = false;
        fsmInfo.MeleeAttackCount++;
        fsmInfo.IsMeleeAttack = true;
        movement.MoveSpeed = 0f;
        animator.SetInteger(AnimatorStringToHash.State, (int)state);
        StartCoroutine(HeavyMeleeAttackCooldown());
    }

    public override void ExitState()
    {
        time = 0f;
        movement.MoveSpeed = fsmInfo.MoveSpeed;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        time += Time.deltaTime;

        if (time >= 0.7f && !isAttack)
        {
            PerformHeavyMeleeAttack();
            isAttack = true;
        }

        if (time >= attackTime)
        {
            controller.TransactionToState(BossPhase2FSMController.STATE.WALK);
            return;
        }
    }

    private IEnumerator HeavyMeleeAttackCooldown()
    {
        yield return waitForSeconds;
        fsmInfo.IsMeleeAttack = false;
    }

    public void PerformHeavyMeleeAttack()
    {
        float xPosition = 0f;
        if (movement.IsRight)
            xPosition = attackTransform.position.x + attackOffset.x;
        else
            xPosition = attackTransform.position.x - attackOffset.x;

        attackPosition = new Vector2(xPosition, attackTransform.position.y + attackOffset.y);

        hitColliders = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0f, hitLayer);

        if (hitColliders != null)
        {
            foreach (Collider2D hitCollider in hitColliders)
            {
                Damageable damageable = hitCollider.GetComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.HitPosition = hitCollider.ClosestPoint(attackPosition);
                    damageable.HitProcess(damager);
                    return;
                }
            }
        }
    }

    private void HeavyMeleeAttackEvent()
    {
        StartCoroutine(HeavyMeleeAttackCoroutine());
    }

    private void SoundFxEvent()
    {
        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.ATTACK2);
    }

    private IEnumerator HeavyMeleeAttackCoroutine()
    {
        float time = 0f;
        while (time <= 0.2f)
        {
            time += Time.deltaTime;
            PerformHeavyMeleeAttack();
            yield return null;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
}
