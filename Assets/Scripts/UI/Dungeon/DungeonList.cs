using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DungeonList : MonoBehaviour
{
    DungeonData dungeonData;
    [Header("UI")]
    public TextMeshProUGUI ui_stage;
    public Image ui_image;
    public TextMeshProUGUI ui_name;
    public Button ui_button;
    public Sprite ui_lock;


    [Header("NEXT")]
    public GameObject ui_popup;

    public void Initialize(int index, DungeonData data)
    {
        dungeonData = data;

        ui_stage.text = "스테이지 " + index;
      
        if (CheckQuest(data.code))
        {
            ui_image.sprite = data.sprite;
            ui_name.text = data.title;
            ui_button.onClick.AddListener(Go);
            ui_button.interactable = true;
        }
        else
        {
            ui_image.sprite = ui_lock;
            ui_name.text = "??????";
            ui_button.interactable = false;
            ui_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "진입 불가";
        }
    }

    public void Go()
    {
        var popup = Instantiate(ui_popup, PlayUI.instance.transform);
        popup.GetComponent<DungeonPopup>().Initialize(dungeonData);
    }

    // 던전코드 찾기
    string CheckQuestCode(int code)
    {
        switch (code)
        {
            // 던전 1
            case 10:
                return "E01";
            // 던전 2
            case 20:
                return "E05";
            // 던전 3
            case 30:
                return "E08";
            // 마지막 던전
            case 40:
                return "E09";
        }
        return null;
    }

    // 반환값: 퀘스트코드
    bool CheckQuest(int code)
    {
        string questCode = CheckQuestCode(code);

        Quest.State state = QuestManager.instance.findQuestWithCode(questCode).state;
        if (questCode.Equals("E09") && state == Quest.State.finish)
        {
            return true;
        }
        else if (state == Quest.State.not || state == Quest.State.start)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
