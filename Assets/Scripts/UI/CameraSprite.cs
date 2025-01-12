using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSprite : MonoBehaviour
{
    public SpriteRenderer srr;

    public Color day;
    public Color night;

    public float oneDay;
    public float currentTime;

    [Range(0.01f, 1f)]
    public float transitionTime;

    bool isSwap = false;

    private void Awake()
    {
        float spritex = srr.sprite.bounds.size.x;
        float spritey = srr.sprite.bounds.size.y;

        float screenY = Camera.main.orthographicSize * 2;
        float screenX = screenY / Screen.height * Screen.width;

        transform.localScale = new Vector2(Mathf.Ceil(screenX / spritex), Mathf.Ceil(screenY / spritey));

        srr.color = day;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= oneDay)
            currentTime = 0;

        if (!isSwap)
        {
            if (Mathf.FloorToInt(oneDay * 0.4f) == Mathf.FloorToInt(currentTime))
            {
                //day - night
                isSwap = true;
                StartCoroutine(SwapColor(srr.color, night));
                GameManager.instance.night = true;
            }
            else if (Mathf.FloorToInt(oneDay * 0.9f) == Mathf.FloorToInt(currentTime))
            {
                //night - day
                isSwap = true;
                StartCoroutine(SwapColor(srr.color, day));
                GameManager.instance.night = false;
            }
        }

        IEnumerator SwapColor(Color start, Color end)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * (1 / (transitionTime * oneDay));
                srr.color = Color.Lerp(start, end, t);
                yield return null;
            }
            isSwap = false;
        }
    }
}
