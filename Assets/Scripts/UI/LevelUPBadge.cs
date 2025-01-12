using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUPBadge : MonoBehaviour
{
    public TextMeshProUGUI level;
    // Start is called before the first frame update
    void Awake()
    {
        name = "LEVEL";
        level.text = Player.instance.level.ToString();
        GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
        StartCoroutine(DelayedDestroy());
    }
    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
