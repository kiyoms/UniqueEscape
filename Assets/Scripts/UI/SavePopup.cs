using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SavePopup : MonoBehaviour
{
    public enum State
    {
        SAVE,LOAD
    };
    [Header("Prefetch data")]
    public State state;

    [Header("Character Info")]
    public TextMeshProUGUI ui_level;
    public TextMeshProUGUI ui_quest;
    public TextMeshProUGUI ui_savetime;

    // Start is called before the first frame update
    void Awake()
    {
        string level;
        string questText = "진행 중인 퀘스트 없음";
        string savetime;

       
        if (state == State.SAVE)
        {
            if (QuestManager.instance.FindQuest() != null)
            {
                questText = QuestManager.instance.FindQuest().title;
            }
            level = string.Format("LV.{0:n0} ({1:F2}%)", Player.instance.level, PlayerStatus.EXP_RATE(Player.instance.level, Player.instance.EXP));
            savetime = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
        }
        else
        {
            if (PlayerPrefs.HasKey(PlayerStatus.QUEST_LASTEST_NAME))
            {
                questText = PlayerPrefs.GetString(PlayerStatus.QUEST_LASTEST_NAME);
            }
            float exp = PlayerStatus.EXP_RATE(PlayerPrefs.GetInt(PlayerStatus.LEVEL), PlayerPrefs.GetInt(PlayerStatus.EXP));

            level = string.Format("LV.{0:n0} ({1:F2}%)", PlayerPrefs.GetInt(PlayerStatus.LEVEL), exp);
            savetime = PlayerPrefs.GetString(PlayerStatus.SAVETIME);
        }
        ui_level.text = level;
        ui_quest.text = questText;
        ui_savetime.text = savetime;
    }

    // Update is called once per framess
    void Update()
    {
        if (state == State.SAVE)
        {
            ui_savetime.text = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
        }
    }
}
