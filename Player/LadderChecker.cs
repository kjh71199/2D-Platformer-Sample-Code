using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ��ٸ��� �浹 �ߴ� �� �Ǵ��ϴ� ������Ʈ
public class LadderChecker : MonoBehaviour
{
    PlayerInputMovement movement;   // �÷��̾� �̵� ������Ʈ ����

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
