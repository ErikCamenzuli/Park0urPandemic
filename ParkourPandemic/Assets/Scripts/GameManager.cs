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

    public GameObject questUIPrefab;
    public GameObject questUISpawnPosition;
    public float questUISpawnYOffset;

    [Header("Quest Spawners")]
    public int questSpawnerMax;
    public int activeQuestSpawners;
    public GameObject questSpawnerPrefab;
    public Transform questSpawnerParent;

    [Header("Arrays")]
    public List<Transform> spawnPositions = new List<Transform>();
    public List<QuestTemplate> questTemplateList = new List<QuestTemplate>();

    PlayerManager playerManager;

    void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NewQuestSpawner(GetQuestTemplate(), GetSpawnPosition());
        }
    }

    public QuestTemplate GetQuestTemplate()
    {
        return (questTemplateList[Random.Range(0, questTemplateList.Count)]);
    }

    public Transform GetSpawnPosition()
    {
        int count = 0;
    Reroll:
        int randomPositionIndex = Random.Range(0, spawnPositions.Count);
        Debug.Log("Trying" + randomPositionIndex.ToString());
        if (spawnPositions[randomPositionIndex].childCount == 0)
        {
            //spawnPositions[randomPositionIndex].gameObject.SetActive(false);
            return spawnPositions[randomPositionIndex];
        }
        else
        {
            Debug.Log("Failed! " + spawnPositions[randomPositionIndex].childCount);
            count++;
            if (count < 99)
                goto Reroll;

            else
            {
                Debug.Log("Reroll Timeout (Shit's frigged yo.");
                return null;
            }
        }
    }

    public void NewQuestSpawner(QuestTemplate template, Transform randomPosition)
    {
        if (activeQuestSpawners < questSpawnerMax)
        {
            Debug.Log("Spawning New Quest Spawner at" + randomPosition.name);
            GameObject questSpawner = Instantiate(questSpawnerPrefab, randomPosition);
            QuestSpawn questSpawn = questSpawner.GetComponent<QuestSpawn>();
            questSpawn.questName.text = template.questName;
            questSpawn.questTime.text = template.timer.ToString();
            questSpawn.primaryMaterial.color = template.primaryColor;
            questSpawn.secondaryMaterial.color = template.secondaryColor;
            questSpawn.template = template;

        }
    }

    public void AddNewQuest(QuestTemplate template, bool isVisable = true)
    {
        Quest newQuest = null;
        if (playerManager.questList.Count < playerManager.questListMax)
        {
            if (template.GetType() == typeof(QuestTemplate))
                newQuest = playerManager.gameObject.AddComponent<Quest>();

            else if (template.GetType() == typeof(QuestPositionTemplate))
                newQuest = playerManager.gameObject.AddComponent<QuestPosition>();

            newQuest.questTemplate = template;
            newQuest.StatInitialisation();
            playerManager.questList.Add(newQuest);

            if (isVisable)
            {
                GameObject questUIObject = Instantiate(questUIPrefab, questUISpawnPosition.transform);
                questUIObject.transform.position -= new Vector3(0, questUISpawnYOffset * playerManager.questList.Count, 0);
                newQuest.questUI = questUIObject.GetComponent<QuestUI>();

                newQuest.questUI.questNameText.text = newQuest.questName;
                newQuest.questUI.descriptionText.text = newQuest.description;
                newQuest.questUI.timerText.text = newQuest.timer.ToString();
                newQuest.questUI.image.color = newQuest.primaryColor;
            }
        }
    }

    public void RemoveQuestUI(Quest quest)
    {
        for (int i = playerManager.questList.IndexOf(quest); i < playerManager.questList.Count; i++)
            playerManager.questList[i].questUI.gameObject.transform.position += new Vector3(0, questUISpawnYOffset, 0);
        Destroy(quest.questUI.gameObject);
        PlayerManager.Instance.questList.Remove(quest);
        Destroy(quest);
    }


}
