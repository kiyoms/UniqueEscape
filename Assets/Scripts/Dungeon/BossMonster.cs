using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 보스 몬스터만 수행하는 스크립트를 수행
public class BossMonster : MonoBehaviour
{
    // 몬스터 정보 스크립트
    private Monster monster;

    bool clear;

    float timer = 0;
    public float attack_time = 5f;

    void Start()
    {
        clear = false;   // 클리어 패널 오픈 여부
        monster = GetComponent<Monster>();
        PlayUI.instance.BossStage();    // 보스 스테이지 출력
    }

    void Update()
    {
        if (monster.state == Monster.State.NORMAL)
        {
            timer += Time.deltaTime;

            if(timer > attack_time)
            {
                timer = 0;
            }
        }
        else
        {
            if(!clear)
            {
                // 사망 상태 변환은 상위 스크립트에서 별도로 수행
                if (!GetComponent<FinalBoss>())
                {
                    clear = PlayUI.instance.StageClear(false);
                    QuestVerify(GetComponentInParent<Dungeon>().code);
                }
            }
        }
    }

    // 던전 클리어 시 퀘스트 완료 확인
    void QuestVerify(int code)
    {
        // 스테이지 1이라면 진행 중인 특정 퀘스트 완료 처리
        if (code == 13)
        {
            Quest quest = QuestManager.instance.findQuestWithCode("E01");
            if (quest != null && quest.state == Quest.State.progress)
            {
                quest.ChangeStateWithPlayer(Quest.State.progress2);
            }
        }
        if (code == 25)
        {
            Quest quest = QuestManager.instance.findQuestWithCode("E05");
            if (quest != null && quest.state == Quest.State.progress)
            {
                quest.ChangeStateWithPlayer(Quest.State.progress2);
            }
        }
        if (code == 37)
        {
            Quest quest = QuestManager.instance.findQuestWithCode("E08");
            if (quest != null && quest.state == Quest.State.progress)
            {
                quest.ChangeStateWithPlayer(Quest.State.progress2);
            }
        }
    }
}
