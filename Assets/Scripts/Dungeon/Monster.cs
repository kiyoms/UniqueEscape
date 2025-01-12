using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public enum State
    {
        NORMAL,DIE
    };
    public State state;

    [Header("MONSTER STATUS")]
    public float maxHP;   // 최대 체력
    public float hp { get; private set; }      // 현재 체력
    public int atk;     // 공격력
    public int exp;     // 획득 경험치

    [Header("MONSTER SOUND")]
    public AudioClip damageClip;   // 사망
    public AudioClip dieClip;   // 사망

    [Header("MONSTER UI")]
    public Slider hpSlider;   // 몬스터 체력바

    Animator animator;  // 몬스터 애니메이터
    Player player;      // 몬스터와 상호작용하는 플레이어 스크립트

    bool collide = false;   // 충돌
    float collisionTimer = 0;   // 충돌 시간
    float collisionTimerPeriod = 3f;    // 충돌 주기

    void Start()
    {
        state = State.NORMAL;

        player = Player.instance;
        animator = GetComponent<Animator>();
        hp = maxHP;    // 몬스터 체력 고정
    }

    void FixedUpdate()
    {
        if(state == State.NORMAL)
        {
            if(hpSlider != null)
            {
                hpSlider.value = hp / maxHP; // 몬스터 체력 바 갱신
            }
            
            if (hp <= 0) // 몬스터의 HP가 0 이하라면
            {
                state = State.DIE;
                StartCoroutine(DieProcess());
                animator.enabled = false;
            }
            collisionTimer += Time.deltaTime;
            if (collide)
            {
                if(collisionTimer > collisionTimerPeriod)
                {
                    if (Player.instance.status != Player.Status.Damaged)
                    {
                        StartCoroutine(DamageProcess());
                    }
                    collisionTimer = 0;
                }
            }
        }
    }
    // 돌발 미션으로 인한 HP 조정
    public void Heal()
    {
        hp = maxHP;
    }

    // 플레이어가 몬스터와 부딪히면 피격 대미지 (1회)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG))
        {
            Player.instance.multiattack++;
            collide = true;
            if(Player.instance.status != Player.Status.Damaged)
            {
                StartCoroutine(DamageProcess());
            }
        }
        else if (collision.CompareTag("Damaged"))
        {
            
            int damage = Mathf.FloorToInt(player.atk * Random.Range(0.8f, 1.2f));
            hp -= damage;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG)){
            collide = false;
        }

    }
    public IEnumerator DamageProcess()
    {
        int damage = Mathf.FloorToInt(atk * Random.Range(0.8f, 1.2f));
        //damage = Mathf.FloorToInt(damage * (1 - (float)(Player.instance.multiattack * 0.1)));

        if (GameManager.instance.night)
        {
            player.Damaged(Mathf.FloorToInt(damage * 1.5f));
        }
        else
        {
            player.Damaged(damage);
        }
        yield return new WaitForSeconds(1f);
    }
    IEnumerator DieProcess()    // 몬스터가 사망할 때
    {
        Destroy(GetComponent<MonsterMoveTest>());       // 몬스터 이동 스크립트 삭제
        animator.speed = 0; // 애니메이션 정지

        // 사망음
        SoundManager.instance.AudioSfxPlay(dieClip);

        // 상위 던전 스크립트 마릿수 감소
        if (!GetComponent<FinalBoss>())
        {
            Dungeon dungeon = GetComponentInParent<Dungeon>();
            dungeon.DieMonster();
        }

        // 플레이어 경험치, 돈 획득
        player.EXP += exp;
        player.money += Mathf.RoundToInt(maxHP * Random.Range(0.5f, 1.5f));

        yield return new WaitForSeconds(0.8f);
        Destroy(this.gameObject);   // 오브젝트 파괴
    }

}
