using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ʒ��� ��� ������ �÷��� ������Ʈ
public class PassThroughPlatform : MonoBehaviour
{
    [SerializeField] PlayerInputMovement playerMovement;    // �÷��̾� �̵� ������Ʈ ����

    Collider2D platformCollider;            // �÷����� �ݶ��̴� ����

    [SerializeField] private bool onPlayer; // �÷��̾ �÷��� ���� �ִ���

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (playerMovement == null) return;

        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.C) && onPlayer)
        {
            StartCoroutine(JumpDownPlatformCoroutine());
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && playerMovement.OnGroundLadder)
        {
            StartCoroutine(LadderDownPlatformCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LadderChecker"))
        {
            platformCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LadderChecker"))
        {
            platformCollider.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            onPlayer = true;
            playerMovement = collision.gameObject.GetComponent<PlayerInputMovement>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            onPlayer = false;
            playerMovement = null;
        }
    }

    // �Ʒ� ������ �÷��� ���
    IEnumerator JumpDownPlatformCoroutine()
    {
        yield return new WaitForEndOfFrame();
        platformCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        platformCollider.enabled = true;
    }

    // ��ٸ��� �÷��� ���
    IEnumerator LadderDownPlatformCoroutine()
    {
        platformCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        platformCollider.enabled = true;
    }
}
