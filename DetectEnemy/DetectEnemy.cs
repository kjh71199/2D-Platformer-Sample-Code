using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 탐지 방식 설정 추상 컴포넌트
public abstract class DetectEnemy : MonoBehaviour
{
    [SerializeField] protected Transform detectTransform;   // 탐지 위치
    [SerializeField] protected float detectRange;           // 탐지 거리(반지름)
    [SerializeField] protected LayerMask detectLayer;       // 탐지 레이어

    // 적 탐지 추상 메서드
    public abstract Collider2D DetectEnemyCollider(Vector2 direction);
}
