using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int Lives = 5;
    public PlayerMovement playerMovementScript;
    public GameObject player;
    public GameObject bigTwilight;
    public Dictionary<string, Queue<GameObject>> poolsDictionary;
    public List<pool> pools;

        [System.Serializable]
        public class pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    private void Awake()
    {
        poolsDictionary = new Dictionary<string, Queue<GameObject>>();
        playerMovementScript = player.GetComponent<PlayerMovement>();

    }
}
