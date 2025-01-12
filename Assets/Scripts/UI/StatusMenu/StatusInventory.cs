using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

// UI 상에서 보여주는 아이템 창 속 아이템
// 아이템 설명을 띄우는 팝업은 StatusMenu.cs에서 다룸
public class StatusInventory : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // 아이템 데이터
    Item item;
    ItemSlot slot;
    float timer = 0;
    bool pressed = false;

    [SerializeField]
    private string data;

    [Header("Prefetch Data")]
    public int index;
    public ItemSlot.Type type;
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI amount;
    public GameObject quick;

    [SerializeField]
    bool set = false;

    void Start()
    {
        if(StatusMenu.instance != null)
        {
            SetItem();
        }
    }

    void Update()
    {
        // 초기 세팅
        if(!set){
            Reset();
            set = true;
        }


        // 수량 조절 함수
        if(slot != null)
        {
            if (slot.type == ItemSlot.Type.Consume)
            {
                amount.text = (slot.amount != 0 ? slot.amount.ToString() : "");
            }
        }
        // 길게 누르기(팝업 띄우기)
        if (pressed)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= Config.LONG_PRESS)
            {
                StatusMenu.instance.ItemPopup(slot);
                pressed = false;
            }
        }
    }
    public void SetItem()
    {
        switch (type)
        {
            case ItemSlot.Type.Equip:
                slot = Player.instance.inventory.equipSlot[index];
                item = slot.equip;
                break;
            case ItemSlot.Type.Consume:
                slot = Player.instance.inventory.consumeSlot[index];
                item = slot.consume;
                break;
            case ItemSlot.Type.Etc:
                slot = Player.instance.inventory.etcSlot[index];
                item = slot.etc;
                break;
        }
    }

    public void Reset()
    {
        SetItem();
        if(slot != null)
        {
            if (slot.amount != 0)
            {
                // 이미지 스프라이트
                image.gameObject.SetActive(true);
                image.sprite = item.itemImage;

                // 버튼 상호작용
                GetComponent<Button>().interactable = true;

                // 소비 아이템
                if (slot.type == ItemSlot.Type.Consume)
                {
                    amount.text = (slot.amount != 0 ? slot.amount.ToString() : "");
                    if (Player.instance.inventory.quickSlot == index)
                    {
                        quick.SetActive(true);
                    }
                    else
                    {
                        quick.SetActive(false);
                    }
                }
            }
            // 수량 0이 됐을 때 이뤄지는 삭제
            else if (slot.amount == 0)
            {
                image.gameObject.SetActive(false);
                GetComponent<Button>().interactable = false;

                quick.SetActive(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // 터치로 일찍 뗐으면
        if (timer < Config.LONG_PRESS - 0.2f)
        {
            slot.Use();
        }
        timer = 0;
        pressed = false;
    }
}
