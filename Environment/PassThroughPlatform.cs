using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아래로 통과 가능한 플랫폼 컴포넌트
public class PassThroughPlatform : MonoBehaviour
{
    [SerializeField] PlayerInputMovement playerMovement;    // 플레이어 이동 컴포넌트 참조

    Collider2D platformCollider;            // 플랫폼의 콜라이더 참조

    [SerializeField] private bool onPlayer; // 플레이어가 플랫폼 위에 있는지

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

    // 아래 점프로 플랫폼 통과
    IEnumerator JumpDownPlatformCoroutine()
    {
        yield return new WaitForEndOfFrame();
        platformCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        platformCollider.enabled = true;
    }

    // 사다리로 플랫폼 통과
    IEnumerator LadderDownPlatformCoroutine()
    {
        platformCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        platformCollider.enabled = true;
    }
}
