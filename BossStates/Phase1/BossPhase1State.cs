using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BossPhase1State : MonoBehaviour
{
    // ���� ���ѻ��±�� ��Ʈ�ѷ�
    protected BossPhase1FSMController controller;
    protected Animator animator;

    protected BossPhase1FSMInfo fsmInfo;   // ���� ����
    protected DirectionMovement movement;
    protected BossSoundFx soundFx;

    [Range(1f, 2f)]
    [SerializeField] protected float animSpeed; // �ִϸ��̼� ��� �ӵ�

    protected virtual void Awake()
    {
        controller = GetComponent<BossPhase1FSMController>();
        animator = GetComponent<Animator>();
        fsmInfo = GetComponent<BossPhase1FSMInfo>();
        movement = GetComponent<DirectionMovement>();
        soundFx = GetComponent<BossSoundFx>();
    }

    // ���� ���� ���� �������̽� �޼ҵ� ����

    // ���� ���� ����(�ٸ� ���¿��� ���̵�) �޼ҵ�
    public virtual void EnterState(BossPhase1FSMController.STATE state, object data = null)
    {
        animator.speed = animSpeed; // (�ӽ�) �ִϸ��̼� �ӵ� ����
    }

    // ���� ���� ������Ʈ �߻� �޼ҵ� (���� ���� ����)
    public abstract void UpdateState();

    // ���� ���� ����(�ٸ� ���·� ���̵�) �߻� �޼ҵ�
    public abstract void ExitState();
    
}
