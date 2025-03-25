using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase2WalkState : BossPhase2State
{
    private float time;

    public override void EnterState(BossPhase2FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        time = 0f;
        animator.SetInteger(AnimatorStringToHash.State, (int)state);
        Move();
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        base.UpdateState();

        time += Time.deltaTime;

        if (time <= 0.5f) return;

        if (fsmInfo.IsMeleeAttack && fsmInfo.IsMagicAttack && fsmInfo.IsHeavyMagicAttack && time > 1f)
        {
            controller.TransactionToState(BossPhase2FSMController.STATE.TELEPORTSTART);
            return;
        }

        if (fsmInfo.MeleeAttackCount > 2)
        {
            fsmInfo.MeleeAttackCount = 0;
            controller.TransactionToState(BossPhase2FSMController.STATE.TELEPORTSTART);
            return;
        }

        if (controller.GetPlayerDistance() <= fsmInfo.MeleeAttackDistance && !fsmInfo.IsMeleeAttack)
        {
            controller.TransactionToState(BossPhase2FSMController.STATE.MELEEATTACK);
            return;
        }

        if (controller.GetPlayerDistance() <= fsmInfo.MagicAttackDistance && !fsmInfo.IsMagicAttack)
        {
            controller.TransactionToState(BossPhase2FSMController.STATE.MAGICATTACK);
            return;
        }

        if (controller.GetPlayerDistance() <= fsmInfo.HeavyMagicAttackDistance && !fsmInfo.IsHeavyMagicAttack)
        {
            controller.TransactionToState(BossPhase2FSMController.STATE.HEAVYMAGICATTACK);
            return;
        }

        Move();
    }

    protected void Move()
    {
        movement.MoveDirection = new Vector2(controller.Player.transform.position.x - transform.position.x, 0f).normalized;
        movement.MoveSpeed = fsmInfo.MoveSpeed;
    }
}
