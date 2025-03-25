using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 UI 컴포넌트
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private RectTransform[] itemUISlots;       // 아이템 슬롯 위치 배열 참조
    [SerializeField] private ItemUI[] itemUIs;                  // 아이템 UI 객체 배열
    [SerializeField] private GameObject itemUIPrefab;           // 아이템 UI 프리팹
    [SerializeField] private InventorySystem inventorySystem;   // 인벤토리 시스템 컴포넌트 참조
    [SerializeField] private ItemInfoUI itemInfoUI;             // 아이템 정보 UI 패널

    [SerializeField] private ItemUI currentSelectedUI;          // 현재 선택된 아이템

    private PlayerInputMovement movement;   // 플레이어 이동 컴포넌트 참조
    private PlayerInputAction action;       // 플레이어 액션 컴포넌트 참조

    public delegate void OnPauseDelegate();
    public static OnPauseDelegate onPauseDelegate;      // 일시정지 델리게이트

    public delegate void OnResumeDelegate();
    public static OnResumeDelegate onResumeDelegate;    // 일시정지 해제 델리게이트

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

    // 선택된 아이템 변경
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

    // 인벤토리 UI 초기화
    public void InitInventoryUI()
    {
        ItemUIs = new ItemUI[itemUISlots.Length];

        for (int i = 0; i < itemUISlots.Length; i++)
        {
            ItemUIs[i] = Instantiate(itemUIPrefab, itemUISlots[i]).GetComponent<ItemUI>();
        }

        itemInfoUI.InitItemInfo();
    }

    // 인벤토리 UI 갱신
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

    // 아이템 선택
    public void ItemSelect(Item item)
    {
        ItemAllDeselect();

        itemInfoUI.ShowItemInfo(this, item);
    }

    // 모든 아이템 선택 해제
    public void ItemAllDeselect()
    {
        for (int i = 0; i < itemUISlots.Length; i++)
        {
            ItemUIs[i].ItemDeselect();
        }
    }

    // 인벤토리 켜기
    public void OpenUI()
    {
        transform.parent.gameObject.SetActive(true);
    }

    // 인벤토리 끄기
    public void CloseUI()
    {
        transform.parent.gameObject.SetActive(false);
    }

    // 일시정지
    private void OnPause()
    {
        Time.timeScale = 0f;
        movement.CanMove = false;
        action.CanAction = false;
    }

    // 일시정지 해제
    private void OnResume()
    {
        Time.timeScale = 1f;
        movement.CanMove = true;
        action.CanAction = true;
    }
}
