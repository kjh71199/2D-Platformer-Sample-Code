using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase2HeavyMagicAttackState : BossPhase2State
{
    [SerializeField] private float attackTime;
    [SerializeField] private float attackYPos;
    [SerializeField] private GameObject projectilePrefab;

    private float time;
    private WaitForSeconds waitForSeconds;

    protected override void Awake()
    {
        base.Awake();
        waitForSeconds = new WaitForSeconds(fsmInfo.HeavyMagicAttackCooldown);
    }

    public override void EnterState(BossPhase2FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        time = 0f;
        fsmInfo.IsHeavyMagicAttack = true;
        movement.MoveSpeed = 0f;
        animator.SetInteger(AnimatorStringToHash.State, (int)state);
        StartCoroutine(HeavyMagicAttackCooldown());
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

    public void HeavyMagicFire()
    {
        Vector2 attackPosition = new Vector2(controller.Player.transform.position.x, attackYPos);
        GameObject bulletGameObject = Instantiate(projectilePrefab, attackPosition, Quaternion.identity);
    }

    private void HeavyMagicSoundFxEvent()
    {
        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.MAGIC2);
    }

    private IEnumerator HeavyMagicAttackCooldown()
    {
        yield return waitForSeconds;
        fsmInfo.IsHeavyMagicAttack = false;
    }
}
