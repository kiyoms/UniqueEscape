using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [Header("HEAL")]
    public GameObject heal;
    public AudioClip heal_sound;

    [Header("HEAL")]
    public GameObject damaged;

    public void Heal()
    {
        SoundManager.instance.AudioSfxPlay(heal_sound);
        StartCoroutine(RemoveEffect(Instantiate(heal, transform)));
    }

    IEnumerator RemoveEffect(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        Destroy(obj);
    }
}
