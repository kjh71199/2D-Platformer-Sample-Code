using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 공격 상태 처리 컴포넌트
public class BossPhase1MeleeAttackState : BossPhase1State
{
    [SerializeField] private float attackTime;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private Vector2 attackSize;
    [SerializeField] private Vector2 attackOffset;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private Damager damager;

    private bool isAttack;
    private float time;
    private Vector2 attackPosition;
    private Collider2D[] hitColliders;
    private WaitForSeconds waitForSeconds;

    protected override void Awake()
    {
        base.Awake();
        waitForSeconds = new WaitForSeconds(fsmInfo.MeleeAttackCooldown);
    }

    public override void EnterState(BossPhase1FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        time = 0f;
        isAttack = false;
        fsmInfo.MeleeAttackCount++;
        fsmInfo.IsMeleeAttack = true;
        movement.MoveSpeed = 0f;
        animator.SetInteger(AnimatorStringToHash.State, (int)state);
        StartCoroutine(NormalMeleeAttackCooldown());
    }

    public override void ExitState()
    {
        time = 0f;
        movement.MoveSpeed = fsmInfo.MoveSpeed;
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        if (time >= 0.8f && !isAttack)
        {
            PerformNormalMeleeAttack();
            isAttack = true;
        }

        if (time >= attackTime)
        {
            controller.TransactionToState(BossPhase1FSMController.STATE.IDLE);
            return;
        }
    }

    private IEnumerator NormalMeleeAttackCooldown()
    {
        yield return waitForSeconds;
        fsmInfo.IsMeleeAttack = false;
    }

    public void PerformNormalMeleeAttack()
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

    private void NormalMeleeAttackEvent()
    {
        StartCoroutine(NormalAttackCoroutine());
    }

    private void SoundFxEvent()
    {
        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.ATTACK1);
    }

    private IEnumerator NormalAttackCoroutine()
    {
        float time = 0f;
        while (time <= 0.2f)
        {
            time += Time.deltaTime;
            PerformNormalMeleeAttack();
            yield return null;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
}
