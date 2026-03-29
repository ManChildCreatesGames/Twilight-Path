using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField]Button startButton;
    [SerializeField]Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startButton != null)
        { startButton.clicked += StartGame; }
        if (quitButton != null)
        { quitButton.clicked += QuitGame; }
    }

    // Update is called once per frame
    void Update()
    {

    }
   public void StartGame()
    {
        SceneManager.LoadScene("StartingScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
