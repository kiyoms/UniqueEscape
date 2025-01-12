using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public GameObject attackArea;

    Monster monster;
    MonsterMoveTest move;
    Animator animator;

    float timer = 0;
    public float attackPeriod = 4;
    public bool area = false;
    Vector3 effectP;

    bool token = false;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        move = GetComponent<MonsterMoveTest>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetInteger("AnimState", 1);
        // 플레이어를 발견하면
        if (move.find)
        {
            timer += Time.deltaTime;
            if (timer > attackPeriod)
            {
                Attack();
                timer = 0;
            }

            effectP = GetComponentInChildren<FinalBossArea>().transform.localPosition;   // 공격범위 이펙트 위치
            float x = Mathf.Abs(effectP.x);
            // 왼쪽
            if (GetComponent<SpriteRenderer>().flipX)
            {

                GetComponentInChildren<FinalBossArea>().transform.localPosition = new Vector2(-x, effectP.y);
            }
            else
            {
                GetComponentInChildren<FinalBossArea>().transform.localPosition = new Vector2(x, effectP.y);
            }

        }

        if(monster.state == Monster.State.DIE && !token)
        {
            Die();
            token = PlayUI.instance.StageClear(true);
        }
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
        StartCoroutine(DamageProcess());
    }
    public void Die()
    {
        animator.SetInteger("AnimState", 3);
        animator.SetTrigger("Death");
    }
    public IEnumerator DamageProcess()
    {
        int damage = Mathf.FloorToInt(monster.atk * Random.Range(0.8f, 1.2f));
        yield return new WaitForSeconds(0.7f);

        if (area)
        {
            if (GameManager.instance.night)
            {
                Player.instance.Damaged(Mathf.FloorToInt(damage * 1.5f));
            }
            else
            {
                Player.instance.Damaged(damage);
            }
        }
        
    }
}
