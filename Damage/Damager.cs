using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ���� �Ӽ� ���� ������Ʈ
public class Damager : MonoBehaviour
{
    [SerializeField] private int damage;                // ���� ������
    [SerializeField] private Vector2 knockBack;         // �˹� ����
    [SerializeField] private float knockBackTime;       // �˹� �ð�
    [SerializeField] GameObject damageEffect;           // ���� ����Ʈ
    [SerializeField] private float effectDestroyTime;   // ���� ����Ʈ ���� �ð�
    [SerializeField] private bool isProjectile;         // ������ ��ü�� ����ü����

    public int Damage { get => damage; set => damage = value; }
    public Vector2 KnockBack { get => knockBack; set => knockBack = value; }
    public float KnockBackTime { get => knockBackTime; set => knockBackTime = value; }
    public GameObject DamageEffect { get => damageEffect; set => damageEffect = value; }
    public float EffectDestroyTime { get => effectDestroyTime; set => effectDestroyTime = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isProjectile) return;

        if (collision.tag.Equals("Environment"))
            Destroy(gameObject);

        if (collision.tag.Equals("Player"))
        {
            bool isInvincible = collision.GetComponent<Damageable>().IsInvincible;

            if (isInvincible) return;

            Destroy(gameObject);
        }
    }
}
