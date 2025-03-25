using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 피격 컴포넌트
public class DamageablePlayer : DamageableHitAnimation
{
    PlayerInputAction playerAction;
    PlayerInputMovement playerMovement;
    PlayerInputAttack playerAttack;
    PlayerSoundFx playerSoundFx;

    private bool isGuard;                       // 가드 상태인지
    private bool isDamage;                      // 피격 상태인지
    private IEnumerator invincibleCorutine;     // 무적 상태 부여 코루틴
    private IEnumerator hitAnimationCoroutine;  // 피격 애니메이션 코루틴

    public bool IsDamage { get => isDamage; set => isDamage = value; }

    protected override void Awake()
    {
        base.Awake();
        playerAction = GetComponent<PlayerInputAction>();
        playerMovement = GetComponent<PlayerInputMovement>();
        playerAttack = GetComponent<PlayerInputAttack>();
        playerSoundFx = GetComponent<PlayerSoundFx>();
    }

    // 피격 실행 재정의(가드 상태 처리, 피격 시 일정 시간 무적 부여 추가)
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

    // 사망 처리 재정의
    protected override void Die()
    {
        IsAlive = false;
        collider2d.enabled = false;
        animator.SetBool(AnimatorStringToHash.Die, true);
        playerSoundFx.PlaySound((int)PlayerSoundFx.PLAYERAUDIO.DIE);

        StartCoroutine(PlayerDieCoroutine());
    }

    // 무적 부여 코루틴
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

    // 가드 성공 시 코루틴
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

    // 피격 애니메이션 이벤트
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

    // 피격 코루틴
    protected IEnumerator PlayerHitAnimationCoroutine()
    {
        IsDamage = true;
        PlayerInputAttack attack = GetComponent<PlayerInputAttack>();
        attack.IsAttack = false;
        yield return new WaitForSeconds(0.4f);
        IsDamage = false;
        hitAnimationCoroutine = null;
    }

    // 사망 코루틴
    protected IEnumerator PlayerDieCoroutine()
    {
        yield return new WaitForSeconds(2f);

        GameManager.Instance.FadeOutAndCurrentScene();

        yield return new WaitForSeconds(0.5f);

        InitializePlayer();
    }

    // 사망 후 부활 시 플레이어 초기화
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
