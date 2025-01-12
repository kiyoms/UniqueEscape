using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static string SAVETIME = "SAVETIME";

    public static string LEVEL = "LEVEL";
    public static string MAX_HP = "MAX_HP";
    public static string MAX_SP = "MAX_SP";
    public static string ATK = "ATK";
    public static string DEF = "DEF";
    public static string EXP = "EXP";
    public static string MONEY = "MONEY";
    public static string MOVE_SPEED = "MOVE_SPEED";

    public static string EQUIPED_SLOT = "EQUIPED";
    public static string ITEM_SLOT_EQUIP = "EQUIPED_SLOT";
    public static string ITEM_SLOT_CONSUME = "CONSUME_SLOT";
    public static string ITEM_SLOT_ETC = "ETC_SLOT";
    public static string ITEM_SLOT_QUICK = "QUICK_SLOT";

    public static string QUEST_FINISHED = "QUEST_FINISHED";
    public static string QUEST_LASTEST = "QUEST_LASTEST";
    public static string QUEST_LASTEST_NAME = "QUEST_LASTEST_NAME";
    public static string QUEST_LASTEST_STATUS = "QUEST_LASTEST_STATUS";

    public static float EXP_RATE(int level, int current)
    {
        float required = Experience.EXP_TABLE[level - 1];
        return (current / required) * 100;
    }
}
