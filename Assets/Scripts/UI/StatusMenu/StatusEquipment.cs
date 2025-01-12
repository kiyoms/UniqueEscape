using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 장비창에서의 장비 아이템
public class StatusEquipment : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Button button;
    EquipItem equipment;
    bool pressed = false;
    public float timer = 0;
    
    public EquipItem.Type type;
    public Image image;
    public GameObject unselected;

    bool set = false;

    void Awake()
    {
        button = GetComponent<Button>();
    }
    void Update()
    {
        if (!set)
        {
            Reset();
            set = true;
        }

        if (equipment != null && pressed)
        {
            timer += Time.unscaledDeltaTime;
            if(timer >= Config.LONG_PRESS)
            {
                StatusMenu.instance.ItemPopup(equipment);
                pressed = false;
            }
        }
    }

    void SetItem()
    {
        // 들어 있는 아이템 세팅
        switch (type)
        {
            case EquipItem.Type.Helmat:
                equipment = Player.instance.inventory.equiped.helmat;
                break;
            case EquipItem.Type.Armor:
                equipment = Player.instance.inventory.equiped.armor;
                break;
            case EquipItem.Type.Weapon:
                equipment = Player.instance.inventory.equiped.weapon;
                break;
            case EquipItem.Type.Glove:
                equipment = Player.instance.inventory.equiped.glove;
                break;
            case EquipItem.Type.Boots:
                equipment = Player.instance.inventory.equiped.boots;
                break;
        }
    }

    public void Reset()
    {
        SetItem();
        if (equipment != null)
        {
            image.sprite = equipment.itemImage;

            button.interactable = true;
            image.gameObject.SetActive(true);
            unselected.SetActive(false);
        }
        else
        {
            button.interactable = false;

            image.gameObject.SetActive(false);
            unselected.SetActive(true);
        }
    }

    // 장착 해제
    void Unequip()
    {
        if(equipment != null)
        {
            print("장착 해제 시도");
                
            // 아이템 수령 함수가 정상 실행되어야 장착 해제 가능
            if (Player.instance.inventory.EquipItemGain(equipment))
            {
                Player.instance.inventory.SetUnequipItem(equipment);
            }
            StatusMenu.instance.ResetInventory();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // 터치로 일찍 뗗다
        if(timer <= Config.LONG_PRESS - 0.2f)
        {
            Unequip();
        }
        timer = 0;
        pressed = false;
    }
}
