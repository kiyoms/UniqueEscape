using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest Data", menuName = "Scriptable Object/Quest Data")]
public class Quest : ScriptableObject
{
    // 퀘스트 상태 enum
    public enum State
    {
        not, start, progress, progress2, finish
    } // 시작 불가, 시작 가능, 진행 중, 완료 가능, 완료

    // 퀘스트 정보
    [Header("Quset Info")]
    public string questCode;
    public string title;
    public string npcCode;
    public string finishNpcCode;
    public string objective;

    public State state;

    // 퀘스트 대화집
    [Header("Quest Dialogue")]
    [Multiline(3)]
    public string[] start_dialogue;
    [Multiline(3)]
    public string[] progress_dialogue;
    [Multiline(3)]
    public string[] progress2_dialogue;

    [Header("Quest Reward")]
    public string nextquest;
    public int exp;
    public int money;
    public EquipItem equip;

    public string[] GetDialogue()
    {
        switch (state)
        {
            case State.start:
                return start_dialogue;
            case State.progress:
                return progress_dialogue;
            case State.progress2:
                return progress2_dialogue;
        }
        return null;
    }
    // 에딧으로 변동시키는 퀘스트 상태
    public void ChangeState(State state)
    {
        this.state = state;
        if(state == State.start)
        {
            QuestManager.instance.UpdateLastQuest(this);
        }
        QuestManager.instance.Reset();
    }

    public void ChangeStateWithPlayer(State state)
    {
        // 인게임 퀘스트 상태 변경
        if (this.state != State.finish)
        {
            // 퀘스트 상태에 변동이 없다면
            if(this.state == state)
            {
                return;
            }
            // 장비 아이템이 보상이라면...
            if(state == State.finish && equip != null)
            {
                if(Player.instance.inventory.maxSlot == Player.instance.inventory.equip_size)
                {
                    PlayUI.instance.ErrorMessagePopup("장비 아이템이 가득 차 완료할 수 없습니다.\n아이템을 비워주세요.");
                    return;
                }
            }
            // 퀘스트 변동. NPC 내 퀘스트 정보 갱신.
            this.state = state;
            QuestManager.instance.UpdateLastQuest(this);

            switch (state)
            {
                case State.progress:
                    PlayUI.instance.MessagePopup("퀘스트를 받았습니다:\n" + title);
                    NoIfQuest();
                    break;
                case State.progress2:
                    PlayUI.instance.MessagePopup("퀘스트를 완료할 수 있습니다:\n" + title);
                    break;
                case State.finish:
                    Reward();
                    PlayUI.instance.MessagePopup("퀘스트를 완료했습니다:\n" + title);
                    break;
            }
        }
    }
    
    // 퀘스트의 특수 처리
    public void NoIfQuest()
    {
        switch (this.questCode)
        {
            case "E03":
                ChangeStateWithPlayer(State.progress2);
                break;
            case "E06":
                ChangeStateWithPlayer(State.progress2);
                break;
            case "E09":
                ChangeStateWithPlayer(State.progress2);
                break;
            case "E99":
                ChangeStateWithPlayer(State.finish);
                // 퀘스트 완료 팝업 즉시 파괴하기
                Destroy(GameObject.FindGameObjectWithTag(Config.MESSAGE_TAG));
                break;
        }
    }

    public void Reward()
    {
        Player.instance.EXP += exp;
        Player.instance.money += money;

        if (questCode.Equals("E09"))
        {
            Destroy(GameObject.FindGameObjectWithTag(Config.MESSAGE_TAG));
            PlayUI.instance.MessagePopup("이제 마지막 던전으로 들어갈 수 있습니다.\n단단히 준비를 하고 가십시오.");
        }

        if (equip != null)
        {
            Player.instance.inventory.EquipItemGain(equip);
        }
    }
}
