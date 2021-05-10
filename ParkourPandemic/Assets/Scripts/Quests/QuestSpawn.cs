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
    float transparencyAura;

    [Header("Colors")]
    public Material defaultMaterial;
    public MeshRenderer meshRenderer;
    CapsuleCollider collider;

    void Start()
    {
        gameManager = GameManager.Instance;
        collider = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        transparencyAura = 0.05f * (Vector3.Distance(gameObject.transform.position, gameManager.playerManager.gameObject.transform.position) / 4);
        if (transparencyAura > 0.58f)
            transparencyAura = 0.58f;
        textCanvas.transform.rotation = Quaternion.LookRotation(textCanvas.transform.position - PlayerManager.Instance.gameObject.transform.position);
        transform.Rotate(0, 0.5f, 0 * Time.deltaTime);
        meshRenderer.materials[3].color = new Color(template.primaryColor.r, template.primaryColor.g, template.primaryColor.b, transparencyAura);

        if (isDestroying && !questStart.isPlaying)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.playerManager.questList.Count < gameManager.playerManager.questListMax)
        {
            collider.enabled = false;
            isDestroying = true;
            questSpawnCircle.SetActive(false);
            questStart.Play(0);
            gameManager.AddNewQuest(template);
            gameManager.activeQuestSpawners--;
        }
    }
}
