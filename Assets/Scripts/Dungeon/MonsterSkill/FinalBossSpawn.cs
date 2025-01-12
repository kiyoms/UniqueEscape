using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossSpawn : MonoBehaviour
{
    public GameObject monster;
    bool token = false;

    private void Awake()
    {
        QuestManager.instance.findQuestWithCode("E99").ChangeState(Quest.State.start);
    }

    void Update()
    {
        if (!token)
        {
            if (QuestManager.instance.findQuestWithCode("E99").state == Quest.State.finish)
            {
                token = true;
                Instantiate(monster, transform.parent);
                Destroy(gameObject);
            }
        }
    }

}
