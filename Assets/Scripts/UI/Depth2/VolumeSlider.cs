using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class VolumeSlider : MonoBehaviour
{
    public enum Type
    {
        HUD_SIZE, HUD_DISTANCE, MASTER, VOLUME
    };
    public Type type;
    public Slider ui_slider;

    public GameObject ui_popup;
    public TextMeshProUGUI ui_value;

    // Update is called once per frame
    void Update()
    {
        if (type == Type.VOLUME)
        {
            ui_value.text = Mathf.FloorToInt(ui_slider.value * 100).ToString();
        }
        else if(type == Type.MASTER)
        {
            ui_value.text = string.Format("{0:F2}", (ui_slider.value + 80) / 80);
        }
        else if(type == Type.HUD_SIZE)
        {
            ui_value.text = string.Format("{0:F2}", ui_slider.value);
        }
        else if (type == Type.HUD_DISTANCE)
        {
            ui_value.text = string.Format("{0:F2}", (ui_slider.value / 100));
        }
    }
    public void Pressed(bool isPressed)
    {
        ui_popup.SetActive(isPressed);
    }
}
