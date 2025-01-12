using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    SpriteRenderer sprite; // 몬스터 스프라이트

    Vector3 originPos;
    Vector3 targetPos;

    bool moving;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originPos = transform.position;
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)    // 이동 중이 아니면 이동할 좌표 지정
        {
            targetPos = new Vector3(originPos.x + Random.Range(-3, 3), originPos.y + Random.Range(-1,1), 0);
            moving = true;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.002f);
            if (targetPos.x - originPos.x > 0)
                sprite.flipX = true;
            else
                sprite.flipX = false;

            if (Vector2.Distance(transform.position, targetPos) < 2f)
            {
                moving = false;
            }
        }
    }

}
