using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase2TeleportAttackState : BossPhase2State
{
    private Collider2D collider2d;
    private SpriteRenderer spriteRenderer;

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
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        waitForSeconds = new WaitForSeconds(fsmInfo.MeleeAttackCooldown);
    }

    public override void EnterState(BossPhase2FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        time = 0f;

        isAttack = false;
        collider2d.enabled = true;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
        fsmInfo.MeleeAttackCount++;
        fsmInfo.IsMeleeAttack = true;
        movement.MoveSpeed = 0f;
        movement.MoveDirection = new Vector2(controller.Player.transform.position.x - transform.position.x, 0f).normalized;
        animator.SetInteger(AnimatorStringToHash.State, (int)state);
        StartCoroutine(MeleeAttackCooldown());
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

        if (time <= 0.5f)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, time * 2);
        }

        if (time >= 0.7f && !isAttack)
        {
            PerformTeleportAttack();
            isAttack = true;
        }

        if (time >= attackTime)
        {
            controller.TransactionToState(BossPhase2FSMController.STATE.WALK);
            return;
        }
    }

    private IEnumerator MeleeAttackCooldown()
    {
        yield return waitForSeconds;
        fsmInfo.IsMeleeAttack = false;
    }

    public void PerformTeleportAttack()
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
                }
            }
        }
    }

    private void TeleportAttackEvent()
    {
        StartCoroutine(TeleportAttackCoroutine());
    }

    private void SoundFxEvent()
    {
        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.ATTACK1);
    }

    private IEnumerator TeleportAttackCoroutine()
    {
        float time = 0f;
        while (time <= 0.2f)
        {
            time += Time.deltaTime;
            PerformTeleportAttack();
            yield return null;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }

}
