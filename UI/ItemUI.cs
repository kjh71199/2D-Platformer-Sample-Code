using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ ���� UI
public class ItemUI : MonoBehaviour
{
    [SerializeField] private Image itemSelectImage; // ������ ����ǥ�� �̹���
    [SerializeField] private Image itemImage;       // ������ �̹���

    private InventoryUI inventoryUI;    // �κ��丮 UI ������Ʈ ����

    private Item item = null;           // �� ������ ���Կ� �Ҵ��� ������ ����

    [SerializeField] private bool isSelected = false;   // �� ������ ������ ���� �Ǿ�����

    [SerializeField] private Color32 selectColor;       // ���� �� ǥ�� ����
    [SerializeField] private Color32 deselectColor;     // ���� ���� �� ǥ�� ����

    public Item Item { get => item; set => item = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }

    // ������ ���� �ʱ�ȭ
    public void Init(InventoryUI inventoryUI, Item item)
    {
        this.inventoryUI = inventoryUI;

        this.Item = item;

        itemImage.sprite = this.Item.ItemIconImage;
        itemImage.color = Color.white;
    }

    // ������ ���� ����
    public void ClearItemUI()
    {
        itemSelectImage.color = deselectColor;
        itemImage.sprite = null;
    }

    // ������ ���� ǥ�� ó��
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

    // ������ ���� ���� ó��
    public void ItemDeselect()
    {
        if (IsSelected)
        {
            itemSelectImage.color = deselectColor;
            IsSelected = false;
        }
    }
}
