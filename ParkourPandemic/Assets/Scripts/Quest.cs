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
    public GameObject questUIObject;

    public QuestTemplate questTemplate;
    public GameObject questManagerObject;

    public virtual void Start()
    {
        if (questTemplate != null)
            StatInitialisation();
        questManagerObject = QuestGenerator.Instance.gameObject;
    }

    public virtual void StatInitialisation()
    {
        questName = questTemplate.questName;
        description = questTemplate.description;
        timer = questTemplate.timer;
        difficulty = questTemplate.difficulty;
    }

    void Update()
    {
        if (isCompleted)
        {
            GameManager.Instance.RemoveQuestUI(this);
        }
    }
}
