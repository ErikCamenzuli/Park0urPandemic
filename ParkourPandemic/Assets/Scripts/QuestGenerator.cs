using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    static QuestGenerator _instance;
    public static QuestGenerator Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<QuestGenerator>();
            return _instance;
        }
    }

    PlayerManager playerManager;
    GameManager gameManager;

    private void Start()
    {
        playerManager = PlayerManager.Instance;
        gameManager = GameManager.Instance;
    }

    public List<QuestTemplate> questTemplateList = new List<QuestTemplate>();

    public QuestTemplate GetQuestTemplate()
    {
        int randomNumber = Random.Range(0, questTemplateList.Count);
        return (questTemplateList[randomNumber]);
    }

    public void NewQuestRollButton()
    {
        if (playerManager.questListMax > playerManager.questList.Count)
        {
            Quest quest = playerManager.AddQuest(GetQuestTemplate());
            if (quest != null)
            {
                gameManager.NewQuestUI(quest);
            }
        }
    }
}
