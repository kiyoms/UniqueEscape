using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemPopup : MonoBehaviour
{
    public enum Type
    {
        Title, Quit, Save
    }
    public Type popupType;
    public Button confirm_button;
    public Button cancel_button;

    void Start()
    {
        if (popupType == Type.Title)
        {
            confirm_button.onClick.AddListener(GameManager.instance.ReturnToTitle);
        }
        else if (popupType == Type.Quit)
        {
            confirm_button.onClick.AddListener(GameManager.instance.QuitGame);
        }
        else
        {
            confirm_button.onClick.AddListener(Save);
        }
        cancel_button.onClick.AddListener(() => DestroyObject());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyObject();
        }
    }
    // 게임 저장
    void Save()
    {
        if (GameManager.instance.GameSave())
        {
            PlayUI.instance.MessagePopup("게임 저장 성공!");
        }
        else
        {
            PlayUI.instance.MessagePopup("게임 저장 실패...");
        }
        DestroyObject();
    }
    // 팝업 파괴
    void DestroyObject()
    {
        // 저장 팝업이라면
        if (popupType == Type.Save)
        {
            PlayUI.instance.DestroyObject(this.gameObject, 0);
        }
        // 일시 정지에서 띄운 팝업이라면
        else
        {
            PlayUI.instance.DestroyObject(this.gameObject, 1);
        }
    }
}
