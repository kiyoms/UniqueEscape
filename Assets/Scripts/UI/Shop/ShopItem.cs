using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopItem : MonoBehaviour
{
    // 아이템 객체
    ConsumeItem consume_item;
    // 아이템 정보
    public TextMeshProUGUI ui_name;
    public TextMeshProUGUI ui_description;
    public Image ui_image;
    public TextMeshProUGUI ui_price;
    // 구매 팝업
    public GameObject buy_popup;

    // 아이템 구매
    public void Set(ConsumeItem item)
    {
        consume_item = item;

        ui_name.text = item.itemName;
        ui_description.text = item.description;
        ui_image.sprite = item.itemImage;
        ui_price.text = string.Format("{0:n0}", item.itemPrice);
    }

    public void Buy()
    {
        GameObject panel = Instantiate(buy_popup, PlayUI.instance.transform);
        panel.GetComponent<BuyPopup>().Initialize(consume_item);
    }
}
