using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 아이템 획득 컴포넌트
public class PlayerItemPickUp : MonoBehaviour
{
    PlayerInputAction action;   // 플레이어 액션 컴포넌트 참조

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
