using UnityEngine;

public class Advanced2DController : MonoBehaviour
{

// variables

[Header("WHATS ACTIVATED")]
public bool movement = true;
public bool jumping = true;
public bool wallJumping = true;
public bool crouching = false;
public bool sliding = true;
public bool cameraFollow = true;

[Header("CONTROLS; W A S D")]
public KeyCode[] controls = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};

[Header("MOVEMENT")]
public float force = 550f;
public int howManyJumps = 1;
private int howManyJumpsR;
private float forceR;


[Header("JUMPING")]
public LayerMask whatIsGround;
public float jumpForce = 600f;
public bool isGrounded;

[Header("MOVEMENT DETECTION / FOOTSTEPS")]
public bool isMoving;
public bool isMovingOnGround;
public Vector2 velocity;
public Vector2 playerPos;

[Header("WALL JUMPING")]
public LayerMask whatIsWall;
public bool isOnWall;
public int HMJOffOfWall = 1;

[Header("CROUCHING")]
public float crouchSpeed = 275f;
public bool isCrouching;

[Header("SLIDING")]
public bool isSliding;
public float slideSpeedMultiplier = 5.5f;

[Header("SLIDE & CROUCH COLLIDERS; SIZEX SIZEY OFFSETX OFFSETY")]
public float[] crouchSlideColliders = {1f, 0.5f, 0f, 0f};
private float[] crouchSlideCollidersR = new float[4];

[Header("CAMERA MOVEMENT")]
public Camera cam;
public float smoothing = 0.1f;
public Vector2 offset = new Vector2(0f, 2f);

Rigidbody2D rb;
BoxCollider2D bc;

private bool A = false;
private bool B = true;
private bool C = false;

// voids

    void Start()
    {
        Rememberer();
    }

    void Update()
    {
        if (movement) 
        {
            if (!isSliding) 
            {
                Movement();
                CounterMovement();
            }
        }
        if (howManyJumps > 0 && Input.GetKeyDown(controls[0]) && isCrouching == false && jumping) 
        {
            Jump();
        }
        Updater();
        if (isGrounded == true && crouching) 
        {
            Crouch();
        }
        if (wallJumping) 
        {
            Wall();
        }
        Clamp();
        if (sliding) 
        {
            Sliding();
        }
    }

    void LateUpdate() 
    {
        if (cameraFollow) 
        {
            CameraFollow();
        }
        MovementDetection();
    }

private void Rememberer() 
{
    bc = GetComponent<BoxCollider2D>();
    rb = GetComponent<Rigidbody2D>();
    crouchSlideCollidersR[0] = bc.size.x;
    crouchSlideCollidersR[1] = bc.size.y;
    crouchSlideCollidersR[2] = bc.offset.x;
    crouchSlideCollidersR[3] = bc.offset.y;
    forceR = force;
    howManyJumpsR = howManyJumps;
}

private void MovementDetection() 
{
// movement detection

    if (rb.velocity.magnitude > 0.1f) 
    {
        isMoving = true;
    } else 
    {
        isMoving = false;
    }
    if (rb.velocity.magnitude > 0.1f && isGrounded == true) 
    {
        isMovingOnGround = true;
    } else if (isGrounded == true || isGrounded == false) 
    {
        isMovingOnGround = false;
    }

velocity = rb.velocity;

}

private void Jump() 
{
    rb.AddForce(Vector2.up * jumpForce);
    howManyJumps --;
}

private void OnTriggerStay2D(Collider2D collider) 
{
    isGrounded = collider != null && (((1 << collider.gameObject.layer) & whatIsGround) != 0);
    isOnWall = collider != null && (((1 << collider.gameObject.layer) & whatIsWall) != 0);
}

private void OnTriggerExit2D(Collider2D collision) 
{
    isGrounded = false;
    isOnWall = false;
    if (isGrounded == false) 
    {
        howManyJumps --;
    }
}

private void Updater() 
{
playerPos = this.transform.position;
    if (isGrounded == true) 
    {
        howManyJumps = howManyJumpsR;
    }
}

private void Movement() 
{
    if (Input.GetKeyDown(controls[1])) 
    {
        rb.AddForce(Vector2.left * force);
        // flip
        if (A == false && B == true) 
        {
        Vector2 flip = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        transform.localScale = flip;
        A = true;
        B = false;
        }
        
    }
        if (Input.GetKeyDown(controls[3])) 
    {
        rb.AddForce(Vector2.right * force);
                if (A == true && B == false) 
        {
        Vector2 flip = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        transform.localScale = flip;
        A = false;
        B = true;
        }
    }
}

private void CounterMovement() 
{
        if (Input.GetKeyUp(controls[1])) 
    {
        rb.velocity = Vector2.left * 0;
    }
        if (Input.GetKeyUp(controls[3])) 
    {
        rb.velocity = Vector2.right * 0;
    }
}

private void Crouch() 
{
    if (Input.GetKeyDown(controls[2])) 
    {
    rb.velocity = Vector2.zero;
    force = crouchSpeed;
    bc.size = new Vector2(crouchSlideColliders[0], crouchSlideColliders[1]);
    bc.offset = new Vector2(crouchSlideColliders[2], crouchSlideColliders[3]);
    
    }

if (Input.GetKeyUp(controls[2]))
    {
    rb.velocity = Vector2.zero;
    force = forceR;
    bc.size = new Vector2(crouchSlideCollidersR[0], crouchSlideCollidersR[1]);
    bc.offset = new Vector2(crouchSlideCollidersR[2], crouchSlideCollidersR[3]);
    isCrouching = false;
    } 
}

private void Wall() 
{
    if (isOnWall == true) 
    {
        howManyJumps = 69420;
        C = true;
    } else 
    {
        if (C == true) 
        {
        howManyJumps = HMJOffOfWall;
        C = false;
        }
    }
}

private void Clamp() 
{
    if (howManyJumps < 0) 
    {
        howManyJumps = 0;
    }
}

private void Sliding() 
{
    if (Input.GetKey(controls[2])) 
    {
        Vector2 slide = new Vector2(Time.deltaTime, 0);
        if (rb.velocity.x > 0f) 
        {
            rb.velocity = rb.velocity - (slide * slideSpeedMultiplier);
        }
        if (rb.velocity.x < 0f) 
        {
            rb.velocity = rb.velocity + (slide * slideSpeedMultiplier);
        }
        bc.size = new Vector2(crouchSlideColliders[0], crouchSlideColliders[1]);
        bc.offset = new Vector2(crouchSlideColliders[2], crouchSlideColliders[3]);    
        isSliding = true;
    }
    if (Input.GetKeyUp(controls[2])) 
    {
        bc.size = new Vector2(crouchSlideCollidersR[0], crouchSlideCollidersR[1]);
        bc.offset = new Vector2(crouchSlideCollidersR[2], crouchSlideCollidersR[3]);
        rb.velocity = Vector2.zero;
        isSliding = false;
    }
}

private void CameraFollow() 
{
    Vector3 vel = Vector3.zero;
    cam.transform.position = Vector3.SmoothDamp(cam.transform.position, new Vector3(this.transform.position.x + offset.x, this.transform.position.y + offset.y, -10f), ref vel, smoothing);
}

}

// subscribe to adrenalyzed and adrenalyzedcantcodeproperly on youtube!!!! ;3
