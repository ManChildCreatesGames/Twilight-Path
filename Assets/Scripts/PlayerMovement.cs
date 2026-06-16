using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // References to input actions and asset map
    public InputActionAsset inptActs;
    private InputAction m_moveAction;
    private InputAction m_jumpAction;

    //other movement variables
    private Vector2 m_position;
    private Rigidbody2D rb2D;
    [SerializeField]int lookDirection;
    float walkSpeed;
    public AudioSource walkSound;
    [SerializeField] GameObject walkObject;

    //jump variables
    float jumpForce;
    [SerializeField] int jumpCount = 0;
    public GameObject groundCheck;
    [SerializeField] GameObject jumpCloud;
    public bool isGrounded;
    public AudioSource jumpSound;
    public AudioSource landSound;

    //lives & score variables
    public int lives = 3;
    public int score = 0;

    //sword variables
    public bool touchedSword;

    //animator variable
    public Animator playerAnimator;


    //checks for input actions and enables them when the object is enabled, disables them when the object is disabled
    private void OnEnable()
    {
        m_moveAction = inptActs.FindAction("Move");
        m_jumpAction = inptActs.FindAction("Jump");
        m_moveAction.Enable();
        m_jumpAction.Enable();
    }
    private void OnDisable()
    {
        m_moveAction.Disable();
        m_jumpAction.Disable();
    }
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("GroundCheck");
        m_moveAction = inptActs.FindAction("Move");
        m_jumpAction = inptActs.FindAction("Jump");
        jumpSound = GetComponent<AudioSource>();
        walkSound = walkObject.GetComponent<AudioSource>();
        landSound = groundCheck.GetComponent<AudioSource>();
    }
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        walkSpeed = 15f;
        jumpForce = 10;
        lookDirection = 1;
        isGrounded = true;
        jumpCloud.SetActive(false);
        touchedSword = false;
    }
    void Update()
    {
        // Check if the player has touched the sword and call the HasSword method if true
        if (touchedSword == true)
        {
            HasSword();
        }

        flipSprite();
        m_position = m_moveAction.ReadValue<Vector2>();
        rb2D.linearVelocity = new Vector2(m_position.x * walkSpeed, rb2D.linearVelocity.y);
        if(m_position.x != 0 && isGrounded == true)
        {
            if (!walkSound.isPlaying)
            {
                walkSound.Play();
            }
            playerAnimator.SetBool("Walking", true);
        }
        else if (m_position.x == 0 || isGrounded == false)
        {
            walkSound.Stop();
            playerAnimator.SetBool("Walking", false);
        }
        if (m_jumpAction != null && m_jumpAction.WasPressedThisFrame() && jumpCount < 2)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            jumpCount++;
            jumpSound.Play();
            jumpCloud.SetActive(true);
        }
        if (isGrounded == false && rb2D.linearVelocityY < 0)
        {
            rb2D.gravityScale = 7f;
            Debug.Log("Falling" + rb2D.gravityScale);
            jumpCloud.SetActive(false);
            if (jumpCount > 1)
            {
                m_jumpAction.Disable();
            }  
        }
        else if (isGrounded == true)
        {
            rb2D.gravityScale = 1f;
            Debug.Log("Falling" + rb2D.gravityScale);
            jumpCount = 0;
            m_jumpAction.Enable();
            jumpCloud.SetActive(false);

        }

        void flipSprite()
        {
            if (m_position.x > 0)
            {
                transform.localScale = new Vector3(2, 1, 1);
                lookDirection = 1;
            }
            else if (m_position.x < 0)
            {
                transform.localScale = new Vector3(-2, 1, 1);
                lookDirection = -1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (groundCheck & other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb2D.gravityScale = 1f;
            Debug.Log("Falling" + rb2D.gravityScale);
            jumpCount = 0;
            m_jumpAction.Enable();
            landSound.Play();
        }
        if (other.gameObject.CompareTag("Barrel"))
        {
           lives -= 1;
            if (lives <= 0)
            {
                // Implement game over logic here, e.g., show game over screen, reset level, etc.
                Debug.Log("Game Over!");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (groundCheck & other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if(other.gameObject.CompareTag("sword"))
        {
            touchedSword = true;
            Destroy(other.gameObject);
        }
    }
    public void HasSword()
    {
        // Implement sword logic here, e.g., enable sword attack, change player appearance, etc.
        playerAnimator.SetBool("withSword", true);
        //debug log to confirm sword pickup
        Debug.Log("Player has sword!");
    }

}