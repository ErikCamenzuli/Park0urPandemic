using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{

    private Rigidbody rb;
    private CapsuleCollider playerCollider;

    [Header("Keybind for Crouch/Slide")]
    public KeyCode slideBind = KeyCode.C;

    public float height;
    public float slidingSpeed;

    [HideInInspector]
    public float reducedHeight;
    [HideInInspector]
    public KeyCode playerForwardBind = KeyCode.W;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        height = playerCollider.height;
    }

    // Update is called once per frame
    void Update()
    {
        //Sliding
        if (Input.GetKeyDown(slideBind) && Input.GetKey(KeyCode.W))
            PlayerSlide();
        else if (Input.GetKeyUp(slideBind))
            PlayerGoUP();

        //Crouching
        if (Input.GetKeyDown(slideBind))
            IsCrouching();
        else if (Input.GetKeyUp(slideBind))
            PlayerGoUP();
    }

    /// <summary>
    /// Crouch
    /// </summary>
    private void IsCrouching()
    {
        playerCollider.height = reducedHeight;
    }

    //Slide
    private void PlayerSlide()
    {
        playerCollider.height = reducedHeight;
        rb.AddForce(transform.forward * slidingSpeed, ForceMode.VelocityChange);
    }

    //Player height normal
    private void PlayerGoUP()
    {
        playerCollider.height = height;
    }

}
