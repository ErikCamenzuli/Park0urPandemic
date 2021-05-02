using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public List<Transform> spawnPositionsList = new List<Transform>();
    GameManager gameManager;
    
    //We use a list becuase Unity doesn't handle Dictionaries in the Inspector.
    void Start()
    {
        gameManager = GameManager.Instance;
        foreach (Transform item in spawnPositionsList)
            gameManager.spawnPositions.Add(item);
    }
}
