using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PlayerManager>();
            return _instance;
        }
    }

    public List<Quest> questList = new List<Quest>();
    public int questListMax;
    public int score;
    public int combo;

    public Quest AddQuest(QuestTemplate template)
    {
        Quest quest = null;
        if (questList.Count < questListMax)
        {
            //I'm sorry ok i dont have time.
            if (template is QuestTemplate && !(template is QuestPositionTemplate))
                quest = gameObject.AddComponent<Quest>();
            else if (template is QuestPositionTemplate positionTemplate)
            {
                quest = gameObject.AddComponent<QuestPosition>();
                if (quest is QuestPosition questPosition)
                    questPosition.positionPrefab = positionTemplate.positionPrefab;
            }

            quest.questTemplate = template;
            quest.StatInitialisation();
            questList.Add(quest);

            return (quest);
        }
        else
            return (quest);

    }
}
