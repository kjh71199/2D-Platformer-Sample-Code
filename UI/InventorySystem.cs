using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �κ��丮 �ý��� ������Ʈ
public class InventorySystem : MonoBehaviour
{
    private static InventorySystem instance;            // �κ��丮 �ý��� �̱��� �ν��Ͻ�
    public static InventorySystem Instance {  get { return instance; } }

    [SerializeField] private ItemList itemList;         // ������ ����Ʈ
    [SerializeField] private int inventorySize;         // �κ��丮 ũ��
    [SerializeField] private InventoryUI inventoryUI;   // �κ��丮 UI ������Ʈ ����

    [SerializeField] private List<Item> hasItemList = new List<Item>(); // ȹ���� ������ ����Ʈ

    public ItemList ItemList { get => itemList; set => itemList = value; }
    public List<Item> HasItemList { get => hasItemList; set => hasItemList = value; }

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

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        inventoryUI.InitInventoryUI();
    }

    // ������ �߰�
    public bool AddItem(int index)
    {
        Item item = ItemList.List[index];

        if (HasItemList.Count >= inventorySize) return false;

        if (item != null)
        {
            HasItemList.Add(item);
        }

        if (inventoryUI.gameObject.activeSelf)
        {
            inventoryUI.UpdateInventoryUI();
        }

        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.transform.parent.gameObject.activeSelf == false)
            {
                inventoryUI.OpenUI();
                InventoryUI.onPauseDelegate();
            }
            else
            {
                InventoryUI.onResumeDelegate();
                inventoryUI.CloseUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventoryUI.transform.parent.gameObject.activeSelf == true)
            {
                InventoryUI.onResumeDelegate();
                inventoryUI.CloseUI();
            }
        }
    }
}
