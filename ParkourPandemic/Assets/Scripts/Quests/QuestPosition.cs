using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPosition : Quest
{
    public GameObject positionPrefab;
    GameObject positionTriggerObject;
    public string distance;

    public override void Start()
    {
        base.Start();
        Transform spawnPostion = gameManager.GetSpawnPosition();
        if (spawnPostion != null)
        {
            positionTriggerObject = Instantiate(positionPrefab, gameManager.GetSpawnPosition());
            questObject = positionTriggerObject;
            PositionTrigger positionTrigger = positionTriggerObject.GetComponent<PositionTrigger>();
            positionTrigger.quest = this;
            positionTrigger.meshRenderer.materials[0] = new Material(positionTrigger.defaultMaterial);
            positionTrigger.meshRenderer.materials[1] = new Material(positionTrigger.defaultMaterial);
            positionTrigger.meshRenderer.materials[0].color = positionTrigger.quest.primaryColor + new Color(0, 0, 0, 230);
            positionTrigger.meshRenderer.materials[1].color = positionTrigger.quest.secondaryColor + new Color(0, 0, 0, 230);
        }
        else
        {
            Debug.Log("Failed to spawn QuestPositionTrigger, Destroying Quest.");
            //RemoveQuest();
        }
    }
    public override void Update()
    {
        base.Update();

        Vector3 playerPosition = gameManager.playerManager.transform.position;

        if (Mathf.RoundToInt(Vector3.Distance(playerPosition, positionTriggerObject.transform.position)) >= 30)
            distance = "Very Far";
        else if (Mathf.RoundToInt(Vector3.Distance(positionTriggerObject.transform.position, playerPosition)) >= 20)
            distance = "Far";
        else if (Mathf.RoundToInt(Vector3.Distance(positionTriggerObject.transform.position, playerPosition)) >= 10)
            distance = "Near";
        else if (Mathf.RoundToInt(Vector3.Distance(positionTriggerObject.transform.position, playerPosition)) >= 5)
            distance = "Close";

        switch (Mathf.RoundToInt(Vector3.Distance(playerPosition, transform.position)))
        {

            default:
                break;
        }

        questUI.distanceText.text = distance;
    }
    public override void StatInitialisation()
    {
        base.StatInitialisation();
        if (questTemplate is QuestPositionTemplate template)
            positionPrefab = template.positionPrefab;
    }
}
