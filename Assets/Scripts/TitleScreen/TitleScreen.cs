using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleScreen : MonoBehaviour
{
    // FIRST UI
    [Header("SECONDARY UI")]
    public RectTransform background;

    // Second UI
    [Header("SECONDARY UI")]
    public GameObject secUI;
    public Button loadGame_Button;
    public GameObject option_panel;

    // 팝업 바
    [Header("POPUP MENU")]
    public GameObject newGame_popup;
    public GameObject newGame2_popup;
    public GameObject loadGame_popup;
    public GameObject quitGame_popup;

    void Start()
    {
        background.sizeDelta = new Vector2(TitleManager.instance.width, TitleManager.instance.height);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            if(TitleManager.instance.state == TitleManager.State.Title)
            {
                OpenPopup("Quit");
            }
        }
    }

    // 탭 투 스타트
    public void StartButton(Button button)
    {
        // 탭 투 스타트 버튼 파괴
        Destroy(button.gameObject);

        // 추가 UI 활성화
        secUI.SetActive(true);

        // 게임 저장 정보가 있다면 활성화
        if (TitleManager.instance.saveData)
        {
            loadGame_Button.interactable = true;
        }
    }

    public void NewGamePopup()
    {
        // 저장 데이터가 있음
        if (TitleManager.instance.saveData)
        {
            OpenPopup("New2");
        }
        // 저장 데이터가 없음
        else
        {
            OpenPopup("New");
        }
    }
    public void LoadGamePopup()
    {
        OpenPopup("Load");
    }
    void OpenPopup(string state)
    {
        GameObject popup;
        switch (state)
        {
            case "New":
                popup = newGame_popup;
                break;
            case "New2":
                popup = newGame2_popup;
                break;
            case "Load":
                popup = loadGame_popup;
                break;
            case "Quit":
                popup = quitGame_popup;
                break;
            default:
                return;
        }
        GameObject obj = Instantiate(popup, transform);
        obj.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
    public void OpenOptions()
    {
        TitleManager.instance.state = TitleManager.State.Option;
        GameObject obj = Instantiate(option_panel, transform);
        obj.GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
}
