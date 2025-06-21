using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
 public void BackToMenu()
    {
        SceneManager.LoadScene("MenuGame");
    }
}
