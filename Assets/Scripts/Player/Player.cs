using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // 싱글톤 접근용 프로퍼티
    public static Player instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Player>();
            }

            return m_instance;
        }
    }
    private static Player m_instance;
    public Animator animator;  // 캐릭터 애니메이터
    //private SpriteRenderer sprite;  // 캐릭터 스프라이트

    // 플레이어 상태 변수
    public enum Status
    {
        Idle, Move, Attack, Damaged, DIE
    }
    public Status status;

    // 캐릭터 능력치
    [Header("Status")]
    public int level = 1;
    public float maxHP = 15;
    public float HP { get; private set; }
    public float maxSP = 100;
    public float SP { get; private set; }
    public int EXP = 0;
    public float atk = 3;
    public float def = 3;
    public int money = 0;
    public float originMoveSpeed = 3;
    public float moveSpeed { get; private set;  }

    // 공격 관련 변수
    public int multiattack = 0;

    // 캐릭터 템창
    public Inventory inventory;
    Effect effect;

    //스태미너 줄어드는 속도
    [Header("Stamina Weight")]
    public float staminaDecreaseTime;
    float currentStaminaDecreaseTime;

    void Awake()
    {
        status = Status.Idle;
        
        SP = maxSP; //스태미너 초기값
        moveSpeed = originMoveSpeed;

        animator = GetComponent<Animator>();
        inventory = GetComponentInChildren<Inventory>();
        effect = GetComponentInChildren<Effect>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HP <= 0 && status != Status.DIE)    // 사망
        {
            animator.SetTrigger("Die");

            // HP 음수 방지
            HP = 0;

            // 상태 변경
            status = Status.DIE;
            // UI 출력
            PlayUI.instance.Die();

        }
        else
        {
            if (GameManager.instance.position < 10) // 마을이라면 항상 최대치 충전
            {
                SP = 100;
            }
            else
            {
                Stamina();
            }
        }
    }
    void Stamina()
    {
        if (SP > 0)
        {
            if(moveSpeed == 1)
            {
                moveSpeed = originMoveSpeed;
            }

            if (currentStaminaDecreaseTime <= staminaDecreaseTime)
            {
                currentStaminaDecreaseTime++;
            }
            else
            {
                SP -= 0.02f;
                currentStaminaDecreaseTime = 0;
            }
        }
        else
        {
            if(moveSpeed != 1) { 
                PlayUI.instance.MessagePopup("스태미너가 부족해 행동에 저하가 생깁니다."); 
            }
            Debuff();
        }
    }

    // 스태미너가 제로라면
    void Debuff()
    {
        moveSpeed = 1;
    }

    // 체력 조정
    // 체력이 넘치면 그 양을 반환함.
    public float Heal(float heal)
    {
        // HP 회복 실시
        HP += heal;
        if (HP >= maxHP)
        {
            HP = maxHP;

            return (HP - maxHP);
        }
        return 0;
    }
    public float HealSP(float heal)
    {
        // HP 회복 실시
        SP += heal;
        if (SP >= maxHP)
        {
            SP = maxHP;

            return (SP - maxSP);
        }
        effect.Heal();
        return 0;
    }

    // 몬스터에게 피해를 입음
    public void Damaged(int damage)
    {
        int totalDamage; // 최종 대미지

        status = Status.Damaged;
        animator.SetTrigger("Damaged");
        GetComponent<Attack>().effect.SetActive(false);
        if(damage - def <= 1)
        {
            totalDamage = 1;
        }
        else
        {
            totalDamage = Mathf.FloorToInt(damage - def);
        }
        HP -= totalDamage;
    }

    // 피해 상태에서 정상 상태로 변환.
    // 애니메이션 속 스크립트에 담김
    void DamagedToIdle()
    {
        GetComponent<Attack>().effect.SetActive(false);
        status = Status.Idle;
    }
    // 레벨업 능력치
    public void LevelUp()
    {
        level += 1;
        maxHP += 20;
        atk += 3;
        def += 3;

        originMoveSpeed += 0.15f;
        HP = maxHP;
    }
}
