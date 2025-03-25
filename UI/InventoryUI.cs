using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �κ��丮 UI ������Ʈ
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private RectTransform[] itemUISlots;       // ������ ���� ��ġ �迭 ����
    [SerializeField] private ItemUI[] itemUIs;                  // ������ UI ��ü �迭
    [SerializeField] private GameObject itemUIPrefab;           // ������ UI ������
    [SerializeField] private InventorySystem inventorySystem;   // �κ��丮 �ý��� ������Ʈ ����
    [SerializeField] private ItemInfoUI itemInfoUI;             // ������ ���� UI �г�

    [SerializeField] private ItemUI currentSelectedUI;          // ���� ���õ� ������

    private PlayerInputMovement movement;   // �÷��̾� �̵� ������Ʈ ����
    private PlayerInputAction action;       // �÷��̾� �׼� ������Ʈ ����

    public delegate void OnPauseDelegate();
    public static OnPauseDelegate onPauseDelegate;      // �Ͻ����� ��������Ʈ

    public delegate void OnResumeDelegate();
    public static OnResumeDelegate onResumeDelegate;    // �Ͻ����� ���� ��������Ʈ

    public ItemUI[] ItemUIs { get => itemUIs; set => itemUIs = value; }

    private void OnEnable()
    {
        onPauseDelegate += OnPause;
        onResumeDelegate += OnResume;

        UpdateInventoryUI();

        if (inventorySystem.HasItemList.Count > 0)
        {
            ItemUIs[0].ItemSelect();
        }
    }

    private void OnDisable()
    {
        onPauseDelegate -= OnPause;
        onResumeDelegate -= OnResume;
    }

    private void Awake()
    {
        movement = FindAnyObjectByType<PlayerInputMovement>();
        action = FindAnyObjectByType<PlayerInputAction>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectChange(0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectChange(1);
        }
    }

    // ���õ� ������ ����
    private void SelectChange(int direction)
    {
        foreach (ItemUI item in ItemUIs)
        {
            if (item.IsSelected)
            {
                currentSelectedUI = item;
                break;
            }
        }

        int index = currentSelectedUI.Item.ItemId;

        if (direction == 0)
        {
            if (index == 0) return;

            itemUIs[index - 1].ItemSelect();
        }
        else if (direction == 1)
        {
            if (index == ItemUIs.Length - 1) return;

            itemUIs[index + 1].ItemSelect();
        }
    }

    // �κ��丮 UI �ʱ�ȭ
    public void InitInventoryUI()
    {
        ItemUIs = new ItemUI[itemUISlots.Length];

        for (int i = 0; i < itemUISlots.Length; i++)
        {
            ItemUIs[i] = Instantiate(itemUIPrefab, itemUISlots[i]).GetComponent<ItemUI>();
        }

        itemInfoUI.InitItemInfo();
    }

    // �κ��丮 UI ����
    public void UpdateInventoryUI()
    {
        for (int i = 0; i < itemUISlots.Length; i++)
        {
            ItemUIs[i].ClearItemUI();
        }

        for (int i = 0; i < inventorySystem.HasItemList.Count; i++)
        {
            Item item = inventorySystem.HasItemList[i];
            ItemUIs[i].Init(this, item);
        }
    }

    // ������ ����
    public void ItemSelect(Item item)
    {
        ItemAllDeselect();

        itemInfoUI.ShowItemInfo(this, item);
    }

    // ��� ������ ���� ����
    public void ItemAllDeselect()
    {
        for (int i = 0; i < itemUISlots.Length; i++)
        {
            ItemUIs[i].ItemDeselect();
        }
    }

    // �κ��丮 �ѱ�
    public void OpenUI()
    {
        transform.parent.gameObject.SetActive(true);
    }

    // �κ��丮 ����
    public void CloseUI()
    {
        transform.parent.gameObject.SetActive(false);
    }

    // �Ͻ�����
    private void OnPause()
    {
        Time.timeScale = 0f;
        movement.CanMove = false;
        action.CanAction = false;
    }

    // �Ͻ����� ����
    private void OnResume()
    {
        Time.timeScale = 1f;
        movement.CanMove = true;
        action.CanAction = true;
    }
}
