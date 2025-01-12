using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public EquipItem item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.instance.inventory.EquipItemGain(item);
    }
    
}
