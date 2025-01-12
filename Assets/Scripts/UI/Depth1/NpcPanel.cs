using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// NPC 대화 정보 스크립트
// ESC로 팝업을 닫을 수 없음
public class NpcPanel : MonoBehaviour
{
    // NPC로부터 가져온 정보
    Npc.Type type;
    string npcName;
    string dialogue;
    Quest quest;
    string[] questDialogue;

    // UI 정보
    public Button button_close;
    [Header("NPC Dialogue")]
    public TextMeshProUGUI ui_name;
    public TextMeshProUGUI ui_dialogue;
    public TextMeshProUGUI button_text;
    public Button button_prev;
    public Button button_next;

    // 퀘스트 및 버튼
    [Header("NPC Button")]
    public Button questButton;
    public Button shopButton;
    public Button saveButton;

    // 저장 팝업
    [Header("Etc UI")]
    public GameObject save_popup;
    [System.Serializable]
    public struct QuestReward
    {
        public GameObject ui;
        public TextMeshProUGUI exp;
        public TextMeshProUGUI money;
        public Image equip;
        public TextMeshProUGUI equip_name;
        public GameObject exp_object;
        public GameObject money_object;
        public GameObject equip_object;
    };
    public QuestReward reward = new QuestReward();

    int order = 0;  // 대화순서

    // NPC 대화 정보 가져오기
    public void Initialize(Npc npc)
    {
        questButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);
        this.npcName = npc.npcName;
        this.dialogue = npc.npcDialogue;
        this.type = npc.npcType;

        if(npc.quest != null)
        {
            QuestLoad(npc.quest);
        }
        Show();
    }
    // NPC 대화창 열기
    void Show()
    {
        // 닫기 버튼
        button_close.onClick.AddListener(()=>DestroyPopup(false));
        // NPC 정보
        ui_name.text = npcName;
        ui_dialogue.text = dialogue;
        // 상호작용 버튼
        if(quest == null)
        {
            switch (type)
            {
                case Npc.Type.Shop:
                    shopButton.gameObject.SetActive(true);
                    shopButton.onClick.AddListener(OpenShop);
                    break;
                case Npc.Type.Save:
                    saveButton.gameObject.SetActive(true);
                    saveButton.onClick.AddListener(SaveGame);
                    break;
            }
        }
        GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
    // NPC 대화창 파괴
    void DestroyPopup(bool sub)
    {
        // 다음 창을 넘기지 않는다면 Depth0으로 조정
        if (!sub)
        {
            PlayUI.instance.depth = 0;
        }
        Destroy(this.gameObject);
    }
    // 퀘스트 정보 가져오기
    public void QuestLoad(Quest q)
    {
        quest = q;
        TextMeshProUGUI quest_title = questButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        string title = quest.title;

        switch (quest.state)
        {
            case Quest.State.start:
                title += " (시작 가능)";
                break;
            case Quest.State.progress:
                title += " (진행 중)";
                break;
            case Quest.State.progress2:
                title += " (완료 가능)";
                break;
        }

        quest_title.text = title;
        questButton.gameObject.SetActive(true);
        questButton.onClick.AddListener(OpenQuest);
    }
    // NPC 대화창 넘기기 및 삭제
    void QuestScript(int order)
    {
        if(order >= 0 && order < questDialogue.Length)
        {
            ui_dialogue.text = questDialogue[order];
            button_text.text = "다음 ▶";

            // 이전 버튼 관련
            if (order == 0)
            {
                button_prev.gameObject.SetActive(false);
            }
            else
            {
                button_prev.gameObject.SetActive(true);
            }
        }
        // 가장 마지막 대사라면
        if (order + 1 == questDialogue.Length)
        {
            switch (quest.state)
            {
                case Quest.State.start:
                    button_text.text = "수락";
                    break;
                case Quest.State.progress:
                    button_text.text = "확인";
                    break;
                case Quest.State.progress2:
                    Reward();

                    button_text.text = "완료";
                    break;
            }
        }
        // 마지막 상호작용 버튼을 누르면
        if(order == questDialogue.Length)
        {
            switch (quest.state)
            {
                case Quest.State.start:
                    quest.ChangeStateWithPlayer(Quest.State.progress);
                    break;
                case Quest.State.progress2:
                    quest.ChangeStateWithPlayer(Quest.State.finish);
                    break;
            }
            DestroyPopup(false);
        }
    }

    void Reward()
    {
        bool trigger = false;
        if(quest.exp != 0)
        {
            reward.exp.text = quest.exp.ToString();
            trigger = true;
        }
        else
        {
            reward.exp_object.SetActive(false);
        }
        if (quest.money != 0)
        {
            reward.money.text = quest.money.ToString();
            trigger = true;
        }
        else
        {
            reward.exp_object.SetActive(false);
        }
        if(quest.equip != null)
        {
            reward.equip.sprite = quest.equip.itemImage;
            reward.equip_name.text = quest.equip.itemName + " 획득!";
            trigger = true;
        }
        else
        {
            reward.equip_object.SetActive(false);
        }

        reward.ui.SetActive(trigger);
    }

    // 퀘스트 대사 이전 이후
    void QuestPrev()
    {
        QuestScript(--order);
    }
    void QuestNext()
    {
        QuestScript(++order);
    }
    // 퀘스트 출력
    void OpenQuest()
    {
        // 퀘스트 버튼 삭제 후 대사 읽어오기
        questButton.gameObject.SetActive(false);
        questDialogue = quest.GetDialogue();

        // 버튼 상호작용 기능 추가
        button_next.gameObject.SetActive(true);
        button_prev.onClick.AddListener(QuestPrev);
        button_next.onClick.AddListener(QuestNext);

        // 첫 번째 퀘스트 대사 출력
        order = 0;
        QuestScript(order);
    }
    // 상점 열기
    void OpenShop()
    {
        if (QuestManager.instance.findQuestWithCode("V01").state == Quest.State.finish)
        {
            PlayUI.instance.OpenShop();
        }
        else
        {
            PlayUI.instance.MessagePopup("먼저 '[에픽] 이곳은 어디지?' 퀘스트를 완료해주세요.");
        }
        DestroyPopup(true);
    }
    // 게임 저장
    void SaveGame()
    {
        if (QuestManager.instance.findQuestWithCode("V00").state == Quest.State.finish)
        {
            PlayUI.instance.OpenSystemPopup(save_popup);
        }
        else
        {
            PlayUI.instance.MessagePopup("먼저 '[에픽] 이곳은 어디지?' 퀘스트를 완료해주세요.");
        }
        DestroyPopup(true);
    }
}
