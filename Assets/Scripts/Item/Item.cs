using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data")]
public class Item : ScriptableObject
{
    public string code;
    public string itemName;
    public string description;
    public Sprite itemImage;
    public int itemPrice;
}
