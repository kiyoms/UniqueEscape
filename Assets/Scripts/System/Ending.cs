using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    int width, height;

    // FIRST UI
    [Header("BACK")]
    public RectTransform background;

    [Header("CONTENT")]
    public RectTransform content;

    public float speed;
    public float init;

    private void Awake()
    {
        width = Screen.width;
        height = Screen.width / 16 * 9;

        background.sizeDelta = new Vector2(width, height);
    }
    private void Update()
    {
        if (content.anchoredPosition.y < 1450)
        {
            init += (Time.deltaTime * speed) + 0.1f;
            content.anchoredPosition = new Vector2(0, init * speed);
        }

        else
        {
            StartCoroutine(Title());
        }
    }
    IEnumerator Title()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Title");
    }
}
