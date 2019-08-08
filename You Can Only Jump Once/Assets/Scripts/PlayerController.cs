using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    public float jumpForce = 1.0f;
    public float slideSpeed = 1.0f;

    public GameObject bulletPrefab;
    public GameObject afterimage;
    public LayerMask hazardLayer;
    public LayerMask goalLayer;
    public Animator anim;
    public SpriteRenderer sprite;
    public GameObject dashIndicator;
    public GameObject reticule;
    public GameObject burst;

    public AudioClip jump;
    public AudioClip dash;
    public AudioClip shoot;
    public AudioClip splosion;
    public AudioClip laserHit;
    public AudioClip spikesHit;
    public AudioClip goal;

    Rigidbody2D rb;
    PlayerCollisions coll;

    bool hasControl = true;
    bool enableWallSlide = true;
    bool dashed = false;
    float facing = 1;
    bool applyFall = true;
    bool prevOnGround = false;
    bool reachedGoal = false;

    // actions
    public bool canJump = true;
    public bool canDash = true;
    public bool canSlide = true;
    public bool canShoot = true;
    public bool canWalljump = true;
    public bool canGetHit = true;
    public bool canDie = true;
    public bool canStopTime = true;

    public bool dead = false;

    //original
    bool originalCanJump = true;
    bool originalCanDash = true;
    bool originalCanSlide = true;
    bool originalCanShoot = true;
    bool originalCanWalljump = true;
    bool originalCanGetHit = true;
    bool originalCanDie = true;
    bool originalCanStopTime = true;

    float originalGravityScale = 1;
    Vector2 originalPosition;
    Vector2 storedVelocity;

    public bool testing = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<PlayerCollisions>();

        originalGravityScale = rb.gravityScale;
        originalPosition = transform.position;

        originalCanJump = canJump;
        originalCanDash = canDash;
        originalCanSlide = canSlide;
        originalCanShoot = canShoot;
        originalCanWalljump = canWalljump;
        originalCanGetHit = canGetHit;
        originalCanDie = canDie;
        originalCanStopTime = canStopTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (reachedGoal || dead || GameManager.instance.paused)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(h, v);

        if (hasControl)
            Walk(dir);

        if (Input.GetButtonDown("Jump"))
        {
            if (coll.onGround && originalCanJump)
                Jump();

            if ((coll.onLeftWall || coll.onRightWall) && originalCanWalljump)
                WallJump();
        }

        if (!Input.GetButton("Jump") && applyFall)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
            }
        }

        if (Input.GetButtonDown("Fire1") && originalCanDash)
        {
            Dash(h, v);
        }

        if (Input.GetButtonDown("Fire2") && originalCanShoot)
        {
            Shoot();
        }

        if ((coll.onLeftWall || coll.onRightWall) && !coll.onGround)
        {
            SlideDownWall();
        }

        if (Physics2D.OverlapCircle((Vector2)transform.position, 0.25f, goalLayer))
        {
            Debug.Log("Goal");
            reachedGoal = true;
            Goal();
        }


        if (Physics2D.OverlapCircle((Vector2)transform.position, 0.25f, hazardLayer))
        {
            Hit();
            SFXManager.instance.PlaySFX(spikesHit);

        }

        if(transform.position.y < -8 ||
           transform.position.y > 7.5f ||
           transform.position.x > 12 ||
           transform.position.x < -12)
        {
            Explode();
        }

        UpdateColor();
        reticule.SetActive(canShoot);
        dashIndicator.SetActive(canDash);

        if(coll.onGround && !prevOnGround)
            anim.SetTrigger("squash");

        prevOnGround = coll.onGround;
    }

    void Walk(Vector2 dir)
    {
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

        if(dir.x != 0)
            facing = Mathf.Sign(dir.x);
    }

    void Jump()
    {
        if (!canJump && !testing)
        {
            Explode();
            return;
        }

        SFXManager.instance.PlaySFX(jump);

        //sprite.color = Color.blue;

        anim.SetTrigger("stretch");

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce;

        canJump = false;
    }

    void WallJump()
    {
        if (!canWalljump && !testing)
        {
            Explode();
            return;
        }

        SFXManager.instance.PlaySFX(jump);

        //sprite.color = Color.magenta;

        enableWallSlide = false;
        TimerManager.instance.AddTimer(EnableSlideDownWall, 0.5f);
        float wallDir = coll.onRightWall ? -1 : 1;
        rb.velocity = new Vector2(7 * wallDir, jumpForce);
        hasControl = false;
        TimerManager.instance.AddTimer(EnableControl, 0.25f);

        canWalljump = false;
    }
    
    void SlideDownWall()
    {
        if (!enableWallSlide)
            return;

        rb.velocity = new Vector2(0, -slideSpeed);
    }

    Vector2 GetMouseDirNormalized()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorldPos - transform.position);
        dir.Normalize();

        return dir;
    }

    void Dash(float h, float v)
    {
        if (!canDash && !testing)
        {
            Explode();
            return;
        }

        SFXManager.instance.PlaySFX(dash);

        applyFall = false;
        hasControl = false;

        rb.velocity = Vector2.zero;
        //rb.velocity += new Vector2(h, v).normalized * 15;
        rb.velocity += GetMouseDirNormalized() * 15;

        TimerManager.instance.AddTimer(EnableControl, 0.2f);

        //sprite.color = Color.green;

        canDash = false;

        TimerManager.instance.AddTimer(AddAfterimage, 0.05f);
        TimerManager.instance.AddTimer(AddAfterimage, 0.10f);
        TimerManager.instance.AddTimer(AddAfterimage, 0.15f);
        TimerManager.instance.AddTimer(AddAfterimage, 0.2f);
        TimerManager.instance.AddTimer(AddAfterimage, 0.25f);
        TimerManager.instance.AddTimer(AddAfterimage, 0.3f);
        TimerManager.instance.AddTimer(AddAfterimage, 0.35f);
    }

    void Shoot()
    {
        if (!canShoot && !testing)
        {
            Explode();
            return;
        }

        SFXManager.instance.PlaySFX(shoot);

        applyFall = false;
        hasControl = false;

        rb.velocity = Vector2.zero;
        //rb.velocity += new Vector2(h, v).normalized * 15;
        rb.velocity += -GetMouseDirNormalized() * 15;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = -rb.velocity;

        TimerManager.instance.AddTimer(EnableControl, 0.2f);

        canShoot = false;
    }

    void UpdateColor()
    {
        sprite.color = Color.Lerp(sprite.color, Color.white, Time.deltaTime * 2 * GameManager.instance.speed);
    }

    void DisableInvincibility()
    {
        canGetHit = false;
    }

    public void Hit()
    {
        Debug.Log("HURT");

        if (canGetHit)
        {
            bool doAdd = true;
            foreach (Timer t in TimerManager.instance.timers)
            {
                if (t.timerName == "playerInv")
                    doAdd = false;
            }

            if (doAdd)
                TimerManager.instance.AddTimer(DisableInvincibility, .5f, "playerInv");
        }

        if (!canGetHit)
        {
            Explode();
            return;
        }
    }

    void Explode()
    {
        SFXManager.instance.PlaySFX(splosion);

        Instantiate(burst, transform.position, transform.rotation);

        if (canDie)
        {
            canDie = false;

            canDash = originalCanDash;
            canJump = originalCanJump;
            canShoot = originalCanShoot;

            hasControl = true;
            rb.velocity = Vector2.zero;
            transform.position = originalPosition;
            Instantiate(burst, transform.position, transform.rotation);

            return;
        }

        sprite.enabled = false;
        canDash = false;
        canDie = false;
        canGetHit = false;
        canJump = false;
        canShoot = false;
        canWalljump = false;
        canStopTime = false;

        dead = true;


        GameManager.instance.RestartLevel();
        //Respawn();
    }

    void Goal()
    {
        SFXManager.instance.PlaySFX(goal);
        GameManager.instance.GoToNextLevel();
        //rb.velocity = Vector2.zero;
        //rb.gravityScale = 0;
    }

    public void Pause()
    {
        storedVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    public void Unpause()
    {
        rb.velocity = storedVelocity;
        ResetGravityScale();
    }

    // re enable stuff
    void ResetGravityScale()
    {
        rb.gravityScale = originalGravityScale;
    }

    void EnableControl()
    {
        hasControl = true;
        rb.velocity = Vector2.zero;
        applyFall = true;
    }

    void EnableSlideDownWall()
    {
        enableWallSlide = true;
    }

    void AddAfterimage()
    {
        GameObject g = Instantiate(afterimage, transform.position, transform.rotation);
        //g.GetComponent<SpriteRenderer>().color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, g.GetComponent<SpriteRenderer>().color.a);
    }

    void Respawn()
    {
        
        sprite.enabled = true;
        dead = false;
        hasControl = true;
        rb.velocity = Vector2.zero;
        transform.position = originalPosition;
        canDash = originalCanDash;
        canJump = originalCanJump;
        canShoot = originalCanShoot;
        canWalljump = originalCanWalljump;
        canGetHit = originalCanGetHit;
        canDie = originalCanDie;
        canStopTime = originalCanStopTime;
        Instantiate(burst, transform.position, transform.rotation);
    }
}

