using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 시스템 컴포넌트
public class InventorySystem : MonoBehaviour
{
    private static InventorySystem instance;            // 인벤토리 시스템 싱글턴 인스턴스
    public static InventorySystem Instance {  get { return instance; } }

    [SerializeField] private ItemList itemList;         // 아이템 리스트
    [SerializeField] private int inventorySize;         // 인벤토리 크기
    [SerializeField] private InventoryUI inventoryUI;   // 인벤토리 UI 컴포넌트 참조

    [SerializeField] private List<Item> hasItemList = new List<Item>(); // 획득한 아이템 리스트

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

    // 아이템 추가
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
