using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestPosition", menuName = "Quests/QuestPosition")]
public class QuestPositionTemplate : QuestTemplate
{
    [Header("Position Data")]
    public GameObject positionPrefab;
}
