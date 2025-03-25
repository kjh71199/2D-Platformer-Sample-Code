using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase2FSMController : MonoBehaviour
{
    // 몬스터 상태들
    public enum STATE { WALK, MELEEATTACK, MAGICATTACK, HEAVYMAGICATTACK, TELEPORTSTART, TELEPORTATTACK, TELEPORTEND, DIE }

    [SerializeField] private Damageable damageable;
    [SerializeField] private BossPhase2State currentState;
    [SerializeField] private BossPhase2State[] monsterStates;

    private GameObject player;

    public Damageable Damageable { get => damageable; set => damageable = value; }
    public GameObject Player { get => player; set => player = value; }

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");

        TransactionToState(STATE.WALK);
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void TransactionToState(STATE state, object data = null)
    {
        if (currentState == monsterStates[(int)STATE.DIE]) return;

        currentState?.ExitState();
        currentState = monsterStates[(int)state];
        currentState?.EnterState(state, data);
    }

    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, Player.transform.position);
    }
}
