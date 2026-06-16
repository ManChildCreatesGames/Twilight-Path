using UnityEngine;
using UnityEngine.UIElements;

public class FireBall : MonoBehaviour
{
   public double upperYBound;
   public double lowerBound;
   public double leftBound;
   public double rightBound;
   public float speed;

    //i want the fire ball to move in a boundary and if it hits the player it will damage the player and then disappear,
    //if it hits the boundary it will change direction and keep moving until it hits the player or the boundary again

    //awake is called when the script instance is being loaded, it is called before any Start functions and also just after a prefab is instantiated
    void Awake()
    {
        var position = transform.position = new Vector2(Random.Range((float)leftBound, (float)rightBound), Random.Range((float)lowerBound, (float)upperYBound));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
