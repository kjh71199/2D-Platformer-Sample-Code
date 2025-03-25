using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���Ÿ� ���� ������Ʈ
public class RangeAttack : MonoBehaviour
{
    protected Animator animator;
    protected DirectionMovement movement;
    protected WaitForSeconds attackDelay;

    [SerializeField] protected Collider2D hitCollider;      // ������ ���� �ݶ��̴�
    [SerializeField] protected LayerMask hitLayer;          // ���� ó�� ���̾�
    [SerializeField] protected DetectEnemy detectEnemy;     // �÷��̾� Ž�� ��� ����
    [SerializeField] protected Transform attackTransform;   // ���� ��ġ
    [SerializeField] protected float attackDelayTime;       // ���� ������
    [SerializeField] protected Vector2 attackDirection;     // ���� ����
    [SerializeField] protected GameObject bulletPrefab;     // ����ü ������

    protected bool isAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<DirectionMovement>();
        attackDelay = new WaitForSeconds(attackDelayTime);
    }

    private void Start()
    {
        StartCoroutine(AttackCoroutine());
    }

    protected virtual void ShootingProcess()
    {
        movement.CanMove = false;
        attackDirection = (hitCollider.transform.position - attackTransform.position).normalized;

        if (hitCollider.transform.position.x < transform.position.x)
        {
            movement.MoveDirection = -Vector2.right;
            movement.Flip();
        }
        else if (hitCollider.transform.position.x > transform.position.x)
        {
            movement.MoveDirection = Vector2.right;
            movement.Flip();
        }

        animator.SetTrigger(AnimatorStringToHash.Attack);
    }

    public virtual void Fire()
    {
        float angle = GetDirectionAngle();
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        GameObject bulletGameObject = Instantiate(bulletPrefab, attackTransform.position, rotation);

        DirectionMovement bulletMovement = bulletGameObject.GetComponent<DirectionMovement>();
        if (bulletMovement != null)
        {
            bulletMovement.MoveDirection = attackDirection;
        }
    }

    protected float GetDirectionAngle()
    {
        float radian = Mathf.Atan2(attackDirection.y, attackDirection.x);
        float degree = radian * Mathf.Rad2Deg;
        return degree;
    }

    protected virtual IEnumerator AttackCoroutine()
    {
        while (true)
        {
            hitCollider = detectEnemy.DetectEnemyCollider(movement.MoveDirection);

            if (hitCollider != null)
            {
                if (!isAttack)
                {
                    ShootingProcess();
                    isAttack = true;
                }
                else
                {
                    yield return attackDelay;
                    isAttack = false;
                }
            }
            else
            {
                movement.CanMove = true;
                yield return null;
            }
        }
    }
}
