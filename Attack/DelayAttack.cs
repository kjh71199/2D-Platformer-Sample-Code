using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �ð� ��� �� ���� ������Ʈ
public class DelayAttack : MeleeAttack
{
    [SerializeField] protected float delayTime; // ��� �ð�

    protected override void AttackProcess()
    {
        StartCoroutine(AttackDelayCoroutine());
    }

    private IEnumerator AttackDelayCoroutine()
    {
        float time = 0f;
        while (time <= delayTime)
        {
            time += Time.deltaTime;
            PerformAttack();
            yield return null;
        }
    }
}
