using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string questName;
    public string description;
    public bool isCompleted;
    public float timer;
    public int difficulty;

    public QuestTemplate questTemplate;

    public void Start()
    {
        if (questTemplate != null)
            StatInitialisation();
    }

    public virtual void StatInitialisation()
    {
        questName = questTemplate.questName;
        description = questTemplate.description;
        timer = questTemplate.timer;
        difficulty = questTemplate.difficulty;
    }
}
