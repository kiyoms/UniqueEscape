using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Player player;

    // 컨트롤러
    [Header("LEFT:MOVE CONTROLLER")]
    public RectTransform lever; // 스틱 레버
    public RectTransform focus;    // 포커스
    public RectTransform controller;    // 컨트롤러 위치
    private float leverRange = 85f;   // 스틱 범위

    // 우측 컨트롤러
    [Header("RIGHT:ATTACK CONTROLLER")]
    public Button attackBtn;

    private void Awake()
    {
        if(Player.instance != null)
        {
            player = Player.instance;
        }
        
        attackBtn.onClick.AddListener(AttackControl);
        focus.gameObject.SetActive(false);
        lever.anchoredPosition = Vector2.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MoveControl(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveControl(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 이동 취소
        if (player != null)
        {
            player.GetComponent<Moving>().VirtualMove();
        }
        // 버튼 색상 위치/색상 조정
        lever.anchoredPosition = Vector2.zero;
        lever.GetComponent<Image>().color = Color.white;

        // 이동 포커스 위치/색상 조정
        focus.rotation = Quaternion.Euler(0, 0, 0);
        focus.gameObject.SetActive(false);   
    }

    public void MoveControl(PointerEventData eventData)
    {
        var inputDir = eventData.position - controller.anchoredPosition;
        var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;
        var vector = clampedDir.normalized;

        if (player != null)
        {
            player.GetComponent<Moving>().VirtualMove(vector);
        }

        // 레버 색상 조정
        lever.GetComponent<Image>().color = Color.gray;

        // 이동 포커스 위치 조정
        focus.gameObject.SetActive(true);
        focus.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg - 45);
    }
    public void AttackControl()
    {
        if (player != null)
        {
            player.GetComponent<Attack>().PlayerAttack();
        }
    }
}
