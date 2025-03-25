using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� Ž�� ��� ���� �߻� ������Ʈ
public abstract class DetectEnemy : MonoBehaviour
{
    [SerializeField] protected Transform detectTransform;   // Ž�� ��ġ
    [SerializeField] protected float detectRange;           // Ž�� �Ÿ�(������)
    [SerializeField] protected LayerMask detectLayer;       // Ž�� ���̾�

    // �� Ž�� �߻� �޼���
    public abstract Collider2D DetectEnemyCollider(Vector2 direction);
}
