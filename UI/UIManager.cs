using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 매니저 컴포넌트
public class UIManager : MonoBehaviour
{
    private static UIManager instance;              // UI 매니저 싱글턴 인스턴스
    public static UIManager Instance {  get { return instance; } }

    [SerializeField] private GameObject player;     // 플레이어 참조
    [SerializeField] private Image hpValueImage;    // 플레이어 체력 바 이미지
    [SerializeField] private Image[] potions;       // 플레이어 포션 이미지 배열

    private DamageablePlayer damageable;            // 플레이어 피격 컴포넌트 참조

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        damageable = player.GetComponent<DamageablePlayer>();

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        hpValueImage.fillAmount = (float)damageable.Hp / damageable.MaxHp;
    }

    // 포션 사용
    public void UseHpPotion(int index)
    {
        potions[index].gameObject.SetActive(false);
    }

    // 부활 시 포션 초기화
    public void InitializePotion()
    {
        foreach (Image potion in potions)
        {
            potion.gameObject.SetActive(true);
        }
    }
}
