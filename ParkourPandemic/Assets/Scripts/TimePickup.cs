using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimePickup : MonoBehaviour
{
    public GameObject cube1;
    public GameObject cube2;
    public TextMeshProUGUI timeBoostText;
    public float timeBoost;
    GameManager gameManager;


    void Start()
    {
        gameManager = GameManager.Instance;
    }
    void Update()
    {
        timeBoostText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timeBoost / 60), Mathf.FloorToInt(timeBoost % 60));
        if (timeBoost > 60)
        {
            timeBoost = 60f;
            timeBoostText.color = Color.green;
        }

        if (gameManager.gameStatesToggle == GameManager.GameStates.Play)
        {
            timeBoost += Time.deltaTime;
            cube1.transform.Rotate(1.5f, 0, 0 * Time.deltaTime);
            cube2.transform.Rotate(0, 1.5f, 0 * Time.deltaTime);
            timeBoostText.transform.rotation = Quaternion.LookRotation(timeBoostText.transform.position - PlayerManager.Instance.gameObject.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (Quest item in gameManager.playerManager.questList)
            item.timer += timeBoost;
        Destroy(this.gameObject);
    }
}
