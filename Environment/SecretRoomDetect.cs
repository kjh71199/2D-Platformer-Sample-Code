using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� �� Ž�� ������Ʈ
public class SecretRoomDetect : MonoBehaviour
{
    SpriteRenderer spriteRenderer;  // ��������Ʈ ������ ����
    Collider2D collider2d;          // �ݶ��̴� ����

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

    // ��й� Ž�� ���� �� ������ ����ȭ
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
