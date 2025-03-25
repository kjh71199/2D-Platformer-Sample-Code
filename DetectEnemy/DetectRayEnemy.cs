using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Àü¹æ Àû Å½Áö ÄÄÆ÷³ÍÆ®
public class DetectRayEnemy : DetectEnemy
{
    public override Collider2D DetectEnemyCollider(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(detectTransform.position, direction, detectRange, detectLayer);

        if (hit.collider != null)
            return hit.collider;
        else
            return null;
    }

}
