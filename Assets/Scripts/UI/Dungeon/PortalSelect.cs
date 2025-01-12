using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PortalSelect : MonoBehaviour
{
    [Header("Top Area")]
    public Button button_close;

    [Header("UI Area")]
    public Transform ui_content_group;
    public GameObject ui_content_group_prefab;
    public GameObject ui_town_content_prefab;
    public GameObject ui_dungeon_content_prefab;

    [System.Serializable]
    public struct TAB
    {
        public TextMeshProUGUI text;
        public GameObject focus;
    };
    public TAB[] ui_tab;
    public int tab;

    List<PrefetechedData.TOWN_DATA> townList;
    List<DungeonData> dungeonList;

    // Start is called before the first frame update
    void Start()
    {
        // 던전 지정
        townList = GameManager.instance.gameData.townData;
        dungeonList = GameManager.instance.gameData.dungeonData;

        // 던전 선택 이름 지정
        name = Config.DUNGEON_SELECT;

        // 뒤로 가기 버튼 추가
        button_close.onClick.AddListener(() => DestroyObject());

        // 탭
        tab = 0;
        SelectTab(1);
    }
    void Update()
    {
        // 뒤로 가기 버튼
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        PlayUI.instance.DestroyObject(this.gameObject, 0);
    }

    // 탭에 맞는 내용 출력
    public void SelectTab(int tab)
    {
        if(this.tab != tab)
        {
            if (ui_content_group.childCount != 0)
            {
                Destroy(ui_content_group.GetChild(0).gameObject);
            }

            ColorUtility.TryParseHtmlString("#F6E19C", out Color color);
            
            ui_tab[0].text.color = Color.gray;
            ui_tab[1].text.color = Color.gray;

            ui_tab[0].focus.SetActive(false);
            ui_tab[1].focus.SetActive(false);

            // 탭에 맞는 
            this.tab = tab;
            ui_tab[tab].text.color = color;
            ui_tab[tab].focus.SetActive(true);

            GameObject ui_content_list = Instantiate(ui_content_group_prefab, ui_content_group);
            ui_content_group.GetComponent<ScrollRect>().content = ui_content_list.GetComponent<RectTransform>();
            if (tab == 0)
            {
                int index = 1;
                foreach (PrefetechedData.TOWN_DATA list in townList)
                {
                    GameObject obj = Instantiate(ui_town_content_prefab, ui_content_list.transform);
                    obj.GetComponent<TownList>().Initialize(index++, list);
                }
            }
            else
            {
                int index = 1;
                foreach (DungeonData list in dungeonList)
                {
                    GameObject obj = Instantiate(ui_dungeon_content_prefab, ui_content_list.transform);
                    obj.GetComponent<DungeonList>().Initialize(index++, list);
                }
            }
        }
    }
}
