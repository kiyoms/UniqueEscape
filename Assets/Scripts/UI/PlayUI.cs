using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayUI : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static PlayUI instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayUI>();
            }

            return m_instance;
        }
    }
    private static PlayUI m_instance;
    private Transform parent;
    // 게임모드
    public enum Status
    {
        GAME, PAUSE, DIE
    }
    public Status status = Status.GAME;

    [Range(150, 350)] public float distance = Config.HUD_DISTANCE_DEFAULT;
    [Range(0.7f, 1.5f)] public float size = Config.HUD_SIZE_DEFAULT;

    public float depth = 0;   // ESC로 조정할 수 있는 UI Depth레벨

    // 게임 플레이 UI
    public GameObject error_message;
    public GameObject noti_message;
    public GameObject levelup_popup;
    [Header("Dungeon Screen Panel")]
    public GameObject boss_panel;
    public GameObject clear_panel;
    public GameObject ending_panel;
    public GameObject die_Panel;

    // 전체화면 UI 패널
    [Header("Full Screen Panel")]
    public GameObject npc_Panel;
    public GameObject shop_Panel;
    public GameObject dungeon_Panel;
    public GameObject status_Panel;
    public GameObject option_Panel;

    // 플레이화면
    VirtualController controller;

    // 게임버튼
    [Header("Pause Control")]
    public GameObject pause;
    public Button pauseBtn;

    void Start()
    {
        if (PlayerPrefs.HasKey(Config.HUD_SIZE))
        {
            size = PlayerPrefs.GetFloat(Config.HUD_SIZE);
        }
        if (PlayerPrefs.HasKey(Config.HUD_DISTANCE))
        {
            distance = PlayerPrefs.GetFloat(Config.HUD_DISTANCE);
        }

        parent = transform;
        controller = GetComponentInChildren<VirtualController>();
    }
    // 0-1단계 Depth (일시정지)
    void Update()
    {
        // Depth가 1 이상이면 일시 정지
        Time.timeScale = (depth >= 1 ? 0 : 1);

        if (status != Status.DIE)
        {
            // ESC 키를 눌렀을 때 상호작용
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (depth == 0)
                {
                    OpenPause(true);
                }
                else if(depth == 1)
                {
                    OpenPause(false);
                }
            }
        }
    }
    public void OpenPause(bool open)
    {
        if (open)
        {
            controller.OnEndDrag(null);
        }
        depth = (open ? 1 : 0);
        pause.SetActive(open);
    }
    // 0단계 Depth (사망/보스/클리어)
    // 사망 시 일시정지 키 씹힘
    IEnumerator DiePopup()
    {
        yield return new WaitForSeconds(1f);
        GameObject panel = Instantiate(die_Panel, transform);
        panel.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
    public void MessagePopup(string message)
    {
        GameObject panel = Instantiate(noti_message, transform);
        panel.GetComponent<MessagePopup>().Popup(message);
    }
    public void ErrorMessagePopup(string message)
    {
        GameObject panel = Instantiate(error_message, transform);
        panel.GetComponent<MessagePopup>().Popup(message);
    }
    // 레벨 업
    public void LevelUp()
    {
        Instantiate(levelup_popup, transform);
    }
    public void Die()
    {
        if(status != Status.DIE)
        {
            status = Status.DIE;
            StartCoroutine(DiePopup());
        }
    }
    public void Revive()
    {
        status = Status.GAME;
    }
    // 보스 스테이지
    public void BossStage()
    {
        Instantiate(boss_panel, transform);
    }
    
    // 스테이지 클리어
    public bool StageClear(bool ending)
    {
        if (!ending)
        {
            Instantiate(clear_panel, transform);
        }
        else
        {
            Instantiate(ending_panel, transform);
        }
        return true;
    }

    // 0.5단계 Depth
    public void OpenNpcPanel(Npc npc)
    {
        depth = 0.5f;
        NpcPanel panel = Instantiate(npc_Panel, parent).GetComponent<NpcPanel>();
        panel.Initialize(npc);
    }
    public void OpenShop()
    {
        depth = 0.5f;
        GameObject panel = Instantiate(shop_Panel, parent);
        panel.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
    public void OpenDungeon()
    {
        depth = 0.5f;
        GameObject panel = Instantiate(dungeon_Panel, parent);
        panel.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
    // 2단계 Depth
    public void OpenStatus(int tab)
    {
        depth = 2;
        GameObject panel = Instantiate(status_Panel, parent);
        panel.GetComponent<StatusMenu>().ChangeItemSlot(tab);
        panel.GetComponent<RectTransform>().rect.Set(0,0,0,0);
    }
    public void OpenOptions()
    {
        depth = 2;
        GameObject panel = Instantiate(option_Panel, parent);
        panel.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
    // 5단계 Depth (시스템 팝업)
    public void OpenSystemPopup(GameObject panel)
    {
        depth = 5;
        GameObject obj = Instantiate(panel, parent);
        obj.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
    // 화면 오브젝트 파괴
    public void DestroyObject(GameObject panel, float depth)
    {
        Destroy(panel);
        this.depth = depth;
    }
}
