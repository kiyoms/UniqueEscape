using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 던전 경고창
public class DungeonPopup : MonoBehaviour
{
    DungeonData dungeonData;

    [Header(Inspector.UI_COMPONENT)]
    public GameObject ui_warning;
    public GameObject ui_loading;

    [Header(Inspector.UI_COMPONENT)]
    public TextMeshProUGUI ui_dungeon_name;
    public TextMeshProUGUI ui_dungeon_name2;

    [Header(Inspector.UI_BOTTOM)]
    public Button ui_confirm;
    public Button ui_cancel;
    public Slider ui_slider;

    // Start is called before the first frame update
    public void Initialize(DungeonData data)
    {
        dungeonData = data;

        ui_dungeon_name.text = data.title;
        ui_dungeon_name2.text = data.title;
        ui_confirm.onClick.AddListener(Confirm);
        ui_cancel.onClick.AddListener(DestroyObject);

        GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }

    void Update()
    {
        // 뒤로 가기 버튼
        if (ui_warning.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyObject();
        }
    }
    void Confirm()
    {
        ui_warning.SetActive(false);
        ui_loading.SetActive(true);
        StartCoroutine(DungeonCoroutine());
    }
    void DestroyObject()
    {
        PlayUI.instance.DestroyObject(this.gameObject, 0.5f);
        if (ui_loading.activeSelf)
        {
            GameObject dungeon_select = GameObject.Find(Config.DUNGEON_SELECT);
            PlayUI.instance.DestroyObject(dungeon_select, 0);
        }
    }

    IEnumerator DungeonCoroutine()
    {
        float timer = 0;

        var obj = Instantiate(dungeonData.data, null);
        DungeonController dungeon = obj.GetComponent<DungeonController>();
        Transform pos = GameObject.FindGameObjectWithTag(Config.DUNGEON_START).transform;
        while (ui_slider.value != 1.0f)
        {
            yield return null;
            timer += Time.deltaTime * 0.7f;
            if (ui_slider.value < 0.5f)
            {
                ui_slider.value = Mathf.Lerp(ui_slider.value, 0.5f, timer);
            }
            else
            {
                ui_slider.value = Mathf.Lerp(ui_slider.value, 1f, timer);
                if (ui_slider.value == 1.0f)
                {
                    GameManager.instance.Warp(dungeonData.code, pos);
                    dungeon.Initialize();
                    DestroyObject();
                    yield break;
                }
            }
        }
    }
}
