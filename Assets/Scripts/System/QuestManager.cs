using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QuestManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static QuestManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<QuestManager>();
            }

            return m_instance;
        }
    }
    private static QuestManager m_instance;

    // 퀘스트 정보
    public Quest[] questlist;
    public List<string> finished;

    [System.Serializable]
    public struct QuestStatus
    {
        public List<string> finished;   // 이미 완료한 퀘스트 목록
        public string last;             // 가장 최근 퀘스트
        public Quest.State lastState;   // 가장 최근 퀘스트 상태
    }
    public QuestStatus questStatus; // 퀘스트 상태

    //public string startQuest;

    // 퀘스트를 처음 열면 PlayerPrefs 저장 상태에 따라 퀘스트 진행 상황 읽기
    void Start()
    {
        // 구조체 초기화
        questStatus.finished = new List<string>();
        questStatus.last = "";
        questStatus.lastState = Quest.State.not;

        // 퀘스트 초기화 함수
        if (!PlayerPrefs.HasKey(PlayerStatus.SAVETIME))
        {
            Initialize();
        }
        Reset();
    }

    // 첫 시작 스크립트
    void Initialize()
    {
        // 모든 퀘스트를 수행 불가로 변경
        for (int i = 0; i < questlist.Length; i++)
        {
            questlist[i].ChangeState(Quest.State.not);
        }
        // 게임을 처음 시작할 때 퀘스트 상태 변경
        findQuestWithCode("V00").ChangeState(Quest.State.start);
    }

    public void SaveQuest()
    {
        string quest = "";

        print(questStatus.finished.Count);

        if (questStatus.finished.Count >= 1)
        {
            foreach (string list in questStatus.finished)
            {
                quest += list;
                quest += ",";
            }
        }
        PlayerPrefs.SetString(PlayerStatus.QUEST_FINISHED, quest);
        PlayerPrefs.SetString(PlayerStatus.QUEST_LASTEST, questStatus.last);
        PlayerPrefs.SetString(PlayerStatus.QUEST_LASTEST_NAME, findQuestWithCode(questStatus.last).title);   // 기록용
        PlayerPrefs.SetInt(PlayerStatus.QUEST_LASTEST_STATUS, (int)questStatus.lastState);
    }
    public void LoadQuest()
    {
        string[] finished = PlayerPrefs.GetString(PlayerStatus.QUEST_FINISHED).Split(',');

        // 튜토리얼 퀘스트는 전부 완료
        findQuestWithCode("V00").ChangeState(Quest.State.finish);
        findQuestWithCode("V01").ChangeState(Quest.State.finish);
        findQuestWithCode("V02").ChangeState(Quest.State.finish);

        // 완료처리
        foreach (string list in finished)
        {
            if (!list.Equals(""))
            {
                questStatus.finished.Add(list);
                findQuestWithCode(list).ChangeState(Quest.State.finish);
            }
        }

        // 가장 최근의 퀘스트 상태 변경
        if (PlayerPrefs.HasKey(PlayerStatus.QUEST_LASTEST))
        {
            questStatus.last = PlayerPrefs.GetString(PlayerStatus.QUEST_LASTEST);
            questStatus.lastState = (Quest.State)PlayerPrefs.GetInt(PlayerStatus.QUEST_LASTEST_STATUS);
            findQuestWithCode(questStatus.last).ChangeState(questStatus.lastState);
        }

        // 퀘스트 갱신
        Reset();
    }
    // 가장 최근의 퀘스트 갱신
    public void UpdateLastQuest(Quest quest)
    {
        // 튜토리얼 퀘스트는 갱신 시도
        if(quest.questCode.Substring(0,1) == "V")
        {
            if (!TutorialQuest(quest))
            {
                Reset();
                return;
            }
        }

        // 퀘스트를 완료했으면
        if (quest.state == Quest.State.finish)
        {
            // 완료한 퀘스트를 리스트에 추가
            // 단, 튜토리얼을 해당사항 없음
            if (!quest.questCode.Equals("V00")){
                questStatus.finished.Add(quest.questCode);
            }

            // 다음 퀘스트가 있다면 최근 퀘스트 갱신
            if (!quest.nextquest.Equals(""))
            {
                Quest nextquest = findQuestWithCode(quest.nextquest);
                nextquest.ChangeState(Quest.State.start);
            }

            // 퀘스트 갱신 함수 호출
            Reset();
        }
        // 수행 중인 퀘스트 상태가 변경되었다면
        else
        {
            if (!questStatus.last.Equals(quest.name))
            {
                questStatus.last = quest.name;
            }
            questStatus.lastState = quest.state;

            print("호출");
            Reset();
        }
    }

    // NPC 이름으로 퀘스트 찾기(이미 완료한 퀘스트 제외)
    public Quest FindQuestWithNpcCode(string npcCode, bool finish)
    {
        if (!finish)
        {
            foreach (Quest quest in questlist)
            {
                if (npcCode.Equals(quest.npcCode))
                {
                    Quest.State state = quest.state;
                    if (state != Quest.State.not && state != Quest.State.finish)
                    {
                        return quest;
                    }
                }
            }
        }
        else
        {
            foreach (Quest quest in questlist)
            {
                if (npcCode.Equals(quest.finishNpcCode))
                {
                    Quest.State state = quest.state;
                    if (state != Quest.State.not && state != Quest.State.finish)
                    {
                        return quest;
                    }
                }
            }
        }
        
        return null;
    }
    // 퀘스트 코드로 퀘스트 찾기(완료 조건을 만족시키기 위해)
    public Quest findQuestWithCode(string code)
    {
        foreach (Quest quest in questlist)
        {
            if (code.Equals(quest.questCode))
            {
                return quest;
            }
        }
        return null;
    }

    // 진행 중인 퀘스트 찾기(일시 정지 창)
    public Quest FindQuest()
    {
        foreach (Quest quest in questlist)
        {
            if (quest.state == Quest.State.progress || quest.state == Quest.State.progress2)
            {
                return quest;
            }
        }
        return null;
    }

    // 튜토리얼 전용 함수
    bool TutorialQuest(Quest quest)
    {
        // 튜토리얼 퀘스트 상태가 변경된다면
        if (quest.questCode.Equals("V00"))
        {
            if (quest.state == Quest.State.progress)
            {
                findQuestWithCode("V01").ChangeState(Quest.State.progress2);
                findQuestWithCode("V02").ChangeState(Quest.State.progress2);
                return false;
            }
            if (quest.state == Quest.State.finish)
            {
                return true;
            }
            return false;
        }
        else
        {
            // 튜토리얼 퀘스트 2개를 완료했으면
            if (findQuestWithCode("V01").state == Quest.State.finish)
            {
                if (findQuestWithCode("V02").state == Quest.State.finish)
                {
                    findQuestWithCode("V00").ChangeStateWithPlayer(Quest.State.progress2);
                }
            }
            return false;
        }
    }

    // 퀘스트 정보를 처음 불러오거나 변경되면 리셋
    public void Reset()
    {
        GameObject[] npc = GameObject.FindGameObjectsWithTag("Npc");
        for (int i = 0; i < npc.Length; i++)
        {
            Npc obj = npc[i].GetComponent<Npc>();
            obj.QuestUpdate();
        }
    }
}
