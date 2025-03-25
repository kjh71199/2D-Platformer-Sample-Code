using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 방향 이동 컴포넌트
public class DirectionMovement : Movement
{
    [SerializeField] protected Vector2 moveDirection;   // 이동 방향
    [SerializeField] private bool isRight = true;       // 현재 오른쪽을 바라보고 있는지
    [SerializeField] private bool canMove = true;       // 현재 움직일 수 있는 상태인지

    public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public bool IsRight { get => isRight; set => isRight = value; }
    public bool CanMove { get => canMove; set => canMove = value; }

    // 움직일 방향 설정
    public void SetDirection(Vector2 direction)
    {
        MoveDirection = direction;
    }

    protected virtual void Update()
    {
        Move();
    }

    // 움직이는 방향에 맞춰 스프라이트 뒤집기
    public virtual void Flip()
    {
        Vector3 scale = transform.localScale;

        if (MoveDirection.x < 0)
        {
            scale.x = -1;
            IsRight = false;
        }
        else if (MoveDirection.x > 0)
        {
            scale.x = 1;
            IsRight = true;
        }

        transform.localScale = scale;
    }

    // 이동 처리
    protected override void Move()
    {
        if (!canMove) return;

        Flip();

        rigidbody2d.velocity = MoveDirection * MoveSpeed;
    }

    public void EnableMovementEvent()
    {
        canMove = true;
    }

    public void DisableMovementEvent()
    {
        canMove = false;
    }
}
