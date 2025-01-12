using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 경험치를 담당하는 스크립트
public class Experience : MonoBehaviour
{
    public static int[] EXP_TABLE = { 10, 50, 100, 200, 350, 600, 800, 1100, 1400 };

    void Update()
    {
        if(Player.instance != null)
        {            
            // 만렙이 아니라면
            if (Player.instance.level < EXP_TABLE.Length)
            {
                if (Player.instance.EXP >= EXP_TABLE[Player.instance.level - 1])
                {
                    Player.instance.EXP -= EXP_TABLE[Player.instance.level - 1]; // 레벨 업에 필요한 경험치 감소

                    Player.instance.LevelUp();   // 능력치 상승시키기
                    PlayUI.instance.LevelUp();  // 레벨업 메시지 출력
                }
            }
            // 만렙이라면 경험치 획득 불가능
            else
            {
                Player.instance.EXP = 0;
            }
        }
        
    }
}
