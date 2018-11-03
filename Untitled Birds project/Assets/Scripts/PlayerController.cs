using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float moveAcceleration;
    public float moveDeceleration;
    public float reversalFactor;
    public float fallingSpeedCap;
    public float gravityScale;
    public float currentSpeedPenalty;
    public float waterSpeedPenalty;

    public bool jumpQueued;
    public float lateJumpToleranceTime;
    public float lateJumpToleranceCounter;

    [Range(1, 10)]
    public float jumpSpeed;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private float activeMoveSpeed;

    public bool canMove;

    public bool isGrounded;
    public LayerMask whatIsGround;
    public Vector3 respawnPosition;

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    private LevelManager levelManager;

    public bool hatless;

    public float groundCheckHeight;

    public AudioSource jumpSound;

    private Vector2 leftFoot;
    private Vector2 rightFoot;

    private bool onPlatform;
    public float onPlatformSpeedModifier;
    public BoxCollider2D _collider;

    public ParticleSystem bubbles;

    public float timeToDrown;
    public float drowningCounter;
    public bool drowning;
    public float baseAirBubbleGravity;
    public float baseAirBubbleRate;
    public float gravityMultiplier = -0.1f;
    public float rateMultiplier = 0.1f;

    public bool autoMoveRight;
    public bool autoMoveLeft;
    public SpeechBubble leftBubble;
    public SpeechBubble rightBubble;

    public ParticleSystem.MainModule newMain;
    public ParticleSystem.EmissionModule newEmission;

    // Use this for initialization
    void Start ()
    {
        levelManager = FindObjectOfType<LevelManager>();
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;

        activeMoveSpeed = moveSpeed;
        // canMove = true;

        _collider = GetComponent<BoxCollider2D>();
        leftFoot = new Vector2(-_collider.size.x / 2f + _collider.offset.x * transform.localScale.x, _collider.offset.y - _collider.size.y / 2f);
        rightFoot = new Vector2(_collider.size.x / 2f + _collider.offset.x * transform.localScale.x, _collider.offset.y - _collider.size.y / 2f);
        

        rb.gravityScale = gravityScale;

        currentSpeedPenalty = 1;

        newMain = bubbles.main;
        newEmission = bubbles.emission;
    }

    private void Update()
    {
        // Drowning
        if (drowning)
        {
            drowningCounter -= Time.deltaTime;

            ParticleSystem.MainModule newMain = bubbles.main;
            ParticleSystem.EmissionModule newEmission = bubbles.emission;
            if (drowningCounter < 0)
            {
                // Game over
                newEmission.enabled = false;
                canMove = false;
                spriteRenderer.color = Color.black;
                bubbles.Clear();
                levelManager.GameOver();
                drowning = false;
            }

            // dT * x + y
            // y = ln(start)
            // x = ln(finish) + (l

            float t = (timeToDrown - drowningCounter) * .12f - 1.6f;
            float bubbleRate = Mathf.Exp(t);

            t = (timeToDrown - drowningCounter) * 0.1f - 4.5f;
            float bubbleGravity = -Mathf.Exp(t);

            
            

            
            newMain.gravityModifierMultiplier = bubbleGravity;
            newEmission.rateOverTimeMultiplier = bubbleRate;
            
        }

        // Jump tolerance counter
        if (lateJumpToleranceCounter > 0)
            lateJumpToleranceCounter -= Time.deltaTime;

        if (Input.GetButtonUp("Jump"))
            jumpQueued = false;

        // Hat check

        if (hatless)
            anim.SetBool("Hatless", hatless);
    }

    void FixedUpdate ()
    {
        bool wasGrounded;
        Transform oldParent = transform.parent;

        // Previously = 0
        float velocity = rb.velocity.x;

        // Grounded check
        wasGrounded = isGrounded;
        Collider2D groundCollider = Physics2D.OverlapArea((Vector2)transform.position + leftFoot, (Vector2)transform.position + rightFoot - new Vector2(0, groundCheckHeight), whatIsGround);
        isGrounded = (groundCollider != null);

        if (isGrounded)
            transform.parent = groundCollider.transform;

        if (isGrounded)
        {
            lateJumpToleranceCounter = 0;

            // On landing
            if (!wasGrounded)
            {
                if (oldParent != null && oldParent.tag == "OneWayPlatform")
                    oldParent.GetComponent<PlatformEffector2D>().rotationalOffset = 0;
            }
        }
        else if (wasGrounded && rb.velocity.y <= 0)
        {
            lateJumpToleranceCounter = lateJumpToleranceTime;
        }


        
        {
            if (onPlatform)
            {
                activeMoveSpeed = moveSpeed * onPlatformSpeedModifier;
            }
            else
            {
                activeMoveSpeed = moveSpeed;
            }


            // Left-right movement
            if ((Input.GetAxisRaw("Horizontal") > 0f && canMove) || autoMoveRight)
            {
                if (rb.velocity.x < 0)
                    velocity = rb.velocity.x + reversalFactor * currentSpeedPenalty * moveDeceleration * Time.deltaTime;
                else
                    velocity = Mathf.Min(rb.velocity.x + moveAcceleration * currentSpeedPenalty * Time.deltaTime, moveSpeed);

                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if ((Input.GetAxisRaw("Horizontal") < 0f && canMove) || autoMoveLeft)
            {
                if (rb.velocity.x > 0)
                    velocity = rb.velocity.x - reversalFactor * currentSpeedPenalty * moveDeceleration * Time.deltaTime;
                else
                    velocity = Mathf.Max(rb.velocity.x + currentSpeedPenalty * -moveAcceleration * Time.deltaTime, -moveSpeed);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                // Decelerate
                if (rb.velocity.x > 0)
                {
                    velocity = Mathf.Max(rb.velocity.x - currentSpeedPenalty * moveDeceleration * Time.deltaTime, 0);
                }
                else if (rb.velocity.x < 0)
                {
                    velocity = Mathf.Min(rb.velocity.x + currentSpeedPenalty * moveDeceleration * Time.deltaTime, 0);
                }
            }

            rb.velocity = new Vector3(velocity, rb.velocity.y, 0);

            // You can jump IF
            //  - you press jump and you are grounded, or
            //  - you press jump while you are not grounded, within the late jump window, or
            //  - you become grounded while while a jump was queued (i.e., by pressing jump during your descent and holding it until you land)

            if (Input.GetButtonDown("Jump") && canMove)
            {
                if (isGrounded || lateJumpToleranceCounter > 0)
                    InitiateJump();
                else if (rb.velocity.y < 0)
                    jumpQueued = true;
            }
            else if (jumpQueued && isGrounded)
                InitiateJump();
            

            // Fall speed multiplier
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (currentSpeedPenalty * fallMultiplier - 1) * Time.deltaTime;
            }

            // Low jump multiplier
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (currentSpeedPenalty * lowJumpMultiplier - 1) * Time.deltaTime;
            }

            // Descending speed cap
            if (rb.velocity.y < fallingSpeedCap)
                rb.velocity = new Vector2(rb.velocity.x, fallingSpeedCap * currentSpeedPenalty);
        }

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("OnGround", isGrounded);
        anim.SetFloat("SpeedY", rb.velocity.y);
        // myAnim.SetFloat("KB", knockbackCounter);
    }

    public void InitiateJump()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && isGrounded && transform.parent != null && transform.parent.tag == "OneWayPlatform")
        {
            GetComponentInParent<PlatformEffector2D>().rotationalOffset = 180;
        }
        else
        {
            anim.SetBool("Jumping", true);
            StartCoroutine("Jump");
            jumpQueued = false;
        }
    }

    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(.05f);
        anim.SetBool("Jumping", false);

        if (isGrounded || (lateJumpToleranceCounter > 0 && rb.velocity.y < 0))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed * Mathf.Sqrt(currentSpeedPenalty), 0f);
            jumpSound.Play();

            if (lateJumpToleranceCounter > 0 && !isGrounded)
                Debug.Log("Late jump by " + (lateJumpToleranceTime - lateJumpToleranceCounter) + "s");

            lateJumpToleranceCounter = 0;
        }
        
    }

    public void Submerge()
    {
        drowning = true;
        drowningCounter = timeToDrown;
        newEmission.enabled = true;
        currentSpeedPenalty = waterSpeedPenalty;
        rb.gravityScale = gravityScale * waterSpeedPenalty;
    }

    public void Emerge()
    {
        drowning = false;
        newEmission.enabled = false;
        rb.gravityScale = gravityScale;
        currentSpeedPenalty = 1;
    }

    private float Mod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
            onPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
            onPlatform = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 left = (Vector2)transform.position + leftFoot;
        Vector2 right = (Vector2)transform.position + rightFoot - new Vector2(0, groundCheckHeight);

        Vector2 center = (left + right) / 2f;
        Vector2 size = (right - left);
        size = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));

        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }
}
