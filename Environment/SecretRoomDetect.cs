using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 비밀 방 탐지 컴포넌트
public class SecretRoomDetect : MonoBehaviour
{
    SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 참조
    Collider2D collider2d;          // 콜라이더 참조

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            StartCoroutine(FadeOutCoroutine());
            collider2d.enabled = false;
        }
    }

    // 비밀방 탐색 성공 시 가림막 투명화
    private IEnumerator FadeOutCoroutine()
    {
        float time = 1f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, time);
            yield return null;
        }
    }
}
