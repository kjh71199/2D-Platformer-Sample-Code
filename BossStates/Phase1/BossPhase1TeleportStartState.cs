using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase1TeleportStartState : BossPhase1State
{
    private Collider2D collider2d;

    [SerializeField] private Vector2 teleportTimeRange;
    [SerializeField] private Vector2 teleportDistanceRange;
    [SerializeField] private Vector2 maxTeleportXPos;
    [SerializeField] private float teleportYPos;

    [SerializeField] private float delayTime;

    private float distance;
    private float time;

    protected override void Awake()
    {
        base.Awake();
        collider2d = GetComponent<Collider2D>();
    }

    public override void EnterState(BossPhase1FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        time = 0f;
        collider2d.enabled = false;
        movement.MoveSpeed = 0f;
        delayTime = Random.Range(teleportTimeRange.x, teleportTimeRange.y);
        distance = Random.Range(teleportDistanceRange.x, teleportDistanceRange.y);
        animator.SetInteger(AnimatorStringToHash.State, (int)state);

        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.TELEPORTSTART);
    }

    public override void ExitState()
    {
        SetTeleportPositioin();
        time = 0f;
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        if (time > delayTime)
        {
            controller.TransactionToState(BossPhase1FSMController.STATE.TELEPORTEND);
            return;
        }
    }

    private void SetTeleportPositioin()
    {
        int rand = Random.Range(0, 2);
        float xPosition = 0f;

        if (rand == 0)
            xPosition = controller.Player.transform.position.x - distance;
        else
            xPosition = controller.Player.transform.position.x + distance;

        if (xPosition < maxTeleportXPos.x || xPosition > maxTeleportXPos.y)
        {
            if (rand == 0)
                xPosition = controller.Player.transform.position.x + distance;
            else
                xPosition = controller.Player.transform.position.x - distance;
        }

        transform.position = new Vector2(xPosition, teleportYPos);
    }
    
    private void TeleportStartSoundFxEvent()
    {
        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.TELEPORTSTART);
    }
}
