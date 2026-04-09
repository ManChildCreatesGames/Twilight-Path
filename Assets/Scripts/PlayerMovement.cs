using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
   public InputActionAsset inptActs;

    private InputAction m_moveAction;
    private InputAction m_jumpAction;

    private Vector2 m_position;
    private Rigidbody2D rb2D;
    [SerializeField]int lookDirection;
    float walkSpeed;
     float jumpForce;

    [SerializeField]int jumpCount = 0;

    public GameObject groundCheck;
    [SerializeField]GameObject jumpCloud;
    public bool isGrounded;

    public AudioSource jumpSound;
    public AudioSource walkSound;
    [SerializeField]GameObject walkObject;
    public AudioSource landSound;
    [SerializeField]Animator playerAnimator;

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
    }
    void Update()
    {
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
        }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (groundCheck & other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
