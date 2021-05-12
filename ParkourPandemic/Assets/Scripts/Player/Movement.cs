using UnityEngine;
using MilkShake;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    public Transform orientation;

    public bool isGrounded;
    public bool isWallRun;
    bool isWallRunning;
    bool isLedgeGrabbable;
    bool isLooking;

    public GameObject wallObject;

    public float speed = 10f;
    public float jump = 3f;
    public float wallRunForce;
    public float maxRunTimeWall;
    public float maxSpeedWall;
    public float cameraTiltMax;
    public float cameraTiltWallRun;

    public float climbSpeed;
    public float climbWall;
    public float distanceWallclimb;

    public AudioSource runningAudio;
    public AudioSource jumpingAudio;
    public AudioSource thudAudio;

    public Shaker shaker;
    public ShakePreset shakerPreset;

    public Vector3 tempPoint;

    private Vector3 wallRunDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;

        if (isGrounded && rb.velocity.magnitude > 5 && rb.velocity.y == 0)
            runningAudio.enabled = true;
        else
            runningAudio.enabled = false;

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded))
        {
            rb.velocity = new Vector3(rb.velocity.x, jump, rb.velocity.z);
            jumpingAudio.Play();
            isGrounded = false;
            rb.useGravity = true;
        }

        isWallRun = (CanWallRun() && y > 0f);

        if (CanWallRun() && y > 0f)
        {
            rb.useGravity = false;
            //rb.velocity = Vector3.zero;
            Vector3 moveDir = (transform.forward * y).normalized;
            rb.velocity = wallRunDir * Vector3.Dot(moveDir, wallRunDir) * wallRunForce * Time.deltaTime;
        }
        else
        {
            Vector3 movePos = transform.right * x + transform.forward * y;
            Vector3 newPos = new Vector3(movePos.x, rb.velocity.y, movePos.z);
            rb.velocity = newPos;
            rb.useGravity = true;
        }

    }

    bool CanWallRun()
    {

        // check if on ground
        RaycastHit groundhit;
        if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out groundhit, 0.1f, ~gameObject.layer))
        {
            return false;
        }

        // check if there's a wall to the right
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            RaycastHit rightHit;
            if (Physics.Raycast(transform.position, transform.right, out rightHit, 0.6f, ~gameObject.layer))
            {
                if (Vector3.Dot(rightHit.normal, -transform.right) > 0.8f)
                {
                    Debug.Log(rightHit.normal);
                    wallRunDir = Vector3.Cross(rightHit.normal, Vector3.up);
                    return true;
                }
            }
        }

        // check if there's a wall to the left
        if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            RaycastHit leftHit;
            if (Physics.Raycast(transform.position, -transform.right, out leftHit, 0.6f, ~gameObject.layer))
            {
                if (Vector3.Dot(leftHit.normal, transform.right) > 0.8f)
                {
                    wallRunDir = Vector3.Cross(leftHit.normal, Vector3.up);
                    return true;
                }

            }
        }
        return false;
    }


    private void OnCollisionStay(Collision other)
    {
        ContactPoint point = other.GetContact(0);
        if (point.normal.y > 0.1f)
            isGrounded = true;
    }

}
