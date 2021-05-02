using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPosition : Quest
{
    public GameObject positionPrefab;

    public override void Start()
    {
        base.Start();
        GameObject positionTriggerObject = Instantiate(positionPrefab, gameManager.GetSpawnPosition());
        if (positionTriggerObject != null)
            positionTriggerObject.GetComponent<PositionTrigger>().quest = this;
        else
            Debug.Log("GetPosition Failed!");
    }

    public override void StatInitialisation()
    {
        base.StatInitialisation();
        if (questTemplate is QuestPositionTemplate template)
            positionPrefab = template.positionPrefab;
    }
}
