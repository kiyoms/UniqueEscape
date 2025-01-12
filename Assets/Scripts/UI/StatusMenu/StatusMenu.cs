using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 능력치 창 (Depth2)
// 상세설명 창 (Depth3)
public class StatusMenu : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static StatusMenu instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<StatusMenu>();
            }

            return m_instance;
        }
    }
    private static StatusMenu m_instance;

    public Button button_back;
    // 능력치
    [Header("Status")]
    public Slider expGauge;
    [System.Serializable]
    public struct Status
    {
        public TextMeshProUGUI lv, exp;
        public TextMeshProUGUI hp, atk, def, speed, money;
    };
    public Status stat;

    [Header("Item Slot")]
    public List<StatusEquipment> equip_invetory;
    public List<StatusInventory> inventory;
    public Transform slotGroup;
    public GameObject inventoryPrefab;
    public GameObject slotPrefab;
    public Tab[] tab = new Tab[3];
    [System.Serializable]
    public struct Tab
    {
        public TextMeshProUGUI text;
        public GameObject focus;
    };
    [Header("Item Popup")]
    public GameObject itemPopup;
    public GameObject equipmentPopup;

    private int itemTab = -1;    // 아이템 탭(-1 미설정 / 0 장비 / 1 소비 / 2 기타)

    void Awake()
    {
        // 패널 파괴 트리거 추가
        if (PlayUI.instance != null)
        {
            button_back.onClick.AddListener(DestroyObject);
        }
        // 능력치 조정
        // 아이템 슬롯 조정
        GetStatus();
    }
    void Update()
    {
        // ESC 키를 이용한 탈출
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyObject();
        }
        GetStatus();
    }
    // 능력치 가져오기
    void GetStatus()
    {
        if (Player.instance != null)
        {
            // 플레이어 인스턴스로부터 정보 가져옴
            Player player = Player.instance;

            float expP = PlayerStatus.EXP_RATE(player.level, player.EXP);
            stat.lv.text = player.level.ToString();
            stat.exp.text = string.Format("{0:F2}%", expP);
            expGauge.value = expP;
            // 하단 능력치
            stat.hp.text = string.Format("{0:n0} / {1:n0}", player.HP, player.maxHP);
            stat.atk.text = string.Format("{0:n0} ~ {1:n0}", player.atk * 0.8, player.atk * 1.2);
            stat.def.text = string.Format("{0:n0}", player.def);
            stat.speed.text = string.Format("{0:F1}", player.originMoveSpeed);

            // 상단 소지금
            stat.money.text = string.Format("{0:n0}", player.money);
        }
    }
    // 아이템 슬롯 가져오기
    void GetItemSlot()
    {
        // 아이템 슬롯 정의(아무것도 없으므로 null)
        ItemSlot[] itemSlots = null;
        inventory = new List<StatusInventory>();
        switch (itemTab)
        {
            case 0:
                if (Player.instance.inventory.equipSlot != null)
                {
                    itemSlots = Player.instance.inventory.equipSlot;
                }
                break;
            case 1:
                if (Player.instance.inventory.consumeSlot != null)
                {
                    itemSlots = Player.instance.inventory.consumeSlot;
                }
                break;
            case 2:
                if (Player.instance.inventory.etcSlot != null)
                {
                    itemSlots = Player.instance.inventory.etcSlot;
                }
                break;
        }
        // 아이템 슬롯을 불러왔다면
        if (itemSlots != null)
        {
            var itemGroup = Instantiate(inventoryPrefab, slotGroup);
            for (int index = 0; index < itemSlots.Length; index++)
            {
                var item = Instantiate(slotPrefab, itemGroup.transform);
                item.GetComponent<StatusInventory>().index = index;
                item.GetComponent<StatusInventory>().type = itemSlots[0].type;
                inventory.Add(item.GetComponent<StatusInventory>());
            }
            ResetInventory();
        }
    }
    // 
    public void ResetInventory()
    {
        foreach(StatusInventory itemlist in inventory)
        {
            itemlist.Reset();
        }
        foreach (StatusEquipment itemlist in equip_invetory)
        {
            itemlist.Reset();
        }
    }


    // 아이템 슬롯 탭 변경
    public void ChangeItemSlot(int index)
    {
        if(itemTab != index)
        {
            itemTab = index;

            // 기존의 슬롯을 전부 지우고 갱신
            if(slotGroup.childCount != 0)
            {
                Destroy(slotGroup.GetChild(0).gameObject);
            }
            GetItemSlot();

            // 선택하지 않은 오브젝트 초기화
            for(int i = 0; i < 3; i++)
            {
                tab[i].text.color = Color.white;
                tab[i].focus.SetActive(false);
            }

            // 오브젝트 컬러/포커스 세팅
            ColorUtility.TryParseHtmlString("#F6E19C", out Color color);
            tab[index].text.color = color;
            tab[index].focus.SetActive(true);
        }
    }
    
    
    // 아이템 설명 출력
    public void ItemPopup(ItemSlot slot)
    {
        GameObject prefab = itemPopup;
        if (slot.type == ItemSlot.Type.Equip)
        {
            prefab = equipmentPopup;
        }
        var popup = Instantiate(prefab, this.transform);
        popup.GetComponent<ItemPopup>().Initialize(slot);
        PlayUI.instance.depth = 3;
    }
    public void ItemPopup(EquipItem item)
    {
        var popup = Instantiate(equipmentPopup, this.transform);
        popup.GetComponent<ItemPopup>().Initialize(item);
        PlayUI.instance.depth = 3;
    }
    // 팝업 파괴
    void DestroyObject()
    {
       PlayUI.instance.DestroyObject(this.gameObject, 1);
    }
}
