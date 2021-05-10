using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    PlayerManager playerManager;
    public Movement movement;

    void Start()
    {
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            movement.isLedgeGrabbable = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            movement.isLedgeGrabbable = false;
        }
    }
}
