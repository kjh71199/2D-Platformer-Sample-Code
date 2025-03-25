using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase1FSMController : MonoBehaviour
{
    // ���� ���µ�
    public enum STATE { IDLE, WALK, MELEEATTACK, MAGICATTACK, TELEPORTSTART, TELEPORTEND, PHASESHIFT }

    [SerializeField] private Damageable damageable;

    // * ���� ���� ���� ����
    [SerializeField] private BossPhase1State currentState;

    // ������ ��� ���µ�
    [SerializeField] private BossPhase1State[] monsterStates;

    // �÷��̾� ����
    private GameObject player;

    public Damageable Damageable { get => damageable; set => damageable = value; }
    public GameObject Player { get => player; set => player = value; }

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");

        // ��� ���·� ����
        TransactionToState(STATE.IDLE);
    }

    private void Update()
    {
        // * ���� ������ ������ ����� ����
        currentState?.UpdateState();
    }

    // * ���� ��ȯ �޼ҵ�
    public void TransactionToState(STATE state, object data = null)
    {
        //Debug.log($"���� ���� ��ȯ : {state}");

        // ���� ���Ͱ� �̹� ��� ���¸� ���� ��ȯ�� ���� ����
        if (currentState == monsterStates[(int)STATE.PHASESHIFT]) return;

        currentState?.ExitState();  // ���� ���� ó��
        currentState = monsterStates[(int)state];   // ���� ��ȯ ó��
        currentState?.EnterState(state, data);  // ���ο� ���� ����
    }

    // �÷��̾�� ���� ���� �Ÿ� ����
    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, Player.transform.position);
    }
}
