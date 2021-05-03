using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public int comboScore;
    public TextMeshProUGUI comboScoreText;

    [Header("Quest Spawners")]
    public int questSpawnerMax;
    public int activeQuestSpawners;
    public GameObject questSpawnerPrefab;
    public Transform questSpawnerParent;

    [Header("Arrays")]
    public List<Transform> spawnPositions = new List<Transform>();
    public List<QuestTemplate> questTemplateList = new List<QuestTemplate>();

    public PlayerManager playerManager;

    void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuestTemplate template = GetQuestTemplate();
            Transform spawnPosition = GetSpawnPosition();
            if (template != null && spawnPosition != null)
                NewQuestSpawner(template, spawnPosition);
        }

        comboScoreText.text = (000 + comboScore).ToString();
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
            GameObject questSpawner = Instantiate(questSpawnerPrefab, randomPosition);
            QuestSpawn questSpawn = questSpawner.GetComponent<QuestSpawn>();

            questSpawn.questName.text = template.questName;
            questSpawn.questTime.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(template.timer / 60), Mathf.FloorToInt(template.timer % 60));

            questSpawn.meshRenderer.materials[0] = new Material(questSpawn.defaultMaterial);
            questSpawn.meshRenderer.materials[1] = new Material(questSpawn.defaultMaterial);
            questSpawn.meshRenderer.materials[0].color = template.primaryColor + new Color(0, 0, 0, 230);
            questSpawn.meshRenderer.materials[1].color = template.secondaryColor + new Color(0 ,0 ,0 ,230);
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
                newQuest.questUI.timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(newQuest.timer / 60), Mathf.FloorToInt(newQuest.timer % 60));
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
