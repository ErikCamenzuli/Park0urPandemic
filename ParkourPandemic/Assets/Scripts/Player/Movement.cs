using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 wallRunDir;

    [Header("Checks")]
    public bool isGrounded;
    public bool isWallRunning;

    [Header("Stats")]
    public float speed = 10f;
    public float jump = 3f;
    public float wallRunForce;

    [Header("Audio")]
    public AudioSource runningAudio;
    public AudioSource jumpingAudio;
    public AudioSource thudAudio;

    [Header("Misc.")]
    public LayerMask wallRunMask;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CanWallRun();
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;

        //Audio
        if (isGrounded && rb.velocity.magnitude > 5 && rb.velocity.y == 0)
            runningAudio.enabled = true;
        else
            runningAudio.enabled = false;

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded))
        {
            rb.velocity = new Vector3(rb.velocity.x, jump, rb.velocity.z);
            jumpingAudio.Play();
            isGrounded = false;
            rb.useGravity = true;
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
                    Debug.Log(rightHit.normal);
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
    private void OnCollisionStay(Collision other)
    {
        ContactPoint point = other.GetContact(0);
        if (point.normal.y > 0.1f)
        {
            isGrounded = true;
            Debug.Log(other.transform.gameObject.name);
        }
    }

}
