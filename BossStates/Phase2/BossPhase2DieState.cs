using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase2DieState : BossPhase2State
{
    private SpriteRenderer spriteRenderer;

    private float time;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void EnterState(BossPhase2FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        time = 1.4f;
        movement.MoveSpeed = 0f;
        animator.SetInteger(AnimatorStringToHash.State, (int)state);

        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.DIE);

        BossRoomManager.onBossDieDelegate();
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        time -= Time.deltaTime;

        if (time <= 1f)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, time);
        }
    }


}
