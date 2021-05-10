using UnityEngine;
using MilkShake;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    public Transform orientation;

    public bool isGrounded;
    public bool isWallRunning;
    public bool isLedgeGrabbable;
    public bool isLooking;

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

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isWallRunning))
        {
            rb.velocity = new Vector3(rb.velocity.x, jump, rb.velocity.z);
            jumpingAudio.Play();
            isGrounded = false;
            isWallRunning = false;
            rb.useGravity = true;
        }

        if (isWallRunning && isLedgeGrabbable && isLooking)
        {
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed);
        }
        Vector3 movePos = transform.right * x + transform.forward * y;
        Vector3 newPos = new Vector3(movePos.x, rb.velocity.y, movePos.z);
        rb.velocity = newPos;    
    }

    void OnCollisionEnter(Collision other)
    {
        thudAudio.Play();
        shaker.Shake(shakerPreset);
        ContactPoint point = other.GetContact(0);
        tempPoint = point.normal;
        Debug.Log(point.normal);

        if ( point.normal.y < 0.1f && (point.normal.x > 0.3f || point.normal.z > 0.3f || point.normal.x > -0.3f || point.normal.z > -0.3f))
        {
            isWallRunning = true;
            rb.useGravity = false;
            wallObject = other.gameObject;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
        isWallRunning = false;
        rb.useGravity = true;
    }

    private void OnCollisionStay(Collision other)
    {
        ContactPoint point = other.GetContact(0);
        if (point.normal.y > 0.1f)
            isGrounded = true;
    }

}
