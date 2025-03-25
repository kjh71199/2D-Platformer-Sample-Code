using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ������ ȹ�� ������Ʈ
public class PlayerItemPickUp : MonoBehaviour
{
    PlayerInputAction action;   // �÷��̾� �׼� ������Ʈ ����

    private void Awake()
    {
        action = GetComponent<PlayerInputAction>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Item"))
        {
            action.ItemObject = collision.GetComponent<ItemObject>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Item"))
        {
            action.ItemObject = null;
        }
    }

}
