using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsLooking : MonoBehaviour
{
    public Movement movement;

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            movement.isLooking = true;
        }
    }
}
