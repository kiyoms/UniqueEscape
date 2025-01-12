using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 클래스
// 이곳에선 아이템 추가와 삭제를 해준다.
public class Inventory : MonoBehaviour
{
    public int maxSlot = 12;

    public struct Equipment
    {
        public EquipItem helmat;
        public EquipItem armor;
        public EquipItem weapon;
        public EquipItem boots;
        public EquipItem glove;
    };
    public Equipment equiped;

    public ItemSlot[] equipSlot { get; private set; }
    public int equip_size = 0;
    public ItemSlot[] consumeSlot { get; private set; }
    public int consume_size = 0;
    public ItemSlot[] etcSlot { get; private set; }
    public int etc_size = 0;
    public int quickSlot { get; private set; }

    // 슬롯 설정
    void Awake()
    {
        // 퀵 슬롯
        quickSlot = -1;

        // 일반 아이템 슬롯
        equipSlot = new ItemSlot[maxSlot];
        consumeSlot = new ItemSlot[maxSlot];
        etcSlot = new ItemSlot[maxSlot];

        // 아이템 슬롯 세팅
        for (int i = 0; i < maxSlot; i++)
        {
            equipSlot[i] = gameObject.AddComponent<ItemSlot>();
            consumeSlot[i] = gameObject.AddComponent<ItemSlot>();
            etcSlot[i] = gameObject.AddComponent<ItemSlot>();
            equipSlot[i].Initialize(i, "Equip");
            consumeSlot[i].Initialize(i, "Consume");
            etcSlot[i].Initialize(i, "Etc");
        }
    }

    // [아이템 획득]
    // 장비/기타템 획득 함수, 칸마다 1개씩 차지하므로
    public bool EquipItemGain(EquipItem equip)
    {
        // 장비 창이 꽉 차지 않으면
        if (equip_size != maxSlot)
        {
            equipSlot[equip_size++].AddItem(equip);
            print((equip_size - 1) + "에 장비 아이템 획득");
            return true;
        }
        else
        {
            PlayUI.instance.ErrorMessagePopup("장비 아이템 창이 꽉 찼습니다.");
            return false;
        }
    }
    // 소비템 획득 함수
    public bool ConsumeItemGain(ConsumeItem consume, int amount)
    {
        int i = 0;

        // 아이템 창이 꽉 차지 않았다면
        if (consume_size != maxSlot)
        {
            while (i < consume_size)
            {
                // 아이템이 이미 있다면 기존 수량에서 추가.
                if (consumeSlot[i].consume == consume)
                {
                    consumeSlot[i].SetAmount(consumeSlot[i].amount + amount);
                    return true;
                }
                i++;
            }
            // 아이템이 없다면 슬롯을 소비하여 추가
            consumeSlot[i].AddItem(consume, amount);
            consume_size++;
            return true;
        }
        else
        {
            PlayUI.instance.ErrorMessagePopup("소비 아이템 창이 꽉 찼습니다.");
            return false;
        }
    }


    public void ItemSave()
    {
        string equip = "";

        // 장착한 아이템 정보를 기록
        equip += (equiped.helmat != null ? equiped.helmat.code + "," : "null,");
        equip += (equiped.armor != null ? equiped.armor.code + "," : "null,");
        equip += (equiped.boots != null ? equiped.boots.code + "," : "null,");
        equip += (equiped.weapon != null ? equiped.weapon.code + "," : "null,");
        equip += (equiped.glove != null ? equiped.glove.code + "," : "null,");

        // 인벤토리 아이템 정보 기록
        string equip_slot = equip_size + "|";
        for(int index = 0; index < equip_size; index++)
        {
            equip_slot += equipSlot[index].equip.code + ",";
        }

        string consume_slot = consume_size + "|";
        for (int index = 0; index < consume_size; index++)
        {
            consume_slot += consumeSlot[index].consume.code + "/" + consumeSlot[index].amount + ",";
        }

        // 기록한 정보를 모두 기록
        PlayerPrefs.SetString(PlayerStatus.EQUIPED_SLOT, equip);
        PlayerPrefs.SetString(PlayerStatus.ITEM_SLOT_EQUIP, equip_slot);
        PlayerPrefs.SetString(PlayerStatus.ITEM_SLOT_CONSUME, consume_slot);
        PlayerPrefs.SetInt(PlayerStatus.ITEM_SLOT_QUICK, quickSlot);
    }


