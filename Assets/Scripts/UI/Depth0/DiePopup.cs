using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DiePopup : MonoBehaviour
{
    public Button btn1, btn2;
    public AudioClip clip;

    void Start()
    {
        Time.timeScale = 0;
        btn1.onClick.AddListener(ReturnToTown);
        btn2.onClick.AddListener(ReturnToTitle);

        GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
    public void Hide()
    {
        Time.timeScale = 1;
        Destroy(this.gameObject);
    }
    
    void ReturnToTown()
    {
        Player.instance.animator.SetTrigger("Revive");

        PlayUI.instance.Revive();
        Player.instance.status = Player.Status.Idle;
        GameManager.instance.Warp(GameManager.instance.last_position, null);

        Hide();
    }
    void ReturnToTitle()
    {
        Hide();
        GameManager.instance.ReturnToTitle();
    }
}
