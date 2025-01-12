using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    // 플레이 HUD 화면
    [Header("UI AREA")]
    public RectTransform status;
    public RectTransform pause;
    public RectTransform left_controller;
    public RectTransform right_controller;

    [Header("UI SLIDER GAUGE")]
    public Slider hp_gauge;
    public Slider sp_gauge;

    [Header("UI TEXT")]
    public TextMeshProUGUI exp_value;
    public TextMeshProUGUI lv_value;
    public TextMeshProUGUI hp_value;
    public TextMeshProUGUI sp_value;

    // 기본값
    [Range(150, 350)] public float distance = 250;
    [Range(0.7f, 1.5f)] public float size = 1.1f;

    private void Awake()
    {
        if (PlayUI.instance != null)
        {
            size = PlayUI.instance.size;
            distance = PlayUI.instance.distance;
        }
        else
        {
            size = TitleManager.instance.size;
            distance = TitleManager.instance.distance;
        }

        Resize();
    }
    void Update()
    {
        if (PlayUI.instance != null)
        {
            size = PlayUI.instance.size;
            distance = PlayUI.instance.distance;
        }
        else
        {
            size = TitleManager.instance.size;
            distance = TitleManager.instance.distance;
        }
        
        Resize();

        if (Player.instance != null)
        {
            Player p = Player.instance;

            // HP/SP GAUGE
            hp_gauge.value = p.HP / p.maxHP;
            sp_gauge.value = p.SP / p.maxSP;

            // TEXT
            hp_value.text = string.Format("{0}/{1}", p.HP, p.maxHP);
            sp_value.text = string.Format("{0:F1}/{1}", p.SP, p.maxSP);
            exp_value.text = string.Format("{0:F2}%", PlayerStatus.EXP_RATE(p.level, p.EXP));
            lv_value.text = p.level.ToString();

            // 상단 UI 크기
        }
    }

    void Resize()
    {
        // 위치 조정
        status.anchoredPosition = new Vector2(distance - 230, -distance + 230);
        pause.anchoredPosition = new Vector2(-distance + 230, -distance + 230);

        left_controller.anchoredPosition = new Vector2(distance, distance);
        right_controller.anchoredPosition = new Vector2(-distance, distance);

        // 크기 조정
        status.localScale = new Vector2(size, size);
        pause.localScale = new Vector2(size, size);
        left_controller.localScale = new Vector2(size, size);
        right_controller.localScale = new Vector2(size - 0.1f, size - 0.1f);
    }
}
