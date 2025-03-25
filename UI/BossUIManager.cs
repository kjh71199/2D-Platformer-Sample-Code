using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 보스 UI 매니저
public class BossUIManager : MonoBehaviour
{
    [SerializeField] Damageable phase1Boss;     // 페이즈 1 보스 참조
    [SerializeField] Damageable phase2Boss;     // 페이즈 2 보스 참조
    [SerializeField] Image hpValue;             // 보스 체력 바 이미지
    [SerializeField] Image hpValueHitEffect;    // 보스 피격 시 체력 바 효과 이미지

    Damageable currentBoss;                     // 현재 보스의 피격 컴포넌트 참조

    public delegate void PhaseShiftDelegate();

    public static PhaseShiftDelegate phaseShiftDelegate;    // 페이즈 전환 델리게이트

    private void OnEnable()
    {
        phaseShiftDelegate += BossChange;
    }

    private void OnDisable()
    {
        phaseShiftDelegate -= BossChange;
    }

    void Start()
    {
        currentBoss = phase1Boss;
        StartCoroutine(BossHpCoroutine());
    }

    // 보스 체력바 피격 효과 코루틴
    private IEnumerator BossHpCoroutine()
    {
        while (true)
        {
            hpValue.fillAmount = (float)currentBoss.Hp / currentBoss.MaxHp;
            hpValueHitEffect.fillAmount = Mathf.Lerp(hpValueHitEffect.fillAmount, hpValue.fillAmount, 0.05f);
            yield return null;
        }
    }

    // 보스 전환
    private void BossChange()
    {
        currentBoss = phase2Boss;
    }
}
