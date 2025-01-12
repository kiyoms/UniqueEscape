using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    public bool ending;

    // 카운트다운 변수
    readonly float limitedTime = 10f;

    public Button button_town;
    public Button button_pause;

    void Start()
    {
        // 마을로 돌아가기 카운트
        if (!ending)
        {
            button_town.onClick.AddListener(Town);
        }
        else
        {
            button_town.onClick.AddListener(Ending);
        }
        // 카운트다운 진행
        button_pause.onClick.AddListener(Wait);

        // RECT 설정
        GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
    void Wait()
    {
        PlayUI.instance.MessagePopup(limitedTime + "초 후 자동 귀환합니다.");
        StartCoroutine(Countdown());
        this.GetComponent<RectTransform>().localScale = new Vector2(0,0);
    }

    // 마을 강제귀환 카운트
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(limitedTime / 2);
        PlayUI.instance.MessagePopup(limitedTime / 2 + "초 후 자동 귀환합니다.");
        yield return new WaitForSeconds(limitedTime / 2);
        if (!ending)
        {
            Town();
        }
        else
        {
            Ending();
        }
    }
    void Town()
    {
        Destroy(this.gameObject);
        GameManager.instance.ReturnToTown(GameManager.instance.last_position);
    }
    public void Ending()
    {
        SceneManager.LoadScene("Ending");
    }
}
