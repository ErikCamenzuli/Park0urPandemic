using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounce;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Boing!");
            collision.gameObject.GetComponent<Rigidbody>().velocity += new Vector3(0, bounce, 0);
        }
    }
}
