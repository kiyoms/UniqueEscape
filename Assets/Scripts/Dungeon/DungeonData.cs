using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 던전에 사용할 데이터
// 던전 내부에 사용하는 상세는 prefab에 들어 있음

[CreateAssetMenu(fileName = "Dungeon Data", menuName = "Scriptable Object/Dungeon Data")]
public class DungeonData : ScriptableObject
{
    public int code; // 시작 위치 코드
    public string title; // 던전이름
    public Sprite sprite;   // 이미지

    public Transform warp; // 워프위치
    public GameObject data; // 던전 데이터 (쌩 prefab)
}
