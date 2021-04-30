using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    public List<GameObject> questUIList = new List<GameObject>();
    public GameObject questUIPrefab;
    public GameObject questUISpawnPosition;
    public float questUISpawnYOffset;

    public void NewQuestUI(Quest quest)
    {
        GameObject questUI = Instantiate(questUIPrefab, questUISpawnPosition.transform);
        quest.questUIObject = questUI;
        questUI.transform.position -= new Vector3(0, questUISpawnYOffset * questUIList.Count, 0);
        questUIList.Add(questUI);
        QuestUI questUITextScriptQuestionMark = questUI.GetComponent<QuestUI>();
        questUITextScriptQuestionMark.questNameText.text = quest.questName;
        questUITextScriptQuestionMark.descriptionText.text = quest.description;
        for (int i = 0; i < quest.difficulty; i++)
            questUITextScriptQuestionMark.difficultyText.text += "*";
        questUITextScriptQuestionMark.timerText.text = quest.timer.ToString();
    }

    public void RemoveQuestUI(Quest quest)
    {
        questUIList.Remove(quest.questUIObject);
        Destroy(quest.questUIObject);
        PlayerManager.Instance.questList.Remove(quest);
        Destroy(quest);
    }


}
