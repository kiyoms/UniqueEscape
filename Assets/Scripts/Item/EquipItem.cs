using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 소비 아이템
[CreateAssetMenu(fileName = "Equipment Item Data", menuName = "Scriptable Object/Equipment Item Data")]

public class EquipItem : Item
{
    public enum Type // 장비유형
    {
        Helmat, Armor, Weapon, Boots, Glove
    };
    public Type type;  // 장비유형
    public float hp;    // 체력
    public float atk;   // 공격력
    public float def;   // 방어력

    [Header("SOUND")]
    public AudioClip equip_sound;
    public AudioClip unequip_sound;

    // 장착/해제 시 능력치만 조정.
    // 장착 시 능력치 상승
    public bool Equip()
    {
        Player.instance.maxHP += hp;
        Player.instance.atk += atk;
        Player.instance.def += def;
        
        PlayUI.instance.MessagePopup("아이템을 장착했습니다.");
        SoundManager.instance.AudioSfxPlay(equip_sound);
        StatusMenu.instance.ResetInventory();
        return true;
    }
    // 해제 시 능력치 하락
    public bool UnEquip()
    {
        Player.instance.maxHP -= hp;
        Player.instance.atk -= atk;
        Player.instance.def -= def;

        PlayUI.instance.MessagePopup("장착을 해제했습니다.");
        SoundManager.instance.AudioSfxPlay(unequip_sound);
        StatusMenu.instance.ResetInventory();
        return true;
    }
}
