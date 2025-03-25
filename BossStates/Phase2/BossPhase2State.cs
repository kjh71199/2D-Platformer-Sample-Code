using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossPhase2State : MonoBehaviour
{
    protected BossPhase2FSMController controller;
    protected Animator animator;

    protected BossPhase2FSMInfo fsmInfo;
    protected DirectionMovement movement;
    protected Damageable damageable;
    protected BossSoundFx soundFx;

    [Range(1f, 2f)]
    [SerializeField] protected float animSpeed;

    protected virtual void Awake()
    {
        controller = GetComponent<BossPhase2FSMController>();
        animator = GetComponent<Animator>();
        fsmInfo = GetComponent<BossPhase2FSMInfo>();
        movement = GetComponent<DirectionMovement>();
        damageable = GetComponent<Damageable>();
        soundFx = GetComponent<BossSoundFx>();
    }

    public virtual void EnterState(BossPhase2FSMController.STATE state, object data = null)
    {
        animator.speed = animSpeed;
    }

    public virtual void UpdateState()
    {
        if (!damageable.IsAlive)
        {
            controller.TransactionToState(BossPhase2FSMController.STATE.DIE);
            return;
        }
    }

    public abstract void ExitState();
}
