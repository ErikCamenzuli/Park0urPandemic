using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPosition : Quest
{
    public GameObject positionPrefab;

    public override void Start()
    {
        base.Start();
        GameObject fuckidk = Instantiate(positionPrefab, QuestGenerator.Instance.gameObject.GetComponent<QuestPositionManager>().randomPositionList[Random.Range(0, QuestGenerator.Instance.gameObject.GetComponent<QuestPositionManager>().randomPositionList.Count)]);
        fuckidk.GetComponent<PositionTrigger>().quest = this;
    }
}
