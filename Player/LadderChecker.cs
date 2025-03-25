using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 사다리와 충돌 했는 지 판단하는 컴포넌트
public class LadderChecker : MonoBehaviour
{
    PlayerInputMovement movement;   // 플레이어 이동 컴포넌트 참조

    private void Awake()
    {
        movement = GetComponentInParent<PlayerInputMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            movement.OnLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            movement.OnLadder = false;
        }
    }
}
