using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    public GameObject player;
    bool climb = false;
    float speed = 4f;

    private void Update()
    {
        if(climb)
        {
            if ((Input.GetKey(KeyCode.W)))
                player.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            climb = true;
            player = other.gameObject;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            climb = false;
            player = null;
        }
    }
}
