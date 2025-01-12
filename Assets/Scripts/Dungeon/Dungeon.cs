using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public enum State
    {
        LOADING, NORMAL, CLEAR
    };
    public State state { get; private set; }

    [Header("Dungeon Data")]
    public AudioClip clip;  // 오디오클립
    public int code;    // 위치코드
    public Transform point;    // 던전 스폰 포인트

    [Header("Dungeon Monster")]
    public GameObject monster;  // 몬스터 prefab
    public int mobCount;  // 스테이지에 생성할 몬스터 수
    public int spawnRange;   // 소환 범위
    public int remainMob { get; private set; }   // 남은 몬스터 수

    [Header("Sudden Mission")]
    public bool sudden = false;

    void Awake()
    {
        state = State.LOADING;
    }
    // 첫 던전 생성 성공 후 실행
    void OnEnable()
    {
        if (state == State.LOADING)
        {
            Initialize();
        }
    }
    void Initialize()
    {
        state = State.NORMAL;
        SoundManager.instance.SetMusic(clip);

        while (remainMob < mobCount)
        {
            Vector3 pos = point.transform.position;

            // 랜덤 위치 계산(스폰 위치랑 너무 가까우면 재계산)
            if (spawnRange != 0)
            {
                float x, y;
                do
                {
                    x = Random.Range(-spawnRange, spawnRange);
                } while (Mathf.Abs(x) < 0.5);
                do
                {
                    y = Random.Range(-spawnRange, spawnRange);
                } while (Mathf.Abs(y) < 0.5);

                pos.x += x;
                pos.y += y;
            }
            GameObject mob = Instantiate(monster, pos, new Quaternion(0, 0, 0, 0));
            mob.name = "MOB_" + code;
            mob.transform.parent = transform;
            remainMob++;
        }

        // 만약 몬스터가 0마리라면 자동으로 완료 처리 후 스크립트 종료
        if (mobCount == 0)
        {
            state = State.CLEAR;
            return;
        }
        if (GetComponentInChildren<BossMonster>())
        {
            return;
        }

        // 플레이어가 던전에 처음 진입하면 몬스터 소환
        // 확률 계산
        sudden = SuddenMission.Probability();
        if (sudden)
        {
            Instantiate(DungeonController.instance.suddenMission, transform);
        }
    }

    // 몬스터 사망 시 수 감소
    public void DieMonster()
    {
        remainMob--;

        // 스테이지 클리어
        if (remainMob == 0)
        {
            PlayUI.instance.MessagePopup("몬스터를 모두 퇴치하여\n다음 지역으로 이동할 수 있습니다.");
            state = State.CLEAR;
        }
    }
}
