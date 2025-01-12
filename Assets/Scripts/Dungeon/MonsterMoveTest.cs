using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveTest : MonoBehaviour
{
    SpriteRenderer sprite; // 몬스터 스프라이트

   /* Vector3 originPos;
    Vector3 targetPos;
   */
    bool moving;

    Rigidbody2D rb;
    Transform target;
    Vector2 randomTarget;

    [Header("추격 속도")]
    [SerializeField] [Range(0.1f, 4f)] float moveSpeed = 1f;

    [Header("근접 거리")]
    [SerializeField] [Range(0.1f, 10f)] float contactDistance = 2f;

    public float timer = 0;
    public bool find { get; private set; }

    bool firstDir; // 첫 방향은 모두 왼쪽을 바라봄

    void Start()
    {
        timer = 0;
        find = false;

        // 스프라이트 정보
        sprite = GetComponent<SpriteRenderer>();
        firstDir = sprite.flipX;

        // 추격 대상 설정
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag(Config.PLAYER_TAG).GetComponent<Transform>();

        moving = false;
    }

    void Update()
    {
        if(Player.instance.status != Player.Status.DIE)
        {    
            FollowTarget();
        }

        //distance = Vector2.Distance(transform.position, target.position);
        rb.rotation = 0;
    }

    void FollowTarget()
    {
        // 거리 내에 들어오면 플레이어 추격
        if (Vector2.Distance(transform.position, target.position) < contactDistance)
        {
            find = true;
            moving = false;
            if (target.position.x - transform.position.x > 0)
            {
                sprite.flipX = !firstDir;
            }
            else
            {
                sprite.flipX = firstDir;
            }
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        // 거리 내에 없다면 자유로이 이동
        else
        {
            find = false;
            if (!moving)
            {
                Vector2 pos = transform.position;

                float x, y;
                do
                {
                    x = Random.Range(-3f, 3f);
                } while (Mathf.Abs(x) < 1);
                do
                {
                    y = Random.Range(-1f, 1f);
                } while (Mathf.Abs(y) < 0.5);

                randomTarget = new Vector2(pos.x + x, pos.y + y);

                moving = true;
            }
            else
            {
                if(timer < 3f)
                {
                    timer += Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, randomTarget, moveSpeed * Time.deltaTime);

                    if (randomTarget.x - transform.position.x > 0)
                    {
                        sprite.flipX = !firstDir;
                    }
                    else
                    {
                        sprite.flipX = firstDir;
                    }


                }
                else if(timer < 4f)
                {
                    timer += Time.deltaTime;
                    if(timer > 3.6f)
                    {
                        timer = 0;
                        moving = false;
                    }
                }
            }
        }
    }
}