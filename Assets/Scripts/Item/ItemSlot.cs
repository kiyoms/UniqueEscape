using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 슬롯 클래스
public class ItemSlot : MonoBehaviour
{
    // 초기 세팅
    public enum Type
    {
        Equip, Consume, Etc
    };

    public Type type { get; private set; }
    public EquipItem equip { get; private set; }
    public ConsumeItem consume { get; private set; }
    public Item etc { get; private set; }
    public int amount { get; private set; }
    public int index { get; private set; }

    // 초기설정
    public void Initialize()
    {
        equip = null;
        consume = null;
        etc = null;
        amount = 0;
    }
    public void Initialize(int index, string type)
    {
        this.index = index; // 색인번호
        switch (type)
        {
            case "Equip":
                this.type = Type.Equip;
                break;
            case "Consume":
                this.type = Type.Consume;
                break;
            case "Etc":
                this.type = Type.Etc;
                break;
        }
        Initialize();
    }
    // 인벤토리에 아이템 추가
    public void AddItem(EquipItem equip)
    {
        if (type == Type.Equip)
        {
            this.equip = equip;
            amount = 1;
        }
    }
    public void AddItem(ConsumeItem consume, int amount)
    {
        if (type == Type.Consume)
        {
            this.consume = consume;
            this.amount = amount;
        }
    }
    public void AddItem(Item etc)
    {
        if (type == Type.Etc)
        {
            this.etc = etc;
            amount = 1;
        }
    }
    // 외부로 인한 인벤토리 아이템 수량 조절
    public void SetAmount(int value)
    {
        amount = value;
        if (amount == 0)
        {
            Initialize();
        }
    }
    // 아이템 사용
    public bool Use()
    {
        // 장비 아이템 장착 함수
        if (equip != null)
        {
            // 장착 명령이 0(신규)라면 아이템 삭제
            if (Player.instance.inventory.SetEquipItem(index, equip) == 0)
            {
                SetAmount(0);
                Player.instance.inventory.Reorder(type);

                return false;
            }
        }
        // 소비 아이템 사용
        else if (consume != null)
        {
            // 아이템 사용 결과에 따른 값 변경
            if (consume.Use())
            {
                amount--;
                // 전부 사용했다면
                if (amount == 0)
                {
                    // 퀵슬롯에서 사용했다면
                    if(Player.instance.inventory.quickSlot == index)
                    {
                        Player.instance.inventory.QuickSlot(-1);
                    }
                    
                    SetAmount(0);
                    Player.instance.inventory.Reorder(type);
                    return false;
                }
            }
        }
        return true;
    }
    public void QuickSlot()
    {
        Player.instance.inventory.QuickSlot(index);
    }
}
