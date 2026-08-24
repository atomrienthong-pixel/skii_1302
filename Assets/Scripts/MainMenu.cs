using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName = "Scene01";

    public void StartGame()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayButton();

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
