using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

    public void ShowControls()
    {
        Debug.Log("Show control instructions here.");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