    public void ItemLoad()
    {
        string temp = PlayerPrefs.GetString(PlayerStatus.EQUIPED_SLOT);
        string[] temp_slot = temp.Split(',');

        print(temp);

        // 장착한 아이템 정보 불러오기
        equiped.helmat = (EquipItem)ItemManager.instance.GetItemWithCode("EquipItem", temp_slot[0]);
        equiped.armor = (EquipItem)ItemManager.instance.GetItemWithCode("EquipItem", temp_slot[1]);
        equiped.boots = (EquipItem)ItemManager.instance.GetItemWithCode("EquipItem", temp_slot[2]);
        equiped.weapon = (EquipItem)ItemManager.instance.GetItemWithCode("EquipItem", temp_slot[3]);
        equiped.glove = (EquipItem)ItemManager.instance.GetItemWithCode("EquipItem", temp_slot[4]);

        // 장비 아이템
        temp = PlayerPrefs.GetString(PlayerStatus.ITEM_SLOT_EQUIP);
        string[] temp_array = temp.Split('|');

        // 슬롯 크기와 아이템 유형 추출
        equip_size = int.Parse(temp_array[0]);
        temp_slot = temp_array[1].Split(',');

        for(int index = 0; index < equip_size; index++)
        {
            equipSlot[index].AddItem((EquipItem)ItemManager.instance.GetItemWithCode("EquipItem", temp_slot[index]));
        }

        // 소비 아이템
        temp = PlayerPrefs.GetString(PlayerStatus.ITEM_SLOT_CONSUME);
        temp_array = temp.Split('|');

        // 슬롯 크기와 아이템 유형 추출
        consume_size = int.Parse(temp_array[0]);
        temp_slot = temp_array[1].Split(',');
        
        for (int index = 0; index < consume_size; index++)
        {
            string[] item_data = temp_slot[index].Split('/');
            consumeSlot[index].AddItem((ConsumeItem)ItemManager.instance.GetItemWithCode("ConsumeItem", item_data[0]), int.Parse(item_data[1]));
        }
        
        // 퀵 슬롯 가져오기
        quickSlot = PlayerPrefs.GetInt(PlayerStatus.ITEM_SLOT_QUICK);
    }


    // 퀵 슬롯 등록함수
    // 같은 값으로 등록을 시도하면 퀵슬롯 해제
    public void QuickSlot(int index)
    {
        if(index == quickSlot)
        {
            quickSlot = -1;
        }
        else
        {
            quickSlot = index;
        }
    }

    // 아이템 스위칭 함수
    public bool SwitchEquipItem(int index, EquipItem before)
    {
        // 장착한 아이템을 해제 후 기존에 장착한 위치로 변경
        equipSlot[index].AddItem(before);
        before.UnEquip();

        return true;
    }

