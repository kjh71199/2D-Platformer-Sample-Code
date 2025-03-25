using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 거점 순환 이동 컴포넌트
public class PatrolMovement : DirectionMovement
{
    DamageableHitAnimation damageableHitAnimation;

    [SerializeField] private Transform[] waypoints; // 거점 위치 배열

    private int currentWaypointIndex = 0;           // 현재 향할 거점의 인덱스

    protected override void Awake()
    {
        base.Awake();
        damageableHitAnimation = GetComponent<DamageableHitAnimation>();
    }

    // 이동 메서드 재정의
    protected override void Move()
    {
        if (!CanMove) return;
        if (waypoints.Length == 0) return;
        if (damageableHitAnimation.IsKnockback) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        rigidbody2d.velocity = Vector2.zero;
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, MoveSpeed * Time.deltaTime);

        if (targetWaypoint.position.x < transform.position.x)
            MoveDirection = -Vector2.right;
        else
            MoveDirection = Vector2.right;

        Flip();

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (IsRight)
            {
                if (currentWaypointIndex < waypoints.Length - 1)
                    currentWaypointIndex++;
                else
                    currentWaypointIndex--;
            }
            else
            {
                if (currentWaypointIndex > 0)
                    currentWaypointIndex--;
                else
                    currentWaypointIndex++;
            }
        }
    }
}
