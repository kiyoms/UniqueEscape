using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class BuyPopup : MonoBehaviour
{
    [Header(Inspector.UI_TOP)]
    public Image image;
    public TextMeshProUGUI ui_name;

    [Header(Inspector.UI_COMPONENT)]
    public TextMeshProUGUI ui_price;
    public TextMeshProUGUI ui_totalPrice;
    public TMP_InputField ui_value;

    // 아이템 수량 및 가격
    ConsumeItem consume_item;
    int value = 1;
    int price;
    int totalPrice;

    [Header("SOUND")]
    public AudioClip purchase;
    bool pressed = false;
    bool plusPressed = false;
    float timer = 0;

    // 아이템 가져오기
    public bool Initialize(ConsumeItem item)
    {
        if (item != null)
        {
            this.consume_item = item;
            price = item.itemPrice;

            image.sprite = item.itemImage;
            ui_name.text = item.itemName;
            ui_price.text = string.Format("{0:n0}", price);

            SetPrice();
            GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
        }
        return false;
    }
    void Update()
    {
        // ESC 아니면 취소버튼
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyObject();
        }
        SetPrice();
        if (pressed)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= Config.LONG_PRESS)
            {
                if ((timer * 100) % 5 < 1)
                {
                    if (plusPressed)
                    {
                        Plus();
                    }
                    else
                    {
                        Minus();
                    }
                }

            }
        }
    }
    void SetPrice()
    {
        ui_value.text = value.ToString();
        totalPrice = price * value;
        ui_totalPrice.text = string.Format("{0:n0}", totalPrice);
        if(totalPrice > Player.instance.money)
        {
            ui_totalPrice.color = Color.red;
        }
        else
        {
            ui_totalPrice.color = Color.white;
        }
    }

    // 버튼 클릭
    public void Plus()
    {
        value++;
    }
    public void Minus()
    {
        if (value > 1)
        {
            value--;
        }
        else
        {
            PlayUI.instance.ErrorMessagePopup("1개 미만으로 설정할 수 없습니다.");
        }
    }
    public void Buy()
    {
        // 플레이어 금액이 총 구매액 이상어아야 함
        if (totalPrice <= Player.instance.money)
        {
            // 구매할 아이템이 비어 있지 않아야 함
            if (consume_item != null)
            {
                // 아이템창에 들어가면 가격에서 차감
                if (Player.instance.inventory.ConsumeItemGain(consume_item, value))
                {
                    SoundManager.instance.AudioSfxPlay(purchase);
                    Player.instance.money -= totalPrice;
                    PlayUI.instance.MessagePopup(ui_name.text + " " + value + "개를 구매했습니다.");
                }
            }
            DestroyObject();
        }
        else
        {
            PlayUI.instance.ErrorMessagePopup("구매에 필요한 금액이 부족합니다.");
        }

    }
    public void TypeSwitch(bool type)
    {
        plusPressed = type;
    }
    public void Pressed(bool press)
    {
        if (press)
        {
            pressed = true;
        }
        else
        {
            timer = 0;
            pressed = false;
        }
    }
    public void Reset()
    {
        value = 1;
    }
    void DestroyObject()
    {
        PlayUI.instance.DestroyObject(this.gameObject, 0.5f);
    }

}
