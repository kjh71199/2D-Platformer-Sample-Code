using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase2FSMInfo : MonoBehaviour
{
    [Header("捞悼 包访 加己")]
    [SerializeField] private float moveSpeed;

    [Header("辟立 傍拜 包访 加己")]
    [SerializeField] private float meleeAttackDistance;
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private int meleeAttackCount;
    [SerializeField] private float meleeAttackCooldown;
    [SerializeField] private bool isMeleeAttack;

    [Header("付过 傍拜 包访 加己")]
    [SerializeField] private float magicAttackDistance;
    [SerializeField] private int magicAttackDamage;
    [SerializeField] private float magicAttackCooldown;
    [SerializeField] private bool isMagicAttack;

    [Header("付过 傍拜2 包访 加己")]
    [SerializeField] private float heavyMagicAttackDistance;
    [SerializeField] private int heavyMagicAttackDamage;
    [SerializeField] private float heavyMagicAttackCooldown;
    [SerializeField] private bool isHeavyMagicAttack;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float MeleeAttackDistance { get => meleeAttackDistance; set => meleeAttackDistance = value; }
    public int MeleeAttackDamage { get => meleeAttackDamage; set => meleeAttackDamage = value; }
    public int MeleeAttackCount { get => meleeAttackCount; set => meleeAttackCount = value; }
    public float MeleeAttackCooldown { get => meleeAttackCooldown; set => meleeAttackCooldown = value; }
    public bool IsMeleeAttack { get => isMeleeAttack; set => isMeleeAttack = value; }
    public float MagicAttackDistance { get => magicAttackDistance; set => magicAttackDistance = value; }
    public int MagicAttackDamage { get => magicAttackDamage; set => magicAttackDamage = value; }
    public float MagicAttackCooldown { get => magicAttackCooldown; set => magicAttackCooldown = value; }
    public bool IsMagicAttack { get => isMagicAttack; set => isMagicAttack = value; }
    public float HeavyMagicAttackDistance { get => heavyMagicAttackDistance; set => heavyMagicAttackDistance = value; }
    public int HeavyMagicAttackDamage { get => heavyMagicAttackDamage; set => heavyMagicAttackDamage = value; }
    public float HeavyMagicAttackCooldown { get => heavyMagicAttackCooldown; set => heavyMagicAttackCooldown = value; }
    public bool IsHeavyMagicAttack { get => isHeavyMagicAttack; set => isHeavyMagicAttack = value; }
}
