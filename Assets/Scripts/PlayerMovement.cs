using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
   public InputActionAsset inptActs;

    private InputAction m_moveAction;
    private InputAction m_jumpAction;

    private Vector2 m_position;
    private Rigidbody2D rb2D;

    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField]int lookDirection = 1;

    public GameObject groundCheck;
    public bool isGrounded;

    public AudioSource jumpSound;
    public AudioSource walkSound;
    public AudioSource landSound;

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
        walkSound = GetComponent<AudioSource>();
        landSound = GetComponent<AudioSource>();
    }
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        walkSpeed = 10f;
        jumpForce = 15;
        lookDirection = 1;
        isGrounded = true;
    }
    void Update()
    {
        flipSprite();
        m_position = m_moveAction.ReadValue<Vector2>();
        rb2D.linearVelocity = new Vector2(m_position.x * walkSpeed, rb2D.linearVelocity.y);
        if (m_jumpAction != null && m_jumpAction.WasPressedThisFrame())
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            jumpSound.Play();
        }
        if (isGrounded == false)
        {
            rb2D.gravityScale = 2f;
            m_jumpAction.Disable();
        }
        else if (isGrounded == true)
        {
            rb2D.gravityScale = 1f;
            m_jumpAction.Enable();

        }

        void flipSprite()
        {
            if (m_position.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                lookDirection = 1;
                walkSound.Play();
            }
            else if (m_position.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                lookDirection = -1;
                walkSound.Play();
            }
        }
    }

        private void OnTriggerEnter2D(Collider2D other)
        {
           if (groundCheck & other.gameObject.CompareTag("Ground"))
           {
            isGrounded = true;
            rb2D.gravityScale = 1f;
            m_jumpAction.Enable();
            landSound.Play();
           }
        }
            
}
