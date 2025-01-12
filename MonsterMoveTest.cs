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


    [Header("추격 속도")]
    [SerializeField] [Range(1f, 4f)] float moveSpeed = 1f;

    [Header("근접 거리")]
    [SerializeField] [Range(0f, 10f)] float contactDistance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
       /* originPos = transform.position;
        moving = false;
       */
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();

    }

    void FollowTarget()
    {
        if (Vector2.Distance(transform.position, target.position) < contactDistance)
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        else
            rb.velocity = Vector2.zero;
    }

}