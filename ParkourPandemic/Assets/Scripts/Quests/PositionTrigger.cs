using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PositionTrigger : MonoBehaviour
{
    public Quest quest;
    public AudioSource dingNoise;
    public GameObject ring;
    bool isDestroying;
    public TextMeshProUGUI timerText;
    public MeshRenderer meshRenderer;
    public Material defaultMaterial;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isDestroying = true;
            ring.SetActive(false);
            timerText.gameObject.SetActive(false);
            dingNoise.Play();
            quest.isCompleted = true;
        }
    }

    void Update()
    {
        timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(quest.timer / 60), Mathf.FloorToInt(quest.timer % 60));
        transform.Rotate(0, 1.5f, 0 * Time.deltaTime);
        if (isDestroying && !dingNoise.isPlaying)
            Destroy(gameObject);
    }
}
