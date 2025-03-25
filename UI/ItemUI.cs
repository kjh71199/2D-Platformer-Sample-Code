using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 아이템 슬롯 UI
public class ItemUI : MonoBehaviour
{
    [SerializeField] private Image itemSelectImage; // 아이템 선택표시 이미지
    [SerializeField] private Image itemImage;       // 아이템 이미지

    private InventoryUI inventoryUI;    // 인벤토리 UI 컴포넌트 참조

    private Item item = null;           // 이 아이템 슬롯에 할당할 아이템 정보

    [SerializeField] private bool isSelected = false;   // 이 아이템 슬롯이 선택 되었는지

    [SerializeField] private Color32 selectColor;       // 선택 시 표시 색상
    [SerializeField] private Color32 deselectColor;     // 선택 해제 시 표시 색상

    public Item Item { get => item; set => item = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }

    // 아이템 슬롯 초기화
    public void Init(InventoryUI inventoryUI, Item item)
    {
        this.inventoryUI = inventoryUI;

        this.Item = item;

        itemImage.sprite = this.Item.ItemIconImage;
        itemImage.color = Color.white;
    }

    // 아이템 슬롯 비우기
    public void ClearItemUI()
    {
        itemSelectImage.color = deselectColor;
        itemImage.sprite = null;
    }

    // 아이템 선택 표시 처리
    public void ItemSelect()
    {
        if (Item == null) return;

        inventoryUI.ItemSelect(Item);

        if (!IsSelected)
        {
            itemSelectImage.color = selectColor;
            IsSelected = true;
        }
    }

    // 아이템 선택 해제 처리
    public void ItemDeselect()
    {
        if (IsSelected)
        {
            itemSelectImage.color = deselectColor;
            IsSelected = false;
        }
    }
}
