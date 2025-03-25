using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �̵� �߻� Ŭ����
public abstract class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;   // �̵� �ӵ�

    protected SpriteRenderer spriteRenderer;    // ��������Ʈ ������ ����

    protected Animator animator;                // �ִϸ����� ����

    protected Rigidbody2D rigidbody2d;          // ��ü ���� ������Ʈ ����

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // �̵� �߻� �޼���
    protected abstract void Move();
}
