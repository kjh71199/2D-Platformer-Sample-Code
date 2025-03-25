using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이동 추상 클래스
public abstract class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;   // 이동 속도

    protected SpriteRenderer spriteRenderer;    // 스프라이트 렌더러 참조

    protected Animator animator;                // 애니메이터 참조

    protected Rigidbody2D rigidbody2d;          // 강체 물리 컴포넌트 참조

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // 이동 추상 메서드
    protected abstract void Move();
}
