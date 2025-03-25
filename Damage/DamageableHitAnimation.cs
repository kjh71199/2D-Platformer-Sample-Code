using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ǰ� �ִϸ��̼� ������Ʈ
public class DamageableHitAnimation : Damageable
{
    [SerializeField] private bool knockbackable;    // �˹� ������ ��������
    private bool isKnockback;                       // �˹� ��������

    protected bool Knockbackable { get => knockbackable; set => knockbackable = value; }
    public bool IsKnockback { get => isKnockback; set => isKnockback = value; }

    // �ǰ� ���� ������(�ǰ� �ִϸ��̼� �߰�, �˹� ȿ�� �߰�)
    public override void HitProcess(Damager damager)
    {
        base.HitProcess(damager);

        if (IsAlive && !IsInvincible)
        {
            animator.SetTrigger(AnimatorStringToHash.Hit);

            Vector2 hitPosition = damager.transform.position;

            if (hitPosition.x < transform.position.x)
            {
                movement.MoveDirection = Vector2.left;
                movement.Flip();
            }
            else
            {
                movement.MoveDirection = Vector2.right;
                movement.Flip();
            }

            if (knockbackable && damager.KnockBackTime > 0)
                KnockBack(hitPosition, damager.KnockBack, damager.KnockBackTime);
        }
    }

    // �˹� �ڷ�ƾ ����
    public void KnockBack(Vector2 hitPosition, Vector2 knockBackForce, float knockBackTime)
    {
        Vector2 knockBackDirection = (transform.position.x - hitPosition.x) > 0 ? Vector2.right : -Vector2.right;
        Vector2 knockBack = new Vector2(knockBackForce.x * knockBackDirection.x, knockBackForce.y);

        StartCoroutine(KnockBackCoroutine(knockBack, knockBackTime));
    }

    // �˹� �ڷ�ƾ
    protected IEnumerator KnockBackCoroutine(Vector2 knockBack, float knockBackTime)
    {    
        float time = 0f;
        rigidbody2d.velocity = Vector3.zero;
        isKnockback = true;
        while (time < knockBackTime)
        {
            time += Time.deltaTime;
            rigidbody2d.velocity = knockBack;
            yield return new WaitForFixedUpdate();
        }
        isKnockback = false;
    }

}
