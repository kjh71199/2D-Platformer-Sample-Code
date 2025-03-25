using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 일정 시간 대기 후 공격 컴포넌트
public class DelayAttack : MeleeAttack
{
    [SerializeField] protected float delayTime; // 대기 시간

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
