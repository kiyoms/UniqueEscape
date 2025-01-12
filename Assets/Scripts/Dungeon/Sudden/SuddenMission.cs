using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuddenMission : MonoBehaviour
{
    public static float probability = 9;
    public enum Type
    {

        MOB_SIZE, MIMIC, OBSTACLE
    };
    public Type type;

    Dungeon dungeon;
    [Header("MINIC")]
    public GameObject effect;
    [Header("MINIC")]
    public GameObject chest;

    public static bool Probability()
    {
        float size = Random.Range(0f,10f);
        if(size < probability)
        {
            return true;
        }
        return false;
    }
    void Start()
    {
        dungeon = GetComponentInParent<Dungeon>();

        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                type = Type.MOB_SIZE;
                MobSize();
                break;
            case 1:
                type = Type.MIMIC;
                Mimic();
                break;
            case 2:
                type = Type.OBSTACLE;
                break;
        }
        print(type);
    }
    void MobSize()
    {
        PlayUI.instance.MessagePopup("알 수 없는 이유로 적의 크기와 능력치가 변화합니다.");

        Monster[] mobs = FindObjectsOfType<Monster>();
        foreach(Monster mob in mobs)
        {
            float weight = Random.Range(0.7f, 1.5f);

            mob.maxHP = Mathf.FloorToInt(mob.maxHP * weight);
            mob.atk = Mathf.FloorToInt(mob.atk * weight);
            mob.transform.localScale = new Vector2(mob.transform.localScale.x * weight, mob.transform.localScale.y * weight);

            StartCoroutine(RemoveEffect(Instantiate(effect, mob.transform)));

            mob.Heal();
        }
    }

    IEnumerator RemoveEffect(Object obj)
    {
        yield return new WaitForSeconds(1);
        Destroy(obj);
    }

    void Mimic()
    {
        PlayUI.instance.MessagePopup("어딘가에 상자가 나타났습니다.\n가까이 다가가 열어보세요.");

        int spawnRange = dungeon.spawnRange;

        Vector3 pos = dungeon.transform.position;

        if (spawnRange != 0)
        {
            float x, y;
            do
            {
                x = Random.Range(-spawnRange, spawnRange);
            } while (Mathf.Abs(x) < 0.5);
            do
            {
                y = Random.Range(-spawnRange, spawnRange);
            } while (Mathf.Abs(y) < 0.5);

            pos.x += x;
            pos.y += y;
        }

        GameObject obj = Instantiate(chest, pos, new Quaternion(0, 0, 0, 0));
        obj.transform.parent = dungeon.transform;
    }
}
