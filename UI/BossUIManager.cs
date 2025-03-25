using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� UI �Ŵ���
public class BossUIManager : MonoBehaviour
{
    [SerializeField] Damageable phase1Boss;     // ������ 1 ���� ����
    [SerializeField] Damageable phase2Boss;     // ������ 2 ���� ����
    [SerializeField] Image hpValue;             // ���� ü�� �� �̹���
    [SerializeField] Image hpValueHitEffect;    // ���� �ǰ� �� ü�� �� ȿ�� �̹���

    Damageable currentBoss;                     // ���� ������ �ǰ� ������Ʈ ����

    public delegate void PhaseShiftDelegate();

    public static PhaseShiftDelegate phaseShiftDelegate;    // ������ ��ȯ ��������Ʈ

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

    // ���� ü�¹� �ǰ� ȿ�� �ڷ�ƾ
    private IEnumerator BossHpCoroutine()
    {
        while (true)
        {
            hpValue.fillAmount = (float)currentBoss.Hp / currentBoss.MaxHp;
            hpValueHitEffect.fillAmount = Mathf.Lerp(hpValueHitEffect.fillAmount, hpValue.fillAmount, 0.05f);
            yield return null;
        }
    }

    // ���� ��ȯ
    private void BossChange()
    {
        currentBoss = phase2Boss;
    }
}
