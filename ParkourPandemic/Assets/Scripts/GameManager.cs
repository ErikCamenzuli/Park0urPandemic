using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

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
    [HideInInspector]
    public PlayerManager playerManager;
    [HideInInspector]
    public Scene currentScene;

    public bool waitingForQuest;
    public Vector2 randomQuestSpawnTime;

    public enum GameStates { Play, Pause}
    public GameStates gameStatesToggle;
    public GameObject playStateUI;
    public GameObject pauseStateUI;
    public TextMeshProUGUI buildingNameText;
    public TextMeshProUGUI wallRunText;
    public Animator buildingNameAnimator;

    public List<Material> questAuraMaterials = new List<Material>();


    public GameObject trainPrefab;
    public Transform trainParentTransform;

    public AudioSource questFailAudio;

    public GameObject ppVolumeObject;
    public PostProcessVolume mainVolume;

    void Start()
    {
        playerManager = PlayerManager.Instance;
        currentScene = SceneManager.GetActiveScene();
        TrainSpawn();
    }

    public void TrainSpawn()
    {
        float delay = 0;
        for (int i = 0; i < 10; i++)
        {
            StartCoroutine(TrainSpawn(0.3f + (delay / 3f)));
            delay++;
        }
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameStatesToggle == GameStates.Play)
                gameStatesToggle = GameStates.Pause;
            else if (gameStatesToggle == GameStates.Pause)
                gameStatesToggle = GameStates.Play;
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(currentScene.name);

        if (waitingForQuest == false)
        {
            StartCoroutine(QuestSpawn(Random.Range(randomQuestSpawnTime.x, randomQuestSpawnTime.y)));
            waitingForQuest = true;
        }

        comboScoreText.text = (000 + comboScore).ToString();

        switch (gameStatesToggle)
        {
            case GameStates.Play:
                Time.timeScale = 1;
                playStateUI.SetActive(true);
                pauseStateUI.SetActive(false);
                ppVolumeObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                AudioListener.volume = 1;
                break;
            case GameStates.Pause:
                Time.timeScale = 0;
                playStateUI.SetActive(false);
                pauseStateUI.SetActive(true);
                ppVolumeObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                AudioListener.volume = 0;
                break;
        }
    }

    IEnumerator QuestSpawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        waitingForQuest = false;
        QuestTemplate template = GetQuestTemplate();
        Transform spawnPosition = GetSpawnPosition();
        if (template != null && spawnPosition != null)
            NewQuestSpawner(template, spawnPosition);
    }

    IEnumerator TrainSpawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Instantiate(trainPrefab, trainParentTransform);
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
        if (spawnPositions[randomPositionIndex].childCount == 0)
        {
            //spawnPositions[randomPositionIndex].gameObject.SetActive(false);
            return spawnPositions[randomPositionIndex];
        }
        else
        {
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
            activeQuestSpawners++;
            GameObject questSpawner = Instantiate(questSpawnerPrefab, randomPosition);
            QuestSpawn questSpawn = questSpawner.GetComponent<QuestSpawn>();

            questSpawn.questName.text = template.questName;
            questSpawn.questTime.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(template.timer / 60), Mathf.FloorToInt(template.timer % 60));

            questSpawn.meshRenderer.materials[0] = new Material(questAuraMaterials[0]);
            questSpawn.meshRenderer.materials[1] = new Material(questAuraMaterials[1]);
            questSpawn.meshRenderer.materials[2] = new Material(questAuraMaterials[2]);
            questSpawn.meshRenderer.materials[3] = new Material(questAuraMaterials[3]);
            questSpawn.meshRenderer.materials[0].color = new Color(template.primaryColor.r, template.primaryColor.g, template.primaryColor.b, 230f / 255f);
            questSpawn.meshRenderer.materials[1].color = new Color(template.primaryColor.r, template.primaryColor.g, template.primaryColor.b, 60f / 255f);
            questSpawn.meshRenderer.materials[2].color = new Color(template.primaryColor.r, template.primaryColor.g, template.primaryColor.b, 30f / 255f);
            questSpawn.meshRenderer.materials[3].color = new Color(template.primaryColor.r, template.primaryColor.g, template.primaryColor.b, 50f / 255f);
            questSpawn.template = template;
        }
        else
            Debug.Log("Max Quest Spawners Active!");
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
                newQuest.questUI.timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(newQuest.timer / 60), Mathf.FloorToInt(newQuest.timer % 60));
                newQuest.questUI.image.color = newQuest.primaryColor;
            }
        }
    }

    public void EndQuest(Quest quest, bool success = true)
    {
        for (int i = playerManager.questList.IndexOf(quest); i < playerManager.questList.Count; i++)
            playerManager.questList[i].questUI.gameObject.transform.position += new Vector3(0, questUISpawnYOffset, 0);
        Destroy(quest.questUI.gameObject);
        PlayerManager.Instance.questList.Remove(quest);
        Destroy(quest);
        if (quest.questObject != null)
            Destroy(quest.questObject);
        if (success)
            comboScore++;
        else
            comboScore = 0;
    }


}
