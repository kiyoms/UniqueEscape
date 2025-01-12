using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Shop : MonoBehaviour
{
    [Header("Top Area")]
    public Button button_close;
    public TextMeshProUGUI ui_price;
    [Header("Shop Area")]
    // 상품 Prefab
    public Transform list_group;
    public GameObject list_object;
    public GameObject ui_nonetext;

    ConsumeItem[] shoplist;
    // Start is called before the first frame update
    void Start()
    {
        // 뒤로 가기 버튼 추가
        button_close.onClick.AddListener(() => DestroyObject());

        // 상품 목록 갱신
        shoplist = ItemManager.instance.shopItemList;

        // 상품이 아무것도 없다면 준비 중이라 표기.
        if(shoplist.Length == 0)
        {
            ui_nonetext.SetActive(true);
            list_group.parent.GetComponent<ScrollRect>().enabled = false;
        }
        // 상품 목록 트랜스폼
        for(int i = 0; i < shoplist.Length; i++)
        {
            GameObject obj = Instantiate(list_object, list_group);
            obj.GetComponent<ShopItem>().Set(shoplist[i]);
        }
    }
    void Update()
    {
        // 뒤로 가기 버튼
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyObject();
        }
        // 플레이어 재화 갱신
        ui_price.text = string.Format("{0:n0}", Player.instance.money);
    }

    void DestroyObject()
    {
        PlayUI.instance.DestroyObject(this.gameObject, 0);
    }
}
