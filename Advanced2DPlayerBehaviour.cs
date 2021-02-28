using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advanced2DPlayerBehaviour : MonoBehaviour
{

//variables

[Header("What's Activated")]

public bool playerMovement = true;
public bool crouching = true;
public bool wallJumping = true;
public bool jumping = true;
public bool lowGravity = true;
[Header("Control Customisation - (Keys) RIGHT, LEFT, JUMP, CROUCH")]


[Header("Player Movement - MAKE SURE THAT GROUND IS ON -Ground- TAG")]

public string[] MainControls;
public string[] SecondaryControls;

[Header("(Mouse) 0 = LEFT CLICK, 1 = RIGHT CLICK, 2 = SCROLL CLICK")]

public int[] MainMB;
public int[] SecondaryMB;

[Header("Game Objects")]
public GameObject feet;

[Header("Decider Variables")]
public float force = 600f;
public float jumpHeight = 475f;
private float jumpHeight1;
public int howManyJumps = 2;
private int howManyJumpsDecider;
private bool A;
public bool isGrounded;

[Header("Low Gravity")]
public float lowGravityAmount = 0.5f;
public float slowMo = 0.8f;
public int timer = 10;

[Header("Crouch Collider Default")]

[Header("Crouching")]

public float defaultOffsetX = 0f;
public float defaultOffsetY = 0f;
public float defaultSizeX = 1f;
public float defaultSizeY = 1f;

[Header("Crouch Collider Customization")]
public float crouchOffsetX = 0f;
public float crouchOffsetY = -0.25f;
public float crouchSizeX = 1f;
public float crouchSizeY = 0.5f;

[Header("Wall Jumping And Sliding - MAKE SURE THAT WALL IS ON -Wall- TAG")]
public bool IsOnWall;
public float onWallJumpHeight = 600f;

[Header("Footsteps / Player Movement Detection")]
public bool isMovingOnGround;
public bool isMoving;

//components
Rigidbody2D rb;
BoxCollider2D bc;

//private booleans

private bool Main;
private bool Secondary;

//voids

void Start() {

howManyJumpsDecider = howManyJumps;

rb = GetComponent<Rigidbody2D>();
bc = GetComponent<BoxCollider2D>();
jumpHeight1 = jumpHeight;

}

    void Update()
    {
if (playerMovement == true) {

        ControlInput();
        ControlSecondaryInput();
        Footsteps();
        CounterMovement();
        CounterSecondaryMovement();
}

if (playerMovement == false) {

rb.velocity = Vector2.up * 0;

}

if (wallJumping == true) {

        PlayerToWallBehaviour();

}

if (crouching == true) {

        Crouching();
    
}

if (jumping == true) {

Jumping();

}

if (lowGravity == true) {

LowGravity();

}


//clamp

        if (isGrounded == true) {

howManyJumps = howManyJumpsDecider;

        }

if (howManyJumps < 0) {

howManyJumps = 0;

}

    }

void ControlInput() {

if (Main == false) {

if (Input.GetKeyDown(MainControls[0])) {

rb.AddForce(-Vector2.right * force);
this.transform.localScale = new Vector2(-1, 1);
Secondary = true;
}

if (Input.GetKeyDown(MainControls[1])) {

rb.AddForce(Vector2.right * force);
this.transform.localScale = new Vector2(1, 1);
Secondary = true;
}

}

}

void ControlSecondaryInput() {

if (Secondary == false) {

if (Input.GetKeyDown(SecondaryControls[0])) {

rb.AddForce(-Vector2.right * force);
this.transform.localScale = new Vector2(-1, 1);
Main = true;

}

if (Input.GetKeyDown(SecondaryControls[1])) {

rb.AddForce(Vector2.right * force);
this.transform.localScale = new Vector2(1, 1);
Main = true;

}
        
}

}
void CounterMovement() {

if (Input.GetKeyUp(MainControls[0])) {

rb.velocity = -Vector2.right * 0;
Secondary = false;

}

if (Input.GetKeyUp(MainControls[1])) {

rb.velocity = Vector2.right * 0;
Secondary = false;

}



}

void CounterSecondaryMovement() {

if (Input.GetKeyUp(SecondaryControls[0])) {

rb.velocity = -Vector2.right * 0;
Main = false;

}

if (Input.GetKeyUp(SecondaryControls[1])) {

rb.velocity = Vector2.right * 0;
Main = false;

}

}

void Footsteps() {

float xMov = Input.GetAxisRaw("Horizontal");
if (xMov != 0) {

isMovingOnGround = true;

}

if (rb.velocity.magnitude < 0.1f) {

isMovingOnGround = false;
isMoving = false;

}

if (isGrounded == false) {

isMovingOnGround = false;

}

if (rb.velocity.magnitude > 0.1f) {

isMoving = true;

}

}

void PlayerToWallBehaviour() {

if (IsOnWall == true) {

howManyJumps = 99999;
jumpHeight = onWallJumpHeight;
A = true;
}

else {

if (A == true) {

howManyJumps = 0;
A = false;
}

jumpHeight = jumpHeight1;

}

}

void OnTriggerEnter2D(Collider2D col) {

if (col.gameObject.tag == "Ground")

isGrounded = true;

}

void OnTriggerExit2D(Collider2D col) {

isGrounded = false;
howManyJumps --;

}

void OnCollisionEnter2D(Collision2D coll) {

if (coll.gameObject.tag == "Wall") {

IsOnWall = true;

}

}

void OnCollisionExit2D(Collision2D coll) {

if (coll.gameObject.tag == "Wall") {

IsOnWall = false;

}

}

void Crouching() {

if (Input.GetKeyDown(MainControls[3])) {

bc.size = new Vector2 (crouchSizeX, crouchSizeY);
bc.offset = new Vector2(crouchOffsetX, crouchOffsetY);

}

if (Input.GetKeyDown(SecondaryControls[3])) {

bc.size = new Vector2 (crouchSizeX, crouchSizeY);
bc.offset = new Vector2(crouchOffsetX, crouchOffsetY);

}

if (Input.GetKeyUp(MainControls[3])) {

bc.size = new Vector2 (defaultSizeX, defaultSizeY);
bc.offset = new Vector2(defaultOffsetX, defaultOffsetY);

}

if (Input.GetKeyUp(SecondaryControls[3])) {

bc.size = new Vector2 (defaultSizeX, defaultSizeY);
bc.offset = new Vector2(defaultOffsetX, defaultOffsetY);

}

}

void Jumping() {

if (Input.GetKeyDown(MainControls[2]) && howManyJumps > 0) {

rb.AddForce(Vector2.up * jumpHeight);
howManyJumps --;

}

if (Input.GetKeyDown(SecondaryControls[2]) && howManyJumps > 0) {

rb.AddForce(Vector2.up * jumpHeight);
howManyJumps --;

}

if (Input.GetKeyUp(MainControls[2])) {

rb.AddForce(Vector2.up * 0);

}

if (Input.GetKeyUp(SecondaryControls[2])) {

rb.AddForce(Vector2.up * 0);

}

}


void LowGravity() {

if (Input.GetMouseButtonDown(MainMB[0])) {

if (timer > 0) {

rb.gravityScale = lowGravityAmount;
Time.timeScale = slowMo;
StartCoroutine(Timer());

}

}

if (Input.GetMouseButtonUp(MainMB[0])) {

rb.gravityScale = 1f;
Time.timeScale = 1f;

}

if (timer <= 0) {

rb.gravityScale = 1f;
Time.timeScale = 1f;

}

}

IEnumerator Timer() {

if (timer > 0) {

yield return new WaitForSeconds(slowMo);
timer --;
if (Input.GetMouseButton(MainMB[0])) {
if (timer >= 0) {

StartCoroutine(Timer());

}


}

}


}

//holy shit this is too much code ;-;

}