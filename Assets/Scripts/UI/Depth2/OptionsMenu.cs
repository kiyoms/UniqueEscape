using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Button returnBtn;

    [Header("VOLUME")]
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;
    [Header("HUD")]
    public Slider hudSize;
    public Slider hudPos;
    public GameObject hud;

    void Awake()
    {
        // 현재 볼륨 설정 가져오기
        SoundManager.instance.mixer.GetFloat("Master_Volume", out float masterVol);
        masterVolume.value = masterVol;
        musicVolume.value = SoundManager.instance.musicSource.volume;
        sfxVolume.value = SoundManager.instance.sfxSource.volume;

        // 크기
        if(PlayUI.instance != null)
        {
            hudSize.value = PlayUI.instance.size;
            hudPos.value = PlayUI.instance.distance;
        }
        else
        {
            hudSize.value = TitleManager.instance.size;
            hudPos.value = TitleManager.instance.distance;
        }
       

        // HUD 크기/거리 리스너 추가
        hudSize.onValueChanged.AddListener(HudSizeControl);
        hudPos.onValueChanged.AddListener(HudPositionControl);

        // 마스터 볼륨 리스너 추가
        masterVolume.onValueChanged.AddListener(MasterVolumeControl);
        musicVolume.onValueChanged.AddListener(MusicVolumeControl);
        sfxVolume.onValueChanged.AddListener(SfxVolumeControl);

        // 나가기 버튼
        returnBtn.onClick.AddListener(DestroyObject);

    }
    
    void Update()
    {
        // ESC 키 입력 시 탈출
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyObject();
        }
    }
    // 마스터 볼륨 
    void MasterVolumeControl(float _value)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.mixer.SetFloat("Master_Volume", _value);
            PlayerPrefs.SetFloat("Master_Volume", _value);
            PlayerPrefs.Save();
        }
    }
    // 배경음 볼륨
    void MusicVolumeControl(float _value)
    {
        if (SoundManager.instance != null)
        {
            print(_value);
            SoundManager.instance.musicSource.volume = _value;
            PlayerPrefs.SetFloat("Music_Volume", musicVolume.value);
            PlayerPrefs.Save();
        }
    }
    // SFX 볼륨
    void SfxVolumeControl(float _value)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.sfxSource.volume = _value;
            PlayerPrefs.SetFloat("SFX_Volume", sfxVolume.value);
            PlayerPrefs.Save();
        }
    }
    void HudSizeControl(float _value)
    {
        PlayerPrefs.SetFloat(Config.HUD_SIZE, _value);
        PlayerPrefs.Save();
        if (PlayUI.instance != null)
        {
            PlayUI.instance.size = _value;
        }
        if (TitleManager.instance != null)
        {
            TitleManager.instance.size = _value;
        }
    }
    void HudPositionControl(float _value)
    {
        PlayerPrefs.SetFloat(Config.HUD_DISTANCE, _value);
        PlayerPrefs.Save();
        if (PlayUI.instance != null)
        {
            PlayUI.instance.distance = _value;
        }
        if (TitleManager.instance != null)
        {
            TitleManager.instance.distance = _value;
        }
    }
    // 패널 파괴
    private void DestroyObject()
    {
        // 타이틀 화면에서 띄웠다면
        if (TitleManager.instance != null)
        {
            TitleManager.instance.DestroyPanel(this.gameObject);
        }
        else
        {
            PlayUI.instance.DestroyObject(this.gameObject, 1);
        }
    }

    public void HUDView(bool isView)
    {
        hud.SetActive(isView);
    }

    public void Reset()
    {
        if (PlayUI.instance != null)
        {
            PlayUI.instance.size = Config.HUD_SIZE_DEFAULT;
            PlayUI.instance.distance = Config.HUD_DISTANCE_DEFAULT;
        }
        else
        {
            TitleManager.instance.size = Config.HUD_SIZE_DEFAULT;
            TitleManager.instance.distance = Config.HUD_DISTANCE_DEFAULT;
        }

        hudSize.value = Config.HUD_SIZE_DEFAULT;
        hudPos.value = Config.HUD_DISTANCE_DEFAULT;

        PlayerPrefs.SetFloat(Config.HUD_SIZE, Config.HUD_SIZE_DEFAULT);
        PlayerPrefs.SetFloat(Config.HUD_DISTANCE, Config.HUD_DISTANCE_DEFAULT);
        PlayerPrefs.Save();
    }
}
