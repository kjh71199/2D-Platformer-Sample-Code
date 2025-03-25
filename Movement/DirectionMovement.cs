using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �̵� ������Ʈ
public class DirectionMovement : Movement
{
    [SerializeField] protected Vector2 moveDirection;   // �̵� ����
    [SerializeField] private bool isRight = true;       // ���� �������� �ٶ󺸰� �ִ���
    [SerializeField] private bool canMove = true;       // ���� ������ �� �ִ� ��������

    public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public bool IsRight { get => isRight; set => isRight = value; }
    public bool CanMove { get => canMove; set => canMove = value; }

    // ������ ���� ����
    public void SetDirection(Vector2 direction)
    {
        MoveDirection = direction;
    }

    protected virtual void Update()
    {
        Move();
    }

    // �����̴� ���⿡ ���� ��������Ʈ ������
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

    // �̵� ó��
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
