using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 FSM 동작관련 공통 정보
public class BossPhase1FSMInfo : MonoBehaviour
{
    [Header("근접 공격 관련 속성")]
    [SerializeField] private float meleeAttackDistance;
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private int meleeAttackCount;
    [SerializeField] private float meleeAttackCooldown;
    [SerializeField] private bool isMeleeAttack;
     
    [Header("마법 공격 관련 속성")]
    [SerializeField] private float magicAttackDistance;
    [SerializeField] private int magicAttackDamage;
    [SerializeField] private float magicAttackCooldown;
    [SerializeField] private bool isMagicAttack;

    [Header("이동 관련 속성")]
    [SerializeField] private float moveSpeed;
    

    // 속성 프로퍼티들
    public float MeleeAttackDistance { get => meleeAttackDistance; set => meleeAttackDistance = value; }
    public int MeleeAttackDamage { get => meleeAttackDamage; set => meleeAttackDamage = value; }
    public int MeleeAttackCount { get => meleeAttackCount; set => meleeAttackCount = value; }
    public float MeleeAttackCooldown { get => meleeAttackCooldown; set => meleeAttackCooldown = value; }
    public bool IsMeleeAttack { get => isMeleeAttack; set => isMeleeAttack = value; }
    public float MagicAttackDistance { get => magicAttackDistance; set => magicAttackDistance = value; }
    public int MagicAttackDamage { get => magicAttackDamage; set => magicAttackDamage = value; }
    public float MagicAttackCooldown { get => magicAttackCooldown; set => magicAttackCooldown = value; }
    public bool IsMagicAttack { get => isMagicAttack; set => isMagicAttack = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
}
