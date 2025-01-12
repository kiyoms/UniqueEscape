using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossArea : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG))
        {
            GetComponentInParent<FinalBoss>().area = true;
        }        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG))
        {
            GetComponentInParent<FinalBoss>().area = false;
        }
    }
}
