using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BossPhase1State : MonoBehaviour
{
    // 몬스터 유한상태기계 컨트롤러
    protected BossPhase1FSMController controller;
    protected Animator animator;

    protected BossPhase1FSMInfo fsmInfo;   // 상태 정보
    protected DirectionMovement movement;
    protected BossSoundFx soundFx;

    [Range(1f, 2f)]
    [SerializeField] protected float animSpeed; // 애니메이션 재생 속도

    protected virtual void Awake()
    {
        controller = GetComponent<BossPhase1FSMController>();
        animator = GetComponent<Animator>();
        fsmInfo = GetComponent<BossPhase1FSMInfo>();
        movement = GetComponent<DirectionMovement>();
        soundFx = GetComponent<BossSoundFx>();
    }

    // 몬스터 상태 관련 인터페이스 메소드 선언

    // 몬스터 상태 시작(다른 상태에서 전이됨) 메소드
    public virtual void EnterState(BossPhase1FSMController.STATE state, object data = null)
    {
        animator.speed = animSpeed; // (임시) 애니메이션 속도 보정
    }

    // 몬스터 상태 업데이트 추상 메소드 (상태 동작 수행)
    public abstract void UpdateState();

    // 몬스터 상태 종료(다른 상태로 전이됨) 추상 메소드
    public abstract void ExitState();
    
}
