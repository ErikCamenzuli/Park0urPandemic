using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabTrigger : MonoBehaviour
{
    public bool isLedgeGrabbableSpace;

    void FixedUpdate()
    {
        isLedgeGrabbableSpace = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Building"))
            isLedgeGrabbableSpace = false;
    }
}
