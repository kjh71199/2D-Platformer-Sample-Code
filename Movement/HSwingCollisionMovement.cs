using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �̵� �� �浹 �� ������ȯ �̵� ������Ʈ
public class HSwingCollisionMovement : DirectionMovement
{
    [SerializeField] protected LayerMask detectLayer;       // �浹 ���� ���̾�

    [SerializeField] protected float detectionDistance;     // �浹 ���� �Ÿ�

    [SerializeField] protected Transform checkTransform;    // �浹 ���̸� �� ��ġ

    // �̵� �޼��� ������
    protected override void Move()
    {
        if (!CanMove) return;

        RaycastHit2D hit = Physics2D.Raycast(checkTransform.position, moveDirection, detectionDistance, detectLayer);

        if (hit.collider != null)
        {
            MoveDirection = -MoveDirection;
            Flip();
        }

        rigidbody2d.velocity = new Vector2(moveDirection.x * MoveSpeed, rigidbody2d.velocity.y);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(checkTransform.position, checkTransform.position + new Vector3(moveDirection.x, 0, 0) * detectionDistance);
    }

}
