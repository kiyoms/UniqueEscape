using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static TitleManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<TitleManager>();
            }

            return m_instance;
        }
    }
    private static TitleManager m_instance;
    public enum State
    {
        Title, Loading, Option
    }
    public State state;

    public int width, height;

    public string nextScene = "Gameplay";
    public bool saveData;

    public Slider progressBar;
    public RectTransform background;
    public GameObject secUI;

    [Range(150, 350)] public float distance = Config.HUD_DISTANCE_DEFAULT;
    [Range(0.7f, 1.5f)] public float size = Config.HUD_SIZE_DEFAULT;

    // Playerprefs에 저장된 데이터를 확인한다.
    void Awake()
    {
        // 데이터 확인
        state = State.Title;
        saveData = PlayerPrefs.HasKey(PlayerStatus.SAVETIME);

        // 배경 크기 조절
        width = Screen.width;
        height = Screen.width / 16 * 9;

        if (PlayerPrefs.HasKey(Config.HUD_SIZE))
        {
            size = PlayerPrefs.GetFloat(Config.HUD_SIZE);
        }
        if (PlayerPrefs.HasKey(Config.HUD_DISTANCE))
        {
            distance = PlayerPrefs.GetFloat(Config.HUD_DISTANCE);
        }

    }

    // 원래 상태로 돌리기
    public void DestroyPanel(GameObject panel)
    {
        state = State.Title;
        Destroy(panel);
    }

    // 게임 시작하기
    public void NewGame(bool delete)
    {
        // 저장한 데이터를 삭제하고 만듦.
        if (delete)
        {
            PlayerPrefs.DeleteAll();
        }
        // 씬 로드
        StartCoroutine(LoadScene());
    }
    // 게임 불러오기
    public void LoadGame()
    {
        StartCoroutine(LoadScene());
    }

    // 게임 종료
    public void QuitGame()
    {
        Application.Quit();
    }
    // 게임 로딩
    IEnumerator LoadScene()
    {
        Destroy(secUI);
        progressBar.gameObject.SetActive(true);

        yield return null;

        Time.timeScale = 1;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);
                if (progressBar.value >= op.progress) { timer = 0f; }
            }
            else
            {
                progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);
                if (progressBar.value == 1.0f) { op.allowSceneActivation = true; yield break; }
            }

        }
    }
}
