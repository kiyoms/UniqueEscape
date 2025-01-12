using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static ItemManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ItemManager>();
            }

            return m_instance;
        }
    }
    private static ItemManager m_instance;

    public ConsumeItem[] shopItemList;

    public EquipItem[] equip_data;
    public ConsumeItem[] consume_data;
    public Item[] etc_data;

    public Object GetItemWithCode(string type, string code)
    {
        if (code.Equals("null")){
            return null;
        }
        switch (type)
        {
            case "EquipItem":
                foreach(EquipItem item in equip_data)
                {
                    if (item.code.Equals(code))
                    {
                        return item;
                    }
                }
                break;
            case "ConsumeItem":
                foreach (ConsumeItem item in consume_data)
                {
                    if (item.code.Equals(code))
                    {
                        return item;
                    }
                }
                break;
            case "Item":
                foreach (Item item in etc_data)
                {
                    if (item.code.Equals(code))
                    {
                        return item;
                    }
                }
                break;
        }
        return null;
    }
}