/**
 * 
 * void Dash()
    {
        if (!canDash && !testing)
        {
            Explode();
            return;
        }
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorldPos - transform.position);
        dir.Normalize();
        Debug.Log(dir);

        // if left of center
        if(dir.x < -0.5)
        {
            // if up-left
            if (dir.y > 0.5)
            {
                rb.velocity = new Vector2(speed * -2, jumpForce);
            }

            // if down-left
            else if (dir.y < -0.5f)
            {
                rb.velocity = new Vector2(speed * -2, -jumpForce);
            }

            // straight left
            else
            {
                rb.velocity = new Vector2(speed * -2, 0);
            }
        }
        // if right of center
        else if(dir.x > 0.5)
        {
            // if up-right
            if(dir.y > 0.5)
            {
                rb.velocity = new Vector2(speed * 2, jumpForce);
            }
            
            // if down-right
            else if(dir.y < -0.5f)
            {
                rb.velocity = new Vector2(speed * 2, -jumpForce);
            }

            // straight right
            else
            {
                rb.velocity = new Vector2(speed * 2, 0);
            }

        }
        else
        {
            // dash up
            if(dir.y > 0)
            {
                rb.velocity = new Vector2(0, 0);
                rb.velocity += Vector2.up * jumpForce;
            }

            // dash down
            else
            {
                rb.velocity = new Vector2(0, -jumpForce);
            }
        }
        dashed = true;

        //rb.velocity = new Vector2(speed * 2, 0);
        //rb.velocity = dir * speed * 2;
        hasControl = false;
        rb.gravityScale = 0;
        TimerManager.instance.AddTimer(ResetGravityScale,0.2f);
        TimerManager.instance.AddTimer(EnableControl, 0.2f);

        canDash = false;
    }
**/
