using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("StoryScene"); // tên scene chính
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Thoát game");
    }
}

