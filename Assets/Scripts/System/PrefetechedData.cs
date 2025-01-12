using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefetechedData : MonoBehaviour
{
    [System.Serializable]
    public struct TOWN_DATA
    {
        public int code;
        public string title;
        public Transform townPositon;
        public Sprite townImage;
        public AudioClip townAudio;
    }
    public List<TOWN_DATA> townData;

    // 던전 정보
    public List<DungeonData> dungeonData;
}
