using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("TutorialLevel");
    }
}
