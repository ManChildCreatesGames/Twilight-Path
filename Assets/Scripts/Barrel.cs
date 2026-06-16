using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public Rigidbody2D rb2D;
    [SerializeField] GameObject groundCheck;
    [SerializeField] SpriteRenderer BarrelXflip;
    [SerializeField] bool isGrounded;
    [SerializeField] Animator Banimtor;
    PlayerMovement player;

   IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        rb2D.gravityScale = 1f;
    }
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        BarrelXflip = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        isGrounded = false;
        Banimtor.SetBool("hitGround", false);
        rb2D.gravityScale = 5f;
        StartCoroutine(wait());
        player = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb2D != null && isGrounded == true && BarrelXflip.flipX == true)
        {
            rb2D.linearVelocity = new Vector2(10f, 0f);
            
        }
        else if (rb2D != null && isGrounded == true && BarrelXflip.flipX == false)
        {
            rb2D.linearVelocity = new Vector2(-10f, 0f);

        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (groundCheck && other.gameObject.CompareTag("Ground"))
        {
            rb2D.gravityScale = 1f;
            isGrounded = true;
            Banimtor.SetBool("hitGround", true);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<PlayerMovement>();
            player.lives -= 1;
        }
        if (other.gameObject.CompareTag("Destroy"))
        {
           
            
            //respawn a replacement barrel from the pool at the spawnpoint
            var pooledObj = GetComponent<PooledObject>();

            if(pooledObj != null && pooledObj.gameManager != null)
            {
                pooledObj.gameManager.ReturnToPool(gameObject, 0f);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (groundCheck && collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Banimtor.SetBool("hitGround", false);
            rb2D.gravityScale = 5f;
        }
    }
}
