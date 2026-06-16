using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [HideInInspector] public string poolKey; //unique pool key to identify which pool this object belongs to
    [HideInInspector] public Transform spawnParent; // where to spawn
    [HideInInspector] public GameManager gameManager; // reference to the game manager to call spawn function
}
