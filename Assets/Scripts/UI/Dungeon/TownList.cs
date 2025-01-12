using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TownList : MonoBehaviour
{
    PrefetechedData.TOWN_DATA townData;
    [Header("UI")]
    public TextMeshProUGUI ui_stage;
    public Image ui_image;
    public TextMeshProUGUI ui_name;
    public Button ui_button;
    public Sprite ui_lock;

    public void Initialize(int index, PrefetechedData.TOWN_DATA data)
    {
        townData = data;

        ui_stage.text = "마을 " + index;

        if (CheckQuest(data.code))
        {
            ui_name.text = data.title;
            ui_image.sprite = data.townImage;
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
        // 이동
        if(townData.code == GameManager.instance.position)
        {
            PlayUI.instance.MessagePopup("현재 위치하고 있는 지역입니다.");
        }
        else
        {
            GameManager.instance.Warp(townData.code, null);

            GameObject dungeon_select = GameObject.Find(Config.DUNGEON_SELECT);
            PlayUI.instance.DestroyObject(dungeon_select, 0);
        }
        
    }

    // 던전코드 찾기
    string CheckQuestCode(int code)
    {
        switch (code)
        {
            // 마을 1
            case 0:
                return "V00";
            // 마을 2
            case 1:
                return "E02";
            // 마을 3
            case 2:
                return "E05";
        }
        return null;
    }

    // 반환값: 퀘스트코드
    bool CheckQuest(int code)
    {
        string questCode = CheckQuestCode(code);

        Quest.State state = QuestManager.instance.findQuestWithCode(questCode).state;
        if (state == Quest.State.not || state == Quest.State.start)
        {
            return false;
        }
        else if(code == 2 && state == Quest.State.finish)
        {
            return true;
        }
        else
        {
            return true;
        }
    }
}
