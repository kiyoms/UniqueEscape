using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePopup : MonoBehaviour
{
    public AudioClip clip;
    [Header("UI COMPONENT")]
    public TextMeshProUGUI popup_message;

    public void Popup(string message)
    {
        SoundManager.instance.AudioSfxPlay(clip);

        // 메시지 길이를 위한 변수 측정
        int flag = 0;
        int extendLine = -1;

        // 메시지 길이 측정
        do
        {
            extendLine++;
            flag = message.IndexOf('\n', flag + 1);
        } while (flag != -1 && extendLine < 5);
        GetComponent<RectTransform>().sizeDelta = new Vector2(1200, 100 + (45 * extendLine));

        // 메시지 띄우기
        popup_message.text = message;
        GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);

        // 기존 메시지는 전부 파괴
        GameObject[] popup = GameObject.FindGameObjectsWithTag(Config.MESSAGE_TAG);
        for (int index = 0; index < popup.Length - 1; index++)
        {
            Destroy(popup[index]);
        }

        // 파괴 코루틴
        StartCoroutine(PopupMessage());
    }

    IEnumerator PopupMessage()
    { 
        yield return new WaitForSecondsRealtime(2f);
        Destroy(this.gameObject);
    }

}
