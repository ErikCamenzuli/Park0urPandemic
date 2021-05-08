using UnityEngine;
using MilkShake;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    public Transform orientation;
    public LayerMask groundCheck;
    public LayerMask isWall;
    
    public bool isGrounded;
    bool isWallRunning;
    bool wallRight;
    bool wallLeft;

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


    // Start is called before the first frame update
    void Start()
    {
        //getting the rigidbody
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ///Summmary
        ///Inputs for player movement on Horizontal and Vertical axis
        ///GetAxisRaw for more "snapy" movement
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;

        //GroundCheck
        //Creating a sphere check for a ground check and checking if the player is grounded
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, 
                                                     transform.position.z), 0.4f, groundCheck);

        if (isGrounded && rb.velocity.magnitude > 5 && rb.velocity.y == 0)
            runningAudio.enabled = true;

        else
            runningAudio.enabled = false;
        //Jump
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jump, rb.velocity.z);
            jumpingAudio.Play();
        }
        

        Vector3 movePos = transform.right * x + transform.forward * y;
        Vector3 newPos = new Vector3(movePos.x, rb.velocity.y, movePos.z);

        rb.velocity = newPos;    

        WallRun();
        WallRunCheck();
    }
    /// <summary>
    /// If inputs A or D are pressed, it will start the wall run
    /// </summary>
    private void WallRun()
    {
        if (Input.GetKey(KeyCode.D) && wallRight) 
            WallRunStart();
        if (Input.GetKey(KeyCode.A) && wallLeft) 
            WallRunStart();
    }
    /// <summary>
    /// Disabling the use of gravity & enables wall running
    /// If the rigidbody magnitude is less than or equaled to the max speed
    ///     add force to the orintation 
    /// Checks for the wall if it's on the right or left
    ///     add force to the orintation 
    ///     else flip orintation if left
    /// </summary>
    private void WallRunStart()
    {
        rb.useGravity = false;
        isWallRunning = true;

        if(rb.velocity.magnitude <= maxSpeedWall)
        {
            rb.AddForce(orientation.forward * wallRunForce * Time.deltaTime);

            if (wallRight)
                rb.AddForce(orientation.right * wallRunForce / 5 * Time.deltaTime);
            else
                rb.AddForce(-orientation.right * wallRunForce / 5 * Time.deltaTime);
        }

    }
    /// <summary>
    /// 
    /// </summary>
    private void WallRunStop()
    {
        rb.useGravity = true;
        isWallRunning = false;

    }

    private void WallRunCheck()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, 1f, isWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, isWall);

        if (!wallLeft && !wallRight) 
            WallRunStop();

    }

    void OnCollisionEnter(Collision other)
    {
        thudAudio.Play();
        shaker.Shake(shakerPreset);
    }

}
