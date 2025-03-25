using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BossPhase2MagicAttakState : BossPhase2State
{
    [SerializeField] private float attackTime;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private GameObject projectilePrefab;

    private float time;
    private WaitForSeconds waitForSeconds;

    protected override void Awake()
    {
        base.Awake();
        waitForSeconds = new WaitForSeconds(fsmInfo.MagicAttackCooldown);
    }

    public override void EnterState(BossPhase2FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        time = 0f;
        fsmInfo.IsMagicAttack = true;
        movement.MoveSpeed = 0f;
        animator.SetInteger(AnimatorStringToHash.State, (int)state);
        StartCoroutine(MagicAttackCooldown());
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

        if (time >= attackTime)
        {
            controller.TransactionToState(BossPhase2FSMController.STATE.WALK);
            return;
        }
    }

    public void MagicFire()
    {
        GameObject bulletGameObject = Instantiate(projectilePrefab, attackTransform.position, Quaternion.identity);
        DirectionMovement bulletMovement = bulletGameObject.GetComponent<DirectionMovement>();
        bulletMovement.MoveDirection = movement.MoveDirection;
        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.MAGIC1);
    }

    private IEnumerator MagicAttackCooldown()
    {
        yield return waitForSeconds;
        fsmInfo.IsMagicAttack = false;
    }
}
