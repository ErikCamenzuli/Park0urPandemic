using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainFinishTrigger : MonoBehaviour
{
    int counter;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            counter++;
            Destroy(other.gameObject);

            if (counter == 10)
            {
                counter = 0;
                GameManager.Instance.TrainSpawn();
            }
        }
    }
}
