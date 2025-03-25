using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 피격 효과 컴포넌트
public class Damageable : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody2d;
    protected Collider2D collider2d;
    protected Animator animator;
    protected DirectionMovement movement;   // 공통 움직임 컴포넌트 참조
    protected Damager damager;              // 공격 속성 정의 컴포넌트

    [SerializeField] private int maxHp;                 // 최대 체력
    [SerializeField] protected int hp;                  // 현재 체력
    [SerializeField] protected string[] collisionTags;  // 충돌 객체의 태그 배열
    [SerializeField] private bool isAlive = true;       // 살아있는 상태인지
    [SerializeField] private bool isInvincible;         // 무적 상태인지
    [SerializeField] private bool isEnvironment;        // 지형인지

    [SerializeField] protected bool showHitColor;       // 피격 시 피격 색상 효과를 재생하는지
    [SerializeField] protected bool showing = false;    // 피격 색상 효과 재생 중인지
    [SerializeField] protected int showHitColorCount;   // 피격 색상 효과 재생 횟수
    [SerializeField] protected float showHitColorTime;  // 피격 색상 효과 재생 시간

    private Vector2 hitPosition;            // 공격을 맞은 위치

    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int Hp { get => hp; set { hp = value; if (hp > maxHp) hp = MaxHp; } }
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
    public Vector2 HitPosition { get => hitPosition; set => hitPosition = value; }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        movement = GetComponent<DirectionMovement>();

        Hp = MaxHp;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        foreach (string tag in collisionTags)
        {
            if (collider.tag.Equals(tag))
            {
                damager = collider.GetComponent<Damager>();

                if (damager != null)
                {
                    HitPosition = collider.ClosestPoint(damager.transform.position);
                    HitProcess(damager);
                }

                break;
            }
        }
    } 

    // 피격 실행
    public virtual void HitProcess(Damager damager)
    {
        if (IsInvincible || !isAlive) return;

        if (showHitColor)
            ShowHitColor();

        if (!isEnvironment)
            TakeDamage(damager.Damage);

        if (damager.DamageEffect != null)
        {
            GameObject hitPrefab = Instantiate(damager.DamageEffect, HitPosition, Quaternion.identity);
            
            if (damager.transform.position.x > transform.position.x)
                hitPrefab.transform.localScale = new Vector3(-1f, 1f, 1f);

            Destroy(hitPrefab, damager.EffectDestroyTime);
        }
    }

    // 데미지 부여
    protected void TakeDamage(int damage)
    {
        Hp -= damage;

        if (Hp <= 0)
        {
            IsAlive = false;
            Die();
        }
    }

    // 피격 색상 효과 코루틴 실행
    protected void ShowHitColor()
    {
        if (!showing)
            StartCoroutine(ShowHitColorCoroutine());
    }

    // 피격 색상 효과 코루틴
    protected IEnumerator ShowHitColorCoroutine()
    {
        showing = true;

        for (int i = 0; i < showHitColorCount; i++)
        {
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(showHitColorTime);

            spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f);

            yield return new WaitForSeconds(showHitColorTime);
        }

        spriteRenderer.color = Color.white;

        showing = false;
    }

    // 사망 처리
    protected virtual void Die()
    {
        IsAlive = false;
        collider2d.enabled = false;
        rigidbody2d.gravityScale = 0f;
        rigidbody2d.velocity = Vector2.zero;
        movement.CanMove = false;
        animator.SetBool(AnimatorStringToHash.Die, true);
        Destroy(gameObject, 2f);
    }

}
