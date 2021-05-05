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

    public Color primaryColor;
    public Color secondaryColor;

    public QuestTemplate questTemplate;
    public GameManager gameManager;
    public Transform spawnPoint;
    public GameObject questObject;

    public QuestUI questUI;

    public virtual void Start()
    {
        gameManager = GameManager.Instance;
        if (questTemplate != null)
            StatInitialisation();
    }

    public virtual void StatInitialisation()
    {
        questName = questTemplate.questName;
        description = questTemplate.description;
        timer = questTemplate.timer;
        difficulty = questTemplate.difficulty;
        primaryColor = questTemplate.primaryColor;
        secondaryColor = questTemplate.secondaryColor;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        questUI.timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer % 60));
        if (isCompleted)
            GameManager.Instance.EndQuest(this);
        if (timer < 0)
        {
            GameManager.Instance.questFailAudio.Play();
            GameManager.Instance.EndQuest(this, false);
        }
    }
}
