using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ǰ� ȿ�� ������Ʈ
public class Damageable : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody2d;
    protected Collider2D collider2d;
    protected Animator animator;
    protected DirectionMovement movement;   // ���� ������ ������Ʈ ����
    protected Damager damager;              // ���� �Ӽ� ���� ������Ʈ

    [SerializeField] private int maxHp;                 // �ִ� ü��
    [SerializeField] protected int hp;                  // ���� ü��
    [SerializeField] protected string[] collisionTags;  // �浹 ��ü�� �±� �迭
    [SerializeField] private bool isAlive = true;       // ����ִ� ��������
    [SerializeField] private bool isInvincible;         // ���� ��������
    [SerializeField] private bool isEnvironment;        // ��������

    [SerializeField] protected bool showHitColor;       // �ǰ� �� �ǰ� ���� ȿ���� ����ϴ���
    [SerializeField] protected bool showing = false;    // �ǰ� ���� ȿ�� ��� ������
    [SerializeField] protected int showHitColorCount;   // �ǰ� ���� ȿ�� ��� Ƚ��
    [SerializeField] protected float showHitColorTime;  // �ǰ� ���� ȿ�� ��� �ð�

    private Vector2 hitPosition;            // ������ ���� ��ġ

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

    // �ǰ� ����
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

    // ������ �ο�
    protected void TakeDamage(int damage)
    {
        Hp -= damage;

        if (Hp <= 0)
        {
            IsAlive = false;
            Die();
        }
    }

    // �ǰ� ���� ȿ�� �ڷ�ƾ ����
    protected void ShowHitColor()
    {
        if (!showing)
            StartCoroutine(ShowHitColorCoroutine());
    }

    // �ǰ� ���� ȿ�� �ڷ�ƾ
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

    // ��� ó��
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
