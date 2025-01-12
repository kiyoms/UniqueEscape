using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePopup : MonoBehaviour
{
    // New2는 저장 데이터가 있을 떄
    public enum Type
    {
        NEW, NEW2, LOAD, QUIT
    }

    [Header("Prefetch Data")]
    public Type popupType;

    [Header("UI Button")]
    public Button confirm_button;
    public Button cancel_button;

    void Start()
    {
        confirm_button.onClick.AddListener(() => Confirm());
        cancel_button.onClick.AddListener(() => DestroyPopup());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(this.gameObject);
        }
    }
    private void Confirm()
    {
        if (popupType == Type.NEW)
        {
            TitleManager.instance.NewGame(false);
        }
        else if (popupType == Type.NEW2)
        {
            TitleManager.instance.NewGame(true);
        }
        else if (popupType == Type.LOAD)
        {
            TitleManager.instance.LoadGame();
        }
        DestroyPopup();
    }

    private void DestroyPopup()
    {
        Destroy(this.gameObject);
    }
}
