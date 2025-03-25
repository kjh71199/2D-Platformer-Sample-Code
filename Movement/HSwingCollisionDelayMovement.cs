using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ���� �̵� �� �浹�� ������ȯ, �߰� ��� �ð� ���� �̵� ������Ʈ
public class HSwingCollisionDelayMovement : HSwingCollisionMovement
{
    MonsterChargeAttack chargeAttack;   // ���� ���� ������Ʈ ����

    [SerializeField] protected float minDelayTime;  // �ּ� ��� �ð�
    [SerializeField] protected float maxDelayTime;  // �ִ� ��� �ð�
    [SerializeField] protected float minMoveTime;   // �ּ� �̵� �ð�
    [SerializeField] protected float maxMoveTime;   // �ִ� �̵� �ð�

    bool isCycle;           // �̵� - ��� ����Ŭ ������
    bool isMove;            // �̵� ������
    bool isDelay;           // ��� ������
    float delayTime;        // ��� �ð�
    float moveTime;         // �̵� �ð�
    float randomDelayTime;  // ���� ��� �ð�
    float randomMoveTime;   // ���� �̵� �ð�
    float guardDelayTime;   // ���������� ������ �� ��� �ð�

    protected override void Awake()
    {
        base.Awake();
        chargeAttack = GetComponent<MonsterChargeAttack>();

        isCycle = true;
    }

    protected override void Update()
    {
        if (chargeAttack.IsCharge)
        {
            isCycle = true;
            return;
        }

        if (chargeAttack.IsGuarded)
        {
            guardDelayTime += Time.deltaTime;
            animator.SetFloat(AnimatorStringToHash.Move, 0f);

            if (guardDelayTime >= 2f)
            {
                chargeAttack.IsGuarded = false;
                guardDelayTime = 0f;
            }
            
            return;
        }

        if (isCycle)
        {
            isCycle = false;
            isMove = true;
            isDelay = false;
            moveTime = 0f;
            delayTime = 0f;
            randomDelayTime = Random.Range(minDelayTime, maxDelayTime);
            randomMoveTime = Random.Range(minMoveTime, maxMoveTime);
        }
        
        if (isMove)
        {
            moveTime += Time.deltaTime;
            animator.SetFloat(AnimatorStringToHash.Move, MoveSpeed);

            if (moveTime >= randomMoveTime)
            {
                isMove = false;
                isDelay = true;
            }

            base.Update();
        }

        if (isDelay)
        {
            delayTime += Time.deltaTime;
            animator.SetFloat(AnimatorStringToHash.Move, 0f);

            if (delayTime >= randomDelayTime)
            {
                isDelay = false;
                isCycle = true;
            }
        }
    }
}
