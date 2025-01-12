using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public enum State
    {
        MONEY,DAMAGE
    };
    public State state;
    bool talkable = false;

    public int money;
    public int damage;

    // Start is called before the first frame update
    void Awake()
    {
        int random = Random.Range(0, 10);
        if(random <= 5)
        {
            state = State.MONEY;
            money = Player.instance.level * Random.Range(70, 120);
        }
        else
        {
            state = State.DAMAGE;
            damage = Mathf.FloorToInt(Player.instance.level * Random.Range(3f, 5f));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG))
        {
            talkable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Config.PLAYER_TAG))
        {
            talkable = false;
        }
    }
    public void Clicked()
    {
        if (talkable)
        {
            Use();
        }
        else
        {
            PlayUI.instance.ErrorMessagePopup("가까이 다가가서 클릭하십시오.");
        }
    }
    void Use()
    {
        switch (state)
        {
            case State.MONEY:
                Player.instance.money += money;
                PlayUI.instance.MessagePopup(money + "원을 획득했습니다!");
                break;
            case State.DAMAGE:
                Player.instance.Damaged(damage);
                PlayUI.instance.MessagePopup(damage + "만큼의 피해를 입었습니다!");
                break;
        }
        Destroy(this.gameObject);
    }
}
