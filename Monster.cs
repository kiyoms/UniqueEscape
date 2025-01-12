using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Animator animator;
    public int maxHP;
    public int atk;
    int HP;

    bool isHurt;
    Color halfA = new Color(1, 1, 1, 0.5f);
    Color fullA = new Color(1, 1, 1, 1);
    bool isknockback = false;
    SpriteRenderer sprite;
    public float speed;


    void Start()
    {
        animator = GetComponent<Animator>();
        HP = maxHP;    // 몬스터의 체력 고정

        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(HP <= 0) // 몬스터의 HP가 0 이하라면
        {
            HP = 0; // 몬스터의 HP가 음수가 되는 것을 방지.
            StartCoroutine(DieProcess());
        }
    }
    IEnumerator DieProcess()
    {
        print("작동");
        animator.speed = 0;
        yield return new WaitForSeconds(0.8f);
        Destroy(this.gameObject);
    }
    void Die()
    {
        print("몬스터 사망");
    }

    public void Hurt(int damage, Vector2 pos)
    {
        if (!isHurt)
        {
            isHurt = true;
            HP = HP - damage;
            print("몬스터 HP: " + HP + " / " + maxHP);
            if (HP <= 0)
            {
                //dead
            }
            else
            {
                float x = transform.position.x - pos.x;
                if (x < 0)
                    x = 1;
                else
                    x = -1;

                StartCoroutine(Knockback(x));
                StartCoroutine(HurtRoutine());
                StartCoroutine(alphablink()); // 피격시 반짝임
            }
        }
    }
    IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(1f);
        isHurt = false;
    }
    IEnumerator alphablink()
    {
        while (isHurt)
        {
            yield return new WaitForSeconds(0.1f);
            sprite.color = halfA;
            yield return new WaitForSeconds(0.1f);
            sprite.color = fullA;
        }
    }
    IEnumerator Knockback(float dir)
    {
        isknockback = true;
        float ctime = 0;
        while (ctime < 0.2f)
        {
            if (transform.rotation.y == 0)
                transform.Translate(Vector2.left * speed * Time.deltaTime * dir);
            else
                transform.Translate(Vector2.left * speed * Time.deltaTime * -1f * dir);

            ctime += Time.deltaTime;
            yield return null;
        }
        isknockback = false;
    }

    // 몬스터랑 플레이어랑 부딪히면 피격 대미지 (1회)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        /*
         if (collision.tag == "Player")
        {
            player.HP -= atk;
            print("몬스터에게 피격.");
        }
        */
       
        if(collision.tag == "Damaged")
        {
            Hurt(collision.GetComponentInParent<Player>().atk, collision.transform.position);
        }
      
    }
}
