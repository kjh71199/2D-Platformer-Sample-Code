using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 대기 상태 처리 컴포넌트
public class BossPhase1IdleState : BossPhase1State
{
    [SerializeField] protected float time;
    [SerializeField] protected float checkTime;
    [SerializeField] protected Vector2 checkTimeRange;

    public override void EnterState(BossPhase1FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        time = 0f;
        checkTime = Random.Range(checkTimeRange.x, checkTimeRange.y);

        movement.MoveSpeed = 0f;

        animator.SetInteger(AnimatorStringToHash.State, (int)state);
    }

    public override void ExitState()
    {
        time = 0f;
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        if (time > checkTime)
        {
            controller.TransactionToState(BossPhase1FSMController.STATE.WALK);
            return;
        }

        if (controller.Damageable.Hp < controller.Damageable.MaxHp * 0.5)
        {
            controller.TransactionToState(BossPhase1FSMController.STATE.PHASESHIFT);
            return;
        }
    }
}
