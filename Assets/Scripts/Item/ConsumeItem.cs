using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 소비 아이템
[CreateAssetMenu(fileName = "Consume Item Data", menuName = "Scriptable Object/Consume Item Data")]

public class ConsumeItem : Item
{
    public float heal; // 회복량 (% 비율: 0.3이면 30%)
    public AudioClip sound;

    public bool Use()
    {
        Player player;
        if(Player.instance != null)
        {
            player = Player.instance;
            if (player.HP == player.maxHP)
            {
                PlayUI.instance.MessagePopup("이미 체력이 가득 찼습니다.");
                return false;
            }
            else
            {
                // HP 비례 회복량 계산
                float healing = (Player.instance.maxHP * heal / 100);
                SoundManager.instance.AudioSfxPlay(sound);
                Player.instance.Heal(healing);
                StatusMenu.instance.ResetInventory();
                return true;
            }
        }
        return false;
    }
}
