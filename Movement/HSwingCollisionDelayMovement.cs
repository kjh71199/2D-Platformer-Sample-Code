using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 수평 이동 중 충돌시 방향전환, 중간 대기 시간 있음 이동 컴포넌트
public class HSwingCollisionDelayMovement : HSwingCollisionMovement
{
    MonsterChargeAttack chargeAttack;   // 돌진 공격 컴포넌트 참조

    [SerializeField] protected float minDelayTime;  // 최소 대기 시간
    [SerializeField] protected float maxDelayTime;  // 최대 대기 시간
    [SerializeField] protected float minMoveTime;   // 최소 이동 시간
    [SerializeField] protected float maxMoveTime;   // 최대 이동 시간

    bool isCycle;           // 이동 - 대기 사이클 중인지
    bool isMove;            // 이동 중인지
    bool isDelay;           // 대기 중인지
    float delayTime;        // 대기 시간
    float moveTime;         // 이동 시간
    float randomDelayTime;  // 랜덤 대기 시간
    float randomMoveTime;   // 랜덤 이동 시간
    float guardDelayTime;   // 돌진공격이 막혔을 시 대기 시간

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
