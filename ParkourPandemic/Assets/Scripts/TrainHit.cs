using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHit : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Boing!");
            collision.gameObject.GetComponent<Rigidbody>().velocity += new Vector3(300, 10, 300);
        }
    }
}
