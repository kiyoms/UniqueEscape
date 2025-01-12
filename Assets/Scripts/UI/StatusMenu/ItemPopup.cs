using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemPopup : MonoBehaviour
{
    EquipItem equipment;
    ItemSlot itemSlot;
    [Header("Item Information")]
    public Image ui_image;
    public TextMeshProUGUI ui_name;
    public TextMeshProUGUI ui_description;

    [Header("Button")]
    public Button close_button;

    [Header("Additional: Equipment")]
    public TextMeshProUGUI ui_status;
    public Button equip_button;
    [Header("Additional: Consume")]
    public Button use_button;
    public Button quick_button;
    public TextMeshProUGUI quick_button_text;

    // 장비 슬롯에서 호출
    void Initialize(Item item)
    {
        // 닫기 버튼 추가
        close_button.onClick.AddListener(DestroyPopup);

        ui_image.sprite = item.itemImage;
        ui_name.text = item.itemName;
        ui_description.text = item.description;

        // RectTransform 변형
        GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
    }
    public void Initialize(EquipItem item)
    {
        equipment = item;

        ui_status.text = SetStatus(item);
        equip_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "장착해제";
        equip_button.onClick.AddListener(UnEquip);

        Initialize((Item)item);
    }
    // 아이템 슬롯에서 호출
    public void Initialize(ItemSlot itemSlot)
    {
        // 아이템 슬롯
        this.itemSlot = itemSlot;

        Item item = null;
        switch (itemSlot.type)
        {
            case ItemSlot.Type.Equip:
                item = itemSlot.equip;
                break;
            case ItemSlot.Type.Consume:
                item = itemSlot.consume;
                break;
            case ItemSlot.Type.Etc:
                item = itemSlot.etc;
                break;
        }

        // 아이템창에서 불러왔다면 적용
        if (item != null)
        {
            // 장비 아이템이라면
            if (itemSlot.type == ItemSlot.Type.Equip)
            {
                ui_status.text = SetStatus(itemSlot.equip);
                equip_button.onClick.AddListener(Use);
            }
            // 소비 아이템이라면
            else if (itemSlot.type == ItemSlot.Type.Consume)
            {
                // 퀵슬롯 여부 확인
                if (Player.instance.inventory.quickSlot == itemSlot.index)
                {
                    quick_button_text.text = "퀵슬롯 해제";
                }
                use_button.onClick.AddListener(Use);
                quick_button.onClick.AddListener(Quick);
            }

            Initialize(item);
        }
    }
    public string SetStatus(EquipItem item)
    {
        string status = "";
        if (item.hp != 0)
        {
            status += ("체력 +" + item.hp.ToString());
        }
        if (item.atk != 0)
        {
            if (item.hp != 0) { status += ", "; }
            status += ("공격력 +" + item.atk.ToString());
        }
        if (item.def != 0)
        {
            if (item.atk != 0) { status += ", "; }
            status += ("방어력 +" + item.def.ToString());
        }
        return status;
    }
    public void Update()
    {
        // ESC 아니면 취소버튼
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyPopup();
        }
    }

    public void Use()
    {
        itemSlot.Use();
        DestroyPopup();
    }

    public void UnEquip()
    {
        if (equipment != null)
        {
            // 아이템 수령 함수가 정상 실행되어야 장착 해제 가능
            if (Player.instance.inventory.EquipItemGain(equipment))
            {
                Player.instance.inventory.SetUnequipItem(equipment);
            }
        }
    }

    public void Quick()
    {
        itemSlot.QuickSlot();
        DestroyPopup();
    }

    void DestroyPopup()
    {
        PlayUI.instance.DestroyObject(this.gameObject, 2);
    }
}
