using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static DungeonController instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<DungeonController>();
            }

            return m_instance;
        }
    }
    private static DungeonController m_instance;
    [Header("Dungeon Data")]
    public Dungeon[] dungeon;
    [Header("Sudden Data")]
    public GameObject suddenMission;

    // 첫 번째 던전 활성화
    public void Initialize()
    {
        gameObject.name = Config.DUNGEON;
        dungeon[0].gameObject.SetActive(true);
    }
    public void DungeonActive(int code)
    {
        int stage = code % 10;
        // 던전 활성 상태가 아니면 활성처리
        if (!dungeon[stage].gameObject.activeSelf)
        {
            dungeon[stage].gameObject.SetActive(true);
        }
    }
}
