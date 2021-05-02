using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSpawn : MonoBehaviour
{
    public GameObject textCanvas;
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questTime;
    public QuestTemplate template;
    GameManager gameManager;
    public GameObject questSpawnCircle;
    public AudioSource questStart;
    bool isDestroying;

    [Header("Colors")]
    public Material primaryMaterial;
    public Material secondaryMaterial;

    void Start()
    {
        gameManager = GameManager.Instance;
    }
    void Update()
    {
        textCanvas.transform.rotation = Quaternion.LookRotation(textCanvas.transform.position - PlayerManager.Instance.gameObject.transform.position);

        if (isDestroying && !questStart.isPlaying)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isDestroying = true;
            questSpawnCircle.SetActive(false);
            questStart.Play(0);
            gameManager.AddNewQuest(template);
        }
    }
}
