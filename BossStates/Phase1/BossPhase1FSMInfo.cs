using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� FSM ���۰��� ���� ����
public class BossPhase1FSMInfo : MonoBehaviour
{
    [Header("���� ���� ���� �Ӽ�")]
    [SerializeField] private float meleeAttackDistance;
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private int meleeAttackCount;
    [SerializeField] private float meleeAttackCooldown;
    [SerializeField] private bool isMeleeAttack;
     
    [Header("���� ���� ���� �Ӽ�")]
    [SerializeField] private float magicAttackDistance;
    [SerializeField] private int magicAttackDamage;
    [SerializeField] private float magicAttackCooldown;
    [SerializeField] private bool isMagicAttack;

    [Header("�̵� ���� �Ӽ�")]
    [SerializeField] private float moveSpeed;
    

    // �Ӽ� ������Ƽ��
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
