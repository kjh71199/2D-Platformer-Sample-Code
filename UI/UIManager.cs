using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI �Ŵ��� ������Ʈ
public class UIManager : MonoBehaviour
{
    private static UIManager instance;              // UI �Ŵ��� �̱��� �ν��Ͻ�
    public static UIManager Instance {  get { return instance; } }

    [SerializeField] private GameObject player;     // �÷��̾� ����
    [SerializeField] private Image hpValueImage;    // �÷��̾� ü�� �� �̹���
    [SerializeField] private Image[] potions;       // �÷��̾� ���� �̹��� �迭

    private DamageablePlayer damageable;            // �÷��̾� �ǰ� ������Ʈ ����

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

    // ���� ���
    public void UseHpPotion(int index)
    {
        potions[index].gameObject.SetActive(false);
    }

    // ��Ȱ �� ���� �ʱ�ȭ
    public void InitializePotion()
    {
        foreach (Image potion in potions)
        {
            potion.gameObject.SetActive(true);
        }
    }
}
