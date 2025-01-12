using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static SoundManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SoundManager>();
            }
            return m_instance;
        }
    }
    private static SoundManager m_instance;

    // 오디오소스
    public AudioMixer mixer;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // 씬 실행 시 저장된 볼륨 값 세팅
    void Start()
    {
        if (PlayerPrefs.HasKey("Master_Volume"))
        {
            mixer.SetFloat("Master_Volume", PlayerPrefs.GetFloat("Master_Volume"));
        }
        if (PlayerPrefs.HasKey("Music_Volume"))
        {
            musicSource.volume = PlayerPrefs.GetFloat("Music_Volume");
        }
        if (PlayerPrefs.HasKey("SFX_Volume"))
        {
            sfxSource.volume = PlayerPrefs.GetFloat("SFX_Volume");
        }
    }

    public void SetMusic(AudioClip clip)
    {
        // 같은 사운드가 아닐 때만 변경.
        if(musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
    public void AudioSfxPlay(AudioClip clip)
    {
        if(clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            print("CLIP가 없음");
        }
    }
}
