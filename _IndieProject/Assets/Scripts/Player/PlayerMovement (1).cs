using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveX;
    private float facingDirection = 1f;
    private SpriteRenderer flip;
    [SerializeField] private Animator anim;
    [SerializeField] private float speed;
    [SerializeField] private float Jspeed = 5f;
    private PlayerHealth playerHealth;
    public float atkRate = 2f;
    float currentAtkTime = 0;
    Vector2 currentVelocity;
    public Transform groundChecker;
    [SerializeField] private LayerMask[] grounds;
    private LayerMask combinedLayerMask;
    public float checkRadius;
    private AudioManager audioManager;

    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 10;
    public float dashCooldown = 1f;
    private float dashTime = 0.2f;
    private float dashDirection; // Store the dash direction
    public TrailRenderer tr;

    private bool doubleJump;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flip = GetComponent<SpriteRenderer>();
        CombineLayerMasks();
    }

    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            SceneManager.LoadScene(0);
        }

        if (isDashing)
        {
            return; // Very important: Exit Update while dashing
        }

        moveX = Input.GetAxisRaw("Horizontal");

        if (moveX != 0)
        {
            facingDirection = moveX;
        }

        currentVelocity = rb.linearVelocity;
        animation2d();

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.linearVelocity = new Vector2(currentVelocity.x, Jspeed);
                doubleJump = false;
            }
            else if (!doubleJump)
            {
                rb.linearVelocity = new Vector2(currentVelocity.x, Jspeed);
                doubleJump = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashingPower, 0f); // Apply dash force in FixedUpdate
            return; // Very important: Exit FixedUpdate while dashing
        }

        movement();
    }

    private void movement()
    {
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }

    void animation2d()
    {
        if (moveX > 0.1f)
        {
            anim.SetBool("IsRunning", true);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveX < 0f)
        {
            anim.SetBool("IsRunning", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }

        if (Time.time >= currentAtkTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                audioManager.playSFX(audioManager.swordSwing);
                currentAtkTime = Time.time + 1f / atkRate;
                anim.SetTrigger("Attack");
            }
        }
    }

    private void StartDash()
    {
        if (!canDash || isDashing) return;

        canDash = false;
        isDashing = true;
        dashDirection = facingDirection;
        tr.emitting = true;

        StartCoroutine(StopDashCoroutine());
    }

    private IEnumerator StopDashCoroutine()
    {
        yield return new WaitForSeconds(dashTime);
        StopDash();
    }

    private void StopDash()
    {
        isDashing = false;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        tr.emitting = false;
        StartCoroutine(DashCooldownCoroutine());
    }

    private IEnumerator DashCooldownCoroutine()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void CombineLayerMasks()
    {
        combinedLayerMask = 0;
        foreach (LayerMask mask in grounds)
        {
            combinedLayerMask |= mask;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundChecker.position, checkRadius, combinedLayerMask);
    }
}