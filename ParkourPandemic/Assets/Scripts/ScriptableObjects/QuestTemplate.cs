using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest")]
public class QuestTemplate : ScriptableObject
{
    [Header("General")]
    public string questName;
    public string description;
    public float timer;
    public int difficulty;

    [Header("ColorSpace")]
    public Color primaryColor;
    public Color secondaryColor;
}
