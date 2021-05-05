using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public Transform playerLook;
    public float mouseSens = 10f;

    private float x = 0;
    private float y = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        x += -Input.GetAxis("Mouse Y") * mouseSens;
        y += Input.GetAxis("Mouse X") * mouseSens;

        x = Mathf.Clamp(x, -90f, 90f);
        if (GameManager.Instance.gameStatesToggle == GameManager.GameStates.Play)
        {
            transform.localRotation = Quaternion.Euler(x, 0, 0);
            playerLook.transform.localRotation = Quaternion.Euler(0, y, 0);
        }
    }
}
