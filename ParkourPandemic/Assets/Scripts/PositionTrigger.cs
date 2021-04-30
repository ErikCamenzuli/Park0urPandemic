using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTrigger : MonoBehaviour
{
    public Quest quest;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            quest.isCompleted = true;
            Destroy(gameObject);
        }
    }
}
