using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 아이템 정보 UI
public class ItemInfoUI : MonoBehaviour
{
    [SerializeField] private Text itemNameText;         // 아이템 이름
    [SerializeField] private Text itemDescriptionText;  // 아이템 설명

    private InventoryUI inventoryUI;
    private Item item;

    // 아이템 정보 UI 초기화
    public void InitItemInfo()
    {
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
    }

    // 아이템 정보 UI 갱신
    public void ShowItemInfo(InventoryUI inventoryUI, Item item)
    {
        this.inventoryUI = inventoryUI;

        this.item = item;

        itemNameText.text = item.ItemName;
        itemDescriptionText.text = item.ItemDescription;
    }
}
