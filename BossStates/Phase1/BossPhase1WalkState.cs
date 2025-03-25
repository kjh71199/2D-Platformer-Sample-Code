using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase1WalkState : BossPhase1State
{
    public override void EnterState(BossPhase1FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);
        animator.SetInteger(AnimatorStringToHash.State, (int)state);
        Move();
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (fsmInfo.IsMeleeAttack && fsmInfo.IsMagicAttack)
        {
            controller.TransactionToState(BossPhase1FSMController.STATE.TELEPORTSTART);
            return;
        }

        if (fsmInfo.MeleeAttackCount > 2)
        {
            fsmInfo.MeleeAttackCount = 0;
            controller.TransactionToState(BossPhase1FSMController.STATE.TELEPORTSTART);
            return;
        }

        if (controller.GetPlayerDistance() <= fsmInfo.MeleeAttackDistance && !fsmInfo.IsMeleeAttack)
        {
            controller.TransactionToState(BossPhase1FSMController.STATE.MELEEATTACK);
            return;
        }

        if (controller.GetPlayerDistance() <= fsmInfo.MagicAttackDistance && !fsmInfo.IsMagicAttack)
        {
            controller.TransactionToState(BossPhase1FSMController.STATE.MAGICATTACK);
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
