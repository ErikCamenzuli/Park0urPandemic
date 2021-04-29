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
            quest = gameObject.AddComponent<Quest>();
            quest.questTemplate = template;
            quest.StatInitialisation();
            questList.Add(quest);
            return (quest);
        }
        else
            return (quest);

    }
}
