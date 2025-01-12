using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickSlot : MonoBehaviour
{
    // 준비용 변수
    int quickSlotIndex;
    bool set = false;
    // 롱클릭 변수
    bool pressed = false;
    float timer = 0;
    private ItemSlot slot;

    [Header(Inspector.UI_COMPONENT)]
    public GameObject ui_unselected;
    public GameObject ui_selected;
    public Image ui_image;
    public TextMeshProUGUI ui_amount;

    void Update()
    {
        if(Player.instance != null)
        {
            SetIndex();
            if (quickSlotIndex != -1 && !set)
            {
                slot = Player.instance.inventory.consumeSlot[quickSlotIndex];
                ui_unselected.gameObject.SetActive(false);
                ui_selected.gameObject.SetActive(true);
                ui_image.sprite = slot.consume.itemImage;
                set = true;
            }
            else if (quickSlotIndex == -1 && set)
            {
                ui_unselected.gameObject.SetActive(true);
                ui_selected.gameObject.SetActive(false);
                set = false;
            }

            if (quickSlotIndex != -1)
            {
                ui_amount.text = slot.amount.ToString();
            }

            // 롱클릭 변수
            if (pressed)
            {
                timer += Time.deltaTime;
                if (timer > Config.LONG_PRESS)
                {
                    PlayUI.instance.OpenPause(true);
                    PlayUI.instance.OpenStatus(1);
                    PlayUI.instance.MessagePopup("소비 아이템을 길게 누르면\n퀵슬롯에 아이템을 등록할 수 있습니다.");
                    pressed = false;
                    timer = 0;
                }

            }
        }
    }
    void SetIndex()
    {
        // 아이템 변경이 일어나면
        if (set)
        {
            if(Player.instance.inventory.quickSlot != -1)
            {
                if (quickSlotIndex != Player.instance.inventory.quickSlot)
                {
                    set = false;
                }
            }
            quickSlotIndex = Player.instance.inventory.quickSlot;
        }
        else
        {
            quickSlotIndex = Player.instance.inventory.quickSlot;
        }
    }
    public void Use()
    {
        if(quickSlotIndex != -1)
        {
            slot.Use();
        }
        else
        {
            PlayUI.instance.MessagePopup("아이템창에서 아이템을 등록해주세요.\n버튼을 길게 누르면 창을 열 수 있습니다.");
        }
    }
    public void OnPressed(bool isPressed)
    {
        if (isPressed)
        {
            pressed = true;
        }
        else
        {
            pressed = false;
            timer = 0;
        }
    }
}
