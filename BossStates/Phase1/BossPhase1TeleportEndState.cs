using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase1TeleportEndState : BossPhase1State
{
    Collider2D collider2d;

    [SerializeField] private float delayTime;

    private float time;

    protected override void Awake()
    {
        base.Awake();
        collider2d = GetComponent<Collider2D>();
    }

    public override void EnterState(BossPhase1FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        movement.MoveDirection = new Vector2(controller.Player.transform.position.x - transform.position.x, 0f).normalized;
        
        animator.SetInteger(AnimatorStringToHash.State, (int)state);

        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.TELEPORTEND);

        time = 0f;
    }

    public override void ExitState()
    {
        time = 0;
        collider2d.enabled = true;
        movement.MoveSpeed = fsmInfo.MoveSpeed;
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        if (time >= delayTime)
        {
            controller.TransactionToState(BossPhase1FSMController.STATE.IDLE);
            return;
        }
    }

    private void TeleportEndSoundFxEvent()
    {
        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.TELEPORTEND);
    }
}
