using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("SelectChampion1"); // t�n scene ch�nh
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Tho�t game");
    }
}

