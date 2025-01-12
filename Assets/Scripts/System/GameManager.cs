using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    private static GameManager m_instance;

    // 테스트 환경
    public enum TestEnv
    {
        PC, Mobile
    }
    public TestEnv testEnv;

    public bool cheat;

    // 상태
    public bool night = false;

    //플레이어 위치변수
    public int position = 0;
    public int last_position = 0; // 마지막 마을

    public PrefetechedData gameData;
    void Start()
    {
        GameLoad();
        ReturnToTown(position);
    }
    public void Warp(int code, Transform pos)
    {
        this.position = code; // 위치 이동
        
        // 마을로 가는 작업이라면
        if (code < 10)
        {
            ReturnToTown(code);

            // 스테이지 2로 가는 마을 작업
            if(code == 1)
            {
                if (QuestManager.instance.findQuestWithCode("E02").state == Quest.State.progress)
                {
                    QuestManager.instance.findQuestWithCode("E02").ChangeStateWithPlayer(Quest.State.progress2);
                }
            }
        }

        else
        {
            Player.instance.transform.position = pos.position;
            DungeonController.instance.DungeonActive(code);
        }

    }

    // 게임을 불러올 때 시작하면서 마을로 돌아갈 때
    // 아니면 게임 중에 마을로 돌아갈 때
    public void ReturnToTown(int code)
    {
        // 체력 최대치
        Player.instance.Heal(Player.instance.maxHP);

        // 마을 위치로 귀환
        position = code;
        last_position = code;
        Player.instance.transform.position = gameData.townData[code].townPositon.position;

        // 마을 오디오로 변환
        SoundManager.instance.SetMusic(gameData.townData[code].townAudio);

        // 던전 리셋
        GameObject dungeon = GameObject.Find(Config.DUNGEON);
        if(dungeon != null)
        {
            Destroy(dungeon);
        }
    }

    // 타이틀로 돌아가기 메뉴
    public void ReturnToTitle()
    {
        Time.timeScale = 1; // 일시 정지된 TimeScale을 0으로 조정.
        SceneManager.LoadScene("Title");
    }

    // 게임 종료
    public void QuitGame()
    {
        Application.Quit();
    }
    public bool GameSave()
    {
        // 플레이어 인스턴스 불러오기
        Player player = Player.instance;

        // 게임 저장 시간 기록
        PlayerPrefs.SetString(PlayerStatus.SAVETIME, System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));

        //플레이어 위치 저장
        PlayerPrefs.SetInt("Position", position);

        // 플레이어 정보 저장
        PlayerPrefs.SetInt(PlayerStatus.LEVEL, player.level);
        PlayerPrefs.SetFloat(PlayerStatus.MAX_HP, player.maxHP);
        PlayerPrefs.SetFloat(PlayerStatus.MAX_SP, player.maxSP);
        PlayerPrefs.SetInt(PlayerStatus.EXP, player.EXP);
        PlayerPrefs.SetFloat(PlayerStatus.ATK, player.atk);
        PlayerPrefs.SetFloat(PlayerStatus.DEF, player.def);
        PlayerPrefs.SetInt(PlayerStatus.MONEY, player.money);
        PlayerPrefs.SetFloat(PlayerStatus.MOVE_SPEED, player.originMoveSpeed);

        // 아이템 상태 저장
        Player.instance.inventory.ItemSave();

        // 퀘스트 저장 함수 호출
        QuestManager.instance.SaveQuest();

        // 플레이어 PREFS 저장
        PlayerPrefs.Save();

        return true;
    }
    public void GameLoad()
    {
        // 저장한게 없다면 리턴
        if (!PlayerPrefs.HasKey(PlayerStatus.SAVETIME))
        {
            return;
        }
        // 플레이어 인스턴스 불러오기
        Player player = Player.instance;

        // 플레이어 위치 불러오기
        position = PlayerPrefs.GetInt("Position");

        // 플레이어 정보 불러오기
        player.level = PlayerPrefs.GetInt(PlayerStatus.LEVEL);
        player.maxHP = PlayerPrefs.GetFloat(PlayerStatus.MAX_HP);
        player.maxSP = PlayerPrefs.GetFloat(PlayerStatus.MAX_SP);
        player.EXP = PlayerPrefs.GetInt(PlayerStatus.EXP);
        player.atk = PlayerPrefs.GetFloat(PlayerStatus.ATK);
        player.def = PlayerPrefs.GetFloat(PlayerStatus.DEF);
        player.money = PlayerPrefs.GetInt(PlayerStatus.MONEY);
        player.originMoveSpeed = PlayerPrefs.GetFloat(PlayerStatus.MOVE_SPEED);

        Player.instance.inventory.ItemLoad();

        // 퀘스트 정보 불러오기
        QuestManager.instance.LoadQuest();
    }
}