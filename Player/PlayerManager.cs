using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator2D))]
public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerInput pi;
    private Rigidbody2D rb;

    public enum Player
    {
        ONE,
        TWO
    }
    
    
    public float movementSpeed = 5.0f;
    public float movementSpeedMod = 1.0f;
    public float invert = 1.0f;

    public float lifeTimer;

    private bool grounded;
    private bool lastGrounded;
    [HideInInspector]
    public float jumpForce;
    public float defaultJumpForce;
    public float jumpTime;
    public float jumpTimeCounter;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool stoppedJumping;

    public float upperBoundScreen = 5;
    public float lowerBoundScreen = -5;

    private bool playerDead = false;

    private bool playerReallyDead = false;

    //Animation stuff
    [SerializeField] private Animator2D animator;
    enum AnimationStates
    { 
        IDLE                = 0,
        RUNNING             = 1,
        JUMPING_ASCENDING   = 2,
        JUMPING_APEX        = 3,
        JUMPING_DESCENDING  = 4,
        BONKING             = 5,
        DYING               = 6,
        CLOWN_MOD           = 7
    }

    private AnimationStates prevAnimState;
    private AnimationStates newAnimState;

    private bool faceRight;
    private bool clownMode;

    [SerializeField] private Player player = Player.ONE;
    private string jumpKey;

    
    // Start is called before the first frame update
    void Start()
    {
        jumpForce = defaultJumpForce;
        pi = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        delay = footStepdelay;

        jumpKey = player == Player.ONE ? "Jump1" : "Jump2";

        pi.SetPlayer(player);
        
        animator = gameObject.GetComponent<Animator2D>();
        prevAnimState = AnimationStates.IDLE;
        faceRight = false;
        clownMode = false;
    }
    public float footStepdelay = 0.1f;
    private bool timerFootstep = false;
    private float delay;

    // Update is called once per frame
    void Update()
    {
        if (!playerDead)
        {
            //lifeTimer -= Time.deltaTime;

            if(lifeTimer < 0.0f)
            {
                PlayerDeath();
            }
            Move();
            Jump();
            SpikeExcitement();
            UpdateAnimations();

            if (timerFootstep)
            {
                delay -= Time.deltaTime;
                if (delay <= 0.0f)
                {
                    int rand = (int)Random.Range(0, 2);
                    SoundManager sm = SoundManager.Instance;
                    switch (rand)
                    {
                        case (0):
                            sm.PlaySound(SoundManager.SoundNames.walkingSlow);
                            break;
                        case (1):
                            sm.PlaySound(SoundManager.SoundNames.walkingNormal);
                            break;
                        case (2):
                            sm.PlaySound(SoundManager.SoundNames.walkingFast);
                            break;
                    }
                    timerFootstep = false;
                    delay = footStepdelay;

                }
            }
        }

        
    }

    private void LateUpdate()
    {
        if(prevAnimState == AnimationStates.DYING && !playerReallyDead)
        {
            if(animator.lastFrameFlag)
            {
                GameManager gm = GameManager.Instance;
                gm.EndGame(player == Player.TWO);
                animator.SetPaused(true);
                playerReallyDead = true;
            }
        }
    }

    void Move()
    {

        rb.velocity = new Vector2(pi.GetHorizontalInput() * movementSpeed * movementSpeedMod * invert, rb.velocity.y);

        if(rb.velocity.x > 0)
        {
            GetComponentInParent<SpriteRenderer>().flipX = true;
        }
        else if(rb.velocity.x < 0)
        {
            GetComponentInParent<SpriteRenderer>().flipX = false;
        }
        timerFootstep = pi.GetHorizontalInput() != 0;
        if(timerFootstep && grounded)
        {
            timerFootstep = true;
        }
        else
        {
            timerFootstep = false;
        }

    }
    void Jump()
    {

        //I placed this code in FixedUpdate because we are using phyics to move.
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        SoundManager sm = SoundManager.Instance;

        //Grounded Check
        if (!lastGrounded && grounded)
        {
            int rand = Random.Range(0, 1);

            switch (rand)
            {
                case (0):
                    sm.PlaySound(SoundManager.SoundNames.impactToPlatform);
                    break;
                case (1):
                    sm.PlaySound(SoundManager.SoundNames.impactToPlatform2);
                    break;

            }
        }
        
        if (Input.GetButtonDown(jumpKey))
        {
            if (grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                stoppedJumping = false;

                //SOUND
                int rand = (int)Random.Range(0, 1);
                
                switch (rand)
                {
                    case (0):
                        sm.PlaySound(SoundManager.SoundNames.jump);
                        break;
                    case (1):
                        sm.PlaySound(SoundManager.SoundNames.jump2);
                        break;
                }
            }
        }

        if ((pi.GetVerticalInput() > 0) && !stoppedJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }


        if (pi.GetVerticalInput() == 0)
        {
            //stop jumping and set your counter to zero.  The timer will reset once we touch the ground again in the update function.
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
        lastGrounded = grounded;

    }

    void SpikeExcitement()
    {
        float temp;
        float lerpprep;
        //could use a selector but its already done now, fuck it
        if (lowerBoundScreen < 0)
        {
            temp = Mathf.Abs(lowerBoundScreen) + upperBoundScreen;
            lerpprep = (rb.position.y + Mathf.Abs(lowerBoundScreen)) / temp;
        }
        else
        {
            temp = upperBoundScreen - lowerBoundScreen ;
            lerpprep = (rb.position.y + Mathf.Abs(lowerBoundScreen)) / temp;
        }

        //no assert becuz cool
        //GetComponentInParent<CrowdPleaser>().crowdMoodModMod = Mathf.Lerp(0.25f, 5.0f, lerpprep);
    }

    void UpdateAnimations()
    {
        if(prevAnimState == AnimationStates.DYING)
        {
            return;
        }
        if (grounded)
        {
            
            if (rb.velocity.x < 0.1f && rb.velocity.x > -0.1f)
            {
                newAnimState = AnimationStates.IDLE;
            }
            else
            {
                newAnimState = AnimationStates.RUNNING;
            }
        }
        else
        {
            if(rb.velocity.y > 1.5f)
            {
                newAnimState = AnimationStates.JUMPING_ASCENDING;
            }
            else if(rb.velocity.y < -1.5f)
            {
                newAnimState = AnimationStates.JUMPING_DESCENDING;
            }
            else 
            {
                newAnimState = AnimationStates.JUMPING_APEX;
            }
        }

        if(newAnimState != prevAnimState)
        {
            int animIndex = !clownMode ? (int)newAnimState : (int)newAnimState + (int)AnimationStates.CLOWN_MOD;
            animator.ChangeAnimation(animIndex);

            prevAnimState = newAnimState;
        }


        bool currentDir = (rb.velocity.x > 0.0f);
        if(currentDir != faceRight)
        {
            faceRight = currentDir;
            animator.Flip(faceRight);
        }
    }

    public void SetClownMode(bool mode)
    {
        clownMode = mode;
    }
    public bool IsClownMode()
    {
        return clownMode;
    }
    public void PlayerDeath()
    {
        
        prevAnimState = AnimationStates.DYING;
        playerDead = true; //haha fuck your anim state buddy
        animator.ChangeAnimation((int)prevAnimState);
        animator.updateSpeed = 0.05f;
        rb.velocity = new Vector2(rb.velocity.x, -6.0f);
        rb.gravityScale = 0.0f;
    }
}
