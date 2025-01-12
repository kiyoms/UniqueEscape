using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // 포탈 정보 클래스
    public int code;    // 위치 코드
    public Transform spawn; // 포탈 이동 위치

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG))
        {
            // 마을->던전
            if (GameManager.instance.position < 9)
            {
                PlayUI.instance.OpenDungeon();
                return;
            }

            // 던전->던전(몬스터 확인)
            else if (code >= 10 && !GameManager.instance.cheat)
            {
                if (DungeonVerify() != 0)
                {
                    PlayUI.instance.ErrorMessagePopup("던전의 모든 몬스터를 퇴치하세요.\n잔여 몬스터 수: " + DungeonVerify() + "마리");
                    return;
                }
            }
            GameManager.instance.Warp(code, spawn);
        }
    }

    // 던전 클리어 확인
    int DungeonVerify()
    {
        Dungeon dungeon = GetComponentInParent<Dungeon>();
        return dungeon.remainMob;
    }
}