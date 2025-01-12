using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPool : MonoBehaviour
{
    [Header("Pool Healing Data")]
    public float maxHP;
    public float maxSP;

    private float currentHP;
    private float currentSP;

    public AudioClip sound;

    [Header("Animation")]
    public GameObject effect;

    bool activate = false;
    float timer = 0;
    float period = 3f;

    private void Awake()
    {
        currentHP = maxHP;
        currentSP = maxSP;
    }

    private void FixedUpdate()
    {
        if (activate && (currentHP > 0 || currentSP > 0))
        {
            timer += Time.deltaTime;
            if(timer > period)
            {
                SoundManager.instance.AudioSfxPlay(sound);

                float remainHP = Player.instance.Heal(maxHP / 8);
                float remainSP = Player.instance.HealSP(maxSP / 8);

                currentHP -= (maxHP / 10);  // 회복한 양만큼 HP 감소
                currentHP += remainHP; // 단, 최대치 이상 회복했다면 그만큼 회복.

                currentSP -= (maxSP / 10);  // 회복한 양만큼 HP 감소
                currentSP += remainSP; // 단, 최대치 이상 회복했다면 그만큼 회복.

                timer = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG)) {
            
            if(currentHP > 0)
            {
                PlayUI.instance.MessagePopup("샘물의 힘으로 일정 간격으로 HP와 SP를 회복합니다.");
                effect.SetActive(true);
                activate = true;

                timer = 1.5f;
            }
            else
            {
                PlayUI.instance.MessagePopup("샘물의 힘이 모두 소멸했습니다.");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG))
        {
            timer = 0;
            activate = false;
            effect.SetActive(false);
        }
    }
}
