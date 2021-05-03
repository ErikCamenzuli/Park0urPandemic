using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPosition : Quest
{
    public GameObject positionPrefab;

    public override void Start()
    {
        base.Start();
        Transform spawnPostion = gameManager.GetSpawnPosition();
        if (spawnPostion != null)
        {
            GameObject positionTriggerObject = Instantiate(positionPrefab, gameManager.GetSpawnPosition());
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

    public override void StatInitialisation()
    {
        base.StatInitialisation();
        if (questTemplate is QuestPositionTemplate template)
            positionPrefab = template.positionPrefab;
    }
}
