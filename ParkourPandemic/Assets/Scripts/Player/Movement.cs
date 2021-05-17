using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 wallRunDir;
    public List<GameObject> groundCheckList = new List<GameObject>();
    float ledgeGrabGoalYPosition;
    bool jumpPress;

    [Header("Checks")]
    public bool isGrounded;
    public bool isWallRunning;
    public bool isLedgeGrabbing;

    [Header("Stats")]
    public float speed = 10f;
    public float movementClamp;
    public float jump = 3f;
    public float gravity;
    public float gravityModifier;
    public float wallRunForce;
    public float wallRunTimer;

    [Header("Audio")]
    public AudioSource runningAudio;
    public AudioSource jumpingAudio;
    public AudioSource thudAudio;

    [Header("Misc.")]
    public LayerMask wallRunMask;
    public LedgeGrabTrigger ledgeGrabTrigger;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            jumpPress = true;
        if (Input.GetKeyDown(KeyCode.LeftControl))
            gravityModifier = 16;
        if (Input.GetKeyUp(KeyCode.LeftControl))
            gravityModifier = 0;
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;

        //Audio
        if (isGrounded && rb.velocity.magnitude > 5 && rb.velocity.y == 0)
            runningAudio.enabled = true;
        else
            runningAudio.enabled = false;

        //Jump
        if (jumpPress)
        {
            jumpPress = false;
            rb.velocity = new Vector3(rb.velocity.x, jump, rb.velocity.z);
            jumpingAudio.Play();
            isGrounded = false;
            rb.useGravity = true;
        }

        //Ledge Grab
        if (Physics.Raycast(transform.position, transform.forward, 0.6f, wallRunMask) && !CanWallRun() && ledgeGrabTrigger.isLedgeGrabbableSpace)
        {
            ledgeGrabGoalYPosition = ledgeGrabTrigger.transform.position.y;
            isLedgeGrabbing = true;
        }

        if (isLedgeGrabbing)
        {
            if (transform.position.y < ledgeGrabGoalYPosition)
                transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed);
            else
            {
                ledgeGrabGoalYPosition = 0;
                isLedgeGrabbing = false;
            }
        }

        //Wall Run
        if (CanWallRun() && y > 0f)
        {
            rb.useGravity = false;
            Vector3 moveDir = (transform.forward * y).normalized;
            rb.velocity = wallRunDir * Vector3.Dot(moveDir, wallRunDir) * wallRunForce * Time.deltaTime;
        }

        //Run
        else
        {
            Vector3 movePos = transform.right * x + transform.forward * y;
            Vector3 newPos = new Vector3(movePos.x, rb.velocity.y, movePos.z);
            rb.useGravity = true;
            rb.velocity = newPos;
        }

        //Movement Clamp
        Vector3 clamp = Vector3.ClampMagnitude(rb.velocity, movementClamp);
        rb.velocity = new Vector3(clamp.x, rb.velocity.y, clamp.z);

        //Gravity
        if (!isGrounded && !isLedgeGrabbing && !isWallRunning)
            rb.velocity += new Vector3(0, -(gravity * Time.deltaTime * gravityModifier), 0);
    }

    bool CanWallRun()
    {
        //Left Wall Check
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            RaycastHit rightHit;
            if (Physics.Raycast(transform.position, transform.right, out rightHit, 0.6f, wallRunMask))
            {
                if (Vector3.Dot(rightHit.normal, -transform.right) > 0.8f)
                {
                    wallRunDir = Vector3.Cross(rightHit.normal, Vector3.up);
                    isWallRunning = true;
                    return true;
                }
            }
        }

        //Right Wall Check
        if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            RaycastHit leftHit;
            if (Physics.Raycast(transform.position, -transform.right, out leftHit, 0.6f, wallRunMask))
            {
                if (Vector3.Dot(leftHit.normal, transform.right) > 0.8f)
                {
                    wallRunDir = Vector3.Cross(leftHit.normal, Vector3.up);
                    isWallRunning = true;
                    return true;
                }

            }
        }
        isWallRunning = false;
        return false;
    }

    //Ground Check
    private void OnCollisionEnter(Collision other)
    {
        ContactPoint point = other.GetContact(0);
        if (point.normal.y > 0.1f)
        {
            groundCheckList.Add(other.transform.gameObject);
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
            foreach (GameObject item in groundCheckList)
            {
                if (item == other.transform.gameObject)
                {
                    groundCheckList.Remove(item);
                    isGrounded = false;
                }
            }
    }

}
