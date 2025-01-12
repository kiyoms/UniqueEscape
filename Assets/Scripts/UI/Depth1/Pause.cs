using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Pause : MonoBehaviour
{
    // 퀘스트 정보 패널
    public GameObject questPanel;
    TextMeshProUGUI quest_title;
    TextMeshProUGUI quest_objective;

    // 팝업
    public GameObject title_popup, quit_popup;

    void Awake()
    {
        // 퀘스트 정보 패널 불러오기
        quest_title = questPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        quest_objective = questPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    void OnEnable()
    {
        LoadQuest();
    }
    public void LoadQuest()
    {
        Quest quest = QuestManager.instance.FindQuest();

        if (quest != null)
        {
            string quest_status = "";
            switch (quest.state)
            {
                case Quest.State.progress:
                    quest_status = " (진행 중)";
                    break;
                case Quest.State.progress2:
                    quest_status = " (완료 가능)";
                    break;
            }

            quest_title.text = quest.title + quest_status;
            quest_objective.text = quest.objective;
        }
        else
        {
            quest_title.text = "진행 중인 퀘스트 없음";
        }
    }
    public void Option()
    {
        PlayUI.instance.OpenOptions();
    }
    public void Status()
    {
        PlayUI.instance.OpenStatus(0);
    }
    public void TitlePopup()
    {
        PlayUI.instance.OpenSystemPopup(title_popup);
    }
    public void QuitPopup()
    {
        PlayUI.instance.OpenSystemPopup(quit_popup);
    }
}
