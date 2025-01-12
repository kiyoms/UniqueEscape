using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Npc : MonoBehaviour
{
    // NPC 유형
    public enum Type
    {
        Normal, Shop, Save
    }
    // NPC 정보
    [Header("NPC Information")]
    public Type npcType;
    public string npcCode;
    public string npcName;
    public string npcDialogue;

    // NPC UI
    [Header("NPC UI")]
    public TextMeshProUGUI ui_npcname;
    public Button ui_talkbutton;
    public Image ui_quest;

    // 퀘스트 정보
    public Quest quest { get; private set; }
    bool talkable;

    void Start()
    {
        talkable = false;

        // NPC UI 구성
        ui_npcname.text = npcName;
        ui_talkbutton.onClick.AddListener(Talk);
        ui_quest.gameObject.SetActive(false);
    }
    void Update()
    {
        // 퀘스트가 있다면 버튼 활성
        ui_quest.gameObject.SetActive(quest != null);
    }
    // 퀘스트 매니저로부터 호출된 퀘스트 가져오기 명령
    public void QuestUpdate()
    {
        // 퀘스트 가져오기, 완료 NPC 가져오기
        quest = QuestManager.instance.FindQuestWithNpcCode(npcCode, false);
        if(quest == null)
        {
            quest = QuestManager.instance.FindQuestWithNpcCode(npcCode, true);
            if(quest != null)
            {
                if (quest.state != Quest.State.progress2)
                {
                    quest = null;
                }
            }
        }

        // 퀘스트 상태에 따라...
        if(quest != null)
        {
            // 퀘스트를 받을 수 없으므로 삭제
            if(quest.state == Quest.State.not)
            {
                quest = null;
            }
            else if (quest.state == Quest.State.progress)
            {
                ui_quest.color = Color.gray;
            }
            else if (quest.state == Quest.State.progress2)
            {
                // 완료할 수 있는 NPC가 다르면 해당 NPC에서 보상을 받을 수 없게 수정
                if(!quest.finishNpcCode.Equals(""))
                {
                    if (quest.finishNpcCode != npcCode)
                    {
                        quest = null;
                    }
                }
                ui_quest.color = Color.yellow;
            }
        }
    }
    // 플레이어의 위치에 따른 트리거
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG))
        {
            talkable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG))
        {
            talkable = false;
        }
    }
    // 대화
    void Talk()
    {
        if (talkable)
        {
            PlayUI.instance.OpenNpcPanel(this);
        }
        else
        {
            PlayUI.instance.MessagePopup("NPC가 멀리 있습니다.\n가까이 다가간 뒤 대화하십시오.");
        }
    }
}