    // 장비 아이템 장착/해제 명령에 따른 아이템 구조체 수정
    // index는 장착 명령이 실행된 아이템 칸
    public int SetEquipItem(int index, EquipItem equip)
    {
        bool flag = false;  // 장착한 아이템의 교체 여부

        // 장착한 아이템이 있는 지 확인
        switch (equip.type)
        {
            case EquipItem.Type.Helmat:
                if (equiped.helmat != null)
                {
                    if(SwitchEquipItem(index, equiped.helmat))
                    {
                        equiped.helmat = equip;
                    }
                    flag = true;
                }
                break;
            case EquipItem.Type.Armor:
                if (equiped.armor != null)
                {
                    if (SwitchEquipItem(index, equiped.armor))
                    {
                        equiped.armor = equip;
                    }
                    flag = true;
                }
                break;
            case EquipItem.Type.Weapon:
                if (equiped.weapon != null)
                {
                    if (SwitchEquipItem(index, equiped.weapon))
                    {
                        equiped.weapon = equip;
                    }
                    flag = true;
                }
                break;
            case EquipItem.Type.Boots:
                if (equiped.boots != null)
                {
                    if (SwitchEquipItem(index, equiped.boots))
                    {
                        equiped.boots = equip;
                    }
                    flag = true;
                }
                break;
        }
        if (flag)
        {
            equip.Equip();
            StatusMenu.instance.ResetInventory();
            return 1;   // 아이템 교체
        }
        // 없으면 새 아이템 장착
        else
        {
            // 새 아이템 장착
            switch (equip.type)
            {
                case EquipItem.Type.Helmat:
                    equiped.helmat = equip;
                    break;
                case EquipItem.Type.Armor:
                    equiped.armor = equip;
                    break;
                case EquipItem.Type.Weapon:
                    equiped.weapon = equip;
                    break;
                case EquipItem.Type.Boots:
                    equiped.boots = equip;
                    break;
            }
            equip.Equip();
            StatusMenu.instance.ResetInventory();
            return 0;   // 아이템 신규
        }
    }
    public bool SetUnequipItem(EquipItem equip)
    {
        switch (equip.type)
        {
            case EquipItem.Type.Helmat:
                equiped.helmat = null;
                break;
            case EquipItem.Type.Armor:
                equiped.armor = null;
                break;
            case EquipItem.Type.Weapon:
                equiped.weapon = null;
                break;
        }
        print("장착 해제");
        return equip.UnEquip(); // 능력치 조정
        StatusMenu.instance.ResetInventory();
    }
    // 아이템이 삭제될 때 발생하는 정렬함수
    public void Reorder(ItemSlot.Type type)
    {
        // 삭제위치 찾기
        int index = 0;
        int maxIndex = 0;
        // 아이템이 삭제된 위치 찾기

        StatusMenu.instance.ResetInventory();

        switch (type)
        {
            case ItemSlot.Type.Equip:
                for (; index < equip_size - 1; index++)
                {
                    if (equipSlot[index].amount == 0)
                    {
                        break;
                    }
                }
                maxIndex = equip_size - 1;
                break;
            case ItemSlot.Type.Consume:
                for (; index < consume_size - 1; index++)
                {
                    if (consumeSlot[index].amount == 0)
                    {
                        break;
                    }
                }
                maxIndex = consume_size - 1;
                break;
            case ItemSlot.Type.Etc:
                for (; index < etc_size - 1; index++)
                {
                    if (etcSlot[index].amount == 0)
                    {
                        break;
                    }
                }
                maxIndex = etc_size - 1;
                break;
        }
        // 아이템이 삭제된 기준으로 정렬
        while (index < maxIndex)
        {
            switch (type)
            {
                case ItemSlot.Type.Equip:
                    if (equipSlot[index + 1].amount == 0)
                    {
                        break;
                    }
                    equipSlot[index].AddItem(equipSlot[index + 1].equip);
                    break;
                case ItemSlot.Type.Consume:
                    if (consumeSlot[index + 1].amount == 0)
                    {
                        break;
                    }
                    consumeSlot[index].AddItem(consumeSlot[index + 1].consume, consumeSlot[index + 1].amount);
                    break;
                case ItemSlot.Type.Etc:
                    if (etcSlot[index + 1].amount == 0)
                    {
                        break;
                    }
                    etcSlot[index].AddItem(etcSlot[index + 1].etc);
                    break;
            }
            index++;
        }
        // 마지막 정렬
        if (index == maxIndex)
        {
            switch (type)
            {
                case ItemSlot.Type.Equip:
                    equipSlot[index].Initialize();
                    equip_size--;
                    break;
                case ItemSlot.Type.Consume:
                    consumeSlot[index].Initialize();
                    consume_size--;
                    break;
                case ItemSlot.Type.Etc:
                    etcSlot[index].Initialize();
                    etc_size--;
                    break;
            }
        }
        StatusMenu.instance.ResetInventory();
    }
}