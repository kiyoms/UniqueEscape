using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    Player player;
    private Vector3 vector;

    private SpriteRenderer sprite;  // 캐릭터 스프라이트
    private Animator animator;  // 캐릭터 애니메이터
    private Rigidbody2D rigid2d;    // Rigidbody

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid2d.rotation = 0;
        if(GameManager.instance.testEnv == GameManager.TestEnv.PC)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                // 플레이어가 공격할 때나 공격당할 때, 사망할 때 이동 불가
                if (player.status == Player.Status.Attack ||
                    player.status == Player.Status.Damaged ||
                    player.status == Player.Status.DIE)
                {
                    rigid2d.velocity = Vector2.zero;
                    return;
                }

                // 이동 방향 설정
                vector.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
                animator.SetBool("Walking", true);  // 걷기 트랜지션 허용
                rigid2d.velocity = new Vector3(vector.x, vector.y, 0) * player.moveSpeed;

                if (sprite.flipX && vector.x < 0)   // 우측을 바라보고 있다면...
                {
                    sprite.flipX = false;
                }
                else if (!sprite.flipX && vector.x > 0)
                {
                    sprite.flipX = true;
                }

            }
            // 걷기가 멈추면
            else
            {
                animator.SetBool("Walking", false);
            }
        }
    }

    public void VirtualMove(Vector2 vector)
    {
            // 플레이어가 공격할 때나 공격당할 때, 사망할 때 이동 불가
            if (player.status == Player.Status.Attack ||
            player.status == Player.Status.Damaged ||
            player.status == Player.Status.DIE)
            {
                rigid2d.velocity = Vector2.zero;
                return;
            }

            // 이동 방향 설정
            animator.SetBool("Walking", true);  // 걷기 트랜지션 허용
            rigid2d.velocity = new Vector2(vector.x, vector.y) * player.moveSpeed;

            if (sprite.flipX && vector.x < 0)   // 우측을 바라보고 있다면...
            {
                sprite.flipX = false;
            }
            else if (!sprite.flipX && vector.x > 0)
            {
                sprite.flipX = true;
            }

        }
    public void VirtualMove()
    {
        animator.SetBool("Walking", false);  // 걷기 트랜지션 허용
        rigid2d.velocity = Vector2.zero;
    }
}