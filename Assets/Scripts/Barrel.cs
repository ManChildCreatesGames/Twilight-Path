using UnityEngine;

public class Barrel : MonoBehaviour
{
    public Rigidbody2D rb2D;
    [SerializeField] GameObject groundCheck;
    [SerializeField] SpriteRenderer BarrelXflip;
    [SerializeField] bool isGrounded;
    [SerializeField] Animator Banimtor;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        BarrelXflip = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        isGrounded = false;
        Banimtor.SetBool("hitGround", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb2D != null && isGrounded == true && BarrelXflip.flipX == true)
        {
            rb2D.linearVelocity = new Vector3(10f, 0f, 0);
            
        }
        else if (rb2D != null && isGrounded == true && BarrelXflip.flipX == false)
        {
            rb2D.linearVelocity = new Vector3(-10f, 0f, 0);

        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (groundCheck & other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Banimtor.SetBool("hitGround", true);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Destroy"))
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (groundCheck & collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Banimtor.SetBool("hitGround", false);
        }
    }
}
