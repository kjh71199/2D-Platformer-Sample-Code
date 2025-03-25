using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� �� Ž�� ������Ʈ
public class DetectAreaEnemy : DetectEnemy
{
    public override Collider2D DetectEnemyCollider(Vector2 direction)
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(detectTransform.position, detectRange, detectLayer);

        if (hitCollider != null)
            return hitCollider;
        else
            return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectTransform.position, detectRange);
    }

}
