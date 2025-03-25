using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 공격 속성 정의 컴포넌트
public class Damager : MonoBehaviour
{
    [SerializeField] private int damage;                // 공격 데미지
    [SerializeField] private Vector2 knockBack;         // 넉백 벡터
    [SerializeField] private float knockBackTime;       // 넉백 시간
    [SerializeField] GameObject damageEffect;           // 공격 이펙트
    [SerializeField] private float effectDestroyTime;   // 공격 이펙트 지속 시간
    [SerializeField] private bool isProjectile;         // 공격한 객체가 투사체인지

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
