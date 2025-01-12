using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Player player;  // 플레이어 스크립트
    private Animator animator;  // 캐릭터 애니메이터
    public GameObject effect; // 공격 이펙트 prefab
    public AudioClip clip;  // 공격 이펙트에 달린 소리


    void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 공격 키 입력
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerAttack();
        }
    }
    public void PlayerAttack()
    {
        // 공격받으면 공격 키를 씹음
        if (player.status == Player.Status.Damaged)
        {
            effect.SetActive(false);
            return;
        }
        player.status = Player.Status.Attack;      // Status 변경
        animator.SetTrigger("Attack");              // 애니메이션 재생
    }

    // 검을 휘두르는 모션 (애니메이션 내 함수로 지정)
    void AttackProcess()
    {
        SoundManager.instance.AudioSfxPlay(clip);
        effect.SetActive(true); // 무기 이펙트 표시
        Vector3 effectP = transform.position;   // 무기 이펙트 위치 저장

        // 무기 이펙트 위치 조정
        if (GetComponent<SpriteRenderer>().flipX)
        {
            effectP = new Vector3(effectP.x + 0.4f, effectP.y, effectP.z);
            effect.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            effectP = new Vector3(effectP.x - 0.3f, effectP.y, effectP.z);
            effect.GetComponent<SpriteRenderer>().flipX = false;
        }
        effect.transform.position = effectP;

    }
    // 검을 휘두르는 모션 종료 (애니메이션 내 함수로 지정)
    void AttackFinish()
    {
        player.status = Player.Status.Idle;
        effect.SetActive(false);
    }
}
