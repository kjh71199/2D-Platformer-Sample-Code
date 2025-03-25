using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase1PhaseShiftState : BossPhase1State
{
    [SerializeField] private GameObject phaseShiftAttackPrefab;
    [SerializeField] private Transform[] phaseShiftAttackTransforms;
    [SerializeField] private GameObject phase2Boss;
    [SerializeField] private Transform activateTransform;

    public override void EnterState(BossPhase1FSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        Collider2D collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;

        animator.SetInteger(AnimatorStringToHash.State, (int)state);
        animator.SetBool(AnimatorStringToHash.Phaseshift, true);

        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.PHASESHIFT);

        StartCoroutine(PhaseShiftAttackCoroutine());
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }

    private IEnumerator PhaseShiftAttackCoroutine()
    {
        yield return new WaitForSeconds(3f);

        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.MAGIC2);
        for (int i = 0; i < phaseShiftAttackTransforms.Length; i++)
        {
            if (i % 2 == 0)
            {
                float attackXpos = phaseShiftAttackTransforms[i].transform.position.x;

                GameObject attack = Instantiate(phaseShiftAttackPrefab, phaseShiftAttackTransforms[i].position, Quaternion.identity);

                if (i == 2)
                    attack.GetComponent<SoundFx>().IsPlay = true;
                else
                    attack.GetComponent<SoundFx>().IsPlay = false;
            }
        }

        yield return new WaitForSeconds(1.5f);

        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.MAGIC2);
        for (int i = 0; i < phaseShiftAttackTransforms.Length; i++)
        {
            if (i % 2 == 1)
            {
                float attackXpos = phaseShiftAttackTransforms[i].transform.position.x;

                GameObject attack = Instantiate(phaseShiftAttackPrefab, phaseShiftAttackTransforms[i].position, Quaternion.identity);

                if (i == 1)
                    attack.GetComponent<SoundFx>().IsPlay = true;
                else
                    attack.GetComponent<SoundFx>().IsPlay = false;
            }
        }

        yield return new WaitForSeconds(1.5f);

        soundFx.PlaySound((int)BossSoundFx.BOSSAUDIO.MAGIC2);
        for (int i = 0; i < phaseShiftAttackTransforms.Length; i++)
        {
            if (i % 2 == 0)
            {
                float attackXpos = phaseShiftAttackTransforms[i].transform.position.x;

                GameObject attack = Instantiate(phaseShiftAttackPrefab, phaseShiftAttackTransforms[i].position, Quaternion.identity);

                if (i == 2)
                    attack.GetComponent<SoundFx>().IsPlay = true;
                else
                    attack.GetComponent<SoundFx>().IsPlay = false;
            }
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(ActivatePhase2Boss());
    }

    private IEnumerator ActivatePhase2Boss()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
        gameObject.transform.position = activateTransform.position;
        movement.MoveDirection = new Vector2(controller.Player.transform.position.x - transform.position.x, 0f).normalized;
        animator.SetInteger(AnimatorStringToHash.State, (int)BossPhase1FSMController.STATE.IDLE);
        float time = 0f;

        while (time <= 1f)
        {
            time += Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, time);
            yield return null;
        }

        gameObject.SetActive(false);

        phase2Boss.SetActive(true);
        Damageable phase2Damageable = phase2Boss.GetComponent<Damageable>();
        phase2Damageable.Hp = GetComponent<Damageable>().Hp;
        BossUIManager.phaseShiftDelegate();
    }
}
