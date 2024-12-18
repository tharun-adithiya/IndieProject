using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private float moveX;

    private SpriteRenderer flip;
    [SerializeField] private Animator anim;
    [SerializeField] private float speed;
    [SerializeField] private float Jspeed=5f;

    public float atkRate=2f;
    float currentAtkTime=0;
    Vector2 currentVelocity;
    public Transform groundChecker;
    [SerializeField] private LayerMask[] grounds;
    private LayerMask combinedLayerMask;
    public float checkRadius;
    public AudioManager audioManager;
    CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flip = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        CombineLayerMasks();
    }

    private bool doubleJump;
    void Update()
    {

        moveX = Input.GetAxisRaw("Horizontal");
        currentVelocity = rb.linearVelocity;
        animation2d();


        if (Input.GetButtonDown("Jump"))
        {
            // Jump when grounded or double jump if not grounded
            if (IsGrounded())
            {
                rb.linearVelocity = new Vector2(currentVelocity.x, Jspeed);  // First jump
                doubleJump = false; // Reset double jump on the ground
            }
            else if (!doubleJump)
            {
                rb.linearVelocity = new Vector2(currentVelocity.x, Jspeed);  // Double jump
                doubleJump = true;  // Disable further double jumping
            }
        }



    }
    private void FixedUpdate()
    {
        movement();
        
    }
    private void movement()
    {
        rb.linearVelocity = new Vector2(moveX * speed,currentVelocity.y);
    }
    void animation2d()
    {
        if (moveX > 0.1f)
        {
            anim.SetBool("IsRunning", true);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
        }
        else if (moveX < 0f)
        {
            anim.SetBool("IsRunning", true);
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }

        if (Time.time >=currentAtkTime)
        {

            if (Input.GetMouseButtonDown(0))
            {
                capsuleCollider.offset =new Vector2(transform.position.x,transform.position.y);
                audioManager.playSFX(audioManager.swordSwing);
                currentAtkTime = Time.time+1f/atkRate;
                Debug.Log("MouseClicked");
                anim.SetTrigger("Attack");
            }
        }

    }
    private void CombineLayerMasks()
    {
        combinedLayerMask = 0; // Start with an empty mask
        foreach (LayerMask mask in grounds)
        {
            combinedLayerMask |= mask; // Combine all LayerMasks
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundChecker.position,checkRadius,combinedLayerMask);
    }
}
