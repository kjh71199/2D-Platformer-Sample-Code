using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 수평 이동 중 충돌 시 방향전환 이동 컴포넌트
public class HSwingCollisionMovement : DirectionMovement
{
    [SerializeField] protected LayerMask detectLayer;       // 충돌 감지 레이어

    [SerializeField] protected float detectionDistance;     // 충돌 감지 거리

    [SerializeField] protected Transform checkTransform;    // 충돌 레이를 쏠 위치

    // 이동 메서드 재정의
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
