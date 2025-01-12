using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarning : MonoBehaviour
{
    public float weight = 0;
    public float speed = 2.5f;
    public RectTransform warning1;
    public RectTransform warning2;

    private void Start()
    {
        GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
        StartCoroutine(DelayedDestroy());
    }
    void Update()
    {
        weight += Time.deltaTime * speed;
        warning1.position = new Vector2(warning1.position.x + weight, warning1.position.y);
        warning2.position = new Vector2(warning2.position.x - weight, warning2.position.y);
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
