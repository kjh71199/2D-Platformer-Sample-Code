using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ ���� UI
public class ItemInfoUI : MonoBehaviour
{
    [SerializeField] private Text itemNameText;         // ������ �̸�
    [SerializeField] private Text itemDescriptionText;  // ������ ����

    private InventoryUI inventoryUI;
    private Item item;

    // ������ ���� UI �ʱ�ȭ
    public void InitItemInfo()
    {
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
    }

    // ������ ���� UI ����
    public void ShowItemInfo(InventoryUI inventoryUI, Item item)
    {
        this.inventoryUI = inventoryUI;

        this.item = item;

        itemNameText.text = item.ItemName;
        itemDescriptionText.text = item.ItemDescription;
    }
}
