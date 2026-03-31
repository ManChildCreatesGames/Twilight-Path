using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField]Button startButton;
    [SerializeField]Button continueButton;
    [SerializeField]float continueButtonAlpha = 0f;
    [SerializeField]float continueTextAlpha = 0f;
    [SerializeField]Button optionsButton;
    [SerializeField]Button closeOptionsButton;
    [SerializeField]GameObject optionsMenu;
    [SerializeField]Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startButton != null)
        { startButton.clicked += StartGame; }
        if (optionsButton != null)
        { optionsButton.clicked += OpenOptions; }
        optionsMenu.gameObject.SetActive(false);
        if (quitButton != null)
        { quitButton.clicked += QuitGame; }
        // Check if there is a saved checkpoint in PlayerPrefs
        if (PlayerPrefs.HasKey("LastCheckpoint"))
        {
            // Get the last checkpoint scene name from PlayerPrefs
            string lastCheckpointScene = PlayerPrefs.GetString("LastCheckpoint");
            if (lastCheckpointScene != null)
            {
                // Enable the continue button and set its click event to load the last checkpoint scene
                continueButton = gameObject.transform.Find("ContinueButton").GetComponent<Button>();
                if (continueButton != null)
                {
                    continueButtonAlpha = 1f; // Set the Button alpha to 1 to make it visible
                    continueTextAlpha = 1f; // Set the Text alpha to 1 to make it visible
                    continueButton.clicked += () => LoadScene(lastCheckpointScene);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
   public void StartGame()
    {
        SceneManager.LoadScene("StartingScene");
    }
    //continue game from last checkpoint
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }
    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
