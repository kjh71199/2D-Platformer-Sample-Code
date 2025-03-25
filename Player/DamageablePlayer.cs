using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� �ǰ� ������Ʈ
public class DamageablePlayer : DamageableHitAnimation
{
    PlayerInputAction playerAction;
    PlayerInputMovement playerMovement;
    PlayerInputAttack playerAttack;
    PlayerSoundFx playerSoundFx;

    private bool isGuard;                       // ���� ��������
    private bool isDamage;                      // �ǰ� ��������
    private IEnumerator invincibleCorutine;     // ���� ���� �ο� �ڷ�ƾ
    private IEnumerator hitAnimationCoroutine;  // �ǰ� �ִϸ��̼� �ڷ�ƾ

    public bool IsDamage { get => isDamage; set => isDamage = value; }

    protected override void Awake()
    {
        base.Awake();
        playerAction = GetComponent<PlayerInputAction>();
        playerMovement = GetComponent<PlayerInputMovement>();
        playerAttack = GetComponent<PlayerInputAttack>();
        playerSoundFx = GetComponent<PlayerSoundFx>();
    }

    // �ǰ� ���� ������(���� ���� ó��, �ǰ� �� ���� �ð� ���� �ο� �߰�)
    public override void HitProcess(Damager damager)
    {
        if (IsInvincible || !IsAlive) return;

        if (playerAction.IsGuard && !isGuard)
        {
            if (damager.transform.position.x > transform.position.x && movement.IsRight ||
                damager.transform.position.x < transform.position.x && !movement.IsRight)
            {
                GameObject guardPrefab = Instantiate(playerAction.GuardEffectPrefab, playerAttack.AttackTransform.position, Quaternion.identity);

                if (!movement.IsRight)
                    guardPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

                Destroy(guardPrefab, playerAction.GuardEffectDestroyTime);
                StartCoroutine(GuardSucceedCoroutine());
                playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.GUARD);

                return;
            }
        }

        base.HitProcess(damager);

        if (invincibleCorutine == null)
        {
            playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.HIT);
            invincibleCorutine = EnableInvincibleCoroutine();
            StartCoroutine(invincibleCorutine);
        }    
        else
        {
            StopCoroutine(invincibleCorutine);
            invincibleCorutine = EnableInvincibleCoroutine();
            StartCoroutine(invincibleCorutine);
        }
    }

    // ��� ó�� ������
    protected override void Die()
    {
        IsAlive = false;
        collider2d.enabled = false;
        animator.SetBool(AnimatorStringToHash.Die, true);
        playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.DIE);

        StartCoroutine(PlayerDieCoroutine());
    }

    // ���� �ο� �ڷ�ƾ
    protected IEnumerator EnableInvincibleCoroutine()
    {
        yield return new WaitForEndOfFrame();
        float time = 0f;
        float maxTime = showHitColorTime * showHitColorCount * 2f;

        while (time <= maxTime)
        {
            time += Time.deltaTime;
            IsInvincible = true;
            yield return null;
        }
        IsInvincible = false;
        invincibleCorutine = null;
    }

    // ���� ���� �� �ڷ�ƾ
    protected IEnumerator GuardSucceedCoroutine()
    {
        float time = 0f;

        isGuard = true;
        
        while (time <= 0.2f)
        {
            time += Time.deltaTime;
            IsInvincible = true;
            yield return null;
        }

        IsInvincible = false;
        isGuard = false;
    }

    // �ǰ� �ִϸ��̼� �̺�Ʈ
    public void PlayerHitAnimationEvent()
    {
        if (hitAnimationCoroutine == null)
        {
            hitAnimationCoroutine = PlayerHitAnimationCoroutine();
            StartCoroutine(hitAnimationCoroutine);
        }
        else
        {
            StopCoroutine(hitAnimationCoroutine);
            hitAnimationCoroutine = PlayerHitAnimationCoroutine();
            StartCoroutine(hitAnimationCoroutine);
        }
    }

    // �ǰ� �ڷ�ƾ
    protected IEnumerator PlayerHitAnimationCoroutine()
    {
        IsDamage = true;
        PlayerInputAttack attack = GetComponent<PlayerInputAttack>();
        attack.IsAttack = false;
        yield return new WaitForSeconds(0.4f);
        IsDamage = false;
        hitAnimationCoroutine = null;
    }

    // ��� �ڷ�ƾ
    protected IEnumerator PlayerDieCoroutine()
    {
        yield return new WaitForSeconds(2f);

        GameManager.Instance.FadeOutAndCurrentScene();

        yield return new WaitForSeconds(0.5f);

        InitializePlayer();
    }

    // ��� �� ��Ȱ �� �÷��̾� �ʱ�ȭ
    protected void InitializePlayer()
    {
        IsAlive = true;
        collider2d.enabled = true;
        Hp = MaxHp;
        playerAction.PotionCount = 3;
        movement.MoveDirection = Vector2.right;
        FindAnyObjectByType<UIManager>().InitializePotion();
        animator.SetFloat(AnimatorStringToHash.Move, 0f);
        animator.SetTrigger(AnimatorStringToHash.Revive);
    }
}
